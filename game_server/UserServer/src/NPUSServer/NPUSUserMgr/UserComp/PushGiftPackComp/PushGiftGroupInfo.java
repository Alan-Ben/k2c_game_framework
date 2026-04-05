package NPUSServer.NPUSUserMgr.UserComp.PushGiftPackComp;

import Common.PushGiftObj.PushGift_ActivePackInfo;
import Common.PushGiftObj.PushGift_GroupInfo;
import GS2GC.p004_PlayerOp.GS2GC_004_074_OnPushGiftPackGroupChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.PushGift.RefPushGiftGroup;
import NPGameRes.Refs.PushGift.RefPushGiftPack;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OrderComp.PlayerGiftPackRecord;
import USDB.Bo.PlayerPushGiftGroupBO;

public class PushGiftGroupInfo
{
    private PushGiftComponent _m_comp;
    // 配置对象缓存
    private RefPushGiftGroup _m_ref;
    // 数据对象
    private PlayerPushGiftGroupBO _m_bo;

    public PushGiftGroupInfo(PushGiftComponent _comp, RefPushGiftGroup _refGroup, PlayerPushGiftGroupBO _bo)
    {
        _m_comp = _comp;
        _m_ref = _refGroup;
        _m_bo = _bo;
    }

    /**
     * 便利方法：获取BM对象
     * @return BM对象
     */
    private BM getBM()
    {
        return _m_comp.getBM();
    }

    public RefPushGiftGroup getRef()
    {
        return _m_ref;
    }

    public RefPushGiftPack getPushGiftRef()
    {
        return RefPushGiftPack.getMgr().get(_m_bo.getPushGiftId());
    }

    public long getGroupId()
    {
        return _m_ref.Id();
    }

    public long getTriggerTimeMs()
    {
        return _m_bo.getTriggerTimeMs();
    }

    public long getActiveTimeMs()
    {
        return _m_bo.getActiveTimeMs();
    }

    /**
     * 是否达到礼包组触发冷却条件
     * <p>
     * 判断当前时间是否已超过上次触发时间 + 组内配置的冷却秒数。
     *
     * @param _nowTimeMS 当前时间戳（毫秒）
     * @return true 表示已过冷却期，可以触发；false 表示仍在冷却中
     */
    public boolean canTrigger(long _nowTimeMS)
    {
        return _nowTimeMS >= _m_bo.getTriggerTimeMs() + _m_ref.next_trigger_need_seconds * 1000L;
    }

    /**
     * 标记已读
     * <p>
     * 执行流程：
     * 1. 委托给激活记录对象处理
     * 2. 通知客户端礼包组状态变更
     */
    public Result markAsRead(long _pushGiftId)
    {
        // 检查礼包ID是否匹配
        if (_m_bo.getPushGiftId() != _pushGiftId)
            return OrderErr.PUSH_GIFT_NOT_ACTIVE;

        _m_bo.saveHasRead(getBM(), true);

        // 通知客户端礼包组状态变更
        _m_comp.getUserData().sendMsgToGC(new GS2GC_004_074_OnPushGiftPackGroupChg(makeProto()));

        return Result.SUCC;
    }


    /**
     * 订单交付后的处理
     * @param _giftPackId
     * @param _context
     */
    public void onOrderDelivery(long _giftPackId, NPPlayerContext _context)
    {
        if (_m_bo.getPushGiftId() == 0)
            return;

        RefPushGiftPack currentPackRef = RefPushGiftPack.getMgr().get(_m_bo.getPushGiftId());
        if (currentPackRef == null)
            return;

        if (currentPackRef.gift_pack_id != _giftPackId)
            return;

        tryTrigger(true, _context);
    }

    /**
     * 尝试触发礼包组
     * <p>
     * 执行流程：
     * 1. 检查触发条件（冷却时间/自动触发配置）
     * 2. 判断当前礼包状态（首次/未过期/已过期）
     * 3. 根据购买状态决定下一个激活的礼包
     * 4. 更新数据库并通知客户端
     * <p>
     * 触发规则：
     * - 自动触发：购买后自动触发，必须配置 after_buy_auto_trigger_next = true
     * - 手动触发：需满足冷却时间条件
     * - 首次触发：激活配置的首个礼包
     * - 未过期且已达购买上限：触发下一个礼包或重置
     * - 已过期：触发降档礼包或下一个礼包或重置
     *
     * @param _isAutoTrigger 是否自动触发（购买后自动触发）
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result tryTrigger(boolean _isAutoTrigger, NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 自动触发不检查冷却时间
        if (_isAutoTrigger)
        {
            // 查询配置
            RefPushGiftPack currentPackRef = RefPushGiftPack.getMgr().get(_m_bo.getPushGiftId());
            if (currentPackRef == null)
                return CommErr.REF_NOT_FOUND;

            // 自动触发必须配置为购买后自动触发下一个
            if (!currentPackRef.after_buy_auto_trigger_next)
                return OrderErr.PUSH_GIFT_CANT_AUTO_TRIGGER;
        } else
        {
            // 检查礼包组的触发冷却时间
            if (!canTrigger(nowTimeMS))
                return OrderErr.PUSH_GIFT_TRIGGER_COOLDOWN;
        }

        // 检查是否有在有效期内的激活礼包
        if (_m_bo.getPushGiftId() == 0)
        {
            // 无激活礼包，触发首次礼包
            _triggerPack(_m_ref.first_trigger_gift_id, nowTimeMS);
        } else
        {
            // 查询配置
            RefPushGiftPack currentPackRef = RefPushGiftPack.getMgr().get(_m_bo.getPushGiftId());
            if (currentPackRef == null)
                return CommErr.REF_NOT_FOUND;

            long expireTimeMs = _m_bo.getTriggerTimeMs() + currentPackRef.continue_time * 1000L;

            // 判断关联礼包是否达到购买上限
            PlayerGiftPackRecord giftPackRecord = _m_comp.getUserData().getOrderComponent().getMgr().lookup(currentPackRef.gift_pack_id);
            boolean hasReachedBuyLimit = giftPackRecord != null && giftPackRecord.reachBuyLimit();

            // 未过期
            if (nowTimeMS < expireTimeMs)
            {
                // 还没达到上限 不允许触发下一个
                if (!hasReachedBuyLimit)
                {
                    return Result.SUCC;
                } else
                {
                    // 已经达到购买上限，触发下一个礼包
                    long nextPushGiftId = currentPackRef.next_trigger_push_gift_id;

                    // 没有下一个礼包
                    if (nextPushGiftId == 0)
                    {
                        // 没有下一个礼包，且需要重置购买次数
                        if (_m_ref.need_reset_buy_count)
                        {
                            _triggerFirstPack(_context, nowTimeMS);
                        }else
                        {
                            return OrderErr.PUSH_GIFT_NO_NEXT_CAN_TRIGGER;
                        }
                    } else
                    {
                        _triggerPack(nextPushGiftId, nowTimeMS);
                    }
                }

            } else
            {
                if (currentPackRef.downgrade_trigger_push_gift_id != 0)
                {
                    // 过期且配置了降档礼包，触发降档礼包
                    _triggerPack(currentPackRef.downgrade_trigger_push_gift_id, nowTimeMS);
                } else
                {
                    if (_m_ref.need_reset_buy_count)
                    {
                        _triggerFirstPack(_context, nowTimeMS);
                    } else
                    {
                        // 已经达到购买上限，触发下一个礼包, 否则继续当前礼包
                        long nextPushGiftId = hasReachedBuyLimit ? currentPackRef.next_trigger_push_gift_id : _m_bo.getPushGiftId();

                        _triggerPack(nextPushGiftId, nowTimeMS);
                    }
                }
            }
        }

        // 通知客户端礼包组状态变更
        _m_comp.getUserData().sendMsgToGC(new GS2GC_004_074_OnPushGiftPackGroupChg(makeProto()));

        return Result.SUCC;
    }

    private void _triggerPack(long nextPushGiftId, long nowTimeMS)
    {
        _m_bo.setPushGiftId(getBM(), nextPushGiftId);
        _m_bo.setTriggerTimeMs(getBM(), nowTimeMS);
        _m_bo.setActiveTimeMs(getBM(), nowTimeMS);
        _m_bo.setHasRead(getBM(), false);
        _m_bo.saveAllMarked(getBM());
    }

    private void _triggerFirstPack(NPPlayerContext _context, long nowTimeMS)
    {
        // 触发第一个礼包
        _triggerPack(_m_ref.first_trigger_gift_id, nowTimeMS);

        // 重置第一个礼包的购买次数
        PlayerGiftPackRecord newPackInfo = _m_comp.getUserData().getOrderComponent().getMgr().lookup(_m_ref.first_trigger_gift_id);
        if (newPackInfo != null)
            newPackInfo.resetBuyCount(_context);
    }

    /**
     * 是否可以购买礼包
     * @param _giftPackId
     * @return
     */
    public boolean canBuyGiftPack(long _giftPackId)
    {
        if (_m_bo.getPushGiftId() == 0)
            return false;

        RefPushGiftPack currentPackRef = RefPushGiftPack.getMgr().get(_m_bo.getPushGiftId());
        if (currentPackRef == null)
            return false;

        return currentPackRef.gift_pack_id == _giftPackId;
    }

    /**
     * 修改触发时间
     * @param _newTimeMs
     */
    public void chgTriggerTimeMs(long _newTimeMs)
    {
        _m_bo.saveTriggerTimeMs(getBM(), _newTimeMs);

        // 通知客户端礼包组状态变更
        _m_comp.getUserData().sendMsgToGC(new GS2GC_004_074_OnPushGiftPackGroupChg(makeProto()));
    }

    /**
     * 修改激活时间
     * @param _newTimeMs
     */
    public void chgActiveTimeMs(long _newTimeMs)
    {
        _m_bo.saveActiveTimeMs(getBM(), _newTimeMs);

        // 通知客户端礼包组状态变更
        _m_comp.getUserData().sendMsgToGC(new GS2GC_004_074_OnPushGiftPackGroupChg(makeProto()));
    }

    /**
     * 清除礼包组信息
     */
    public void clearGroupInfo()
    {
        _m_bo.setPushGiftId(getBM(), 0);
        _m_bo.setTriggerTimeMs(getBM(), 0);
        _m_bo.setActiveTimeMs(getBM(), 0);
        _m_bo.setHasRead(getBM(), false);
        _m_bo.saveAllMarked(getBM());

        // 通知客户端礼包组状态变更
        _m_comp.getUserData().sendMsgToGC(new GS2GC_004_074_OnPushGiftPackGroupChg(makeProto()));
    }

    /**
     * 构造礼包信息
     * @return
     */
    public PushGift_ActivePackInfo makePackInfo()
    {
        PushGift_ActivePackInfo activePackInfo = new PushGift_ActivePackInfo();
        activePackInfo.setPushGiftId(_m_bo.getPushGiftId());
        activePackInfo.setActivateTimeMs(_m_bo.getActiveTimeMs());
        activePackInfo.setHasRead(_m_bo.getHasRead());
        return activePackInfo;
    }

    /**
     * 构造协议对象
     * @return PushGift_GroupInfo协议对象
     */
    public PushGift_GroupInfo makeProto()
    {
        PushGift_GroupInfo groupInfo = new PushGift_GroupInfo();
        groupInfo.setGroupId(_m_ref.Id());
        groupInfo.setLastTriggerTimeMs(_m_bo.getTriggerTimeMs());
        groupInfo.setActivePack(makePackInfo());
        return groupInfo;
    }
}

package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MailObj.Mail_Data;
import Common.RechargeRebateEnum.ERechargeRebateType;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_007_RetRechargeRebateInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_110_OnRechargeRebateRewardDraw;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.RechargeRebatePlayerInfoBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 充值返利信息抽象基类
 *
 * 负责管理特定类型（每日/累计/累天）的充值返利逻辑
 *
 * 主要功能：
 * 1. 玩家数据管理
 * 2. 充值计数更新
 * 3. 奖励领取
 * 4. 未领奖励补发
 *
 * 线程安全：使用MutexObject保护玩家数据映射
 */
public abstract class _ARechargeRebateInfo
{
    // 持有活动引用
    protected RechargeRebateActivity _m_activity;

    // 配置引用
    protected RefRechargeRebateGroup _m_groupRef;

    // 玩家数据映射：CID -> 玩家组数据
    protected Map<Long, RechargeRebateGroupData> _m_playerDataMap;

    // 线程安全锁
    protected MutexObject _m_mutex;

    public _ARechargeRebateInfo(RechargeRebateActivity _activity, RefRechargeRebateGroup _groupRef)
    {
        _m_activity = _activity;
        _m_groupRef = _groupRef;
        _m_playerDataMap = new HashMap<>();
        _m_mutex = new MutexObject();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取活动对象
     */
    public RechargeRebateActivity getActivity()
    {
        return _m_activity;
    }

    /**
     * 获取服务器对象
     */
    public NPUserServer getUSServer()
    {
        return _m_activity.getUSServer();
    }

    /**
     * 获取组配置
     */
    public RefRechargeRebateGroup getGroupRef()
    {
        return _m_groupRef;
    }

    public long getGroupId()
    {
        return _m_groupRef.Id();
    }

    /**
     * 获取返利类型
     */
    public abstract ERechargeRebateType getType();

    /**
     * 初始化BO数据
     */
    public void initBo(RechargeRebatePlayerInfoBO _bo)
    {
        _lock();
        try
        {
            long cid = _bo.getCid();
            RechargeRebateGroupData data = new RechargeRebateGroupData(this, _m_groupRef, _bo);
            _m_playerDataMap.put(cid, data);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取或创建玩家数据
     */
    protected RechargeRebateGroupData ensurePlayerData(long _cid)
    {
        _lock();
        try
        {
            RechargeRebateGroupData data = lookupPlayerData(_cid);
            if (data == null)
            {
                data = new RechargeRebateGroupData(this, _m_groupRef);
                _m_playerDataMap.put(_cid, data);
            }
            return data;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找玩家数据
     */
    protected RechargeRebateGroupData lookupPlayerData(long _cid)
    {
        _lock();
        try
        {
            return _m_playerDataMap.get(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 处理充值事件
     * @param _cid    玩家CID
     * @param _vipExp VIP经验值
     */
    public abstract void onRecharge(long _cid, long _vipExp);

    /**
     * 领取奖励（通用实现）
     *
     * 执行流程：
     * 1. 检查玩家数据是否存在
     * 2. 检查是否已领取
     * 3. 查找档位配置
     * 4. 检查是否达到目标
     * 5. 发放奖励并记录
     * 6. 推送领取成功消息
     *
     * @param _userdata 玩家数据
     * @param _stepId 档位ID
     * @param _context 操作上下文
     * @return 执行结果
     */
    public Result drawReward(NPUSUserData _userdata, long _stepId, NPPlayerContext _context)
    {
        // 查找档位配置
        RefRechargeRebateStep stepRef = RefRechargeRebateStep.getMgr().lookupStepRef(_m_groupRef.Id(), _stepId);
        if (stepRef == null)
            return PlayerErr.RECHARGE_REBATE_STEP_NOT_FOUND;

        _lock();
        try{
            long cid = _userdata.getCid();
            RechargeRebateGroupData data = lookupPlayerData(cid);
            if (data == null)
                return PlayerErr.RECHARGE_REBATE_NOT_MEET_REQUIRE;

            // 检查是否达到目标
            if (data.getCount() < stepRef.target_count)
                return PlayerErr.RECHARGE_REBATE_NOT_MEET_REQUIRE;

            // 记录已领取
            Result result = data.recordDrawStep(cid, _stepId);
            if (!result.isSucc())
                return result;
        }finally
        {
            _unlock();
        }

        // 发放奖励
        _userdata.gainItemList(stepRef.reward_list, _context);

        // 推送领取成功
        GS2GC_033_110_OnRechargeRebateRewardDraw proto = new GS2GC_033_110_OnRechargeRebateRewardDraw();
        proto.setActivityInstanceId(getActivity().getInstanceId());
        proto.setGroupId(_m_groupRef.id);
        proto.setStepId(_stepId);
        _userdata.sendMsgToGC(proto);

        return Result.SUCC;
    }

    /**
     * 每日零点重置（仅每日充值返利需要）
     */
    public abstract void onCrossDay();

    /**
     * 填充未领取奖励列表（通用实现）
     *
     * 遍历所有档位，收集已达成但未领取的奖励
     *
     * @param _cid 玩家CID
     * @param _rewardList 奖励列表（输出参数）
     */
    public void fillUnclaimedRewards(long _cid, List<NPCommonCostItem> _rewardList)
    {
        _lock();
        try{
            RechargeRebateGroupData data = lookupPlayerData(_cid);
            if (data == null)
                return;

            long currentCount = data.getCount();
            List<RefRechargeRebateStep> steps = RefRechargeRebateStep.getMgr().getAllSteps(getGroupId());

            for (RefRechargeRebateStep step : steps)
            {
                // 已达成但未领取
                if (currentCount >= step.target_count && !data.hadDrawStep(step.id))
                {
                    if (step.reward_list != null && !step.reward_list.isEmpty())
                    {
                        _rewardList.addAll(step.reward_list);
                    }
                }
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 发送未领取奖励邮件
     */
    public void sendUnclaimedRewards()
    {
        _lock();
        try
        {
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SERVER_CROSS_DAY);

            for (Map.Entry<Long, RechargeRebateGroupData> entry : _m_playerDataMap.entrySet())
            {
                long cid = entry.getKey();

                List<NPCommonCostItem> rewardList = new ArrayList<>();

                fillUnclaimedRewards(cid, rewardList);

                if (rewardList.isEmpty())
                    continue;

                // 发送邮件
                Mail_Data mailData = new Mail_Data();
                mailData.setMailRefId(_m_groupRef.mail_id);
                mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(rewardList));

                ALSynTaskManager.getInstance().regTask(() -> MailSystem.addMail(getUSServer(), cid, mailData, context));

            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 填充协议数据
     *
     * @param _proto 协议对象
     * @param _cid 玩家CID
     */
    public void fillProto(GS2GC_033_007_RetRechargeRebateInfo _proto, long _cid)
    {
        RechargeRebateGroupData data = ensurePlayerData(_cid);

        _proto.addGroupInfoList(data.makeProto());
    }

}

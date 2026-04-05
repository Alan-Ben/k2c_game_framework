package NPUSServer.NPUSUserMgr.UserComp.BagItemComp;

import ALBasicCommon.ALBasicCommonFun;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityEnum.EActivityState;
import Common.PlayerEnum.EPlayerEventRecordType;
import CommonEnum.EActivityItemExpireTimeType;
import MJLog.MJLog;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.NPCommon_BagItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENCounterDealType;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Activity.RefActivityBagItem;
import NPGameRes.Refs.BagItem.RefBagItem;
import NPGameRes.Refs.BagItem.RefBagItemUse;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBagItemBO;

import java.util.Map;

public class BagItemInfo
{
    //玩家组件
    private BagItemComponent _m_compBagItemComponent;
    //BO数据
    private long _m_lDbid;
    private long _m_lItemCount;
    private int _m_iLastGetTimeS;
    private int _m_iNewGetTimeS;
    private int _m_iLastClickTimeS;
    private long _m_lTotalGainCount;
    private long _m_lTotalConsumeCount;
    private long _m_lRelativeActivityInstanceId; //关联的活动实例ID
    //物品使用对象
    private RefBagItem _m_refBagItemRef;
    //物品使用配置对象
    private RefBagItemUse _m_refBagItemUseRef;

    public BagItemInfo(BagItemComponent _bagItemComponent, PlayerBagItemBO _bagItemBO, RefBagItem _refBagItem)
    {
        _m_compBagItemComponent = _bagItemComponent;

        _m_lDbid = _bagItemBO.getId();
        _m_lItemCount = _bagItemBO.getItemCount();
        _m_iLastGetTimeS = _bagItemBO.getLastGetTimeS();
        _m_iNewGetTimeS = _bagItemBO.getNewGetTimeS();
        _m_iLastClickTimeS = _bagItemBO.getLastClickTimeS();
        _m_lTotalGainCount = _bagItemBO.getTotalGainCount();
        _m_lTotalConsumeCount = _bagItemBO.getTotalConsumeCount();
        _m_lRelativeActivityInstanceId = _bagItemBO.getRelativeActivityInstanceId();

        _m_refBagItemRef = _refBagItem;
        _m_refBagItemUseRef = RefBagItemUse.getMgr().get(_m_refBagItemRef.id);
    }

    public BagItemComponent getComp()
    {
        return _m_compBagItemComponent;
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    public long getItemCount()
    {
        return _m_lItemCount;
    }

    public int getLastGetTimeS()
    {
        return _m_iLastGetTimeS;
    }

    public int getNewGetTimeS()
    {
        return _m_iNewGetTimeS;
    }

    public int getLastClickTimeS()
    {
        return _m_iLastClickTimeS;
    }

    public long getTotalGainCount()
    {
        return _m_lTotalGainCount;
    }

    public long getTotalConsumeCount()
    {
        return _m_lTotalConsumeCount;
    }

    public RefBagItem getRef()
    {
        return _m_refBagItemRef;
    }

    public RefBagItemUse getBagItemUseRef()
    {
        return _m_refBagItemUseRef;
    }

    public long getItemId()
    {
        return _m_refBagItemRef.id;
    }

    public long getRelativeActivityInstanceId()
    {
        return _m_lRelativeActivityInstanceId;
    }

    /**
     * 增加物品背包数量
     * @param _addCount
     * @param _relativeActivityInstanceId
     */
    protected void addCount(long _addCount, long _relativeActivityInstanceId)
    {
        _m_lItemCount = getItemCount() + _addCount;
        _m_lTotalGainCount = _m_lTotalGainCount + _addCount;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("itemCount", _m_lItemCount);
        updateValue.addValueObj("total_gain_count", _m_lTotalGainCount);

        if (_m_refBagItemRef.need_redtip_when_add)
        {
            _m_iLastGetTimeS = CommonFunc.getNowTimeSec();
            updateValue.addValueObj("lastGetTimeS", _m_iLastGetTimeS);
        }

        if (_m_lRelativeActivityInstanceId == 0 && _relativeActivityInstanceId != 0)
        {
            _m_lRelativeActivityInstanceId = _relativeActivityInstanceId;
            updateValue.addValueObj("relative_activity_instance_id", _m_lRelativeActivityInstanceId);
        }

        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);
    }

    /*******
     * 扣除
     * @param _spendCount
     * @return
     */
    protected long spendCount(long _spendCount, NPPlayerContext _context)
    {
        //无数据变化
        if (0 == _spendCount)
            return _m_lItemCount;

        //计算新值
        long newCount = _m_lItemCount - _spendCount;
        if (newCount < 0)
            newCount = 0;

        //计算实际消耗数值
        long realSpendCount = _m_lItemCount - newCount;

        //保存数据
        _m_lItemCount = newCount;
        _m_lTotalConsumeCount = _m_lTotalConsumeCount + realSpendCount;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("itemCount", _m_lItemCount);
        updateValue.addValueObj("total_consume_count", _m_lTotalConsumeCount);
        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);

        //消耗记录
        if (getRef().is_spend_record)
        {
            NPSynPlayerEvnetRecordTask.asyncRecord(getComp().getUserData(), ENCounterDealType.ADD,
                    EPlayerEventRecordType.BAG_ITEM_SPEND.ordinal(), getRef().id, realSpendCount);
        }

        return newCount;
    }

    /**
     * 强制扣除，支持扣到负数（仅用于GM命令）
     * @param _spendCount 扣除数量
     * @param _context 上下文
     */
    protected void gmConsumeCount(long _spendCount, NPPlayerContext _context)
    {
        //计算新值，允许负数
        _m_lItemCount = _m_lItemCount - _spendCount;

        //保存数据
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("itemCount", _m_lItemCount);
        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);
    }

    /**
     * 设置
     * @param _count
     */
    protected void setCount(long _count)
    {
        if (_m_lItemCount == _count)
            return;

        _m_lItemCount = _count;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("itemCount", _m_lItemCount);
        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);
    }

    /**
     * 刷新最后一个点击时间
     * @return
     */
    public void refreshLastClickTimeS()
    {
        _m_iLastClickTimeS = ALBasicCommonFun.getNowTime();

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("lastClickTimeS", _m_iLastClickTimeS);
        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);
    }

    /**
     * 返回背包物品协议对象
     * @return
     */
    public NPCommon_BagItemInfo toProto()
    {
        NPCommon_BagItemInfo protocol = new NPCommon_BagItemInfo();
        protocol.setItemId(getItemId());
        protocol.setItemCount(getItemCount());
        protocol.setLastGetTimeS(getLastGetTimeS());
        protocol.setClickItemTimeS(getLastClickTimeS());
        protocol.setNewItemTimeS(getNewGetTimeS());

        return protocol;
    }

    /**
     * 检查物品是否过期
     * @param _activityInstanceId
     * @param _state
     * @param _collectorMap
     * @param _context
     */
    public void checkExpire(Long _activityInstanceId, EActivityState _state, Map<Long, NPItemCostCollector_nosafe> _collectorMap, NPPlayerContext _context)
    {
        //检查是否是匹配的活动实例
        if (_m_lRelativeActivityInstanceId != _activityInstanceId)
            return;

        RefActivityBagItem refActivityBagItem = RefActivityBagItem.getMgr().get(getItemId());
        if (refActivityBagItem == null)
        {
            USLog.warn("BagItemInfo.checkExpire: RefActivityBagItem not found for cid:{} itemId:{} count:{}",
                    _m_compBagItemComponent.getUserData().getCid(), getItemId(), _m_lItemCount);
            return;
        }

        //判断活动当前状态是否满足过期状态
        boolean shouldExpire = false;
        if (refActivityBagItem.expire_type == EActivityItemExpireTimeType.REWARDING)
        {
            shouldExpire = (_state == EActivityState.REWARDING || _AActivityBase.CLOSE_STATE.contains(_state));
        } else if (refActivityBagItem.expire_type == EActivityItemExpireTimeType.CLOSED)
        {
            shouldExpire = (_AActivityBase.CLOSE_STATE.contains(_state));
        }
        if (!shouldExpire)
            return;

        //只有数量大于0才需要补发
        if(_m_lItemCount != 0)
            _collectorMap.computeIfAbsent(refActivityBagItem.activity_id, k -> new NPItemCostCollector_nosafe())
                 .addItemList(CommonFunc.itemMultiple(refActivityBagItem.item_list, _m_lItemCount));

        // 记录过期日志 (action 4) - 在清空数量之前记录
        long expiredCount = _m_lItemCount;
        MJLog.logItemChg(getComp().getUserData(), ENPItemType.BAG_ITEM, getItemId(), 4,
                expiredCount, expiredCount, 0L, _context.getContextId());

        _m_lItemCount = 0;
        _m_lRelativeActivityInstanceId = 0;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("itemCount", _m_lItemCount);
        updateValue.addValueObj("relative_activity_instance_id", _m_lRelativeActivityInstanceId);
        getComp().getUSServer().getBM().getBM(PlayerBagItemBO.class).update("id", _m_lDbid, updateValue);

        //推送协议
        _m_compBagItemComponent.getUserData().sendMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(this));

        // 记录物品日志
        _m_compBagItemComponent.logItem(getItemId(), expiredCount, 0, ELogItem_Type.CONSUME, _context);
    }
}

package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import ALBasicServer.ALBasicMutex.MutexManager;
import Common.RechargeRebateEnum.ERechargeRebateType;
import CommonEnum.ECommonActivityType;
import CommonEnum.ECurrency;
import EventSystem.NPHandlerEntry;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_007_RetRechargeRebateInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPGameEvent;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CURRENCY;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPEvent.EventMgr.EventObj._INPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityBaseBO;
import USDB.Bo.RechargeRebateActivityInfoBO;
import USDB.Bo.RechargeRebatePlayerInfoBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 充值返利活动
 * <p>
 * 包含三种返利类型的综合活动系统：
 * 1. 每日充值返利 - 每日累计VIP点数，零点重置
 * 2. 累计充值返利 - 活动期间累计VIP点数
 * 3. 累天充值返利 - 活动期间累计充值天数
 * <p>
 * 主要功能：
 * - 监听VIP点数变化事件
 * - 管理三种返利类型的数据
 * - 每日零点重置（仅每日充值返利）
 * - 活动结束补发未领取奖励
 */
public class RechargeRebateActivity extends _AActivityBase
{
    private RechargeRebateActivityInfoBO _m_activityBo;

    // 组ID到返利信息的映射（快速查找）
    private List<_ARechargeRebateInfo> _m_rebateInfoList;

    // 线程安全锁
    private MutexManager _m_mutex;

    // VIP点数变化事件监听器
    private List<NPHandlerEntry<_INPGlobalUserEventObj>> _m_entryList;

    public RechargeRebateActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);

        _m_mutex = new MutexManager();
        _m_rebateInfoList = new ArrayList<>();
        _m_entryList = new ArrayList<>();

        // 初始化三种返利类型的管理器
        List<RefRechargeRebateGroup> groupList = RefRechargeRebateGroup.getMgr().getList();
        for (RefRechargeRebateGroup group : groupList)
        {
            ERechargeRebateType type = group.type;

            _ARechargeRebateInfo info;
            switch (type)
            {
                case DAILY:
                    info = new RechargeRebateInfo_Daily(this, group);
                    break;
                case TOTAL:
                    info = new RechargeRebateInfo_Total(this, group);
                    break;
                case DAYS:
                    info = new RechargeRebateInfo_Days(this, group);
                    break;
                default:
                    USLog.error(getUSServer(), "RechargeRebateActivity _subInitStatic unknown type: groupId={}, type={}", group.id, type);
                    continue;
            }

            _m_rebateInfoList.add(info);
        }
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    @Override
    public int getActivityTypeId()
    {
        return ECommonActivityType.RECHARGE_REBATE.ordinal();
    }

    @Override
    protected boolean _subInitNew()
    {
        RechargeRebateActivityInfoBO bo = new RechargeRebateActivityInfoBO();
        bo.setActivityInstanceId(getUSServer().getBM(), getInstanceId());
        bo.setLastCheckDate(getUSServer().getBM(), CommonFunc.getNowTagYYYYMMDD());
        bo.insert(getUSServer().getBM());

        _m_activityBo = bo;

        return true;
    }

    @Override
    protected boolean _subInitStatic()
    {
        // 初始化玩家数据
        List<RechargeRebateActivityInfoBO> mainBoList = getUSServer().getBM().getBM(RechargeRebateActivityInfoBO.class)
                .s_findAll("activity_instance_id", getInstanceId());
        if (mainBoList == null)
        {
            USLog.error(getUSServer(), "RechargeRebateActivity _subInitStatic mainBoList is null");
            return false;
        }
        if (mainBoList.isEmpty())
        {
            RechargeRebateActivityInfoBO bo = new RechargeRebateActivityInfoBO();
            bo.setActivityInstanceId(getUSServer().getBM(), getInstanceId());
            bo.setLastCheckDate(getUSServer().getBM(), CommonFunc.getNowTagYYYYMMDD());
            bo.insert(getUSServer().getBM());

            _m_activityBo = bo;
        }else
        {
            _m_activityBo = mainBoList.get(0);
        }

        // 初始化玩家数据
        List<RechargeRebatePlayerInfoBO> boList = getUSServer().getBM().getBM(RechargeRebatePlayerInfoBO.class)
                .s_findAll("activity_instance_id", getInstanceId());
        if (boList == null)
        {
            USLog.error(getUSServer(), "RechargeRebateActivity _subInitStatic boList is null");
            return false;
        }

        for (RechargeRebatePlayerInfoBO bo : boList)
        {
            _ARechargeRebateInfo info = lookupRebateInfo(bo.getGroupId());
            if (info == null)
            {
                USLog.error(getUSServer(), "RechargeRebateActivity _subInitStatic group not found: groupId={}, cid={}", bo.getGroupId(), bo.getCid());
                continue;
            }

            info.initBo(bo);
        }

        return true;
    }

    @Override
    protected void _regExtraEvent()
    {
        // 监听VIP点数变化事件
        NPHandlerEntry<_INPGlobalUserEventObj> vipExpGainEntry = getUSServer().getGlobalEventHandlerMgr()
                .regHandler(Event_P_GAIN_CURRENCY.ID, this, new HandlerTwo<_ALogicEventBase, _INPGlobalUserEventObj>()
                {
                    @Override
                    public void handle(_ALogicEventBase _event, _INPGlobalUserEventObj _obj)
                    {
                        if (_obj.getUserData() == null)
                            return;

                        if (_event.getContext() == null || _event.getContext().getContextId() != ENPGameEvent.ORDER_DELIVERY.ordinal())
                            return;

                        Event_P_GAIN_CURRENCY evt = _event instanceof Event_P_GAIN_CURRENCY ? ((Event_P_GAIN_CURRENCY) _event) : null;
                        if (evt == null)
                            return;

                        if (evt.get_TYPE() != ECurrency.VIP_EXP.ordinal())
                            return;

                        // 只处理VIP经验增加的情况
                        long addExp = evt.get_NUM();
                        if (addExp <= 0)
                            return;

                        long cid = _obj.getCid();

                        _lock();
                        try{
                            // 通知所有返利类型处理充值事件
                            for (_ARechargeRebateInfo info : _m_rebateInfoList)
                            {
                                if (info != null)
                                {
                                    info.onRecharge(cid, addExp);
                                }
                            }
                        }finally
                        {
                            _unlock();
                        }
                    }
                });


        checkCrossDay();

        getUSServer().getUsUserMgr().OnServerCrossDay.addHandler(this, new HandlerNone()
        {
            @Override
            public void handle()
            {
                checkCrossDay();
            }
        });

        _lock();
        try
        {
            _m_entryList.add(vipExpGainEntry);
        } finally
        {
            _unlock();
        }
    }

    @Override
    protected void _onActivityStart()
    {
        // 活动开始时无需特殊处理
    }

    @Override
    protected void _onActivityEnd()
    {
        // 活动结束时无需特殊处理（补发在_onActivityClosed中）
    }

    @Override
    protected void _onActivityClosed()
    {
        _lock();
        try
        {
            // 注销事件监听器
            for (NPHandlerEntry<_INPGlobalUserEventObj> entry : _m_entryList)
            {
                getUSServer().getGlobalEventHandlerMgr().unregHandler(entry);
            }
            _m_entryList.clear();

            getUSServer().getUsUserMgr().OnServerCrossDay.clear(this);


            // 补发未领取的奖励
            for (_ARechargeRebateInfo info : _m_rebateInfoList)
            {
                if (info == null)
                    continue;

                info.sendUnclaimedRewards();
            }
        } finally
        {
            _unlock();
        }

    }

    @Override
    protected void _discard()
    {
        getUSServer().getBM().getBM(RechargeRebatePlayerInfoBO.class).delAll("activity_instance_id", getInstanceId());
    }

    /**
     * 根据组ID获取返利信息
     */
    public _ARechargeRebateInfo lookupRebateInfo(long _groupId)
    {
        _lock();
        try
        {
            for (_ARechargeRebateInfo rebateInfo : _m_rebateInfoList)
            {
                if (rebateInfo.getGroupId() == _groupId)
                    return rebateInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 领取奖励
     */
    public Result drawReward(NPUSUserData _userdata, long _groupId, long _stepId, NPPlayerContext _context)
    {
        _ARechargeRebateInfo info = lookupRebateInfo(_groupId);
        if (info == null)
        {
            USLog.error(getUSServer(), "RechargeRebateActivity drawReward group not found: groupId={}, cid={}", _groupId, _userdata.getCid());
            return CommErr.REF_NOT_FOUND;
        }

        return info.drawReward(_userdata, _stepId, _context);
    }

    /**
     * 填充协议数据
     */
    public void fillProto(GS2GC_033_007_RetRechargeRebateInfo _proto, long _cid)
    {
        _lock();
        try
        {
            for (_ARechargeRebateInfo info : _m_rebateInfoList)
            {
                if (info != null)
                {
                    info.fillProto(_proto, _cid);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 跨天检查
     */
    public void checkCrossDay()
    {
        _lock();
        try
        {
            int nowTag = CommonFunc.getNowTagYYYYMMDD();
            if (_m_activityBo.getLastCheckDate() >= nowTag)
                return;

            _m_activityBo.saveLastCheckDate(getUSServer().getBM(), nowTag);

            for (_ARechargeRebateInfo rebateInfo : _m_rebateInfoList)
            {
                if (rebateInfo.getType() != ERechargeRebateType.DAILY)
                    continue;

                rebateInfo.onCrossDay();
            }
        } finally
        {
            _unlock();
        }
    }
}

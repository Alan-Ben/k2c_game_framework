package NPUSServer.CommonActivityMgr.Activities.EarningsGoal;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.MailObj.Mail_Data;
import CommonEnum.ECommonActivityType;
import EventSystem.NPHandlerEntry;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_001_RetEarningsGoalInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPGameEvent;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MAX_EARNINGS_CHG;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.HonorReward.EarningsGoalInfo_HonorReward;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.Reward.EarningsGoalInfo_Reward;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPEvent.EventMgr.EventObj._INPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityBaseBO;
import USDB.Bo.EarningsGoalDrawRecordBO;
import USDB.Bo.EarningsGoalJoinRecordBO;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

@SuppressWarnings({"unchecked", "rawtypes"})
public class EarningsGoalActivity extends _AActivityBase
{
    private _AEarningsGoalInfo[] _m_rewardList;
    private Set<Long> _m_joinCidSet;
    private MutexAtom _m_mutex;

    private NPHandlerEntry<_INPGlobalUserEventObj> _m_evtEntry;

    public EarningsGoalActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);

        _m_mutex = new MutexAtom();

        _m_rewardList = new _AEarningsGoalInfo[EEarningsGoalType.EEarningsGoalType_Length];
        _m_rewardList[EEarningsGoalType.REWARD.ordinal()] = new EarningsGoalInfo_Reward(this);
        _m_rewardList[EEarningsGoalType.HONOR_REWARD.ordinal()] = new EarningsGoalInfo_HonorReward(this);

        _m_joinCidSet = new HashSet<>();
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
        return ECommonActivityType.EARNINGS_GOAL.ordinal();
    }

    @Override
    protected boolean _subInitNew()
    {
        return true;
    }

    @Override
    protected boolean _subInitStatic()
    {
        //初始化达成记录
        List<EarningsGoalRecordBO> recordBoList = getUSServer().getBM().getBM(EarningsGoalRecordBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (recordBoList == null)
        {
            USLog.error(getUSServer(), "EarningsGoalActivity _subInitStatic recordBoList is null");
            return false;
        }

        for (EarningsGoalRecordBO bo : recordBoList)
        {
            int type = bo.getType();
            _AEarningsGoalInfo goalInfo = _m_rewardList[type];
            if (goalInfo == null)
            {
                USLog.error(getUSServer(), "EarningsGoalActivity _subInitStatic _AEarningsGoalInfo is null, type:{} ", type);
                continue;
            }

            goalInfo._initBo(bo);
        }

        //初始化领奖记录
        List<EarningsGoalDrawRecordBO> drawRecordBoList = getUSServer().getBM().getBM(EarningsGoalDrawRecordBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (drawRecordBoList == null)
        {
            USLog.error(getUSServer(), "EarningsGoalActivity _subInitStatic drawRecordBoList is null");
            return false;
        }
        for (EarningsGoalDrawRecordBO bo : drawRecordBoList)
        {
            int type = bo.getType();
            _AEarningsGoalInfo goalInfo = _m_rewardList[type];
            if (goalInfo == null)
            {
                USLog.error(getUSServer(), "EarningsGoalActivity _subInitStatic _AEarningsGoalInfo is null, type:{} ", type);
                continue;
            }

            goalInfo._initDrawRecordBo(bo);
        }

        //初始化参加记录
        List<EarningsGoalJoinRecordBO> joinRecordBoList = getUSServer().getBM().getBM(EarningsGoalJoinRecordBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (joinRecordBoList == null)
        {
            USLog.error(getUSServer(), "EarningsGoalActivity _subInitStatic joinRecordBoList is null");
            return false;
        }
        for (EarningsGoalJoinRecordBO bo : joinRecordBoList)
        {
            _m_joinCidSet.add(bo.getCid());
        }

        return true;
    }

    @Override
    protected void _regExtraEvent()
    {
        _m_evtEntry = getUSServer().getGlobalEventHandlerMgr().regHandler(Event_P_MAX_EARNINGS_CHG.ID, this, new HandlerTwo<_ALogicEventBase, _INPGlobalUserEventObj>()
        {
            @Override
            public void handle(_ALogicEventBase _event, _INPGlobalUserEventObj _obj)
            {
                if (_obj.getUserData() == null)
                    return;

                Event_P_MAX_EARNINGS_CHG event = _event instanceof Event_P_MAX_EARNINGS_CHG ? ((Event_P_MAX_EARNINGS_CHG) _event) : null;
                if (event == null)
                    return;

                for (_AEarningsGoalInfo _goal : _m_rewardList)
                {
                    _goal.checkReachNewGoal(_obj.getCid(),_obj.getUserData().toChatPlayerProto(), event.get_EARNINGS());
                }

                _recordJoin(_obj.getCid());
            }
        });
    }

    /**
     * 记录参加活动
     * @param _cid
     */
    private void _recordJoin(long _cid)
    {
        _lock();
        try
        {
            if (_m_joinCidSet.contains(_cid))
                return;

            EarningsGoalJoinRecordBO bo = new EarningsGoalJoinRecordBO();
            bo.setActivityInstanceId(getUSServer().getBM(), getInstanceId());
            bo.setCid(getUSServer().getBM(), _cid);
            bo.insert(getUSServer().getBM());

            _m_joinCidSet.add(_cid);
        } finally
        {
            _unlock();
        }
    }

    @Override
    protected void _onActivityStart()
    {

    }

    @Override
    protected void _onActivityEnd()
    {

    }

    @Override
    protected void _onActivityClosed()
    {
        //注销活动注册事件
        if (_m_evtEntry != null)
        {
            getUSServer().getGlobalEventHandlerMgr().unregHandler(_m_evtEntry);
            _m_evtEntry = null;
        }

        //补发奖励
        ArrayList<Long> joinCidList;

        _lock();
        try
        {
            joinCidList = new ArrayList<>(_m_joinCidSet);
        } finally
        {
            _unlock();
        }

        //遍历参加列表，补发奖励
        for (long cid : joinCidList)
        {
            List<NPCommonCostItem> rewardList = new ArrayList<>();
            for (_AEarningsGoalInfo goalInfo : _m_rewardList)
            {
                goalInfo.fillDispatchRewardList(cid, rewardList);
            }

            if (rewardList.isEmpty())
                continue;

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().earning_goal_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(rewardList));
            MailSystem.addMail(getUSServer(), cid, mailData, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE));
        }
    }

    @Override
    protected void _discard()
    {
        getUSServer().getBM().getBM(EarningsGoalRecordBO.class).delAll("activity_instance_id", getInstanceId());
        getUSServer().getBM().getBM(EarningsGoalDrawRecordBO.class).delAll("activity_instance_id", getInstanceId());
        getUSServer().getBM().getBM(EarningsGoalJoinRecordBO.class).delAll("activity_instance_id", getInstanceId());
    }

    /**
     * 获取奖励信息
     * @param _type
     * @return
     */
    public _AEarningsGoalInfo getRewardInfo(EEarningsGoalType _type)
    {
        return _m_rewardList[_type.ordinal()];
    }

    /**
     * 领取奖励
     * @param _userdata
     * @param _type
     * @param _refId
     * @param _context
     * @return
     */
    public Result drawReward(NPUSUserData _userdata, EEarningsGoalType _type, long _refId, NPPlayerContext _context)
    {
        _AEarningsGoalInfo rewardInfo = getRewardInfo(_type);
        if (rewardInfo == null)
        {
            USLog.error(getUSServer(), "EarningsGoalActivity drawReward _AEarningsGoalInfo is null, type:{} ", _type);
            return CommErr.SYS_ERR;
        }

        return rewardInfo.drawReward(_userdata, _refId, _context);
    }

    /**
     * 填充协议
     * @param _proto
     * @param _cid
     */
    public void fillProto(GS2GC_033_001_RetEarningsGoalInfo _proto, long _cid)
    {
        for (_AEarningsGoalInfo goalInfo : _m_rewardList)
        {
            goalInfo.fillProto(_proto, _cid);
        }
    }
}

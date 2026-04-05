package NPUSServer.CommonActivityMgr.Activities.EarningsGoal;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_001_RetEarningsGoalInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.EarningsGoalDrawRecordBO;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.ArrayList;
import java.util.List;

public abstract class _AEarningsGoalInfo<T extends _AEarningsGoalRecord<R>, R extends RefBase>
{
    protected EarningsGoalActivity _m_activity;
    protected List<T> _m_rewardList;
    protected MutexObject _m_mutex;

    public _AEarningsGoalInfo(EarningsGoalActivity _activity)
    {
        _m_activity = _activity;
        _m_rewardList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    public EarningsGoalActivity getActivity()
    {
        return _m_activity;
    }

    public NPUserServer getUSServer()
    {
        return _m_activity.getUSServer();
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
     * 初始化已领取的cid列表
     * @param _bo
     */
    public void _initDrawRecordBo(EarningsGoalDrawRecordBO _bo)
    {
        T record = lookupGoal(_bo.getRefId());
        if (record == null)
        {
            USLog.error(getUSServer(), "EarningsGoalActivity _initDrawRecordBo _AEarningsGoalInfo is null, refId:{} ", _bo.getRefId());
            return;
        }

        record._initDrawRecordBo(_bo);
    }

    /**
     * 查询达成信息
     * @param _refId
     * @return
     */
    public T lookupGoal(long _refId)
    {
        _lock();
        try
        {
            for (T rewardInfo : _m_rewardList)
            {
                if (rewardInfo.getRefId() == _refId)
                    return rewardInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造数据对象
     * @param _cid
     * @param refId
     * @return
     */
    protected EarningsGoalRecordBO makeBo(long _cid, long refId)
    {
        EarningsGoalRecordBO bo = new EarningsGoalRecordBO();
        bo.setActivityInstanceId(getUSServer().getBM(), getActivity().getInstanceId());
        bo.setType(getUSServer().getBM(), getGoalType().ordinal());
        bo.setRefId(getUSServer().getBM(), refId);
        bo.setFirstReachCid(getUSServer().getBM(), _cid);
        bo.setTimestamp(getUSServer().getBM(), CommonFunc.getNowTimeMS());
        bo.insert(getUSServer().getBM());
        return bo;
    }

    /**
     * 领取奖励
     * @param _userdata
     * @param _refId
     * @return
     */
    public Result drawReward(NPUSUserData _userdata, long _refId, NPPlayerContext _context)
    {
        T goalInfo = lookupGoal(_refId);
        if (goalInfo == null)
            return PlayerErr.EARNING_GOAL_REWARD_NOT_MEET_REQUIRE;

        return goalInfo.drawReward(_userdata, _context);
    }

    /**
     * 填充奖励列表
     * @param _cid
     * @param _rewardList
     */
    public void fillDispatchRewardList(long _cid, List<NPCommonCostItem> _rewardList)
    {
        for (T reward : _m_rewardList)
        {
            if (reward.hadDraw(_cid))
                continue;

            List<NPCommonCostItem> rewardList = reward.getRewardList(_cid);
            if (rewardList == null || rewardList.isEmpty())
                continue;

            _rewardList.addAll(rewardList);
        }
    }

    /**
     * 获取类型
     * @return
     */
    public abstract EEarningsGoalType getGoalType();

    /**
     * 初始化数据
     * @param _bo
     */
    public abstract void _initBo(EarningsGoalRecordBO _bo);

    /**
     * 检查是否达成新的目标
     * @param _cid
     * @param _chatPlayerProto
     * @param _earnings
     */
    public abstract void checkReachNewGoal(long _cid, NPCommon_ChatPlayerContent _chatPlayerProto, long _earnings);

    /**
     * 填充协议
     * @param _proto
     * @param _cid
     */
    public abstract void fillProto(GS2GC_033_001_RetEarningsGoalInfo _proto, long _cid);
}

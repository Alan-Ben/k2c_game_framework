package NPUSServer.CommonActivityMgr.Activities.EarningsGoal;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.RefData.Ref.RefBase;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.EarningsGoalDrawRecordBO;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.ArrayList;
import java.util.List;

public abstract class _AEarningsGoalRecord<R extends RefBase>
{
    private _AEarningsGoalInfo _m_goalInfo;
    private R _m_ref;
    private EarningsGoalRecordBO _m_bo;
    private List<Long> _m_hadDrawCidList;
    private MutexAtom _m_mutex;

    public _AEarningsGoalRecord(_AEarningsGoalInfo _goalInfo, R _ref, EarningsGoalRecordBO _bo)
    {
        _m_goalInfo = _goalInfo;
        _m_ref = _ref;
        _m_bo = _bo;
        _m_hadDrawCidList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    public _AEarningsGoalInfo getGoalInfo()
    {
        return _m_goalInfo;
    }

    public EarningsGoalRecordBO getBo()
    {
        return _m_bo;
    }

    public R getRef()
    {
        return _m_ref;
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }

    public long getFirstReachCid()
    {
        return _m_bo.getFirstReachCid();
    }

    public long getTimestamp()
    {
        return _m_bo.getTimestamp();
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
     * 初始化已经领取的cid列表
     * @param _bo
     */
    public void _initDrawRecordBo(EarningsGoalDrawRecordBO _bo)
    {
        _m_hadDrawCidList.add(_bo.getCid());
    }

    /**
     * 是否已领取
     * @param _cid
     * @return
     */
    public boolean hadDraw(long _cid)
    {
        _lock();
        try
        {
            return _m_hadDrawCidList.contains(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 领取奖励
     * @return
     */
    public Result drawReward(NPUSUserData _userData, NPPlayerContext _context)
    {
        List<NPCommonCostItem> rewardList;

        _lock();
        try
        {
            //检查是否已领取
            if (hadDraw(_userData.getCid()))
                return PlayerErr.EARNING_GOAL_REWARD_HAD_DRAW;

            //查询奖励
            rewardList = getRewardList(_userData.getCid());
            if (rewardList == null)
                return PlayerErr.EARNING_GOAL_REWARD_NOT_MEET_REQUIRE;

            //标记已领取
            markHadDraw(_userData);
        } finally
        {
            _unlock();
        }

        _userData.gainItemList(rewardList, _context);

        return Result.SUCC;
    }

    /**
     * 标记已领取
     */
    public void markHadDraw(NPUSUserData _userdata)
    {
        _lock();
        try
        {
            EarningsGoalDrawRecordBO bo = new EarningsGoalDrawRecordBO();
            bo.setActivityInstanceId(_m_goalInfo.getUSServer().getBM(), _m_goalInfo.getActivity().getInstanceId());
            bo.setType(_m_goalInfo.getUSServer().getBM(), _m_goalInfo.getGoalType().ordinal());
            bo.setRefId(_m_goalInfo.getUSServer().getBM(), getRefId());
            bo.setCid(_m_goalInfo.getUSServer().getBM(), _userdata.getCid());
            bo.insert(_m_goalInfo.getUSServer().getBM());

            _m_hadDrawCidList.add(_userdata.getCid());

            _onSuccDraw(_userdata);
        } finally
        {
            _unlock();
        }
    }

    protected abstract void _onSuccDraw(NPUSUserData _userdata);

    /**
     * 获取奖励列表
     * @param _cid
     * @return
     */
    public abstract List<NPCommonCostItem> getRewardList(long _cid);
}

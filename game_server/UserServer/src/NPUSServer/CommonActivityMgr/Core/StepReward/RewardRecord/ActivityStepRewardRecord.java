package NPUSServer.CommonActivityMgr.Core.StepReward.RewardRecord;

import NPCommon.DB.BM.BM;
import USDB.Bo.ActivityStepRewardDrawRecordBO;
import USDB.Bo.ActivityStepRewardMailRecordBO;

import java.util.ArrayList;
import java.util.List;

public class ActivityStepRewardRecord
{
    private ActivityStepRewardRecordList _m_mgr;
    private long _m_cid;
    private List<Integer> _m_hasDrawStepList;
    private boolean _m_hadSendMail;

    public ActivityStepRewardRecord(ActivityStepRewardRecordList _mgr, long _cid)
    {
        _m_mgr = _mgr;
        _m_cid = _cid;
        _m_hasDrawStepList = new ArrayList<>();
        _m_hadSendMail = false;
    }

    /**
     * 是否已发送邮件
     * @return 是否已发送邮件
     */
    public boolean hadSendMail()
    {
        return _m_hadSendMail;
    }

    /**
     * 初始化已领取记录
     * @param _stepId 步骤id
     */
    protected void _initAddRecord(int _stepId)
    {
        _m_hasDrawStepList.add(_stepId);
    }

    /**
     * 初始化标记已发送邮件
     */
    protected void _initSetHadSendMail()
    {
        _m_hadSendMail = true;
    }

    /**
     * 是否已领取过奖励
     * @param _step 阶段
     * @return 是否已领取过奖励
     */
    public boolean hadDrawReward(int _step)
    {
        return _m_hadSendMail || _m_hasDrawStepList.contains(_step);
    }

    /**
     * 添加领取记录
     * @param _step
     * @return
     */
    public boolean addDrawRecord(int _step)
    {
        //检查是否已领取过奖励
        if (hadDrawReward(_step))
            return false;

        BM bmObj = _m_mgr.getStepRewardInfo().getActivity().getUSServer().getBM();

        ActivityStepRewardDrawRecordBO bo = new ActivityStepRewardDrawRecordBO();
        bo.setActivityInstanceId(bmObj, _m_mgr.getActivityInstanceId());
        bo.setStepRewardId(bmObj, _m_mgr.getStepRewardId());
        bo.setCid(bmObj, _m_cid);
        bo.setStep(bmObj, _step);
        bo.insert(bmObj);

        _m_hasDrawStepList.add(_step);
        return true;
    }

    /**
     * 标记为已经发送邮件
     */
    public boolean markHadSendMail()
    {
        //检查是否已补发过奖励
        if (_m_hadSendMail)
            return false;

        BM bmObj = _m_mgr.getStepRewardInfo().getActivity().getUSServer().getBM();

        ActivityStepRewardMailRecordBO bo = new ActivityStepRewardMailRecordBO();
        bo.setActivityInstanceId(bmObj, _m_mgr.getActivityInstanceId());
        bo.setStepRewardId(bmObj, _m_mgr.getStepRewardId());
        bo.setCid(bmObj, _m_cid);
        bo.insert(bmObj);

        _m_hadSendMail = true;
        return true;
    }

    public List<Integer> getHadDrawList()
    {
        return new ArrayList<>(_m_hasDrawStepList);
    }
}

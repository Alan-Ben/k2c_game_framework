package NPUSServer.CommonActivityMgr.Activities.EarningsGoal.HonorReward;

import Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_106_OnEarningsGoalHonorRewardDraw;
import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.EarningGoal.RefEarningGoalHonorReward;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalRecord;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.List;

public class EarningsGoalRecord_HonorReward extends _AEarningsGoalRecord<RefEarningGoalHonorReward>
{
    public EarningsGoalRecord_HonorReward(EarningsGoalInfo_HonorReward _goalInfo, RefEarningGoalHonorReward _ref, EarningsGoalRecordBO _bo)
    {
        super(_goalInfo, _ref, _bo);
    }

    @Override
    protected void _onSuccDraw(NPUSUserData _userdata)
    {
        _userdata.sendMsgToGC(new GS2GC_033_106_OnEarningsGoalHonorRewardDraw(getRefId(), getGoalInfo().getActivity().getInstanceId()));
    }

    @Override
    public List<NPCommonCostItem> getRewardList(long _cid)
    {
        //判断是否是首达本人，给首达奖励
        if (_cid != getFirstReachCid())
            return null;

        return getRef().first_gain_item_list;
    }

    /**
     * 构造协议
     * @return
     */
    public EarningsGoal_HonorRewardInfo makeProto()
    {
        EarningsGoal_HonorRewardInfo proto = new EarningsGoal_HonorRewardInfo();
        proto.setRefId(getRefId());
        proto.setFirstReachCid(getFirstReachCid());
        proto.setTimestamp(getTimestamp());
        return proto;
    }
}

package NPUSServer.CommonActivityMgr.Activities.EarningsGoal.Reward;

import Common.EarningsGoalObj.EarningsGoal_RewardInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_105_OnEarningsGoalRewardDraw;
import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.EarningGoal.RefEarningGoalReward;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalRecord;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.ArrayList;
import java.util.List;

public class EarningsGoalRecord_Reward extends _AEarningsGoalRecord<RefEarningGoalReward>
{
    public EarningsGoalRecord_Reward(EarningsGoalInfo_Reward _goalInfo, RefEarningGoalReward _ref, EarningsGoalRecordBO _bo)
    {
        super(_goalInfo, _ref, _bo);
    }

    @Override
    protected void _onSuccDraw(NPUSUserData _userdata)
    {
        _userdata.sendMsgToGC(new GS2GC_033_105_OnEarningsGoalRewardDraw(getRefId(), getGoalInfo().getActivity().getInstanceId()));
    }

    @Override
    public List<NPCommonCostItem> getRewardList(long _cid)
    {
        //全民奖励
        List<NPCommonCostItem> rewardList = new ArrayList<>(getRef().all_gain_item_list);
        //判断是否是首达本人，给首达奖励
        if (_cid == getFirstReachCid())
            rewardList.addAll(getRef().first_gain_item_list);
        return rewardList;
    }

    /**
     * 构造协议
     * @return
     */
    public EarningsGoal_RewardInfo makeProto()
    {
        EarningsGoal_RewardInfo proto = new EarningsGoal_RewardInfo();
        proto.setRefId(getRefId());
        proto.setFirstReachCid(getFirstReachCid());
        proto.setTimestamp(getTimestamp());
        return proto;
    }
}

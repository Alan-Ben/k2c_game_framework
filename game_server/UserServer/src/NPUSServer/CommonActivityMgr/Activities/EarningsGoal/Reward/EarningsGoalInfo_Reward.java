package NPUSServer.CommonActivityMgr.Activities.EarningsGoal.Reward;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_001_RetEarningsGoalInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_101_OnEarningsGoalRewardReach;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.EarningGoal.RefEarningGoalReward;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EEarningsGoalType;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalInfo;
import NPUSServer.USLog;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.List;

public class EarningsGoalInfo_Reward extends _AEarningsGoalInfo<EarningsGoalRecord_Reward, RefEarningGoalReward>
{
    public EarningsGoalInfo_Reward(EarningsGoalActivity _activity)
    {
        super(_activity);
    }

    @Override
    public EEarningsGoalType getGoalType()
    {
        return EEarningsGoalType.REWARD;
    }

    @Override
    public void _initBo(EarningsGoalRecordBO _bo)
    {
        RefEarningGoalReward ref = RefEarningGoalReward.getMgr().get(_bo.getRefId());
        if (ref == null)
        {
            USLog.error(getActivity().getUSServer(), "EarningsGoalInfo_Reward _initBo ref is null, refId:{}", _bo.getRefId());
            return;
        }

        EarningsGoalRecord_Reward record = new EarningsGoalRecord_Reward(this, ref, _bo);
        _m_rewardList.add(record);
    }

    @Override
    public void checkReachNewGoal(long _cid, NPCommon_ChatPlayerContent _chatPlayerProto, long _earnings)
    {
        List<RefEarningGoalReward> list = RefEarningGoalReward.getMgr().getList();
        for (RefEarningGoalReward ref : list)
        {
            EarningsGoalRecord_Reward record;

            _lock();
            try
            {
                // 查询记录
                EarningsGoalRecord_Reward goal = lookupGoal(ref.Id());

                // 检查收益是否达到目标
                long refId = ref.Id();
                if (ref.earning_goal > _earnings)
                    continue;

                // 根据记录状态决定操作
                if (goal == null)
                {
                    // 记录不存在 - 创建新记录
                    record = new EarningsGoalRecord_Reward(this, ref, makeBo(_cid, refId));
                    _m_rewardList.add(record);
                } else if (goal.getFirstReachCid() == 0)
                {
                    // 记录存在但first_reach_cid=0 - 更新记录
                    EarningsGoalRecordBO bo = goal.getBo();
                    if (bo != null)
                    {
                        bo.setFirstReachCid(getUSServer().getBM(), _cid);
                        bo.setTimestamp(getUSServer().getBM(), CommonFunc.getNowTimeMS());
                        bo.saveAllMarked(getUSServer().getBM());

                        record = goal;
                    } else
                    {
                        continue;
                    }
                } else
                {
                    // 记录存在且first_reach_cid != 0 - 已有首达玩家，跳过
                    continue;
                }
            } finally
            {
                _unlock();
            }

            // 如果需要广播，异步发送通知
            ALSynTaskManager.getInstance().regTask(() -> getUSServer().getUsUserMgr().broadCastMessage(
                    new GS2GC_033_101_OnEarningsGoalRewardReach(record.makeProto(), getActivity().getInstanceId())));
        }
    }

    /**
     * 填充协议
     * @param _proto
     * @param _cid
     */
    public void fillProto(GS2GC_033_001_RetEarningsGoalInfo _proto, long _cid)
    {
        _lock();
        try
        {
            for (EarningsGoalRecord_Reward record : _m_rewardList)
            {
                _proto.addRewardInfoList(record.makeProto());

                if (record.hadDraw(_cid))
                    _proto.addHadDrawRewardList(record.getRefId());
            }
        } finally
        {
            _unlock();
        }
    }

}

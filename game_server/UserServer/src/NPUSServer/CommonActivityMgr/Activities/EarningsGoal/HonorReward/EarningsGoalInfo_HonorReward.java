package NPUSServer.CommonActivityMgr.Activities.EarningsGoal.HonorReward;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpChatObj.ChatObj_SystemLog;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_001_RetEarningsGoalInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_102_OnEarningsGoalHonorRewardReach;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPChatMsgType;
import NPGameRes.Refs.EarningGoal.RefEarningGoalHonorReward;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EEarningsGoalType;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal._AEarningsGoalInfo;
import NPUSServer.USLog;
import USDB.Bo.EarningsGoalRecordBO;

import java.util.List;

public class EarningsGoalInfo_HonorReward extends _AEarningsGoalInfo<EarningsGoalRecord_HonorReward, RefEarningGoalHonorReward>
{
    public EarningsGoalInfo_HonorReward(EarningsGoalActivity _activity)
    {
        super(_activity);
    }

    @Override
    public EEarningsGoalType getGoalType()
    {
        return EEarningsGoalType.HONOR_REWARD;
    }

    @Override
    public void _initBo(EarningsGoalRecordBO _bo)
    {
        RefEarningGoalHonorReward ref = RefEarningGoalHonorReward.getMgr().get(_bo.getRefId());
        if (ref == null)
        {
            USLog.error(getActivity().getUSServer(), "EarningsGoalInfo_HonorReward _initBo ref is null, refId:{}", _bo.getRefId());
            return;
        }

        EarningsGoalRecord_HonorReward record = new EarningsGoalRecord_HonorReward(this, ref, _bo);
        _m_rewardList.add(record);
    }

    @Override
    public void checkReachNewGoal(long _cid, NPCommon_ChatPlayerContent _chatPlayerProto, long _earnings)
    {
        List<RefEarningGoalHonorReward> list = RefEarningGoalHonorReward.getMgr().getList();
        for (RefEarningGoalHonorReward ref : list)
        {
            EarningsGoalRecord_HonorReward record;

            _lock();
            try
            {
                // 查询记录
                EarningsGoalRecord_HonorReward goal = lookupGoal(ref.Id());

                // 检查收益是否达到目标
                long refId = ref.Id();
                if (ref.earning_goal > _earnings)
                    continue;

                // 根据记录状态决定操作
                if (goal == null)
                {
                    // 记录不存在 - 创建新记录
                    record = new EarningsGoalRecord_HonorReward(this, ref, makeBo(_cid, refId));
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
                    }else
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

            // 异步发送协议通知
            ALSynTaskManager.getInstance().regTask(() -> getUSServer().getUsUserMgr().broadCastMessage(
                    new GS2GC_033_102_OnEarningsGoalHonorRewardReach(record.makeProto(), getActivity().getInstanceId())));

            if (ref.system_log_id != 0)
            {
                //聊天内容数据
                ChatObj_SystemLog chatProto = new ChatObj_SystemLog();
                chatProto.setLogType(ref.system_log_id);
                ALSynTaskManager.getInstance().regTask(() ->
                        ChatRoomApi.sendUsRoomSysMsg(getUSServer(), ENPChatMsgType.SYSTEM_LOG, _chatPlayerProto.makePackage(), chatProto.makePackage(), null)
                );
            }
        }
    }

    @Override
    public void fillProto(GS2GC_033_001_RetEarningsGoalInfo _proto, long _cid)
    {
        _lock();
        try
        {
            for (EarningsGoalRecord_HonorReward record : _m_rewardList)
            {
                _proto.addHonorRewardInfoList(record.makeProto());

                if (record.hadDraw(_cid))
                    _proto.addHadDrawHonorRewardList(record.getRefId());
            }
        } finally
        {
            _unlock();
        }
    }


}

package ActivitiesV02.MsgDealers.p202_NumMergeOp;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Activities.RegularActivity.Player.NumMergePlayerInfo;
import ActivitiesV02.Err.NumMergeErr;
import ActivitiesV02.MsgDealers.US2GCWriter_202_NumMergeOp;
import CommonEnum.ECommonActivityType;
import Hotfix.V02.GC2GS.p202_NumMergeOp.GC2GS_202_001_ReqNumMergeInit;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 数字合并-初始化活动数据消息处理器
 *
 * 功能：
 * 1. 验证活动实例是否存在
 * 2. 检查活动运行状态
 * 3. 返回完整的游戏状态信息
 */
public class MsgDealer_GC2GS_202_001_ReqNumMergeInit extends NPUserMsgDealer<GC2GS_202_001_ReqNumMergeInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_202_001_ReqNumMergeInit _msg)
    {
        // 获取玩家数据
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        // 查找数字合并活动实例
        NumMergeActivity activity = getUSServer().getCommActivityMgr()
                .lookupOneActivityByType(ECommonActivityType.NUM_MERGE, NumMergeActivity.class);
        if (activity == null)
        {
            _committer.commitFailRes(NumMergeErr.NUM_MERGE_ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 检查活动状态
        if (!activity.isRunning())
        {
            _committer.commitFailRes(NumMergeErr.NUM_MERGE_ACTIVITY_NOT_RUNNING.getCode());
            return;
        }

        // 获取玩家游戏信息
        NumMergePlayerInfo playerInfo = activity.getPlayerMgr().ensurePlayerInfo(userData);
        if (playerInfo == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_202_NumMergeOp.make_001_RetNumMergeInit(playerInfo.buildGameInfo()));
    }
}
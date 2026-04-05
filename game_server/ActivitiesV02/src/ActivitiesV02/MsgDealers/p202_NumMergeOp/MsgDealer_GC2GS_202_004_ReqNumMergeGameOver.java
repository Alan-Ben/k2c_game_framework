package ActivitiesV02.MsgDealers.p202_NumMergeOp;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Activities.RegularActivity.Player.NumMergePlayerInfo;
import ActivitiesV02.Err.NumMergeErr;
import ActivitiesV02.MsgDealers.US2GCWriter_202_NumMergeOp;
import CommonEnum.ECommonActivityType;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_GameEvent;
import Hotfix.V02.GC2GS.p202_NumMergeOp.GC2GS_202_004_ReqNumMergeGameOver;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 数字合并-游戏结束/重新开始消息处理器
 *
 * 功能：
 * 1. 验证活动和玩家状态
 * 2. 重置游戏棋盘
 * 3. 返回成功响应
 */
public class MsgDealer_GC2GS_202_004_ReqNumMergeGameOver extends NPUserMsgDealer<GC2GS_202_004_ReqNumMergeGameOver>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_202_004_ReqNumMergeGameOver _msg)
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

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENumMerge_GameEvent.NUM_MERGE_GAME_OVER.value());

        // 重置游戏棋盘
        Result result = playerInfo.resetBoard(userData, false, context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_202_NumMergeOp.make_004_RetNumMergeGameOver());
    }
}

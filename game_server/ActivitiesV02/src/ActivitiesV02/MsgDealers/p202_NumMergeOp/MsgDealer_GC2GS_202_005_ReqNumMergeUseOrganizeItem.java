package ActivitiesV02.MsgDealers.p202_NumMergeOp;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Activities.RegularActivity.Player.NumMergePlayerInfo;
import ActivitiesV02.Err.NumMergeErr;
import ActivitiesV02.MsgDealers.US2GCWriter_202_NumMergeOp;
import ActivitiesV02.Refs.NumMerge.RefNumMergeOther;
import CommonEnum.ECommonActivityType;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_GameEvent;
import Hotfix.V02.GC2GS.p202_NumMergeOp.GC2GS_202_005_ReqNumMergeUseOrganizeItem;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 数字合并-使用重排道具消息处理器
 *
 * 功能：
 * 1. 验证活动和玩家状态
 * 2. 检查玩家是否有重排道具
 * 3. 消耗道具并重新排列棋盘
 * 4. 返回更新后的棋盘数据
 */
public class MsgDealer_GC2GS_202_005_ReqNumMergeUseOrganizeItem extends NPUserMsgDealer<GC2GS_202_005_ReqNumMergeUseOrganizeItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_202_005_ReqNumMergeUseOrganizeItem _msg)
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
        NPPlayerContext context = NPPlayerContext.createNew(ENumMerge_GameEvent.NUM_MERGE_USE_ORGANIZE_ITEM.value());

        // 检查道具数量
        if (!userData.spendItem(RefNumMergeOther.Ref().num_merge_organize_item, 1, context))
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        // 执行使用重排道具操作
        Result result = playerInfo.useOrganizeItem(userData, context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_202_NumMergeOp.make_005_RetNumMergeUseOrganizeItem());
    }
}

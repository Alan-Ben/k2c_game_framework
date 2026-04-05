package ActivitiesV01.MsgDealers.p201_TileMatchOp;

import ActivitiesV01.Activities.TileMatchActivity.TileMatchActivity;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchPlayerInfo;
import ActivitiesV01.MsgDealers.US2GCWriter_201_TileMatchOp;
import CommonEnum.ECommonActivityType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_GameEvent;
import Hotfix.V01.GC2GS.p201_TileMatchOp.GC2GS_201_005_ReqTileMatchDrawStepReward;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_201_005_ReqTileMatchDrawStepReward extends NPUserMsgDealer<GC2GS_201_005_ReqTileMatchDrawStepReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_201_005_ReqTileMatchDrawStepReward _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        TileMatchActivity activity = getUSServer().getCommActivityMgr().lookupOneActivityByType(ECommonActivityType.TILE_MATCH, TileMatchActivity.class);
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //检查活动状态
        if (!activity.isPlaying())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        TileMatchPlayerInfo playerInfo = activity.getPlayerMgr().ensure(_committer.getUserData().getCid());

        NPPlayerContext context = NPPlayerContext.createNew(ETileMatch_GameEvent.TILE_MATCH_DRAW_STEP_REWARD.value());

        Result result = playerInfo.drawStepReward(userData, context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto());

        _committer.commitSucRes(US2GCWriter_201_TileMatchOp.make_005_RetTileMatchDrawStepReward());
    }
}

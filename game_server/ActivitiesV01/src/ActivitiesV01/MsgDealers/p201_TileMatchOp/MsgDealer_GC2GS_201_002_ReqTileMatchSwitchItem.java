package ActivitiesV01.MsgDealers.p201_TileMatchOp;

import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchGameLogicContext;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchPlayerGameInfo;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchActivity;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchPlayerInfo;
import ActivitiesV01.MsgDealers.US2GCWriter_201_TileMatchOp;
import CommonEnum.ECommonActivityType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_GameEvent;
import Hotfix.V01.GC2GS.p201_TileMatchOp.GC2GS_201_002_ReqTileMatchSwitchItem;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_201_002_ReqTileMatchSwitchItem extends NPUserMsgDealer<GC2GS_201_002_ReqTileMatchSwitchItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_201_002_ReqTileMatchSwitchItem _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ETileMatch_GameEvent.TILE_MATCH_SWITCH.value());

        //获取游戏信息
        ResultOne<TileMatchPlayerGameInfo> gameInfoResult =
                playerInfo.ensureGame(_committer.getUserData(), _msg.getModeType(), context);
        if (!gameInfoResult.isSucc())
        {
            _committer.commitFailRes(gameInfoResult.getCode());
            return;
        }

        TileMatchPlayerGameInfo gameInfo = gameInfoResult.getData();

        //执行逻辑
        ResultOne<TileMatchGameLogicContext> runResult = gameInfo.switchBlock(userData, _msg.getStartIndex(), _msg.getEndIndex(), context);
        if (!runResult.isSucc())
        {
            _committer.commitFailRes(runResult.getCode());
            return;
        }

        //推送逻辑结果
        runResult.getData().pushLogicResult(userData, _msg.getModeType());

        _committer.commitSucRes(US2GCWriter_201_TileMatchOp.make_002_RetTileMatchSwitch());
    }
}

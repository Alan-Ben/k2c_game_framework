package ActivitiesV01.MsgDealers.p201_TileMatchOp;

import ActivitiesV01.Activities.TileMatchActivity.TileMatchActivity;
import ActivitiesV01.Activities.TileMatchActivity.TileMatchPlayerInfo;
import ActivitiesV01.MsgDealers.US2GCWriter_201_TileMatchOp;
import CommonEnum.ECommonActivityType;
import Hotfix.V01.GC2GS.p201_TileMatchOp.GC2GS_201_004_ReqTileMatchInit;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_201_004_ReqTileMatchInit extends NPUserMsgDealer<GC2GS_201_004_ReqTileMatchInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_201_004_ReqTileMatchInit _msg)
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

        _committer.commitSucRes(US2GCWriter_201_TileMatchOp.make_004_RetTileMatchInit(playerInfo.makeInfo()));
    }
}

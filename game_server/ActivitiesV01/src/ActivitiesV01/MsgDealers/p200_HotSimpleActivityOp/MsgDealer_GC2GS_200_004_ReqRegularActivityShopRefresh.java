package ActivitiesV01.MsgDealers.p200_HotSimpleActivityOp;

import ActivitiesV01.Activities.RegularActivity.RegularActivity;
import ActivitiesV01.MsgDealers.US2GCWriter_200_HotSimpleActivityOp;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_004_ReqRegularActivityShopRefresh;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_200_004_ReqRegularActivityShopRefresh extends NPUserMsgDealer<GC2GS_200_004_ReqRegularActivityShopRefresh>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_200_004_ReqRegularActivityShopRefresh _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        RegularActivity activity = getUSServer().getCommActivityMgr().lookupActivity(_msg.getActivityInstanceId(), RegularActivity.class);
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //检查活动状态
        if (!activity.isRunning())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        Result result = activity.getPlayerMgr().refreshShop(userData);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_200_HotSimpleActivityOp.make_004_RetRegularActivityShopRefresh());
    }
}
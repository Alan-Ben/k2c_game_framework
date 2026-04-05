package ActivitiesV01.MsgDealers.p200_HotSimpleActivityOp;

import ActivitiesV01.Activities.RegularActivity.RegularActivity;
import ActivitiesV01.MsgDealers.US2GCWriter_200_HotSimpleActivityOp;
import Hotfix.V01.Enum.RegularActivityEnum.ERegularActivity_GameEvent;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_003_ReqRegularActivityUseItem;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_200_003_ReqRegularActivityUseItem extends NPUserMsgDealer<GC2GS_200_003_ReqRegularActivityUseItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_200_003_ReqRegularActivityUseItem _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ERegularActivity_GameEvent.REGULAR_USE_ITEM.value());
        Result result = activity.getPlayerMgr().useItem(userData, _msg.getItemId(), _msg.getIsTen(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_200_HotSimpleActivityOp.make_003_RetRegularActivityUseItem(context));
    }
}
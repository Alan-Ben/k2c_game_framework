package ActivitiesV01.MsgDealers.p200_HotSimpleActivityOp;

import ActivitiesV01.Activities.RegularActivity.RegularActivity;
import ActivitiesV01.MsgDealers.US2GCWriter_200_HotSimpleActivityOp;
import Hotfix.V01.Enum.RegularActivityEnum.ERegularActivity_GameEvent;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_002_ReqRegularActivityShopBuyItem;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_200_002_ReqRegularActivityShopBuyItem extends NPUserMsgDealer<GC2GS_200_002_ReqRegularActivityShopBuyItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_200_002_ReqRegularActivityShopBuyItem _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ERegularActivity_GameEvent.REGULAR_SHOP_BUY_ITEM.value());
        Result result = activity.getPlayerMgr().buyItem(userData, _msg.getItemId(), _msg.getNum(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        _committer.commitSucRes(US2GCWriter_200_HotSimpleActivityOp.make_002_RetRegularActivityShopBuyItem());
    }
}
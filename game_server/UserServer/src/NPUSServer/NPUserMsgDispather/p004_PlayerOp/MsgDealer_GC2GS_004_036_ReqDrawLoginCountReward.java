package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_036_ReqDrawLoginCountReward;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.RefSevenDayLogin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_036_ReqDrawLoginCountReward extends NPUserMsgDealer<GC2GS_004_036_ReqDrawLoginCountReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_036_ReqDrawLoginCountReward _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        RefSevenDayLogin refSevenDayLogin = RefSevenDayLogin.getMgr().get(_msg.getDay());
        if (refSevenDayLogin == null)
        {
            _commiter.commitFailRes(PlayerErr.SEVEN_DAYS_LOGIN_REWARD_NOT_FOUND.getCode());
            return;
        }

        //检查是否达到要求天数
        long loginDay = userData.getParam(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT);
        if (loginDay < _msg.getDay())
        {
            _commiter.commitFailRes(PlayerErr.SEVEN_DAYS_LOGIN_NOT_REACH.getCode());
            return;
        }

        boolean canDraw = userData.getSevenLoginComponent().addHadDrawDay(_msg.getDay());
        if (!canDraw)
        {
            _commiter.commitFailRes(PlayerErr.SEVEN_DAYS_LOGIN_REWARD_HAD_DRAW.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_SEVEN_DAYS_LOGIN_REWARD);

        userData.gainItemList(refSevenDayLogin.reward_item_list, context);

        userData.sendMsgToGC(context.getCollector().toProto());
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_036_RetDrawLoginCountReward());
    }
}

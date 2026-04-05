package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_017_ReqWeekCardActiveFreeTrial;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_017_ReqWeekCardActiveFreeTrial extends NPUserMsgDealer<GC2GS_004_017_ReqWeekCardActiveFreeTrial>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_017_ReqWeekCardActiveFreeTrial _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVE_WEEK_CARD_FREE_TRIAL);

        //激活周卡
        Result result = userData.getWeekCardComponent().activeFreeTrial(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

       //回包处理
       _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_017_RetWeekCardActiveFreeTrial());
    }
}

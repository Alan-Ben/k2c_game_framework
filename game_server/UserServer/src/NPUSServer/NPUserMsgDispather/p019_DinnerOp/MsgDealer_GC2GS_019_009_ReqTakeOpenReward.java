package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;
import Common.DinnerObj.Dinner_ResultInfo;
import GC2GS.p019_DinnerOp.GC2GS_019_009_ReqTakeOpenReward;
import NPCommon.ErrMain.DinnerErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_009_ReqTakeOpenReward extends NPUserMsgDealer<GC2GS_019_009_ReqTakeOpenReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_009_ReqTakeOpenReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        if(!userData.getDinnerComponent().hasOwnerReward())
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_OWNER_REWARD_NOT_FOUND.getCode());
        	return;
        }
        
        //获取开宴结算奖励
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_TAKE_OWNER_RESULT);
        Dinner_ResultInfo result = userData.getDinnerComponent().takeOwnerReward(context);
        if(null == result)
        {
        	_commiter.commitFailRes(DinnerErr.DINNER_OWNER_REWARD_FAIL.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_009_RetTakeOpenReward(result));
    }
}
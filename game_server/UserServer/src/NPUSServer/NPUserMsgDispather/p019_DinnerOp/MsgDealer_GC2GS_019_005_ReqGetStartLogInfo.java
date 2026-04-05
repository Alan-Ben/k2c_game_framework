package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;
import Common.DinnerObj.Dinner_StartLogInfo;
import GC2GS.p019_DinnerOp.GC2GS_019_005_ReqGetStartLogInfo;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_005_ReqGetStartLogInfo extends NPUserMsgDealer<GC2GS_019_005_ReqGetStartLogInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_005_ReqGetStartLogInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        userData.getDinnerComponent().getStartLogList().getLog(_msg.getInstanceId(), new HandlerTwo<Result, Dinner_StartLogInfo>() 
        {	
			@Override
			public void handle(Result _res, Dinner_StartLogInfo _info) 
			{
				if(!_res.isSucc())
				{
					_commiter.commitFailRes(_res.getCode());
				}
				else
				{
					_commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_005_RetGetStartLogInfo(_info));
				}
			}
		});
    }
}
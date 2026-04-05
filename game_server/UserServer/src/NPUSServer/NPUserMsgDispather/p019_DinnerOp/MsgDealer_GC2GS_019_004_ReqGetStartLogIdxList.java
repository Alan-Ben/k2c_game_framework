package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;

import Common.DinnerObj.Dinner_StartLogIdx;
import GC2GS.p019_DinnerOp.GC2GS_019_004_ReqGetStartLogIdxList;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;

import java.util.List;
public class  MsgDealer_GC2GS_019_004_ReqGetStartLogIdxList extends NPUserMsgDealer<GC2GS_019_004_ReqGetStartLogIdxList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_004_ReqGetStartLogIdxList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        userData.getDinnerComponent().getStartLogList().makeStartLogList(new HandlerTwo<Result, List<Dinner_StartLogIdx>>() 
        {	
			@Override
			public void handle(Result _res, List<Dinner_StartLogIdx> _idxList) 
			{
				if(!_res.isSucc())
				{
					_commiter.commitFailRes(_res.getCode());
				}
				else
				{
					_commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_004_RetGetStartLogIdxList(_idxList));
				}
			}
		});
    }
}
package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import Common.MarsObj.Mars_ExplorePVPLogIdx;
import GC2GS.p041_MarsExploreOp.GC2GS_041_014_ReqGetCollectPVPLogIdxList;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

import java.util.List;

public class MsgDealer_GC2GS_041_014_ReqGetCollectPVPLogIdxList extends NPUserMsgDealer<GC2GS_041_014_ReqGetCollectPVPLogIdxList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_014_ReqGetCollectPVPLogIdxList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        userData.getMarsExploreComponent().getPVPLogObj().makeLogList(new HandlerTwo<Result, List<Mars_ExplorePVPLogIdx>>() 
        {	
			@Override
			public void handle(Result _res, List<Mars_ExplorePVPLogIdx> _idxList) 
			{
				if(!_res.isSucc())
				{
					_commiter.commitFailRes(_res.getCode());
				}
				else
				{
					_commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_014_RetGetCollectPVPLogIdxList(_idxList));
				}
			}
		});
    }
}

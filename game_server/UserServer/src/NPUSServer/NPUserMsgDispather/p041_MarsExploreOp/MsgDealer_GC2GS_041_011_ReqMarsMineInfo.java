package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_011_ReqMarsMineInfo;
import NPCommon.ErrMain.MarsErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

public class MsgDealer_GC2GS_041_011_ReqMarsMineInfo extends NPUserMsgDealer<GC2GS_041_011_ReqMarsMineInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_011_ReqMarsMineInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //先获取矿数据，再转变队伍状态
        MarsMineSystem.GetMarsMine(getUSServer(), _msg.getId(), (err, p) -> 
		{
			if(err > 0)
			{
	        	//火星矿不存在的特殊处理，需要移除该数据
	        	if(err == MarsErr.MARS_MINE_NOT_FOUND.getCode())
	        	{
	        		userData.getMarsMineComponent().removeMine(_msg.getId(), NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
				}
	        	
	        	_commiter.commitFailRes(err);
	        	
	        	return;
	        }
			
			_commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_011_RetMarsMineInfo(p));
		});
    }
}

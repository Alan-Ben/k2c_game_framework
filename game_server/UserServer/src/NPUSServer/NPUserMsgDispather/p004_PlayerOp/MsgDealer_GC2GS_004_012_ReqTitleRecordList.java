package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import AllRpcData.US_Service.Grave.GetGravePlayerTitleRecordList;
import GC2GS.p004_PlayerOp.GC2GS_004_012_ReqTitleRecordList;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import RPC._ARpcCallBack;

public class MsgDealer_GC2GS_004_012_ReqTitleRecordList extends NPUserMsgDealer<GC2GS_004_012_ReqTitleRecordList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_012_ReqTitleRecordList _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        int usId = CommonFunc.parseServerTypeIdFromCid(_msg.getCid());
        if(userData.getUSServer().getServerTypeId() == usId) //本服玩家
        {
        	_commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_012_RetTitleRecordList(
        			userData.getUSServer().getGraveMgr().getGravePlayerTitleRecordMgr().getRecordList(_msg.getCid())));
        }
        else //跨服玩家
        {
        	GetGravePlayerTitleRecordList rpc = new GetGravePlayerTitleRecordList();
        	rpc.req().setCid(_msg.getCid());
        	
        	userData.getUSServer().rpc2us().requestTo(usId, rpc, 
        			new _ARpcCallBack<GetGravePlayerTitleRecordList>() 
        	{
				@Override
				public void call_back(int _errCode, GetGravePlayerTitleRecordList _rpc) 
				{
					if(_errCode > 0)
					{
						_commiter.commitFailRes(_errCode);
						return;
					}
					
					_commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_012_RetTitleRecordList(_rpc.retObj().getTitleIdList()));
				}
			});
        }
    }
}

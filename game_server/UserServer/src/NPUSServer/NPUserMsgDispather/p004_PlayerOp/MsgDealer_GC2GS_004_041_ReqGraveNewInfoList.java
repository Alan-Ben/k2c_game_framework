package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_041_ReqGraveNewInfoList;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_041_ReqGraveNewInfoList extends NPUserMsgDealer<GC2GS_004_041_ReqGraveNewInfoList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_041_ReqGraveNewInfoList _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        //获取杰出者所需要的全部称号的最新新晋杰出者数据
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_041_RetGraveNewInfoList(userData, RefGeneral.Ref().graveTitleIdList));
    }
}

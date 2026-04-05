package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_035_ReqSelfLikeCount;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_035_ReqSelfLikeCount extends NPUserMsgDealer<GC2GS_004_035_ReqSelfLikeCount>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_035_ReqSelfLikeCount _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_035_RetSelfLikeCount(
                getUSServer().getCollectLikeMgr().selfGetCount(userData.getCid())));
    }
}

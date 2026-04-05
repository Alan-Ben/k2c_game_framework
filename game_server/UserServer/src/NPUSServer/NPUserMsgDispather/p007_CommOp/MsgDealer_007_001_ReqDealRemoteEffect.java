package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_001_ReqDealRemoteEffect;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_007_001_ReqDealRemoteEffect extends NPUserMsgDealer<GC2GS_007_001_ReqDealRemoteEffect>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_001_ReqDealRemoteEffect _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getRemoteEffectDealMgr().tryDealRemoteEffect(_msg.getClientSerialize(), _msg.getRefId());

        _commiter.commitSucRes(null);
    }
}

package NPUSServer.NPUserMsgDispather.p011_ClientDataOp;

import GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData;
import GS2GC.p011_ClientDataOp.GS2GC_011_001_RetQueryClientData;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ClientDataComp.ClientData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_011_001_ReqQueryClientDataNPUser extends NPUserMsgDealer<GC2GS_011_001_ReqQueryClientData>
{

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_011_001_ReqQueryClientData _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        GS2GC_011_001_RetQueryClientData retMsg = new GS2GC_011_001_RetQueryClientData();
        retMsg.setIndex(_msg.getIndex());

        ClientData clientData = userData.getClientDataComp().ensurseClientData(_msg.getIndex());
        clientData.fillProto(retMsg);

        _commiter.commitSucRes(retMsg);
    }
}

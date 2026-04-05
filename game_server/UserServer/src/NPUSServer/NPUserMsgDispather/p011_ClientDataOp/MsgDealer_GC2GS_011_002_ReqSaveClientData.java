package NPUSServer.NPUserMsgDispather.p011_ClientDataOp;

import GC2GS.p011_ClientDataOp.GC2GS_011_002_ReqSaveClientData;
import GS2GC.p011_ClientDataOp.GS2GC_011_002_RetSaveClientData;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ClientDataComp.ClientData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_011_002_ReqSaveClientData extends NPUserMsgDealer<GC2GS_011_002_ReqSaveClientData>
{

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_011_002_ReqSaveClientData _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        GS2GC_011_002_RetSaveClientData retMsg = new GS2GC_011_002_RetSaveClientData();

        ClientData clientData = userData.getClientDataComp().ensurseClientData(_msg.getIndex());
        clientData.saveClientData(_msg.getData());

        _commiter.commitSucRes(retMsg);
    }
}

package MGClient.GSListener;

import ALBasicClient._AALBasicClientListener;
import ALBasicProtocolPack._IALProtocolStructure;
import MGClient.ClientPlayer.ClientPlayer;
import MGClient.GlobalStorage;
import NPCommon.Log.CommLog;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_005_RequestEnterUS;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_011_ReqPlayerJoinedUSList;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_012_ReqMostRecommendedUSInfo;

import java.nio.ByteBuffer;

public class GSListener extends _AALBasicClientListener
{
    ClientPlayer _m_owner;

    public GSListener(String _serverIP, int _serverPort)
    {
        super(_serverIP, _serverPort);

    }

    @Override
    public void ConnectFail()
    {
        CommLog.info("login to gate server ConnectFail ");
    }

    @Override
    public void Disconnect()
    {
        _m_owner.onKicked();
        _m_owner = null;
        CommLog.info(" gate server Disconnected ");
    }

    @Override
    public void LoginFail()
    {
        CommLog.info("login to gate server failed ");
    }

    @Override
    public void LoginSuc(String arg0)
    {
        CommLog.info("login to gate server succ " + arg0);

        CommLog.info("sending NPGC2GS_001_011_ReqPlayerJoinedUSList ...");
        NPGC2GS_001_011_ReqPlayerJoinedUSList proto = new NPGC2GS_001_011_ReqPlayerJoinedUSList();
        send(proto);

        CommLog.info("sending NPGC2GS_001_012_ReqUSInfoList ...");
        NPGC2GS_001_012_ReqMostRecommendedUSInfo proto2 = new NPGC2GS_001_012_ReqMostRecommendedUSInfo();
        send(proto2);

        CommLog.info("sending NPGC2GS_001_005_RequestEnterUS ...");
        NPGC2GS_001_005_RequestEnterUS enterUSProto = new NPGC2GS_001_005_RequestEnterUS();
        enterUSProto.setServerLogicId(GlobalStorage.getInstance().getRecentServerId());
        send(enterUSProto);
    }

    public void setOwner(ClientPlayer _owner)
    {
        this._m_owner = _owner;
    }

    public ClientPlayer getOwner()
    {
        return _m_owner;
    }

    @Override
    public void receiveMes(ByteBuffer _buff)
    {
        GSRawMsgDispather.getInstance().DealProtocol(this, _buff);

    }

    @Override
    protected void _onSendProtocolFail(ByteBuffer arg0)
    {
    }

    @Override
    protected void _onSendProtocolFail(_IALProtocolStructure arg0)
    {
    }

    @Override
    protected void _onSendProtocolFail(ByteBuffer arg0, ByteBuffer arg1)
    {
    }

    @Override
    public void onBuffLengthOverSize(ByteBuffer p1, ByteBuffer p2)
    {
        CommLog.error("onBuffLengthOverSize msg:[{}-{}]", p1.get(0), p1.get(1), new Exception());
    }

//////////////////////////////////////////////////////////////////////////


}

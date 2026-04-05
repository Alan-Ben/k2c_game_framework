package MGClient.LSListener;

import ALBasicClient._AALBasicClientListener;
import ALBasicProtocolPack._IALProtocolStructure;
import MGClient.ClientPlayer.ClientPlayer;
import MGClient.Common.ELoginStat;
import MGClient.Common.LoginStat;
import NPCommon.Log.CommLog;
import NPGC2LS.p001_BasicOp.NPGC2LS_001_001_ReqBasicInfo;

import java.nio.ByteBuffer;


public class LSListener extends _AALBasicClientListener
{
    ClientPlayer _m_owner;

    public LSListener(String _serverIP, int _serverPort)
    {
        super(_serverIP, _serverPort);
    }

    @Override
    public void ConnectFail()
    {
        getOwner().OnLsConnectFailed.onEvent();
        LoginStat.getInstance().incStat(ELoginStat.eConnectFaild);
        CommLog.info("player：{} login to login server ConnectFail", getOwner().getName());
        _m_owner = null;
    }

    @Override
    public void Disconnect()//连接被断开
    {
        if (getOwner() != null)
        {
            getOwner().OnLsDisconnected.onEvent();
            LoginStat.getInstance().incStat(ELoginStat.eDisconnted);
            CommLog.info("player：{} Disconnected from login server", getOwner().getName());
            _m_owner = null;
        }
    }

    @Override
    public void LoginFail()
    {
        CommLog.info("player:{} login test stop cause of  loginFail", getOwner().getAccountId());
        _m_owner = null;
        LoginStat.getInstance().incStat(ELoginStat.eLoginFaild);

    }

    @Override
    public void LoginSuc(String customString)
    {
        LoginStat.getInstance().incStat(ELoginStat.eLoginSucc);
        getOwner().OnLsLoginSucc.onEvent();

        if (getOwner().getPlayerType() == ClientPlayer.EClientPlayerType.login_test)
        {
            //CommLog.info("player:{} login test stop here!succ,customString:{}",getOwner().getName(),customString);
            logout();
            return;
        }

        CommLog.info("login to login server succ custom string" + customString);
        CommLog.info("sending NPGC2LS_001_001_ReqBasicInfo ...");
        NPGC2LS_001_001_ReqBasicInfo proto = new NPGC2LS_001_001_ReqBasicInfo();
        send(proto);


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
        LSMsgDispather.getInstance().DealProtocol(this, _buff);

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

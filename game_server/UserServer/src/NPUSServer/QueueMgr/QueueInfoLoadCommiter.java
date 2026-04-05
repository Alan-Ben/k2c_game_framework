package NPUSServer.QueueMgr;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NP2US_RB.p002_GSOp.NP2US_RB_002_001_RegUserGate;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_Writer_001_BasicOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;

/*********************
 * 当前玩家数据加载处理的Commiter对象
 * @author mj
 *
 */
public class QueueInfoLoadCommiter implements _IWCGBasicRequestCommiter
{
    private NPUserServer _m_server;
    //用户Id
    private long _m_lGSSessionId;
    //对应GS服务器Id
    private int _m_iGSId;
    //对应GS服务器用户的信息序列号
    private long _m_lGSInfoSerialize;

    private long _m_lCid;

    protected QueueInfoLoadCommiter(NPUserServer _server, long _gsSessionId, int _gsId, long _gsInfoSerialize, long _cid)
    {
        _m_server = _server;

        _m_lGSSessionId = _gsSessionId;

        _m_iGSId = _gsId;
        _m_lGSInfoSerialize = _gsInfoSerialize;

        _m_lCid = _cid;
    }

    public NPUserServer getUSServer(){return _m_server;}

    /*******************
     * 用户数据加载的回调处理，用于记录本队列信息加载用户数据的时候的处理操作
     */
    @Override
    public void commitFailRes(int _errCode)
    {
        //被封禁导致登录失败
        if (_errCode == PlayerErr.PLAYER_ACCOUNT_IS_FREEZE.getCode())
        {
            //玩家被冻结
            getUSServer().sendMessageToBSServer(EServerType.GATE.ordinal(), _m_iGSId
                    , NP2GS_Writer_001_BasicOp.make_004_OnEnterUSButFreeze(
                            getUSServer().getPlayerFreezeMgr().lookupFreezeTime(_m_lCid), _m_lCid, _m_lGSSessionId, _m_lGSInfoSerialize));
            return;
        }

        //向对应Gs返回加载失败处理
        getUSServer().sendMessageToBSServer(EServerType.GATE.ordinal(), _m_iGSId
                , NP2GS_Writer_001_BasicOp.make_003_OnUserDataLoaded(_errCode, _m_lGSSessionId, _m_lGSInfoSerialize, _m_lCid));
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _msg)
    {
        //尝试获取用户对象
        NP2US_RB_002_001_RegUserGate commitMsg = (NP2US_RB_002_001_RegUserGate) _msg;
        if (null == commitMsg)
        {
            //向对应Gs返回加载失败处理
            getUSServer().sendMessageToBSServer(EServerType.GATE.ordinal(), _m_iGSId
                    , NP2GS_Writer_001_BasicOp.make_003_OnUserDataLoaded(CommErr.OBJ_ERR.getCode(), _m_lGSSessionId, _m_lGSInfoSerialize, _m_lCid));
            return;
        }

        //加载成功则返回
        //返回消息通知数据已经加载完毕
        getUSServer().sendMessageToBSServer(EServerType.GATE.ordinal(), _m_iGSId
                , NP2GS_Writer_001_BasicOp.make_003_OnUserDataLoaded(Result.SUCC.getCode(), _m_lGSSessionId, _m_lGSInfoSerialize, _m_lCid));
    }

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return null;
    }
}

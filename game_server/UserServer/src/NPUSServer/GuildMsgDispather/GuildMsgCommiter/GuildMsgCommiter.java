package NPUSServer.GuildMsgDispather.GuildMsgCommiter;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_020_RetGuildMsg;
import NPUSServer.Guild.GuildInfo;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

/***
 * 本地用户向公会请求操作时的返回消息处理对象
 */
public class GuildMsgCommiter implements _IWCGBasicRequestCommiter
{
    private long _m_lCid;
    private GuildInfo _m_guildInfo;
    private _IWCGBasicRequestCommiter _m_uCommiter;

    //附加信息的内容
    private ByteBuffer _m_addInfo;

    //是否本服直接处理的消息，非本服消息在回包的时候需要封装为1-20消息进行处理
    private boolean _m_bIsLocalMsg;

    public GuildMsgCommiter(long _cid, GuildInfo _guildInfo, _IWCGBasicRequestCommiter _commiter, ByteBuffer _addInfo, boolean _isLocalMsg)
    {
        this._m_lCid = _cid;
        this._m_guildInfo = _guildInfo;
        this._m_uCommiter = _commiter;
        this._m_addInfo = _addInfo;
        this._m_bIsLocalMsg = _isLocalMsg;
    }

    public long getCid() {return _m_lCid;}
    public GuildInfo getGuildInfo() {return _m_guildInfo;}
    public ByteBuffer getAddInfo() {return _m_addInfo;}

    @Override
    public _IALProtocolReceiver getRequestDealer() {
        return _m_uCommiter.getRequestDealer();
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg) {
        if(_m_bIsLocalMsg) {
            _m_uCommiter.commitSucRes(_retMsg);
        }
        else
        {
            //非本服消息需要封装为1-20消息进行处理，由于1-20默认直接转发用户，因此此处需要使用fullpackage
            _m_uCommiter.commitSucRes(new NP2US_RB_001_020_RetGuildMsg(_retMsg.makeFullPackage().array()));
        }
    }

    @Override
    public void commitFailRes(int _errCode) {
        _m_uCommiter.commitFailRes(_errCode);
    }
}

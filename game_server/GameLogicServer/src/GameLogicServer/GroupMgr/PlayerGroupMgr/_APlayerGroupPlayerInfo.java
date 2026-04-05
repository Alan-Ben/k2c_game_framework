package GameLogicServer.GroupMgr.PlayerGroupMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import GameLogicServer.GameLogicServer;

/**
 * 玩家归属主体里的玩家基础数据及基础方法
 */
public abstract class _APlayerGroupPlayerInfo
{
    // 归属的管理对象
    private _ATPlayerGroupPlayerMgr<?> _m_pmPlayerMgr;
    // 玩家CID
    private long _m_lCid;

    public _APlayerGroupPlayerInfo(_ATPlayerGroupPlayerMgr<?> _playerMgr, long _cid)
    {
        _m_pmPlayerMgr = _playerMgr;
        _m_lCid = _cid;
    }

    public _ATPlayerGroupPlayerMgr<?> getPlayerMgr() {return _m_pmPlayerMgr;}
    public long getCid() {return _m_lCid;}

    /**
     * 发送消息给客户端，转发至玩家所在的 US 服务器
     * @param _proto 待发送的协议对象
     */
    public void sendGCMsg(_IALProtocolStructure _proto)
    {
        GameLogicServer.getInstance().sendMsg2GC(_m_lCid, _proto);
    }
}

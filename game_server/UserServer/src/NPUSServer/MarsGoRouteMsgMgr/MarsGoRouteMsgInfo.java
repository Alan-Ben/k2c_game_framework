package NPUSServer.MarsGoRouteMsgMgr;

import Common.MarsObj.Mars_GoRoute_StageMsg;
import USDB.Bo.MarsGoRouteStageMsgBO;

/**
 * 火星前往路线-单条阶段留言
 */
public class MarsGoRouteMsgInfo
{
    // 数据库主键ID（0 表示尚未入库）
    private long _m_dbId;
    // 发送者名称
    private String _m_playerName;
    // 留言内容
    private String _m_content;
    // 留言时间（毫秒）
    private long _m_timeMs;

    /** 从 BO 加载（已入库） */
    public MarsGoRouteMsgInfo(MarsGoRouteStageMsgBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_playerName = _bo.getPlayerName();
        _m_content = _bo.getContent();
        _m_timeMs = _bo.getCreatedMs();
    }

    public long getDbId() {return _m_dbId;}

    public void setDbId(long _dbId) {_m_dbId = _dbId;}

    /** 构造协议对象 */
    public Mars_GoRoute_StageMsg toProto()
    {
        Mars_GoRoute_StageMsg proto = new Mars_GoRoute_StageMsg();
        proto.setPlayerName(_m_playerName);
        proto.setContent(_m_content);
        proto.setTimeMs(_m_timeMs);
        return proto;
    }
}

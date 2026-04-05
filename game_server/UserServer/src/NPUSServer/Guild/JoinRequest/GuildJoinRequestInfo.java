package NPUSServer.Guild.JoinRequest;

import Common.GuildObj.Guild_JoinRequestInfo;
import NPCommon.DB.BM.BM;
import USDB.Bo.GuildJoinRequestBO;

public class GuildJoinRequestInfo
{
    private long _m_dbId;
    private long _m_guildId;
    private long _m_cid;
    private long _m_requestTimeMs;

    public GuildJoinRequestInfo(GuildJoinRequestBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_guildId = _bo.getGuildId();
        _m_cid = _bo.getCid();
        _m_requestTimeMs = _bo.getRequestTimeMs();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public long getGuildId()
    {
        return _m_guildId;
    }

    public long getCid()
    {
        return _m_cid;
    }

    public long getRequestTimeMs()
    {
        return _m_requestTimeMs;
    }

    public Guild_JoinRequestInfo makeProto()
    {
        Guild_JoinRequestInfo proto = new Guild_JoinRequestInfo();
        proto.setDbId(_m_dbId);
        proto.setCid(_m_cid);
        return proto;
    }

    /**
     * 销毁数据
     * @param _bmObj
     */
    public void discard(BM _bmObj)
    {
        _bmObj.getBM(GuildJoinRequestBO.class).delAll("id", _m_dbId);
    }
}

package NPUSServer.Guild.Log;

import Common.GuildEnum.EGuildLogType;
import Common.GuildObj.Guild_LogInfo;
import USDB.Bo.GuildLogBO;

public class GuildLogInfo
{
    private long _m_dbId;
    private EGuildLogType _msgType;
    private long _m_sendTimeMs;
    private byte[] _m_data;

    public GuildLogInfo(GuildLogBO _bo)
    {
        _m_dbId = _bo.getId();
        _msgType = EGuildLogType.EGuildLogType_FromInt(_bo.getType());
        _m_sendTimeMs = _bo.getSendTimeMs();
        _m_data = _bo.getData();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public void discard(GuildLogMgr _mgr)
    {
        _mgr.getGuildInfo().getGuildMgr().getServer().getBM().getBM(GuildLogBO.class).delAll("id", _m_dbId);
    }

    public Guild_LogInfo makeProto()
    {
        Guild_LogInfo proto = new Guild_LogInfo();
        proto.setLogType(_msgType);
        proto.setData(_m_data);
        proto.setSendTimeMs(_m_sendTimeMs);
        return proto;
    }
}

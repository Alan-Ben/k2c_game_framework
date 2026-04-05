package NPUSServer.Guild.MarsMineBattleReport;

import Common.MarsEnum.EMarsExplorePVPLogType;
import NPCommon.DB.BM.BM;
import USDB.Bo.GuildMarsMineBattleReportBO;

/**
 * 联盟火星矿战报信息
 * 存储单条联盟成员的火星矿战报数据，供盟友查询
 */
public class GuildMarsMineBattleReportInfo
{
    // 管理器引用
    private GuildMarsMineBattleReportMgr _m_mgr;

    // 数据库主键ID
    private long _m_dbId;

    // 发起战斗的玩家CID
    private long _m_cid;

    // 战报类型
    private EMarsExplorePVPLogType _m_logType;

    // 创建时间（毫秒）
    private long _m_createdAt;

    // 战报数据（序列化字节）
    private byte[] _m_logData;

    public GuildMarsMineBattleReportInfo(GuildMarsMineBattleReportMgr _mgr, GuildMarsMineBattleReportBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_logType = EMarsExplorePVPLogType.EMarsExplorePVPLogType_FromInt(_bo.getLogType());
        _m_createdAt = _bo.getCreatedAt();
        _m_logData = _bo.getLogData();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public long getCid()
    {
        return _m_cid;
    }

    public EMarsExplorePVPLogType getLogType()
    {
        return _m_logType;
    }

    public long getCreatedAt()
    {
        return _m_createdAt;
    }

    public byte[] getLogData()
    {
        return _m_logData;
    }

    public BM getBM()
    {
        return _m_mgr.getBM();
    }

    /**
     * 从数据库删除该战报记录
     */
    public void deleteFromDB()
    {
        getBM().getBM(GuildMarsMineBattleReportBO.class).delAll("id", _m_dbId);
    }
}

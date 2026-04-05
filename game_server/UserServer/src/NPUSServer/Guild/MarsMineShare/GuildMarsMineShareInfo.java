package NPUSServer.Guild.MarsMineShare;

import Common.GuildObj.Guild_MineShareInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import USDB.Bo.GuildMarsMineShareBO;

/**
 * 联盟分享矿信息
 */
public class GuildMarsMineShareInfo
{
    // 管理器引用
    private GuildMarsMineShareMgr _m_mgr;

    // 数据库主键ID
    private long _m_dbId;

    // 发现者CID
    private long _m_finderCid;

    // 矿实例ID
    private long _m_mineInstanceId;

    // 矿有效期毫秒时间戳
    private long _m_mineEndShowMs;

    // 位置ID
    private long _m_posId;

    public GuildMarsMineShareInfo(GuildMarsMineShareMgr _mgr, GuildMarsMineShareBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_finderCid = _bo.getFinderCid();
        _m_mineInstanceId = _bo.getMineInstanceId();
        _m_mineEndShowMs = _bo.getMineEndShowMs();
        _m_posId = _bo.getPosId();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public long getFinderCid()
    {
        return _m_finderCid;
    }

    public long getMineInstanceId()
    {
        return _m_mineInstanceId;
    }

    public long getMineEndShowMs()
    {
        return _m_mineEndShowMs;
    }

    public GuildInfo getGuildInfo()
    {
        return _m_mgr.getGuildInfo();
    }

    public long getPosId()
    {
        return _m_posId;
    }

    public BM getBM()
    {
        return _m_mgr.getBM();
    }

    /**
     * 检查矿是否已过期
     */
    public boolean isExpired()
    {
        return CommonFunc.getNowTimeMS() >= _m_mineEndShowMs;
    }

    /**
     * 删除数据库记录
     */
    public void deleteFromDB()
    {
        getBM().getBM(GuildMarsMineShareBO.class).delAll("id", _m_dbId);
    }

    /**
     * 转换为协议对象
     */
    public Guild_MineShareInfo toProto()
    {
        Guild_MineShareInfo proto = new Guild_MineShareInfo();
        proto.setId(_m_dbId);
        proto.setMineInstanceId(_m_mineInstanceId);
        proto.setFinderCid(_m_finderCid);
        proto.setMineEndShowMs(_m_mineEndShowMs);
        return proto;
    }
}

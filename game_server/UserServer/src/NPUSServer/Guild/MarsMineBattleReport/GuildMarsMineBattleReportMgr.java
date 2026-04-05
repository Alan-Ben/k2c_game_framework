package NPUSServer.Guild.MarsMineBattleReport;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MarsEnum.EMarsExplorePVPLogType;
import Common.MarsObj.Mars_GuildBattleReportIdx;
import Common.ServerObj.ServerObj_MarsExplorePVPLog;
import NPCommon.DB.BM.BM;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildMarsMineBattleReportBO;

import java.util.ArrayList;

/**
 * 联盟火星矿战报管理器
 * 管理联盟内所有成员的火星矿战报，供盟友查询
 *
 * 主要功能：
 * 1. 添加新战报（内存 + 数据库）
 * 2. 超出上限时自动移除最旧记录
 * 3. 初始化时从数据库加载历史战报
 */
public class GuildMarsMineBattleReportMgr
{
    // 每个联盟最多保存的战报数量
    private static final int MAX_BATTLE_REPORT_COUNT = 30;

    // 联盟数据对象
    private GuildInfo _m_guildInfo;

    // 战报列表（按时间升序，旧的在前）
    private ArrayList<GuildMarsMineBattleReportInfo> _m_reportList;

    // 列表锁
    private MutexAtom _m_mutex;

    private long _m_latestReportId; // 最新战报的数据库ID，用于红点判断

    public GuildMarsMineBattleReportMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_reportList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    public BM getBM()
    {
        return _m_guildInfo.getGuildMgr().getServer().getBM();
    }

    public NPUserServer getUSServer()
    {
        return _m_guildInfo.getGuildMgr().getServer();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 服务器启动时从BO初始化战报数据
     * @param _bo 数据库对象
     */
    public void _initFromBO(GuildMarsMineBattleReportBO _bo)
    {
        _m_reportList.add(new GuildMarsMineBattleReportInfo(this, _bo));
        _m_latestReportId = _bo.getId();
    }

    /**
     * 添加一条战报
     * 超出上限时自动移除最旧记录
     *
     * @param _cid      发起战斗的玩家CID
     * @param _logType  战报类型
     * @param _logObj   战报数据对象
     */
    public void addBattleReport(long _cid, EMarsExplorePVPLogType _logType, ServerObj_MarsExplorePVPLog _logObj)
    {
        _lock();
        try
        {
            // 超出上限则移除最旧记录
            _checkRemoveOverLimitItem();

            // 插入数据库
            GuildMarsMineBattleReportBO bo = new GuildMarsMineBattleReportBO();
            bo.setGuildId(getBM(), _m_guildInfo.getGuildId());
            bo.setCid(getBM(), _cid);
            bo.setLogType(getBM(), _logType.ordinal());
            bo.setCreatedAt(getBM(), _logObj.getCreatedAt());
            bo.setLogData(getBM(), _logObj.getLogData());
            bo.insert(getBM());

            // 添加到内存列表
            _m_reportList.add(new GuildMarsMineBattleReportInfo(this, bo));
            _m_latestReportId = bo.getId();

            // 推送最新战报ID给联盟所有在线成员
            final long newId = bo.getId();
            ALSynTaskManager.getInstance().regTask(() ->
                    _m_guildInfo.getMemberMgr().broadcastMsg(
                            US2GCWriter_032_GuildOp.make_081_OnGuildMarsBattleReportAdd(newId)));
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查并移除超出上限的最旧战报
     */
    private void _checkRemoveOverLimitItem()
    {
        while (_m_reportList.size() >= MAX_BATTLE_REPORT_COUNT)
        {
            GuildMarsMineBattleReportInfo oldest = _m_reportList.remove(0);
            oldest.deleteFromDB();
        }
    }

    /**
     * 获取最新战报的数据库ID，用于红点判断；无战报时返回0
     */
    public long getLatestReportId()
    {
        return _m_latestReportId;
    }

    /**
     * 构建战报列表Proto，供协议响应使用
     * 按时间从新到旧排序返回（列表末尾为最新）
     */
    public ArrayList<Mars_GuildBattleReportIdx> makeReportListProto()
    {
        _lock();
        try
        {
            ArrayList<Mars_GuildBattleReportIdx> result = new ArrayList<>();
            for (GuildMarsMineBattleReportInfo info : _m_reportList)
            {
                Mars_GuildBattleReportIdx idx = new Mars_GuildBattleReportIdx(
                        info.getDbId(),
                        info.getCid(),
                        info.getLogType(),
                        info.getCreatedAt(),
                        info.getLogData()
                );
                result.add(idx);
            }
            return result;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 联盟解散时清理所有战报数据
     */
    public void discard()
    {
        _lock();
        try
        {
            _m_reportList.clear();
            getBM().getBM(GuildMarsMineBattleReportBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
        }
        finally
        {
            _unlock();
        }
    }
}

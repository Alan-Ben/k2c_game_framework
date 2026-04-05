package NPUSServer.EarningsMarqueeMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.EarningsMarqueeRecordBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 赚速跑马灯管理器
 * <p>
 * 主要功能：
 * 1. 管理所有赚速目标的跑马灯触发记录
 * 2. 检查玩家赚速变化并触发跑马灯
 * 3. 从配置加载赚速目标和限制
 * <p>
 * 线程安全：使用锁保护内部数据
 */
public class EarningsMarqueeMgr
{
    private NPUserServer _m_server;
    private MutexObject _m_mutex;

    // 所有赚速目标信息列表
    private List<EarningsMarqueeInfo> _m_infoList;

    public EarningsMarqueeMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_mutex = new MutexObject();
        _m_infoList = new ArrayList<>();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    /**
     * 从数据库初始化
     */
    public boolean initFromDB()
    {
        // 从配置加载赚速目标
        WCGPairIntList config = RefGeneral.Ref().earnings_marquee_config;
        if (config == null || config.getList().isEmpty())
        {
            USLog.sys(_m_server, "EarningsMarqueeMgr.initFromDB - no config found, skip initialization");
            return true;
        }

        // 创建赚速目标信息对象
        for (WCGPairInt pair : config.getList())
        {
            long earningsGoal = pair.first();
            int maxCount = pair.second();

            EarningsMarqueeInfo info = new EarningsMarqueeInfo(this, earningsGoal, maxCount);
            _m_infoList.add(info);
        }

        // 从数据库加载已触发记录
        List<EarningsMarqueeRecordBO> boList = _m_server.getBM().getBM(EarningsMarqueeRecordBO.class).s_findAll();
        if (boList == null)
        {
            USLog.error(_m_server, "EarningsMarqueeMgr.initFromDB - load records from db failed");
            return false;
        }

        for (EarningsMarqueeRecordBO bo : boList)
        {
            EarningsMarqueeInfo info = lookup(bo.getEarningsGoal());
            if (info != null)
            {
                info.initAddRecord(bo);
            }
        }

        return true;
    }

    /**
     * 查找赚速目标信息
     */
    public EarningsMarqueeInfo lookup(long _earningsGoal)
    {
        _lock();
        try
        {
            for (EarningsMarqueeInfo info : _m_infoList)
            {
                if (info.getEarningsGoal() == _earningsGoal)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查并触发赚速跑马灯
     * <p>
     * 执行流程：
     * 1. 遍历所有配置的赚速目标
     * 2. 检查玩家是否超过目标且未触发过
     * 3. 检查该目标是否还有配额
     * 4. 添加记录并异步触发跑马灯
     * @param _cid      玩家CID
     * @param _earnings 玩家当前最高赚速
     */
    public void checkAndTriggerMarquee(long _cid, String _name, long _earnings)
    {
        // 检查配置是否为空
        if (_m_infoList.isEmpty())
            return;

        long marqueeRefId = RefGeneral.Ref().earnings_marquee_ref_id;
        if (marqueeRefId == 0)
        {
            USLog.error(_m_server, "EarningsMarqueeMgr.triggerMarquee - marquee ref id not configured");
            return;
        }

        _lock();
        try{
            // 遍历所有赚速目标
            for (EarningsMarqueeInfo info : _m_infoList)
            {
                // 检查是否超过目标
                if (_earnings < info.getEarningsGoal())
                    continue;

                // 添加记录
                boolean success = info.addRecord(_cid);
                if (!success)
                    continue;

                ALSynTaskManager.getInstance().regTask(() ->
                {
                    List<String> paramList = new ArrayList<>();
                    paramList.add(_name);
                    paramList.add(String.valueOf(info.getEarningsGoal()));
                    _m_server.getMarqueeMgr().cmdAddMarquee(marqueeRefId, paramList);
                });
            }
        }finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("EarningsMarqueeMgr{");
        sb.append("goalCount=").append(_m_infoList.size()).append("\n");

        _lock();
        try
        {
            for (EarningsMarqueeInfo info : _m_infoList)
            {
                sb.append("  [goal=").append(info.getEarningsGoal())
                  .append(", triggered=").append(info.getTriggeredCount())
                  .append("/").append(info.getMaxCount());

                // 打印已触发的玩家CID列表
                List<Long> cidList = info.getTriggeredCidList();
                if (!cidList.isEmpty())
                {
                    sb.append(", cids=").append(cidList);
                }

                sb.append("]\n");
            }
        }
        finally
        {
            _unlock();
        }

        sb.append("}");
        return sb.toString();
    }
}

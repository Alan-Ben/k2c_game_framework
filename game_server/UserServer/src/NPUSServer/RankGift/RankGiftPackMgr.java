package NPUSServer.RankGift;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RankGift.RefRankGiftPack;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsRankGiftPackBO;

import java.util.List;

/**
 * 冲榜礼包管理器（服务器级别）
 * <p>
 * 主要功能：
 * 1. 管理已激活礼包数据（数据库只有1条记录）
 * 2. 定时检查已激活礼包是否过期，过期则从配表生成新数据并激活
 * <p>
 * 设计特点：
 * - 服务器级别单例管理器
 * - 配表只有一条数据，过期时取配表第一条生成新礼包
 * - 过期时才从配表生成新数据并缓存到数据库
 * - 避免热更配表导致已激活数据异常
 * <p>
 * 线程安全：由UserServer主线程调用，无需加锁
 */
public class RankGiftPackMgr
{
    // 服务器引用
    private NPUserServer _m_server;

    // 已激活的礼包（数据库有记录）
    private RankGiftPackInfo _m_activatedPack;

    private MutexAtom _m_mutex;

    // 上次"ref end_time is invalid"错误日志输出时间（毫秒），用于限制日志频率
    private long _m_lastEndTimeInvalidErrorLogTimeMs = 0;

    /**
     * 构造函数
     * @param _server 服务器对象
     */
    public RankGiftPackMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_mutex = new MutexAtom();
    }

    /**
     * 获取服务器对象
     */
    public NPUserServer getServer()
    {
        return _m_server;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取已激活的礼包
     * @return 已激活的礼包，如果没有返回null
     */
    public RankGiftPackInfo getActivatedPack()
    {
        return _m_activatedPack;
    }

    /**
     * 初始化管理器
     * <p>
     * 执行流程：
     * 1. 从数据库加载已激活礼包数据
     * 2. 数据库中最多只有1条已激活记录
     * @return true表示初始化成功，false表示失败
     */
    public boolean init()
    {
        List<UsRankGiftPackBO> boList = getServer().getBM().getBM(UsRankGiftPackBO.class).s_findAll();
        if (boList == null)
        {
            USLog.error(getServer(), "RankGiftPackMgr.init - database query failed: load rank gift pack data failed");
            return false;
        }

        if (boList.isEmpty())
        {
            USLog.info(getServer(), "RankGiftPackMgr.init - load success: no activated pack");
            return true;
        }

        if (boList.size() > 1)
        {
            USLog.error(getServer(), "RankGiftPackMgr.init - data error: multiple packs found, count={}", boList.size());
        }

        // 只取第一条作为已激活礼包
        _m_activatedPack = new RankGiftPackInfo(this, boList.get(0));

        USLog.info(getServer(), "RankGiftPackMgr.init - load success: activatedPackDbId={}", _m_activatedPack.getDbId());
        return true;
    }

    /**
     * 定时检查（每秒调用一次）
     * <p>
     * 执行流程：
     * 1. 如果没有已激活礼包，从配表生成新礼包并激活
     * 2. 如果有已激活礼包且已过期，删除旧礼包，从配表生成新礼包并激活
     * 3. 如果有已激活礼包且未过期，不做任何操作
     */
    public void tick5Sec()
    {
        _lock();
        try{
            long nowTimeMs = CommonFunc.getNowTimeMS();
            boolean needActivate = false;
            boolean needPush = false;

            // 检查是否需要激活新礼包
            if (_m_activatedPack == null)
            {
                // 没有已激活礼包，需要激活
                needActivate = true;
            }
            else if (nowTimeMs > _m_activatedPack.getEndTimeMs())
            {
                // 已激活礼包已过期，需要删除并重新激活
                USLog.info(getServer(), "RankGiftPackMgr.tick5Sec - activated pack expired: dbId={}, endTimeMs={}, nowTimeMs={}",
                        _m_activatedPack.getDbId(), _m_activatedPack.getEndTimeMs(), nowTimeMs);

                _m_activatedPack.deleteFromDB();
                _m_activatedPack = null;
                needActivate = true;
                needPush = true;
            }

            // 如果需要激活新礼包
            if (needActivate)
            {
                // 从配表获取第一条数据
                List<RefRankGiftPack> refList = RefRankGiftPack.getMgr().getList();
                if (refList == null || refList.isEmpty())
                {
                    USLog.error(getServer(), "RankGiftPackMgr.tick5Sec - activate failed: ref table is empty");

                    // 5秒后再次检查
                    ALSynTaskManager.getInstance().regTask(this::tick5Sec, 5000);
                    return;
                }

                RefRankGiftPack ref = refList.get(0);
                if (ref.end_time.getTimeMs() <= 0 || CommonFunc.getNowTimeMS() > ref.end_time.getTimeMs())
                {
                    // 限制错误日志频率：30分钟内只报错一次
                    long currentTimeMs = CommonFunc.getNowTimeMS();
                    if (currentTimeMs - _m_lastEndTimeInvalidErrorLogTimeMs > 30 * 60 * 1000)
                    {
                        USLog.error(getServer(), "RankGiftPackMgr.tick5Sec - activate failed: ref end_time is invalid");
                        _m_lastEndTimeInvalidErrorLogTimeMs = currentTimeMs;
                    }

                    // 5秒后再次检查
                    ALSynTaskManager.getInstance().regTask(this::tick5Sec, 5000);
                    return;
                }

                // 创建新的BO对象并插入数据库
                UsRankGiftPackBO newBo = new UsRankGiftPackBO();
                newBo.setUiResPathId(getServer().getBM(), ref.ui_res_path_id);
                newBo.setSale(getServer().getBM(), ref.sale);
                newBo.setBuyLimit(getServer().getBM(), ref.buy_limit);
                newBo.setName(getServer().getBM(), ref.name);
                newBo.setCost(getServer().getBM(), ref.cost.toString());
                newBo.setOriCost(getServer().getBM(), ref.ori_cost.toString());
                newBo.setRewardItemList(getServer().getBM(), CommonFunc.list2String(ref.reward_item_list, ';'));
                newBo.setEndTimeMs(getServer().getBM(), ref.end_time.getTimeMs());
                newBo.setActivateTimeMs(getServer().getBM(), nowTimeMs); // 设置激活时间
                newBo.insert(getServer().getBM());

                // 创建新的RankGiftPackInfo对象
                _m_activatedPack = new RankGiftPackInfo(this, newBo);

                USLog.info(getServer(), "RankGiftPackMgr.tick5Sec - activate done: new pack activated, dbId={}, endTimeMs={}",
                        _m_activatedPack.getDbId(), ref.end_time.getTimeMs());

                needPush = true;
            }

            if (needPush)
            {
                // 通知所有在线用户礼包信息变更
                notifyOnlineUserPackChg();
            }

            // 5秒后再次检查
            ALSynTaskManager.getInstance().regTask(this::tick5Sec, 5000);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 修改已激活礼包信息
     * @param _uiResPathId 资源路径ID
     * @param _sale 折扣
     * @param _buyLimit 购买限制
     * @param _name 礼包名称
     * @param _cost 消耗道具
     * @param _oriCost 原价消耗道具
     * @param _rewardList 奖励列表
     * @param _endDate 结束日期
     * @param _endTime 结束时间
     */
    public boolean modifyActiveGiftPackInfo(long _uiResPathId, int _sale, int _buyLimit, String _name, String _cost, String _oriCost, String _rewardList, String _endDate, String _endTime)
    {
        _lock();
        try{
            if (_m_activatedPack == null)
                return false;

            _m_activatedPack.modifyInfo(_uiResPathId, _sale, _buyLimit, _name, _cost, _oriCost, _rewardList, _endDate, _endTime);

            // 通知所有在线用户礼包信息变更
            notifyOnlineUserPackChg();

            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 修改已激活礼包结束时间
     * @param _endDate
     * @param _endTime
     * @return
     */
    public boolean chgActivePackEndTime(String _endDate, String _endTime)
    {
        _lock();
        try{
            if (_m_activatedPack == null)
                return false;

            _m_activatedPack.chgEndTime(_endDate, _endTime);

            notifyOnlineUserPackChg();

            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 通知所有在线用户礼包信息变更
     */
    public void notifyOnlineUserPackChg()
    {
        // 通知所有在线用户礼包信息变更
        ALSynTaskManager.getInstance().regTask(()->{
            for (NPUSUserData userdate : getServer().getUsUserMgr().getAllCacheUserData())
            {
                userdate.safeCall(()->{
                    userdate.getRankGiftPackComponent().onPackChg();
                });
            }
        });
    }
}

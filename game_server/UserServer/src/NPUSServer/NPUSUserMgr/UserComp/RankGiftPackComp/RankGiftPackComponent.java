package NPUSServer.NPUSUserMgr.UserComp.RankGiftPackComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RankGiftPackObj.RankGiftPack_Info;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankGiftPackErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.RankGift.RankGiftPackInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerRankGiftPackBO;

import java.util.List;

/**
 * 玩家冲榜礼包组件
 * <p>
 * 主要功能：
 * 1. 记录玩家对当前礼包的购买次数
 * 2. 自动处理礼包切换（通过 dbId 比对）
 * 3. 提供购买次数查询和记录接口
 * <p>
 * 设计特点：
 * - 单记录模式：每个玩家最多只有一条数据库记录
 * - 自动归零：礼包切换时，getBuyCount() 自动返回 0
 * - 懒加载插入：首次购买时才创建数据库记录
 * - 原子性保证：通过 dbId 确保客户端和服务器数据一致
 * <p>
 * 线程安全：通过玩家级别锁保护数据一致性
 */
public class RankGiftPackComponent extends _ANPUserComponent
{
    // 数据库记录ID，0表示未插入数据库
    private long _m_dbId;

    // 玩家记录的礼包实例ID（us_rank_gift_pack表的id）
    private long _m_rankGiftPackInstanceId;

    // 已购买次数
    private int _m_buyCount;

    /**
     * 构造函数
     * @param _userData 玩家数据对象
     */
    public RankGiftPackComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.RANK_GIFT_PACK);
        _m_dbId = 0;
        _m_rankGiftPackInstanceId = 0;
        _m_buyCount = 0;
    }

    /**
     * 初始化组件：从数据库加载玩家购买记录
     * <p>
     * 执行流程：
     * 1. 查询玩家的购买记录（最多一条）
     * 2. 如果存在记录，加载到内存
     * 3. 标记组件初始化完成
     */
    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerRankGiftPackBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerRankGiftPackBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerRankGiftPackBO> _boList)
                    {
                        // 加载购买记录（应该最多只有一条）
                        if (!_boList.isEmpty())
                        {
                            PlayerRankGiftPackBO bo = _boList.get(0);
                            _m_dbId = bo.getId();
                            _m_rankGiftPackInstanceId = bo.getRankGiftPackInstanceId();
                            _m_buyCount = bo.getBuyCount();

                            // 如果查询到多条记录，记录警告日志
                            if (_boList.size() > 1)
                            {
                                USLog.error(getUSServer(),
                                        "RankGiftPackComponent._init - multiple records found: cid={}, count={}",
                                        getUserData().getCid(), _boList.size());
                            }
                        }

                        // 标记组件初始化完成
                        setInited();
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(getUSServer(), "RankGiftPackComponent._init - load data failed: cid={}",
                                getUserData().getCid());
                        getUserData().setDataLoadFail();
                    }
                });
    }

    /**
     * 获取依赖组件列表
     * @return null（无依赖）
     */
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    /**
     * 所有组件初始化完成后的回调
     */
    @Override
    public void onInited()
    {
        // 无需处理
    }

    /**
     * 释放资源
     */
    @Override
    public void dispose()
    {
        // 无需处理
    }

    /**
     * 获取玩家已购买次数
     * <p>
     * 该方法会对玩家加锁后，比较传入的礼包实例ID与当前记录的实例ID：
     * - 如果ID匹配，返回已购买次数
     * - 如果ID不匹配，返回0（礼包已切换）
     *
     * @param _packInstanceId 礼包实例ID
     * @return 已购买次数，如果礼包已切换则返回0
     */
    public int getBuyCount(long _packInstanceId)
    {
        getUserData().lockUser();
        try
        {
            return _m_rankGiftPackInstanceId == _packInstanceId ? _m_buyCount : 0;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录购买
     * <p>
     * 执行流程：
     * 1. 更新礼包实例ID（如果礼包切换，会自动更新）
     * 2. 增加购买次数
     * 3. 持久化到数据库（懒加载插入或增量更新）
     *
     * @param _packInstanceId 礼包实例ID
     * @param _count          购买次数
     */
    public void recordPurchase(long _packInstanceId, int _count)
    {
        // 如果礼包切换了，重置购买次数
        if (_m_rankGiftPackInstanceId != _packInstanceId)
            _m_buyCount = 0;

        // 更新实例ID和购买次数
        _m_rankGiftPackInstanceId = _packInstanceId;
        _m_buyCount += _count;

        // 持久化到数据库
        if (makeSureBoInsert())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("rank_gift_pack_instance_id", _m_rankGiftPackInstanceId);
            updateValue.addValueObj("buy_count", _m_buyCount);
            getBM().getBM(PlayerRankGiftPackBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 确保数据库记录已创建
     * <p>
     * 执行流程：
     * 1. 检查是否已插入数据库（_m_dbId != 0）
     * 2. 如果未插入，创建新BO并插入数据库
     * 3. 缓存生成的数据库ID
     * @return true=记录已存在，false=新创建记录
     */
    private boolean makeSureBoInsert()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                return true; // 已插入

            // 创建新记录
            PlayerRankGiftPackBO bo = new PlayerRankGiftPackBO();
            bo.setCid(getBM(), getUserData().getCid());
            bo.setRankGiftPackInstanceId(getBM(), _m_rankGiftPackInstanceId);
            bo.setBuyCount(getBM(), _m_buyCount);
            bo.insert(getBM());

            _m_dbId = bo.getId();
            return false; // 新创建
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 购买礼包
     * <p>
     * 执行流程：
     * 1. 获取当前激活的礼包
     * 2. 原子性校验：验证客户端 dbId 是否匹配
     * 3. 验证购买次数限制
     * 4. 扣除消耗道具
     * 5. 发放奖励道具
     * 6. 记录购买次数
     * 7. 发送购买结果通知
     *
     * @param clientDbId 客户端传来的礼包实例ID
     * @param context    操作上下文
     * @return 操作结果
     */
    public Result buyGiftPack(long clientDbId, NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 获取当前激活的礼包
            RankGiftPackInfo activePack = getUSServer().getRankGiftPackMgr().getActivatedPack();
            if (activePack == null)
                return RankGiftPackErr.NO_ACTIVE_PACK;

            if (activePack.getDbId() != clientDbId)
                return RankGiftPackErr.PACK_EXPIRED;

            // 验证购买次数限制
            int currentBuyCount = getBuyCount(activePack.getDbId());
            if (currentBuyCount >= activePack.getBuyLimit())
                return RankGiftPackErr.BUY_LIMIT_EXCEEDED;

            // 扣除消耗道具
            NPCommonCostItem costItem = activePack.getCostItem();
            if (costItem != null && !getUserData().hasItem(costItem))
                return CommErr.ITEM_NOT_ENOUGH;

            if (costItem != null && !getUserData().spendItem(costItem, context))
                return CommErr.CONSUME_FAIL;

            // 发放奖励道具
            List<NPCommonCostItem> rewardList = activePack.getRewardItemList();
            if (rewardList != null)
                getUserData().gainItemList(rewardList, context);

            // 记录购买次数
            recordPurchase(activePack.getDbId(), 1);

            // 发送购买结果通知
            RankGiftPack_Info proto = activePack.toProto();
            proto.setHadBuyTimes(getBuyCount(activePack.getDbId()));
            getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_064_OnRankGiftPackChg(proto));

            return Result.SUCC;

        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转换为协议对象
     * <p>
     * 获取当前激活的礼包信息，并设置玩家购买次数（如果礼包已切换则返回0）。
     * 如果当前没有激活的礼包，返回null。
     *
     * @return 礼包信息协议对象，如果没有激活礼包则返回null
     */
    public RankGiftPack_Info toProto()
    {
        getUserData().lockUser();
        try
        {
            // 获取服务器级别的已激活礼包
            RankGiftPackInfo activePack = getUSServer().getRankGiftPackMgr().getActivatedPack();
            // 没有激活礼包，返回空信息
            if (activePack == null)
                return null;

            RankGiftPack_Info proto = activePack.toProto();
            proto.setHadBuyTimes(getBuyCount(proto.getDbId()));
            return proto;
        } finally
        {
            getUserData().unlockUser();
    }
    }

    /**
     * 礼包变更推送
     */
    public void onPackChg()
    {
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_064_OnRankGiftPackChg(toProto()));
    }
}
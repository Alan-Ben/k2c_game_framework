package NPUSServer.NPUSUserMgr.UserComp.GuildCooperateComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList;
import Common.GuildCooperateObj.GuildCooperate_RewardPointPos;
import GS2GC.p002_InitOp.GS2GC_002_075_RetGuildCooperateInit;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.GuildCooperateErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerGuildCooperateDrawRecordBO;
import USDB.Bo.PlayerGuildCooperateHeroUseRecordBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 联盟协作玩家组件
 * <p>
 * 主要功能：
 * 1. 管理玩家的大臣使用记录（每日重置）
 * 2. 管理玩家的奖励领取记录（协作刷新时重置）
 * 3. 提供双重刷新时间检查机制
 * 4. 处理玩家个人数据的持久化
 * <p>
 * 设计特点：
 * - 跨联盟数据保留：不绑定guild_id，支持玩家换联盟
 * - 双重刷新机制：大臣使用和奖励领取有不同的重置时机
 * - 简单高效：使用分号分隔字符串存储，避免JSON解析开销
 * <p>
 * 线程安全：通过getUserData().lockUser()保证数据一致性
 */
public class GuildCooperateComponent extends _ANPUserComponent
{
    // 已使用大臣列表缓存
    private List<GuildCooperateHeroUseRecord> _m_usedHeroList;

    // 已领取奖励据点列表缓存（格式：areaId:index）
    private List<GuildCooperate_RewardPointPos> _m_drawnRewardPointList;

    /**
     * 构造函数
     * @param userData 玩家数据对象
     */
    public GuildCooperateComponent(NPUSUserData userData)
    {
        super(userData, ENPPlayerCompType.GUILD_COOPERATE_COMP);

        _m_usedHeroList = new ArrayList<>();
        _m_drawnRewardPointList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        // 异步初始化数据
        final ALProcess process = ALProcess.CreateProcess("guild_cooperate_init");

        // 加载玩家大臣使用记录
        process.addResDelegateProcess(action -> _loadHeroUseRecords(action::dealAction), "load_hero_use_records",
                () -> USLog.error(getUSServer(), "player:{} load guild cooperate hero use records failed.", getUserData().getCid()), false);

        // 加载玩家奖励领取记录
        process.addResDelegateProcess(action -> _loadDrawRecords(action::dealAction), "load_draw_records",
                () -> USLog.error(getUSServer(), "player:{} load guild cooperate draw records failed.", getUserData().getCid()), false);

        // 处理初始化结果
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "player:{} guild cooperate component init failed.", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        // 无依赖组件
        return null;
    }

    @Override
    public void onInited()
    {
    }

    @Override
    public void dispose()
    {
    }

    /**
     * 加载大臣使用记录数据
     * <p>
     * 执行流程：
     * 1. 查询玩家所有大臣使用记录
     * 2. 构造大臣使用记录对象并添加到缓存列表
     * 3. 完成加载回调
     * @param callback 加载完成回调
     */
    private void _loadHeroUseRecords(_ICallBackBool callback)
    {
        getUSServer().getBM().getBM(PlayerGuildCooperateHeroUseRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerGuildCooperateHeroUseRecordBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerGuildCooperateHeroUseRecordBO> _boList)
                    {
                        // 构造大臣使用记录对象
                        for (PlayerGuildCooperateHeroUseRecordBO bo : _boList)
                        {
                            GuildCooperateHeroUseRecord record = new GuildCooperateHeroUseRecord(GuildCooperateComponent.this, bo);
                            _m_usedHeroList.add(record);
                        }

                        callback.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            callback.onRunOver(false);
                            USLog.error(getUSServer(), "GuildCooperateComponent load hero use records failed: cid={}", getUserData().getCid());
                            return;
                        }

                        // 无记录时也算成功
                        callback.onRunOver(true);
                    }
                });
    }

    /**
     * 加载奖励领取记录数据
     * <p>
     * 执行流程：
     * 1. 查询玩家所有奖励领取记录
     * 2. 构造奖励据点位置对象并添加到缓存列表
     * 3. 完成加载回调
     * @param callback 加载完成回调
     */
    private void _loadDrawRecords(_ICallBackBool callback)
    {
        getUSServer().getBM().getBM(PlayerGuildCooperateDrawRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerGuildCooperateDrawRecordBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerGuildCooperateDrawRecordBO> _boList)
                    {
                        // 构造奖励据点位置对象
                        for (PlayerGuildCooperateDrawRecordBO bo : _boList)
                        {
                            GuildCooperate_RewardPointPos pos = new GuildCooperate_RewardPointPos();
                            pos.setAreaId(bo.getAreaId());
                            pos.setIndex(bo.getRewardPointIndex());
                            _m_drawnRewardPointList.add(pos);
                        }

                        callback.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            callback.onRunOver(false);
                            USLog.error(getUSServer(), "GuildCooperateComponent load draw records failed: cid={}", getUserData().getCid());
                            return;
                        }

                        callback.onRunOver(true);
                    }
                });
    }


    /**
     * 记录奖励领取
     * <p>
     * 执行流程：
     * 1. 检查是否已经领取过该奖励
     * 2. 创建新的领取记录并插入数据库
     * 3. 添加到缓存列表中
     * @param areaId           区域ID
     * @param rewardPointIndex 奖励据点索引
     * @return 是否记录成功
     */
    public Result recordRewardDraw(long areaId, int rewardPointIndex, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查重置领取记录
            checkResetDrawRecords(_context);

            // 检查是否已经领取过
            if (hasDrawnReward(areaId, rewardPointIndex))
                return GuildCooperateErr.REWARD_ALREADY_DRAWN;

            // 创建数据库记录
            PlayerGuildCooperateDrawRecordBO bo = new PlayerGuildCooperateDrawRecordBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setAreaId(getUSServer().getBM(), areaId);
            bo.setRewardPointIndex(getUSServer().getBM(), rewardPointIndex);
            bo.insert(getUSServer().getBM());

            // 添加到缓存列表
            GuildCooperate_RewardPointPos pos = new GuildCooperate_RewardPointPos(areaId, rewardPointIndex);
            _m_drawnRewardPointList.add(pos);

            getUserData().sendMsgToGC(US2GCWriter_032_GuildOp.make_078_OnHadDrawRewardPointListChg(makeDrawnRewardPointProto()));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否需要重置领取记录
     */
    public void checkResetDrawRecords(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 获取上次刷新时间
            long lastRefreshTimeMs = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.GUILD_COOPERATE_DRAW_RECORD_LAST_REFRESH_TIME_MS);
            // 计算当前周期的刷新时间点
            long beforeFreshTimeTagMS = RefGeneral.Ref().guild_cooperate_refresh_time.getBeforeFreshTimeTagMS(CommonFunc.getNowTimeMS());
            if (beforeFreshTimeTagMS <= lastRefreshTimeMs)
                return;

            // 清空缓存列表
            _m_drawnRewardPointList.clear();

            // 删除数据库记录
            getUSServer().getBM().getBM(PlayerGuildCooperateDrawRecordBO.class).delAll("cid", getUserData().getCid());

            // 更新最后刷新时间记录
            getUserData().getRecordComponent().setRecord(ENPPlayerRecordParam.GUILD_COOPERATE_DRAW_RECORD_LAST_REFRESH_TIME_MS, CommonFunc.getNowTimeMS(), _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否已领取奖励
     * @param areaId 区域ID
     * @param rewardPointIndex 奖励据点索引
     * @return 是否已领取
     */
    public boolean hasDrawnReward(long areaId, int rewardPointIndex)
    {
        getUserData().lockUser();
        try
        {
            for (GuildCooperate_RewardPointPos pos : _m_drawnRewardPointList)
            {
                if (pos.getAreaId() == areaId && pos.getIndex() == rewardPointIndex)
                {
                    return true;
                }
            }
            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找已使用大臣记录
     * @param heroId 大臣ID
     * @return 大臣使用记录，如果未找到返回null
     */
    public GuildCooperateHeroUseRecord lookupHeroUseRecord(long heroId)
    {
        getUserData().lockUser();
        try
        {
            for (GuildCooperateHeroUseRecord record : _m_usedHeroList)
            {
                if (record.getHeroId() == heroId)
                {
                    return record;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录大臣使用
     * 
     * 执行流程：
     * 1. 查找或创建大臣使用记录
     * 2. 检查大臣是否可用（未超过使用次数限制）
     * 3. 记录使用次数并更新数据库
     * 4. 自动处理每日刷新逻辑
     * 
     * @param heroId 大臣ID
     * @param context 玩家操作上下文
     * @return 记录结果，成功返回Result.SUCC，失败返回对应错误码
     */
    public Result recordHeroUse(long heroId, NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 查找现有的大臣使用记录
            GuildCooperateHeroUseRecord record = lookupHeroUseRecord(heroId);

            if (record == null)
            {
                // 首次使用该大臣，创建新记录
                PlayerGuildCooperateHeroUseRecordBO bo = new PlayerGuildCooperateHeroUseRecordBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setHeroId(getUSServer().getBM(), heroId);
                bo.setUseCount(getUSServer().getBM(), 0);
                bo.setRecoverCount(getUSServer().getBM(), 0);
                bo.setLastRefreshTimeMs(getUSServer().getBM(), CommonFunc.getTodayZeroClockMS(0));
                bo.insert(getUSServer().getBM());

                // 创建记录对象并添加到缓存列表
                record = new GuildCooperateHeroUseRecord(this, bo);
                _m_usedHeroList.add(record);
            }

            // 记录使用
            return record.recordUse(getUSServer().getBM());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加大臣恢复次数
     * 
     * 执行流程：
     * 1. 查找大臣使用记录
     * 2. 验证大臣确实已被使用
     * 3. 增加恢复次数并更新数据库
     * 
     * @param heroId 大臣ID
     * @param context 玩家操作上下文
     * @return 恢复结果，成功返回Result.SUCC，失败返回对应错误码
     */
    public Result addHeroRecoveryCount(long heroId, NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 查找大臣使用记录
            GuildCooperateHeroUseRecord record = lookupHeroUseRecord(heroId);

            // 该大臣从未使用过，无法恢复
            if (record == null)
                return GuildCooperateErr.HERO_NOT_USED;

            // 增加恢复次数
            return record.recoverUse(getUSServer().getBM());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 回滚大臣使用记录
     * 
     * 用于当记录大臣使用后，后续操作失败时回滚使用次数。
     * 执行流程：
     * 1. 查找大臣使用记录
     * 2. 验证记录存在且使用次数大于0
     * 3. 减少使用次数并更新数据库
     * 4. 推送数据变更通知给客户端
     * 
     * @param heroId 大臣ID
     * @param context 玩家操作上下文
     */
    public void rollbackHeroUse(long heroId, NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 查找大臣使用记录
            GuildCooperateHeroUseRecord record = lookupHeroUseRecord(heroId);
            if (record == null)
                return;

            // 调用记录对象的回滚方法
            record.rollbackUse(getUSServer().getBM());
        } finally
        {
            getUserData().unlockUser();
        }
    }



    /**
     * 构造已领取奖励据点列表的协议对象
     * @return
     */
    public GuildCooperate_HadDrawRewardPointList makeDrawnRewardPointProto()
    {
        getUserData().lockUser();
        try
        {
            GuildCooperate_HadDrawRewardPointList proto = new GuildCooperate_HadDrawRewardPointList();
            proto.setLastRefreshTimeMs(getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.GUILD_COOPERATE_DRAW_RECORD_LAST_REFRESH_TIME_MS));
            proto.getHadDrawList().addAll(_m_drawnRewardPointList);
            return proto;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充初始化协议数据
     * @param _proto
     */
    public void fillProto(GS2GC_002_075_RetGuildCooperateInit _proto)
    {
        getUserData().lockUser();
        try
        {
            _proto.setHadDrawRewardPointList(makeDrawnRewardPointProto());
            for (GuildCooperateHeroUseRecord heroUseRecord : _m_usedHeroList)
            {
                _proto.addHeroUseInfoList(heroUseRecord.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 恢复所有大臣使用次数
     * @param context 玩家操作上下文
     */
    public void recoverAllHeroUseRecords(NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            for (GuildCooperateHeroUseRecord heroUseRecord : _m_usedHeroList)
            {
                heroUseRecord.recoverUse(getUSServer().getBM());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除所有奖励领取记录
     *
     * 执行流程：
     * 1. 清空内存缓存列表
     * 2. 删除数据库中所有相关记录
     * 3. 重置刷新时间记录
     * 4. 推送数据变更通知
     *
     * @param context 玩家操作上下文
     */
    public void clearAllDrawRecords(NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 清空缓存列表
            _m_drawnRewardPointList.clear();

            // 删除数据库记录
            getUSServer().getBM().getBM(PlayerGuildCooperateDrawRecordBO.class).delAll("cid", getUserData().getCid());

            // 推送数据变更通知
            getUserData().sendMsgToGC(US2GCWriter_032_GuildOp.make_078_OnHadDrawRewardPointListChg(makeDrawnRewardPointProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
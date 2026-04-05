package NPUSServer.NPUSUserMgr.UserComp.RushExchangeComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RushExchangeObj.RushExchange_Info;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.RushExchangeErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RushExchange.RefRushExchange;
import NPGameRes.Refs.RushExchange.RefRushExchangeGroup;
import NPGameRes.Refs.RushExchange.RefRushExchangeItem;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerRushExchangeBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 限时兑换组件
 * <p>
 * 主要功能：
 * 1. 管理玩家的限时兑换数据
 * 2. 处理兑换请求（支持钻石补充）
 * 3. 处理礼包刷新轮换
 * 4. 处理奖励领取
 * 5. 次数重置和时间管理
 * <p>
 * 设计特点：
 * - 单个数据记录管理
 * - 懒加载数据库插入
 * - 礼包组不放回随机
 * - 支持钻石补充不足道具
 * - 自动时间管理
 * <p>
 * 线程安全：通过玩家锁保证线程安全
 */
public class RushExchangeComponent extends _ANPUserComponent
{
    // 数据库ID，0表示未插入数据库
    private long _m_dbId;

    // 礼包组ID
    private long _m_groupId;

    // 当前礼包配置ID
    private long _m_refId;

    // 当前兑换开始时间（如果没兑换影响刷新礼包时间）
    private long _m_activeTimeMs;

    // 兑换时间（影响什么时候可以领奖）
    private long _m_exchangeTimeMs;

    // 是否已领奖
    private boolean _m_isRewarded;

    // 当天兑换次数
    private int _m_todayExchangeCount;

    // 下次刷新兑换次数时间
    private long _m_nextResetCountTimeMs;

    // 已使用过的礼包ID列表（用于不放回随机，需要持久化）
    private List<Long> _m_usedRefIdList;

    /**
     * 构造函数
     * @param _userData 玩家数据
     */
    public RushExchangeComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.RUSH_EXCHANGE);

        _m_dbId = 0;
        _m_usedRefIdList = new ArrayList<>();
    }

    /**
     * 组件初始化
     * 异步加载数据库数据，初始化完成后设置组件为已初始化状态
     */
    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("rush_exchange_init");

        // 加载数据库数据
        process.addResDelegateProcess(
                action -> _initDataFromDB(action::dealAction),
                "init_data",
                () -> USLog.error(getUSServer(), "RushExchangeComponent._init - load data failed, cid={}", getUserData().getCid()),
                false);

        // 开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "RushExchangeComponent._init - process stopped, cid={}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 从数据库加载数据
     * @param _handler 回调处理器
     */
    private void _initDataFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerRushExchangeBO.class).findAll(
                "cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerRushExchangeBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerRushExchangeBO> _boList)
                    {
                        if (!_boList.isEmpty())
                        {
                            // 从数据库加载数据
                            PlayerRushExchangeBO bo = _boList.get(0);
                            loadFromBO(bo);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 从BO对象加载数据
     * @param _bo 数据库BO对象
     */
    private void loadFromBO(PlayerRushExchangeBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_groupId = _bo.getGroupId();
        _m_refId = _bo.getRefId();
        _m_activeTimeMs = _bo.getActiveTimeMs();
        _m_exchangeTimeMs = _bo.getExchangeTimeMs();
        _m_isRewarded = _bo.getIsRewarded();
        _m_todayExchangeCount = _bo.getTodayExchangeCount();
        _m_nextResetCountTimeMs = _bo.getNextResetCountTimeMs();

        // 反序列化已使用的礼包ID列表
        String usedRefIdsStr = _bo.getUsedRefIds();
        if (usedRefIdsStr != null && !usedRefIdsStr.isEmpty())
        {
            _m_usedRefIdList = CommonFunc.listLongFromString(usedRefIdsStr);
        } else
        {
            _m_usedRefIdList = new ArrayList<>();
        }
    }

    /**
     * 尝试在数据库创建记录（懒加载）
     * @return true=首次创建，false=已存在
     */
    private boolean tryCreateInDB()
    {
        if (_m_dbId != 0)
            return false; // 已插入

        // 创建BO对象并插入数据库
        PlayerRushExchangeBO bo = new PlayerRushExchangeBO();
        bo.setCid(getUSServer().getBM(), getUserData().getCid());
        bo.setGroupId(getUSServer().getBM(), _m_groupId);
        bo.setRefId(getUSServer().getBM(), _m_refId);
        bo.setActiveTimeMs(getUSServer().getBM(), _m_activeTimeMs);
        bo.setExchangeTimeMs(getUSServer().getBM(), _m_exchangeTimeMs);
        bo.setIsRewarded(getUSServer().getBM(), _m_isRewarded);
        bo.setTodayExchangeCount(getUSServer().getBM(), _m_todayExchangeCount);
        bo.setNextResetCountTimeMs(getUSServer().getBM(), _m_nextResetCountTimeMs);
        bo.setUsedRefIds(getUSServer().getBM(), CommonFunc.list2String(_m_usedRefIdList, ';'));
        bo.insert(getUSServer().getBM());

        _m_dbId = bo.getId();
        return true; // 首次插入返回true
    }

    /**
     * 组件依赖
     * @return null表示无依赖
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
        tryRefresh();
    }

    /**
     * 组件销毁
     */
    @Override
    public void dispose()
    {
        // 无需清理
    }

    /**
     * 判断是否可以刷新礼包
     * 检查是否有进行中的兑换、是否达到刷新时间、次数限制等条件
     * @return 可刷新返回SUCC，否则返回相应错误码
     */
    private Result canRefresh()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 有进行中的兑换，不刷新
        if (_m_exchangeTimeMs != 0)
        {
            if (!_m_isRewarded)
            {
                return RushExchangeErr.EXCHANGE_IN_PROGRESS;
            } else
            {
                //判断是否达到当天兑换上限
                int todayExchangeCount = nowTimeMS > _m_nextResetCountTimeMs ? 0 : _m_todayExchangeCount;
                if (todayExchangeCount >= getMaxExchangeCountPerDay())
                {
                    return RushExchangeErr.EXCHANGE_COUNT_NOT_ENOUGH;
                } else
                {
                    return Result.SUCC;
                }
            }
        } else
        {
            if (_m_groupId == 0)
            {
                return Result.SUCC;
            } else
            {
                if (nowTimeMS > _m_activeTimeMs + getRefreshIntervalSec() * 1000L)
                {
                    return Result.SUCC;
                } else
                {
                    return RushExchangeErr.EXCHANGE_CANNOT_REFRESH;
                }
            }
        }
    }

    /**
     * 执行刷新礼包逻辑
     * 验证礼包组有效性，使用不放回随机选择礼包，更新状态并持久化
     * @param _nowTimeMS 当前时间毫秒
     * @return true=刷新成功，false=无可用礼包组
     */
    public boolean doRefresh(long _nowTimeMS)
    {
        boolean needChgGroup = false;

        // 获取礼包组配置
        RefRushExchangeGroup groupRef = RefRushExchangeGroup.getMgr().get(_m_groupId);
        List<RefRushExchange> refList = RefRushExchange.getMgr().getListByGroupId(_m_groupId);
        if (groupRef != null)
        {
            if (refList == null || refList.isEmpty()
                    || !NPPlayerConditionDealerMgr.IsEnable(groupRef.condition, getUserData(), null))
            {
                needChgGroup = true;
            }
        } else
        {
            needChgGroup = true;
        }

        // 如果需要更换礼包组，随机选择一个新的礼包组
        if (needChgGroup || _m_groupId == 0)
        {
            List<RefRushExchangeGroup> allGroupRefs = RefRushExchangeGroup.getMgr().getList();

            List<List<RefRushExchange>> validGroupRefs = new ArrayList<>();

            for (RefRushExchangeGroup refGroup : allGroupRefs)
            {
                List<RefRushExchange> tempRefList = RefRushExchange.getMgr().getListByGroupId(refGroup.group_id);

                if (tempRefList != null && !tempRefList.isEmpty()
                        && NPPlayerConditionDealerMgr.IsEnable(refGroup.condition, getUserData(), null))
                {
                    validGroupRefs.add(tempRefList);
                }
            }

            if (validGroupRefs.isEmpty())
                return false;

            refList = CommonFunc.randSelect(validGroupRefs);
            _m_usedRefIdList.clear();
        }

        // 从refList中选择一个未使用过的礼包
        List<RefRushExchange> candidateRefs = new ArrayList<>();
        for (RefRushExchange ref : refList)
        {
            if (!_m_usedRefIdList.contains(ref.id))
            {
                candidateRefs.add(ref);
            }
        }

        // 如果所有礼包都使用过，清空已使用列表重新选择
        if (candidateRefs.isEmpty())
        {
            _m_usedRefIdList.clear();
            candidateRefs.addAll(refList);
        }

        RefRushExchange selectedRef = CommonFunc.randSelect(candidateRefs);

        // 更新当前礼包信息
        _m_refId = selectedRef.id;
        _m_groupId = selectedRef.group_id;

        // 计算新的激活时间（按上一个激活时间的整数倍计算）
        long refreshIntervalMs = getRefreshIntervalSec() * 1000L;
        if (_m_activeTimeMs == 0 || needChgGroup) {
            // 首次刷新或更换礼包组，使用当前时间作为基准
            _m_activeTimeMs = CommonFunc.getTodayZeroClockMS(0);
        }

        // 非首次刷新，计算从上次激活时间开始的整数倍刷新周期
        long passedTime = _nowTimeMS - _m_activeTimeMs;
        long passedPeriods = passedTime / refreshIntervalMs; // 向上取整
        _m_activeTimeMs = _m_activeTimeMs + passedPeriods * refreshIntervalMs;

        _m_usedRefIdList.add(selectedRef.id);
        _m_exchangeTimeMs = 0;
        _m_isRewarded = false;

        // 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("ref_id", _m_refId);
            updateValue.addValueObj("group_id", _m_groupId);
            updateValue.addValueObj("active_time_ms", _m_activeTimeMs);
            updateValue.addValueObj("used_ref_ids", CommonFunc.list2String(_m_usedRefIdList, ';'));
            updateValue.addValueObj("exchange_time_ms", _m_exchangeTimeMs);
            updateValue.addValueObj("is_rewarded", _m_isRewarded ? 1 : 0);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 推送变更协议
        pushInfoChange();

        return true;
    }

    /**
     * 重置每日兑换次数
     */
    public void checkResetDailyCount()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        if (nowTimeMS > _m_nextResetCountTimeMs)
        {
            _m_todayExchangeCount = 0;
            _m_nextResetCountTimeMs = CommonFunc.getNextZeroClockTimeMS();

            // 保存数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("today_exchange_count", _m_todayExchangeCount);
                updateValue.addValueObj("next_reset_count_time_ms", _m_nextResetCountTimeMs);
                getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送变更协议
            pushInfoChange();
        }
    }

    /**
     * 请求兑换
     * 检查次数限制、礼包配置，扣除道具（支持钻石补充），记录兑换时间和次数，保存数据并推送变更
     *
     * @param _useGemSupplement 是否使用钻石补充不足道具
     * @param _context 操作上下文
     * @return 兑换结果
     *
     * 线程安全：方法外部已通过玩家锁保护
     */
    public Result requestExchange(boolean _useGemSupplement, NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 1. 检查次数是否达到上限
        int maxExchangeCount = getMaxExchangeCountPerDay();
        if ((nowTimeMS > _m_nextResetCountTimeMs ? 0 : _m_todayExchangeCount) >= maxExchangeCount)
            return RushExchangeErr.EXCHANGE_COUNT_NOT_ENOUGH;

        // 2. 检查是否有进行中的兑换
        if (_m_exchangeTimeMs != 0)
            return RushExchangeErr.EXCHANGE_IN_PROGRESS;

        // 3. 获取当前礼包配置
        RefRushExchange exchangeRef = RefRushExchange.getMgr().get(_m_refId);
        if (exchangeRef == null)
        {
            USLog.error(getUSServer(), "RushExchangeComponent.requestExchange - ref not found: cid={}, refId={}",
                    getUserData().getCid(), _m_refId);
            return CommErr.REF_NOT_FOUND;
        }

        // 4. 检查道具配置
        List<NPCommonCostItem> costList = exchangeRef.cost_list;
        if (costList == null || costList.isEmpty())
            return CommErr.REF_NOT_FOUND;

        // 5. 计算实际消耗列表（根据是否使用钻石补充）
        List<NPCommonCostItem> actualCostList;
        if (_useGemSupplement)
        {
            // 使用钻石补充，计算完整消耗列表（包含道具和钻石）
            actualCostList = calculateCostWithGemSupplement(costList);
            if (actualCostList == null)
            {
                // 有道具无法用钻石补充
                return CommErr.ITEM_NOT_ENOUGH;
            }
        } else
        {
            // 不使用钻石补充，直接使用原始消耗列表
            actualCostList = costList;
        }

        // 6. 扣除道具（或道具+钻石）
        boolean spendSuccess = getUserData().spendItem(actualCostList, _context);
        if (!spendSuccess)
            return CommErr.CONSUME_FAIL;

        // 8. 设置兑换时间
        _m_exchangeTimeMs = nowTimeMS;

        // 9. 增加兑换次数
        if (nowTimeMS > _m_nextResetCountTimeMs)
        {
            // 超过重置时间，重置次数和下次重置时间
            _m_todayExchangeCount = 0;
            _m_nextResetCountTimeMs = CommonFunc.getNextZeroClockTimeMS();
        }
        _m_todayExchangeCount++;

        // 10. 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("exchange_time_ms", _m_exchangeTimeMs);
            updateValue.addValueObj("today_exchange_count", _m_todayExchangeCount);
            updateValue.addValueObj("next_reset_count_time_ms", _m_nextResetCountTimeMs);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 11. 推送变更协议
        pushInfoChange();

        return Result.SUCC;
    }

    /**
     * 计算带钻石补充的完整消耗列表
     * 对于不足的道具，查询钻石价值表计算需要的钻石数量，将玩家拥有的道具+补充钻石合并为完整消耗列表
     *
     * @param _costList 需要消耗的道具列表
     * @return 完整消耗列表（包含道具和钻石），如果有道具无法补充则返回null
     *
     * 线程安全：只读操作，无需加锁
     */
    private List<NPCommonCostItem> calculateCostWithGemSupplement(List<NPCommonCostItem> _costList)
    {
        // 结果列表
        List<NPCommonCostItem> resultList = new ArrayList<>();
        long totalGemCost = 0;

        for (NPCommonCostItem costItem : _costList)
        {
            // 获取玩家拥有的数量
            long ownCount = getUserData().getItemCount(costItem);
            long needCount = costItem.getCount();

            if (ownCount >= needCount)
            {
                // 玩家道具足够，直接加入消耗列表
                resultList.add(costItem);
            } else
            {
                // 道具不足，需要钻石补充
                long lackCount = needCount - ownCount;

                // 查找道具的钻石价值配置
                RefRushExchangeItem itemRef = RefRushExchangeItem.getMgr().lookupByItem(costItem);
                if (itemRef == null)
                {
                    // 该道具无法用钻石补充，返回null
                    USLog.error(getUSServer(),
                            "RushExchangeComponent.calculateCostWithGemSupplement - item cannot be supplemented with gem: itemType={}, itemId={}",
                            costItem.getItemType(), costItem.getItemId());
                    return null;
                }

                // 如果玩家有部分道具，加入消耗列表
                if (ownCount > 0)
                {
                    NPCommonCostItem ownItem = new NPCommonCostItem(costItem.getItemType(), costItem.getItemId(), ownCount);
                    resultList.add(ownItem);
                }

                // 累加需要补充的钻石
                totalGemCost += lackCount * itemRef.gem_count;
            }
        }

        // 如果需要钻石补充，添加钻石消耗项
        if (totalGemCost > 0)
        {
            NPCommonCostItem gemItem = new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), totalGemCost);
            resultList.add(gemItem);
        }

        return resultList;
    }

    /**
     * 请求刷新礼包
     * 检查刷新条件，执行礼包刷新并推送变更
     * @return 刷新结果
     */
    public Result tryRefresh()
    {
        // 先检查并重置每日兑换次数
        checkResetDailyCount();

        // 检查是否可以刷新
        Result canRefreshResult = canRefresh();
        if (!canRefreshResult.isSucc())
            return canRefreshResult;

        // 执行刷新
        if (!doRefresh(CommonFunc.getNowTimeMS()))
            return CommErr.REF_NOT_FOUND;

        return Result.SUCC;
    }

    /**
     * 请求领取奖励
     * 检查兑换记录和等待时间，发放奖励，标记已领取，保存数据，推送变更，尝试刷新下一个礼包
     * @param _context 操作上下文
     * @return 领取结果
     */
    public Result requestClaimReward(NPPlayerContext _context)
    {
        // 1. 检查是否有兑换记录
        if (_m_exchangeTimeMs == 0)
            return RushExchangeErr.EXCHANGE_NOT_FOUND;

        // 2. 检查是否已经领奖
        if (_m_isRewarded)
            return RushExchangeErr.EXCHANGE_ALREADY_REWARDED;

        // 3. 检查是否到达可领奖时间
        long currentTimeMs = CommonFunc.getNowTimeMS();
        long waitSec = getExchangeWaitSec();
        long canClaimTimeMs = _m_exchangeTimeMs + waitSec * 1000L;

        if (currentTimeMs < canClaimTimeMs)
            return RushExchangeErr.EXCHANGE_TIME_NOT_REACH;

        // 4. 获取奖励配置并发放
        RefRushExchange exchangeRef = RefRushExchange.getMgr().get(_m_refId);
        if (exchangeRef == null || exchangeRef.reward_list == null || exchangeRef.reward_list.isEmpty())
        {
            USLog.error(getUSServer(), "RushExchangeComponent.requestClaimReward - reward not found: cid={}, refId={}",
                    getUserData().getCid(), _m_refId);
            return CommErr.REF_NOT_FOUND;
        }

        // 发放奖励
        getUserData().gainItemList(exchangeRef.reward_list, _context);

        // 5. 标记已领奖
        _m_isRewarded = true;

        // 6. 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("is_rewarded", _m_isRewarded ? 1 : 0);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 7. 推送变更协议
        pushInfoChange();

        // 8. 尝试刷新下一个礼包
        tryRefresh();

        return Result.SUCC;
    }

    /**
     * 推送信息变更协议
     */
    private void pushInfoChange()
    {
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_075_OnRushExchangeChg(toProto()));
    }

    /**
     * 转换为协议对象
     * @return 协议对象
     */
    public RushExchange_Info toProto()
    {
        RushExchange_Info info = new RushExchange_Info();
        info.setGroupId(_m_groupId);
        info.setRefId(_m_refId);
        info.setActiveTimeMs(_m_activeTimeMs);
        info.setExchangeTimeMs(_m_exchangeTimeMs);
        info.setIsRewarded(_m_isRewarded);
        info.setTodayExchangeCount(_m_todayExchangeCount);
        info.setNextResetCountTimeMs(_m_nextResetCountTimeMs);
        return info;
    }

    // ========== 配置读取方法 ==========

    /**
     * 获取每日最大兑换次数
     * 从RefGeneral配置表读取每日可兑换次数
     * @return 每日最大兑换次数
     */
    private int getMaxExchangeCountPerDay()
    {
        return RefGeneral.Ref().rush_exchange_day_can_exchange_times;
    }

    /**
     * 获取兑换完成所需等待时间（秒）
     * 从RefGeneral配置表读取兑换后需要等待的时间
     * @return 等待时间（秒）
     */
    private int getExchangeWaitSec()
    {
        return RefGeneral.Ref().rush_exchange_done_need_wait_sec;
    }

    /**
     * 获取礼包刷新间隔时间（秒）
     * 从RefGeneral配置表读取礼包自动刷新的时间间隔
     * @return 刷新间隔（秒）
     */
    private int getRefreshIntervalSec()
    {
        return RefGeneral.Ref().rush_exchange_refresh_sec;
    }

    // ========== GM命令支持方法 ==========

    /**
     * GM命令：设置兑换完成，可以马上领奖
     * 将兑换时间设置为很早的时间，使玩家可以立即领取奖励
     * @return 操作结果
     */
    public Result gmSetExchangeDone()
    {
        // 检查是否有兑换记录
        if (_m_exchangeTimeMs == 0)
            return RushExchangeErr.EXCHANGE_NOT_FOUND;

        // 检查是否已经领奖
        if (_m_isRewarded)
            return RushExchangeErr.EXCHANGE_ALREADY_REWARDED;

        // 将兑换时间设置为很早的时间（当前时间 - 等待时间 - 1小时）
        long waitSec = getExchangeWaitSec();
        _m_exchangeTimeMs = CommonFunc.getNowTimeMS() - waitSec * 1000L - 3600000L;

        // 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("exchange_time_ms", _m_exchangeTimeMs);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 推送变更协议
        pushInfoChange();

        return Result.SUCC;
    }

    /**
     * GM命令：直接发起兑换（不消耗道具）
     * 跳过道具消耗检查，直接发起兑换
     * @param _context 操作上下文
     * @return 兑换结果
     */
    public Result gmExchangeNoCost(NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 1. 检查次数是否达到上限
        int maxExchangeCount = getMaxExchangeCountPerDay();
        if ((nowTimeMS > _m_nextResetCountTimeMs ? 0 : _m_todayExchangeCount) >= maxExchangeCount)
            return RushExchangeErr.EXCHANGE_COUNT_NOT_ENOUGH;

        // 2. 检查是否有进行中的兑换
        if (_m_exchangeTimeMs != 0)
            return RushExchangeErr.EXCHANGE_IN_PROGRESS;

        // 3. 获取当前礼包配置（仅用于验证）
        RefRushExchange exchangeRef = RefRushExchange.getMgr().get(_m_refId);
        if (exchangeRef == null)
        {
            USLog.error(getUSServer(), "RushExchangeComponent.gmExchangeNoCost - ref not found: cid={}, refId={}",
                    getUserData().getCid(), _m_refId);
            return CommErr.REF_NOT_FOUND;
        }

        // 4. 设置兑换时间（不扣除道具）
        _m_exchangeTimeMs = nowTimeMS;

        // 5. 增加兑换次数
        if (nowTimeMS > _m_nextResetCountTimeMs)
        {
            // 超过重置时间，重置次数和下次重置时间
            _m_todayExchangeCount = 0;
            _m_nextResetCountTimeMs = CommonFunc.getNextZeroClockTimeMS();
        }
        _m_todayExchangeCount++;

        // 6. 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("exchange_time_ms", _m_exchangeTimeMs);
            updateValue.addValueObj("today_exchange_count", _m_todayExchangeCount);
            updateValue.addValueObj("next_reset_count_time_ms", _m_nextResetCountTimeMs);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 7. 推送变更协议
        pushInfoChange();

        return Result.SUCC;
    }

    /**
     * GM命令：清除当天兑换次数
     * 将今日兑换次数重置为0，并更新下次重置时间
     */
    public void gmClearTodayCount()
    {
        _m_todayExchangeCount = 0;
        _m_nextResetCountTimeMs = CommonFunc.getNextZeroClockTimeMS();

        // 保存数据库
        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("today_exchange_count", _m_todayExchangeCount);
            updateValue.addValueObj("next_reset_count_time_ms", _m_nextResetCountTimeMs);
            getUSServer().getBM().getBM(PlayerRushExchangeBO.class).update("id", _m_dbId, updateValue);
        }

        // 推送变更协议
        pushInfoChange();
    }

    public long getGroupId()
    {
        return _m_groupId;
    }

    public long getRefId()
    {
        return _m_refId;
    }
}

package NPUSServer.NPUSUserMgr.UserComp.InnComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.InnObj.*;
import MJLog.MJEventLog;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Game.CS.CSSyncRandom;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPair;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.Inn.RefInnLevel;
import NPGameRes.Refs.Inn.RefInnMedalLevel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_INN_ADD_GUEST;
import NPUSServer.Common.Event.Events.Event_P_INN_POPULARITY_CHG;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish.InnDishInfo;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish.InnDishMgr;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Guest.InnGuestInfo;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Guest.InnGuestMgr;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.SpecialGuest.InnSpecialGuestMgr;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Station.InnStationInfo;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Station.InnStationMgr;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerInnBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class InnComponent extends _ANPUserComponent
{
    private PlayerInnBO _m_bo;

    private RefInnLevel _m_levelRef;
    private RefInnMedalLevel _m_medalLevelRef;

    private NPPlayerPropertyContainer _m_propContainer;

    // 各种管理器
    private InnStationMgr _m_stationMgr;
    private InnDishMgr _m_dishMgr;
    private InnGuestMgr _m_guestMgr;
    private InnSpecialGuestMgr _m_specialGuestMgr;

    // 接待客人列表
    private long _m_hadReceiveGuestNum;
    // 接待客人的详细信息
    private List<Inn_ReceiveInfo> _m_receiveList;

    private CSSyncRandom _m_syncRandom;

    public InnComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.INN);

        _m_propContainer = new NPPlayerPropertyContainer();

        // 初始化管理器
        _m_stationMgr = new InnStationMgr(this);
        _m_dishMgr = new InnDishMgr(this);
        _m_guestMgr = new InnGuestMgr(this);
        _m_specialGuestMgr = new InnSpecialGuestMgr(this);

        _m_receiveList = new ArrayList<>();

        _m_syncRandom = new CSSyncRandom(getUserData().getCid());
    }

    @Override
    protected void _init()
    {
        // 使用ALProcess进行多步骤初始化
        ALProcess process = ALProcess.CreateProcess("inn_component_init");

        // 步骤1: 加载旅店主数据
        process.addResDelegateProcess(_action -> _initInnBo(_action::dealAction),
                "init_inn_bo", null, false);

        // 步骤2: 加载设施数据
        process.addResDelegateProcess(_action -> _m_stationMgr.init(_action::dealAction),
                "init_inn_station", null, false);

        // 步骤3: 加载菜品数据
        process.addResDelegateProcess(_action -> _m_dishMgr.init(_action::dealAction),
                "init_inn_dish", null, false);

        // 步骤4: 加载客人数据
        process.addResDelegateProcess(_action -> _m_guestMgr.init(_action::dealAction),
                "init_inn_guest", null, false);

        // 步骤5: 加载特殊客人数据
        process.addResDelegateProcess(_action -> _m_specialGuestMgr.init(_action::dealAction),
                "init_inn_special_guest", null, false);

        // 执行初始化流程
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }

            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "Player {} inn component init failed", getUserData().getCid());
                getUserData().setDataLoadFail();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return new ENPPlayerCompType[]{ENPPlayerCompType.PLAYER_COMP};
    }

    @Override
    public void onInited()
    {
        _m_stationMgr.checkDefaultStations();
        _m_dishMgr.checkDefaultDishes();
        _m_guestMgr.checkDefaultUnlockGuest();
    }

    @Override
    public void dispose()
    {

    }

    /**
     * 初始化旅店BO数据
     */
    private void _initInnBo(final _ICallBackBool _callBack)
    {
        getUSServer().getBM().getBM(PlayerInnBO.class).findOne("cid", getUserData().getCid(),
                new _ASelectCallback<PlayerInnBO>()
                {
                    @Override
                    public void dealSuc(PlayerInnBO bo)
                    {
                        _setBo(bo, _callBack);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            // 数据库错误
                            USLog.error(getUSServer(), "Player {} inn bo load failed", getUserData().getCid());
                            _callBack.onRunOver(false);
                            return;
                        }

                        // 数据不存在，创建默认数据
                        PlayerInnBO bo = new PlayerInnBO();
                        bo.setCid(getUSServer().getBM(), getUserData().getCid());
                        bo.setLevel(getUSServer().getBM(), 0);  // 默认等级0
                        bo.setMedalLevel(getUSServer().getBM(), 0);  // 默认奖牌等级0
                        bo.setPopularity(getUSServer().getBM(), 0);  // 默认人气值0
                        bo.insert(getUSServer().getBM());

                        _setBo(bo, _callBack);
                    }
                });
    }

    /**
     * 设置旅店BO数据
     * @param _bo
     * @param _callBack
     */
    private void _setBo(PlayerInnBO _bo, _ICallBackBool _callBack)
    {
        _m_bo = _bo;
        _m_levelRef = RefInnLevel.getMgr().get(_m_bo.getLevel());
        _m_medalLevelRef = RefInnMedalLevel.getMgr().get(_m_bo.getMedalLevel());

        // 设置旅店等级属性
        _m_propContainer.setValue(ENPPlayerPropertyType.INN_RECEIVE_GUEST_LIMIT,
                _m_levelRef != null ? _m_levelRef.receive_guest_limit : 0);
        // 设置奖牌等级属性
        getUserData().getBonusMgr().addTotalModifier(_m_medalLevelRef != null ? _m_medalLevelRef.bonus_prop_modifier : null);

        // 读取接待列表
        Inn_ReceiveList receiveList = new Inn_ReceiveList();
        if (_m_bo.getReceiveList() != null)
        {
            receiveList.readPackage(ByteBuffer.wrap(_m_bo.getReceiveList()));

            _m_receiveList = receiveList.getReceiveList();
            _m_hadReceiveGuestNum = receiveList.getHadBeenReceiveNum();
        }

        _callBack.onRunOver(true);
    }

    private static int getReceiveCostMs()
    {
        return Math.max(1, RefGeneral.Ref().inn_receive_cost_sec) * 1000;
    }

    /**
     * 获取旅店数据BO
     */
    public PlayerInnBO getInnBO()
    {
        return _m_bo;
    }

    /**
     * 获取当前人气值
     */
    public long getPopularity()
    {
        return _m_bo.getPopularity();
    }

    /**
     * 获取当前旅店等级
     */
    public int getInnLevel()
    {
        return _m_bo.getLevel();
    }

    /**
     * 获取当前奖牌等级
     */
    public int getMedalLevel()
    {
        return _m_bo.getMedalLevel();
    }

    /**
     * 获取设施管理器
     */
    public InnStationMgr getStationMgr()
    {
        return _m_stationMgr;
    }

    /**
     * 获取菜品管理器
     */
    public InnDishMgr getDishMgr()
    {
        return _m_dishMgr;
    }

    /**
     * 获取客人管理器
     */
    public InnGuestMgr getGuestMgr()
    {
        return _m_guestMgr;
    }

    /**
     * 获取特殊客人管理器
     */
    public InnSpecialGuestMgr getSpecialGuestMgr()
    {
        return _m_specialGuestMgr;
    }

    public NPPlayerPropertyContainer getPlayerPropertyContainer()
    {
        return _m_propContainer;
    }

    protected void _lock()
    {
        getUserData().lockUser();
    }

    protected void _unlock()
    {
        getUserData().unlockUser();
    }

    /**
     * 获得人气值
     * @param addPopularity 增加的人气值
     * @param _context
     * @return 是否成功
     */
    public void addPopularity(long addPopularity, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (addPopularity <= 0)
                return;

            long newPopularity = _m_bo.getPopularity() + addPopularity;
            _m_bo.savePopularity(getUSServer().getBM(), newPopularity);

            // 推送到客户端
            getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_058_OnInnPopularityChg(newPopularity));

            // 获得人气值后检查旅店升级
            checkInnLevelUp();

            //触发事件
            Event_P_INN_POPULARITY_CHG event = new Event_P_INN_POPULARITY_CHG(_context, newPopularity);
            getUserData().onLogicEvent(event);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查旅店升级
     * @return 是否有升级
     */
    public void checkInnLevelUp()
    {
        _lock();
        try
        {
            int newLevel = _m_bo.getLevel();
            long currentPopularity = _m_bo.getPopularity();

            RefInnLevel newLevelRef = null;

            // 检查是否可以升级到更高等级
            while (true)
            {
                int nextLevel = newLevel + 1;

                // 如果没有下一级配置，说明已达到最高等级
                RefInnLevel nextLevelRef = RefInnLevel.getMgr().get(nextLevel);
                if (nextLevelRef == null)
                    break;

                // 检查是否满足升级条件
                if (currentPopularity >= nextLevelRef.need_popularity)
                {
                    newLevel = nextLevel;
                    newLevelRef = nextLevelRef;
                } else
                {
                    break;
                }
            }

            if (newLevelRef == null)
                return;

            _m_levelRef = newLevelRef;
            // 更新旅店等级
            _m_bo.setLevel(getUSServer().getBM(), newLevel);
            if (_m_bo.getFirstTimeUpgradeTimeMs() == 0)
            {
                _m_bo.setFirstTimeUpgradeTimeMs(getUSServer().getBM(), CommonFunc.getNowTimeMS());
                // 推送到客户端
                getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_063_OnInnFirstTimeUpgradeTimeMsChg(_m_bo.getFirstTimeUpgradeTimeMs()));
            }
            _m_bo.saveAllMarked(getUSServer().getBM());

            // 设置旅店等级属性
            _m_propContainer.setValue(ENPPlayerPropertyType.INN_RECEIVE_GUEST_LIMIT, newLevelRef.receive_guest_limit);
            // 推送到客户端
            getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_057_OnInnLevelChg(newLevel));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 奖牌升级
     * @return 升级结果
     */
    public Result medalUpgrade(NPPlayerContext _context)
    {
        _lock();
        try
        {
            int currentMedalLevel = _m_bo.getMedalLevel();
            int innLevel = _m_bo.getLevel();

            int nextMedalLevel = currentMedalLevel + 1;

            // 如果没有下一级配置，说明已达到最高等级
            RefInnMedalLevel nextMedalRef = RefInnMedalLevel.getMgr().get(nextMedalLevel);
            if (nextMedalRef == null)
                return InnErr.INN_MEDAL_LEVEL_MAX;

            // 检查是否满足升级条件（需要达到指定的旅店等级）
            if (innLevel < nextMedalRef.need_inn_level)
                return InnErr.INN_LEVEL_NOT_ENOUGH;

            _chgMedalLevel(nextMedalLevel, nextMedalRef);

            // 记录日志（勋章ID固定为1）
            MJEventLog.logInnUpdate(getUserData(), 1,
                    currentMedalLevel, nextMedalLevel);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 作弊修改奖牌等级
     * @param _level
     * @param _context
     */
    public Result cheatSetMedalLevel(int _level, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_level <= 0)
                return CommErr.PARAM_ERROR;

            RefInnMedalLevel refLevel = RefInnMedalLevel.getMgr().get(_level);
            if (refLevel == null)
                return CommErr.PARAM_ERROR;

            // 执行修改
            _chgMedalLevel(_level, refLevel);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 修改等级
     * @param _nextMedalLevel
     * @param _nextMedalRef
     */
    private void _chgMedalLevel(int _nextMedalLevel, RefInnMedalLevel _nextMedalRef)
    {
        _lock();
        try
        {
            // 执行升级
            _m_bo.saveMedalLevel(getUSServer().getBM(), _nextMedalLevel);

            // 设置奖牌等级属性
            getUserData().getBonusMgr().replaceTotal(
                    _m_medalLevelRef != null ? _m_medalLevelRef.bonus_prop_modifier : null,
                    _nextMedalRef.bonus_prop_modifier);

            _m_medalLevelRef = _nextMedalRef;

            // 推送到客户端
            getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_059_OnInnMedalLevelChg(_nextMedalLevel));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 接待客人
     */
    public Result receiveGuest(boolean _isAKey, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 检查是否有客人可以接待
            if (!_m_guestMgr.hasGuest())
                return InnErr.INN_NO_GUEST_CAN_RECEIVE;

            // 检查是否有菜品
            if (_m_dishMgr.getHadUnlockDishNum() <= 0)
                return InnErr.INN_NO_DISH_CAN_RECEIVE;

            // 需要接待的客人数量
            int needReceiveCount = 1;

            // 如果是一键接待
            if (_isAKey)
            {
                // 检查是否满足一键接待条件
                if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().inn_one_key_receive_guest_unlock_id, getUserData(), null))
                    return CommErr.CONDITION_NOT_ENABLE;

                // 获取当前的CD数量
                PlayerLazyCD lazyCd = getUserData().getLazyCDComponent().lookupCD(RefGeneral.Ref().inn_receive_guest_lazy_cd_id);
                if (lazyCd != null)
                    needReceiveCount = lazyCd.getCount();
            }

            // 消耗CD
            if (!getUserData().spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().inn_receive_guest_lazy_cd_id, needReceiveCount, _context))
                return InnErr.INN_RECEIVE_CD;

            // 添加接待客人
            Result result = addGuest(needReceiveCount, _context);
            if (!result.isSucc())
                return result;

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取在队列中的客人数量
     * @return
     */
    public long getInLineGuestNum()
    {
        getUserData().lockUser();
        try
        {
            long inLineGuestNum = 0;
            for (Inn_ReceiveInfo receiveInfo : _m_receiveList)
            {
                inLineGuestNum += receiveInfo.getNeedReceiveNum();
            }
            return inLineGuestNum;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取总的客人数量
     * @return
     */
    public long getTotalNeedReceiveGuestNum()
    {
        getUserData().lockUser();
        try
        {
            return _m_hadReceiveGuestNum + getInLineGuestNum();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加接待客人
     *
     * 执行流程：
     * 1. 检查是否有客人可以接待
     * 2. 检查是否有菜品可用
     * 3. 检查队列客人数量是否达到上限
     * 4. 调用核心添加逻辑
     *
     * @param _num 添加的客人数量
     * @param _context 玩家上下文
     * @return 操作结果
     */
    public Result addGuest(int _num, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 检查是否有客人可以接待
            if (!_m_guestMgr.hasGuest())
                return InnErr.INN_NO_GUEST_CAN_RECEIVE;

            // 检查是否有菜品
            if (_m_dishMgr.getHadUnlockDishNum() <= 0)
                return InnErr.INN_NO_DISH_CAN_RECEIVE;

            // 检查队列客人数量是否达到上限
            if (getInLineGuestNum() >= RefGeneral.Ref().inn_inline_guest_num_limit)
                return InnErr.INN_INLINE_GUEST_NUM_LIMIT;

            Inn_ReceiveInfo targetReceiveInfo = null;

            long nowTimeMs = CommonFunc.getNowTimeMS();
            if (!_m_receiveList.isEmpty())
            {
                Inn_ReceiveInfo finalReceiveInfo = _m_receiveList.get(_m_receiveList.size() - 1);
                long receiveStartTimeMs = finalReceiveInfo.getStartTimeMs() + (long) finalReceiveInfo.getNeedReceiveNum() * getReceiveCostMs();
                if (receiveStartTimeMs > nowTimeMs)
                {
                    // 如果最后一个接待信息的开始时间还没有到，则使用最后一个接待信息
                    targetReceiveInfo = finalReceiveInfo;
                    targetReceiveInfo.setNeedReceiveNum(targetReceiveInfo.getNeedReceiveNum() + _num);
                } else
                {
                    // 否则创建新的接待信息
                    targetReceiveInfo = new Inn_ReceiveInfo(nowTimeMs, _num);
                    _m_receiveList.add(targetReceiveInfo);
                }
            } else
            {
                targetReceiveInfo = new Inn_ReceiveInfo(nowTimeMs, _num);
                _m_receiveList.add(targetReceiveInfo);
            }

            _onGuestAdd(_num, _context);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 直接添加接待客人（无检查，立即可结算）
     * <p>
     * 功能：跳过所有前置检查，直接添加指定数量的客人到接待队列
     * 特性：添加的客人立即可结算，无需等待
     * 用途：用于GM命令、玩家效果、新手引导等特殊场景
     *
     * @param _num     添加的客人数量
     * @param _context 玩家上下文
     * @return 操作结果
     */
    public Result addGuestDirectly(int _num, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 计算开始时间：设置为过去时间，让所有客人立即可结算
            // 开始时间 = 当前时间 - (客人数量 * 每个客人耗时)
            long nowTimeMs = CommonFunc.getNowTimeMS();
            long startTimeMs = nowTimeMs - ((long) _num * getReceiveCostMs());

            // 创建新的接待信息，使用过去的时间
            Inn_ReceiveInfo receiveInfo = new Inn_ReceiveInfo(startTimeMs, _num);

            // 维护列表的时间顺序：按开始时间从早到晚排列
            // 由于使用过去时间，通常应该插入到列表开头
            if (_m_receiveList.isEmpty())
            {
                // 列表为空，直接添加
                _m_receiveList.add(receiveInfo);
            }
            else
            {
                // 列表不为空，找到正确的插入位置（保持时间顺序）
                int insertIndex = 0;
                for (int i = 0; i < _m_receiveList.size(); i++)
                {
                    Inn_ReceiveInfo existingInfo = _m_receiveList.get(i);
                    if (startTimeMs < existingInfo.getStartTimeMs())
                    {
                        // 找到第一个开始时间比新记录晚的位置
                        insertIndex = i;
                        break;
                    }
                    insertIndex = i + 1;
                }
                _m_receiveList.add(insertIndex, receiveInfo);
            }

            // 保存到数据库
            _onGuestAdd(_num, _context);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加客人后的处理
     * @param _num
     * @param _context
     */
    private void _onGuestAdd(int _num, NPPlayerContext _context)
    {
        Inn_ReceiveList receiveList = new Inn_ReceiveList(_m_hadReceiveGuestNum, (ArrayList<Inn_ReceiveInfo>) _m_receiveList);
        _m_bo.saveReceiveList(getUSServer().getBM(), receiveList.makePackage().array());

        // 发送接待列表到客户端
        getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_050_OnInnReceiveChg(receiveList));

        // 触发事件
        getUserData().onLogicEvent(new Event_P_INN_ADD_GUEST(_context, _num));
    }

    /**
     * 结算客人
     * @param _needSettleNum 需要结算的客人数量
     * @param _context
     * @return
     */
    public ResultOne<Inn_SettleInfo> settle(int _needSettleNum, NPPlayerContext _context)
    {
        _lock();
        try
        {
            long canSettleGuestNum = getCanSettleGuestNum();
            if (_needSettleNum <= 0 || _needSettleNum > canSettleGuestNum)
                return ResultOne.failed(InnErr.INN_NO_GUEST_CAN_SETTLE);

            // 区间内已处理蓝图次数
            int rangeHadSettleCount = _m_bo.getRangeHadSettleCount();
            int rangeHadGainCount = _m_bo.getRangeHadGainCount();

            // 客人随机池
            WCGPair<WCGPair<Long, Integer>, Long> guestRandomPool = null;
            // 菜品随机池
            WCGPair<WCGPair<Long, Integer>, Long> dishRandomPool = null;

            // 菜品收益缓存
            Map<InnDishInfo, DishBaseProfitInfo> dishBaseProfitMap = new HashMap<>();
            Map<InnDishInfo, Inn_DishSettleInfo> dishSettleInfoMap = new HashMap<>();

            Inn_SettleInfo settleInfo = new Inn_SettleInfo();
            settleInfo.setReceiveNum(_needSettleNum);

            // 遍历结算客人
            for (long i = 1; i <= _needSettleNum; i++)
            {
                long lineUpId = _m_hadReceiveGuestNum + i;

                // 构造客人和菜品随机池
                if (guestRandomPool == null || (guestRandomPool.second != -1 && guestRandomPool.second >= lineUpId))
                    guestRandomPool = _m_guestMgr.getRandomPool(lineUpId);

                if (dishRandomPool == null || (dishRandomPool.second != -1 && dishRandomPool.second >= lineUpId))
                    dishRandomPool = _m_dishMgr.getRandomPool(lineUpId);

                InnGuestInfo guestInfo;
                InnDishInfo dishInfo;

                if (guestRandomPool.first == null || dishRandomPool.first == null)
                {
                    guestInfo = _m_guestMgr.lookupGuestInfo(2001);
                    if (guestInfo == null)
                    {
                        USLog.error(getUSServer(), "Player {} default guest 2001 not found for settlement", getUserData().getCid());
                        continue;
                    }

                    dishInfo = _m_dishMgr.lookupDish(1001);
                    if (dishInfo == null)
                    {
                        USLog.error(getUSServer(), "Player {} default dish 1001 not found for settlement", getUserData().getCid());
                        continue;
                    }
                } else
                {
                    // 随机获取客人和菜品索引
                    int guestIndex;
                    if (lineUpId == guestRandomPool.first.first)
                    {
                        guestIndex = guestRandomPool.first.second;
                    } else
                    {
                        guestIndex = _m_syncRandom.nextInt(lineUpId, guestRandomPool.first.second + 1);
                    }
                    int dishIndex = _m_syncRandom.nextInt(lineUpId, dishRandomPool.first.second + 1);

                    guestInfo = _m_guestMgr.getGuestByIndex(guestIndex);
                    if (guestInfo == null)
                    {
                        USLog.error(getUSServer(), "Player {} guest index {} not found for settlement", getUserData().getCid(), guestIndex);
                        continue;
                    }

                    dishInfo = _m_dishMgr.getUnlockedDishByIndex(dishIndex);
                    if (dishInfo == null)
                    {
                        USLog.error(getUSServer(), "Player {} dish index {} not found for settlement", getUserData().getCid(), dishIndex);
                        continue;
                    }
                }

                // 获取菜品收益
                DishBaseProfitInfo dishBaseProfitInfo = dishBaseProfitMap.get(dishInfo);
                if (dishBaseProfitInfo == null)
                {
                    // 如果没有缓存菜品收益信息，则计算并缓存
                    dishBaseProfitInfo = calDishProfit(dishInfo);
                    dishBaseProfitMap.put(dishInfo, dishBaseProfitInfo);
                }

                // 随机是否可以获得蓝图
                if (rangeHadSettleCount >= RefGeneral.Ref().inn_gain_station_blueprint_limit_interval)
                {
                    // 如果超过了区间限制，则重置结算次数
                    rangeHadSettleCount = 0;
                    rangeHadGainCount = 0;
                }
                // 如果当前区间内还没达到获得上限，则有一定概率获得蓝图
                if (rangeHadGainCount < RefGeneral.Ref().inn_interval_gain_station_blueprint_limit
                        && CommonFunc.randomInt(10000) <= RefGeneral.Ref().inn_receive_gain_station_blueprint_wei)
                {
                    settleInfo.setAddStationBlueprintNum(settleInfo.getAddStationBlueprintNum() + 1);
                    rangeHadGainCount++;
                }
                rangeHadSettleCount++;

                // 更新结算信息
                settleInfo.setAddAffection(dishBaseProfitInfo.getAddAffection() + settleInfo.getAddAffection());
                settleInfo.setAddPopularity(dishBaseProfitInfo.getAddPopularity() + settleInfo.getAddPopularity());

                InnDishInfo finalDishInfo = dishInfo;
                Inn_DishSettleInfo dishSettleInfo = dishSettleInfoMap.computeIfAbsent(dishInfo, k ->
                        new Inn_DishSettleInfo(finalDishInfo.getDishId(), 0, 0));
                dishSettleInfo.setReceiveNum(dishSettleInfo.getReceiveNum() + 1);
                // 计算增加的熟练度
                long addFitness = (long) Math.ceil(dishBaseProfitInfo.getAddFitness() * (10000 + guestInfo.getRef().finesse_add) / 10000d);
                dishSettleInfo.setAddFinesse(dishSettleInfo.getAddFinesse() + addFitness);
            }

            // 获取对应的奖励
            // 蓝图
            getUserData().gainItem(RefGeneral.Ref().inn_station_blueprint_item, settleInfo.getAddStationBlueprintNum(), _context);
            // 心意值
            getUserData().gainItem(RefGeneral.Ref().inn_affection_item, settleInfo.getAddAffection(), _context);
            // 人气值
            addPopularity(settleInfo.getAddPopularity(), _context);
            // 熟练度
            dishSettleInfoMap.forEach((dishInfo, dishSettleInfo) ->
            {
                dishInfo.addFinesse(dishSettleInfo.getAddFinesse(), _context);
                settleInfo.addDishList(dishSettleInfo);
            });

            // 更新接待列表
            _m_hadReceiveGuestNum += _needSettleNum;
            for (Inn_ReceiveInfo receiveInfo : _m_receiveList)
            {
                if (_needSettleNum <= 0)
                    break;

                if (receiveInfo.getNeedReceiveNum() <= _needSettleNum)
                {
                    _needSettleNum -= receiveInfo.getNeedReceiveNum();
                    receiveInfo.setNeedReceiveNum(0);
                }else
                {
                    receiveInfo.setStartTimeMs(receiveInfo.getStartTimeMs() + (long) _needSettleNum * getReceiveCostMs());
                    receiveInfo.setNeedReceiveNum(receiveInfo.getNeedReceiveNum() - _needSettleNum);
                    break;
                }
            }
            // 移除已接待完成的客人信息
            _m_receiveList.removeIf(receiveInfo -> receiveInfo.getNeedReceiveNum() <= 0);
            Inn_ReceiveList receiveList = new Inn_ReceiveList(_m_hadReceiveGuestNum, (ArrayList<Inn_ReceiveInfo>) _m_receiveList);

            getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_050_OnInnReceiveChg(makeReceiveList()));

            _m_bo.setReceiveList(getUSServer().getBM(), receiveList.makePackage().array());
            _m_bo.setRangeHadGainCount(getUSServer().getBM(), rangeHadGainCount);
            _m_bo.setRangeHadSettleCount(getUSServer().getBM(), rangeHadSettleCount);
            _m_bo.saveAllMarked(getUSServer().getBM());
            
            //MJ运营日志
            MJEventLog.logInnOrderSettle(getUserData(), settleInfo.getReceiveNum(),
                    settleInfo.getAddPopularity(), settleInfo.getAddStationBlueprintNum(),
                    settleInfo.getAddAffection());

            return ResultOne.succ(settleInfo);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 作弊设置旅店等级
     * @param _level
     * @param _context
     * @return
     */
    public Result cheatSetLevel(int _level, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_level <= 0)
                return CommErr.PARAM_ERROR;

            RefInnLevel refLevel = RefInnLevel.getMgr().get(_level);
            if (refLevel == null)
                return CommErr.PARAM_ERROR;

            addPopularity(refLevel.need_popularity - _m_bo.getPopularity(), _context);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 计算菜品收益
     * @param _dishInfo
     * @return
     */
    private DishBaseProfitInfo calDishProfit(InnDishInfo _dishInfo)
    {
        _lock();
        try
        {
            DishBaseProfitInfo profitInfo = new DishBaseProfitInfo();

            for (Long stationId : _dishInfo.getRefDish().need_station_id_list)
            {
                InnStationInfo stationInfo = _m_stationMgr.lookupStation(stationId);
                if (stationInfo == null)
                {
                    USLog.warn(getUSServer(), "Player {} station {} not found for dish {}", getUserData().getCid(), stationId, _dishInfo.getDishId());
                    continue;
                }

                // 计算菜品收益
                stationInfo.dealProfit(profitInfo);
            }

            return profitInfo;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造接待信息协议
     * @return
     */
    public Inn_ReceiveList makeReceiveList()
    {
        Inn_ReceiveList proto = new Inn_ReceiveList();
        proto.setHadBeenReceiveNum(_m_hadReceiveGuestNum);
        proto.getReceiveList().addAll(_m_receiveList);
        return proto;
    }

    /**
     * 填充协议
     */
    public Inn_Info makeProto()
    {
        _lock();
        try
        {
            Inn_Info proto = new Inn_Info();
            proto.setLevel(_m_bo.getLevel());
            proto.setPopularity(_m_bo.getPopularity());
            proto.setMedalLevel(_m_bo.getMedalLevel());
            proto.setReceiveList(makeReceiveList());
            proto.setFirstTimeUpgradeTimeMs(_m_bo.getFirstTimeUpgradeTimeMs());
            _m_stationMgr.fillStationList(proto.getStationList());
            _m_dishMgr.fillDishList(proto.getDishList());
            _m_guestMgr.fillGuestList(proto.getGuestList());
            _m_specialGuestMgr.fillSpecialGuestList(proto.getSpecialGuestList());
            proto.setRandomSeed(_m_syncRandom.getSeed());
            return proto;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取已接待客人数量
     * @return
     */
    public long getHadReceiveGuestNum()
    {
        getUserData().lockUser();
        try
        {
            // 如果接待列表为空，直接返回已接待客人数量
            if (_m_receiveList.isEmpty())
                return _m_hadReceiveGuestNum;

            return _m_hadReceiveGuestNum + getCanSettleGuestNum();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取当前可以结算的客人数量
     * @return
     */
    private long getCanSettleGuestNum()
    {
        long recordCount = 0;
        long nowTimeMS = CommonFunc.getNowTimeMS();
        for (int i = 0; i < _m_receiveList.size(); i++)
        {
            // 如果有下一个接待信息，检查其开始时间
            Inn_ReceiveInfo receiveInfo = _m_receiveList.get(i);
            if (i + 1 < _m_receiveList.size())
            {
                Inn_ReceiveInfo nextReceiveInfo = _m_receiveList.get(i + 1);
                // 如果下一个接待信息的开始时间小于当前时间，说明这批客人已经全部接待完成
                if (nextReceiveInfo.getStartTimeMs() < nowTimeMS)
                {
                    recordCount += receiveInfo.getNeedReceiveNum();
                    continue;
                }
            }

            // 如果这批客人还没有完全接待完成，计算已经接待完成的数量
            long elapsedTimeMs = nowTimeMS - receiveInfo.getStartTimeMs();
            if (elapsedTimeMs > 0)
            {
                long completedGuests = elapsedTimeMs / getReceiveCostMs();
                // 确保不超过需要接待的总数
                completedGuests = Math.min(completedGuests, receiveInfo.getNeedReceiveNum());
                recordCount += completedGuests;
            }
            // 后续的接待信息肯定还没开始，可以直接跳出循环
            break;
        }
        return recordCount;
    }

    /**
     * 菜品收益信息
     */
    public static class DishBaseProfitInfo
    {
        private long _m_addPopularity;
        private long _m_addAffection;
        private long _m_addFitness;

        public long getAddPopularity()
        {
            return _m_addPopularity;
        }

        public void addPopularity(long _value)
        {
            _m_addPopularity += _value;
        }

        public long getAddAffection()
        {
            return _m_addAffection;
        }

        public void addAffection(long _value)
        {
            _m_addAffection += _value;
        }

        public long getAddFitness()
        {
            return _m_addFitness;
        }

        public void addFitness(long _value)
        {
            _m_addFitness += _value;
        }
    }
}

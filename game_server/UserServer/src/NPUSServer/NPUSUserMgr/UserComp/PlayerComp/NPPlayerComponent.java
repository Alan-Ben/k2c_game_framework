package NPUSServer.NPUSUserMgr.UserComp.PlayerComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MailObj.Mail_Data;
import CommonEnum.ECurrency;
import MJLog.MJSectionLog;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.DB.BM.BM;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateNone;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.*;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyMgr;
import NPGameRes.Refs.Player.RefPlayerLevel;
import NPGameRes.Refs.Player.RefVip;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefRemoteEffect;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_DRAW_DAILY_REWARD;
import NPUSServer.Common.Event.Events.Event_P_MAX_EARNINGS_CHG;
import NPUSServer.Common.Event.Events.Event_P_PLAYER_UPGRADE;
import NPUSServer.DinnerMgr.DinnerCheckNpcTask;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerComp.Param.NPPlayerParamMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.UserComp._ITickableComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBO;
import USDB.Bo.PlayerStoreReviewsBO;
import USLOGDB.Bo.LogPlayerLevelUpBO;
import USLOGDB.Bo.LogPlayerSetNameBO;
import USLOGDB.Bo.LogSectionPlayerV2BO;


public class NPPlayerComponent extends _ANPUserComponent implements _ITickableComponent, _IHandlerHolder
{
    //玩家属性管理对象
    private NPPlayerPropertyMgr _m_mgrPropertyMgr;

    //玩家数据对象
    private PlayerBO _m_boPlayerBo;
    //领主等级数据
    private RefPlayerLevel _m_curLevelRef;
    //vip等级数据
    private RefVip _m_curVipRef;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;

    //每秒赚速
    private long _m_earnings;
    //自动点击每秒赚速
    private long _m_autoTapEarnings;
    //赚速变更触发器
    private ADelegateNone _m_earningsChgDelegate;
    //自动点击赚速变更触发器
    private ADelegateNone _m_autoTapEarningsChgDelegate;

    //玩家参数管理器
    private NPPlayerParamMgr _m_pmParamMgr;

    //当前称号数据
    private PlayerCurTitleMgr _m_mgrCurTitleMgr;

    //赚速计算处理器
    private LazyTaskDealer _m_earningsCalLazyDealer;

    public NPPlayerComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.PLAYER_COMP);

        _m_mgrPropertyMgr = new NPPlayerPropertyMgr();

        //玩家属性容器
        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer(getClass().getName());
        _m_pcPlayerPropertyContainer.addModifier(RefGeneral.Ref().player_property);

        //赚速变更触发器
        _m_earningsChgDelegate = new ADelegateNone(this);
        _m_autoTapEarningsChgDelegate = new ADelegateNone(this);

        _m_pmParamMgr = new NPPlayerParamMgr();
        
        _m_mgrCurTitleMgr = new PlayerCurTitleMgr(getUserData());

        _m_earningsCalLazyDealer = new LazyTaskDealer(() -> _calEarnings(false), 100);
    }

    public PlayerBO getBo()
    {
        return _m_boPlayerBo;
    }

    public NPPlayerPropertyMgr getPropertyMgr() { return _m_mgrPropertyMgr; }
    public NPPlayerParamMgr getParamMgr() { return _m_pmParamMgr; }
    public RefPlayerLevel getCurLevel() { return _m_curLevelRef; }
    public int getCurLvl() { return null == _m_curLevelRef ? 0 : _m_curLevelRef.lvl; }
    
    public PlayerCurTitleMgr getCurTitleMgr() {return _m_mgrCurTitleMgr;}

    @Override
    protected void _init()
    {
        //这个任务不做任何处理，由NPSynLoadPlayerDataTask在玩家数据加载的地方处理
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    private void setCurLevel(RefPlayerLevel _levelRef)
    {
        if (null == _levelRef)
        {
            USLog.error(getUSServer(), "player:{} set setCurLevel null", getUserData().getCid(), new Exception(""));
            return;
        }

        //玩家等级没有变化
        if (_levelRef == _m_curLevelRef)
            return;

        //玩家等级加成处理
        if (null != _m_curLevelRef)
        {
            _m_pcPlayerPropertyContainer.removeModifier(_m_curLevelRef.player_property);
        }
        _m_curLevelRef = _levelRef;
        _m_pcPlayerPropertyContainer.addModifier(_m_curLevelRef.player_property);
    }

    @Override
    public void onInited()
    {
        //注册组件的玩家属性加成
        _m_mgrPropertyMgr.regPropertyContainer(_m_pcPlayerPropertyContainer);//玩家属性
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getBuffComponent().getPlayerPropertyContainer());//buff组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getRecordComponent().getPlayerPropertyContainer());//记录组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getHeroComponent().getPlayerPropertyContainer());//大臣组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getConsortComponent().getPlayerPropertyContainer());//妃子组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getInnComponent().getPlayerPropertyContainer());//旅店组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getTreasureHuntComponent().getPropertyContainer());//太空寻宝组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getMarsComponent().getPlayerPropertyContainer());//火星组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getPrivilegeCardComponent().getPlayerPropertyContainer());//权益卡组件
        _m_mgrPropertyMgr.regPropertyContainer(getUserData().getForeverAddComponent().getPlayerPropertyContainer());//永久加成组件

        //初始计算玩家属性
        _m_mgrPropertyMgr.initProperties();
    }

    /**
     * 所有组件初始化完成后的处理
     */
    public void onAllCompInitedDeal()
    {
        //相关组件属性计算
        getUserData().getConsortComponent()._initCalAllConsort();
        getUserData().getHeroComponent()._initCalAllHero();
        getUserData().getBuildingComponent()._initCalAllBuilding();

        _calEarnings(true);

        //检查提交玩家的最大赚速
        checkExceedMaxEarningsRecord();
        getUserData().getBuildingComponent().checkExceedMaxEarningsRecord();
        getUserData().getChildComponent().checkExceedMaxEarningsRecord();
        
        //计算火星探险队伍实力
        getUserData().getMarsExploreComponent().getTeamMgr()._initCalTeamPower();
        //火星建筑相关产出计算
        getUserData().getMarsBuildingComponent()._onAllCompInitedDeal();

        //检查太空寻宝奇物产出速度变化
        getUserData().getTreasureHuntComponent().getTreasureMgr().checkAllTreasureOutputChange();
        
        //计算权益卡收益
        getUserData().getPrivilegeCardComponent().autoSettleAll();

        //确保玩家缓存存在
        ensurePlayerCache();

        //检查玩家所属宴会的相关数据补齐（NPC赴宴）
        ALSynTaskManager.getInstance().regTask(new DinnerCheckNpcTask(getUserData()));

        //截面日志
        if(CommonFunc.getNowTagYYYYMMDD() != getBo().getLastLogSectionDay())
        {
            getBo().saveLastLogSectionDay(getUSServer().getBM(), CommonFunc.getNowTagYYYYMMDD());

            //策划截面日志
            logSection(ELogSectionType.DAY);
            //梦加截面日志
            MJSectionLog.sectionLog(getUserData());
        }

        //给自己的属性容器增加一个监听属性变更的事件，不需要调用监听的 clear()
        _m_mgrPropertyMgr.propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerPropertyType _type, Long _preValue, Long _curVal)
            {
                //CachePlayer同步修改属性变化
                PlayerCacheFunc.updateProperty(getUserData(), _type, _curVal);

                //赚速加成变更要重新计算
                if (_type == ENPPlayerPropertyType.EXTRA_EARNINGS_ADD_PER)
                    setNeedCalEarnings();
            }
        });

        //给自己的参数容器增加一个监听参数变更的事件，不需要调用监听的clear()
        getUserData().Events.OnParamChg.addHandler(this, new HandlerThree<ENPPlayerParam, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerParam _type, Long _oldVal, Long _curVal)
            {
                //CachePlayer同步修改参数变化
                PlayerCacheFunc.updateParam(getUserData(), _type, _curVal);
            }
        });
    }



    @Override
    public void dispose()
    {

    }

    //确保玩家缓存存在
    private void ensurePlayerCache()
    {
        getUserData().getCacheComponent().onAllComponentInitedEnsureCache();
    }

    public void initBo(PlayerBO _bo)
    {
        //设置数据对象
        _m_boPlayerBo = _bo;

        //初始化参数
        _m_pmParamMgr.initMgr(this);

        //初始化等级
        if (!reInitLevel())
        {
            getUserData().setDataLoadFail();
            return;
        }

        _m_curVipRef = RefVip.getMgr().get(getParamV(ENPPlayerParam.VIP_LVL));
        if (null == _m_curVipRef)
        {
            USLog.error(getUSServer(), "player:{} initBo get vip ref failed, lvl:{}",
                    getUserData().getCid(), getParamV(ENPPlayerParam.VIP_LVL), new Exception(""));
            getUserData().setDataLoadFail();
            return;
        }
        _m_pcPlayerPropertyContainer.addModifier(_m_curVipRef.player_property);
        
        //加载当前称号数据
        getCurTitleMgr()._initFromBo(_m_boPlayerBo);

        //设置本模块初始化，同时从这里开始进行其他各模块的初始化处理
        setInited();
    }

    @Override
    public void tick1Sec()
    {
        //属性统计
        _m_mgrPropertyMgr.calculateChgProperties();
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    public String getName()
    {
        return _m_boPlayerBo.getCname();
    }

    public ADelegateNone getEarningsChgDelegate()
    {
        return _m_earningsChgDelegate;
    }

    public ADelegateNone getAutoTapEarningsChgDelegate()
    {
        return _m_autoTapEarningsChgDelegate;
    }

    /**
     * 获取自动点击产出速度
     * @return 自动点击产出速度
     */
    public long getAutoTapEarnings()
    {
        getUserData().lockUser();
        try{
            return _m_autoTapEarnings;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取产出速度
     * @return 产出速度
     */
    public long getEarnings()
    {
        getUserData().lockUser();
        try{
            return _m_earnings;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /***********
     * 获取对应参数的值
     * @param _eParam
     * @return
     */
    public long getParamV(ENPPlayerParam _eParam)
    {
        return getParamMgr().getParamV(_eParam);
    }


    /*************
     * 设置玩家名称
     * @param _newName
     */
    public Result setPlayerName(String _newName, boolean _isCost, NPPlayerContext _context)
    {
        //同步新的用户名到US名字检查管理器
        Result syncResult = getUserData().getUSServer().getUserNameEqualMgr().updateName(_m_boPlayerBo.getCname(), _newName);
        if (!syncResult.isSucc())
        {
            return syncResult;
        }

        _m_boPlayerBo.saveCname(getUSServer().getBM(), _newName);

        //推送协议
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_055_OnPlayerNameUpdated(_newName));

        //修改玩家缓存
        PlayerCacheFunc.updateName(getUserData(), _m_boPlayerBo.getCname());

        //数据日志
        BM bmObj = getUSServer().getBM();
        LogPlayerSetNameBO logBo = new LogPlayerSetNameBO();
        logBo.setCid(bmObj, getUserData().getCid());
        logBo.setName(bmObj, _m_boPlayerBo.getCname());
        logBo.setIsCost(bmObj, _isCost);
        CommLogDB.log(bmObj, logBo, _context);


        //返回成功
        return Result.SUCC;
    }

    /**************
     * 设置对应的值
     * @param _eParam
     * @param _value
     */
    public void setParam(ENPPlayerParam _eParam, long _value)
    {
        getParamMgr().setParam(_eParam, _value);
    }

    public void incParam(ENPPlayerParam _eParam)
    {
        incParam(_eParam, 1);
    }

    public void incParam(ENPPlayerParam _eParam, long _value)
    {
        long value = getParamV(_eParam);
        getParamMgr().setParam(_eParam, value + _value);
    }

    /**
     * 执行远端效果
     * @param _ref
     * @param _context
     * @return
     */
    public boolean execRemoteEffect(RefRemoteEffect _ref, NPPlayerContext _context)
    {
        return execRemoteEffect(_ref, _context, null);
    }

    public boolean execRemoteEffect(RefRemoteEffect _ref, NPPlayerContext _context, NPVarInfo _variableVarInfo)
    {
        getUserData().lockUser();

        try
        {
            //检查条件是否满足（默认是全部满足）
            if (!NPPlayerConditionDealerMgr.IsEnable(_ref.condition, getUserData(), null))
                return false;

            //检查是否可以足额扣除消耗
            if (!getUserData().hasCostItemList(_ref.cost_item_list.getItemTypeObjList())
                    || !getUserData().spendItem(_ref.cost_item_list.getItemTypeObjList(), _context))
                return false;

            //执行效果
            NPPlayerEffectDealer.dealEffect(_ref.effect_list.getPlayerEffectList(), getUserData(), null, _context);

            //发送成功协议
            getUserData().pushMsgToGC(US2GCWriter_007_CommOp.make_001_RetDealRemoteEffect(Result.SUCC.getCode(), 0, _ref.id));

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查玩家是否可以升级
     * 一次最多升1级
     * @param _curLvl
     * @param _isForce
     * @param _context
     */
    public Result checkLevelUp(int _curLvl, boolean _isForce, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try{
            //判断玩家等级是否匹配，不匹配则不处理
            if(_m_curLevelRef.lvl != _curLvl)
                return CommErr.PLAYER_CACHE_ERR;

            //玩家当前经验
            long expCount = getUserData().getCurrencyComponent().getItemCount(ECurrency.P_EXP.ordinal());

            //原等级配置
            RefPlayerLevel oldLevelRef = _m_curLevelRef;

            NPItemCostCollector_nosafe itemCollector = new NPItemCostCollector_nosafe();

            //查找下一级的等级配置
            RefPlayerLevel nextLevelRef = RefPlayerLevel.getMgr().get(_m_curLevelRef.lvl + 1);
            if (nextLevelRef == null)
                return CommErr.REF_NOT_FOUND;

            //如果经验不够则直接跳出
            if (expCount < nextLevelRef.exp)
                return CommErr.CONDITION_NOT_ENABLE;

            if (!_isForce)
            {
                //判断升级条件是否满足
                if (!NPPlayerConditionDealerMgr.IsEnable(nextLevelRef.lvl_up_cond, getUserData(), null))
                    return CommErr.CONDITION_NOT_ENABLE;

                //判断如果赚速不够则直接跳出
                if(getEarnings() < nextLevelRef.earnings)
                    return CommErr.CONDITION_NOT_ENABLE;

                //检查额外道具是否可以足额扣除消耗
                if (!getUserData().hasCostItemList(nextLevelRef.current_lvl_consume)
                        || !getUserData().spendItem(nextLevelRef.current_lvl_consume, _context))
                    return CommErr.CONDITION_NOT_ENABLE;
            }

            //统计奖励道具
            itemCollector.addItemList(nextLevelRef.reward_item_list);

            //刷新等级数据
            _m_curLevelRef = nextLevelRef;

            // 保存等级
            setParam(ENPPlayerParam.LEVEL, _m_curLevelRef.lvl);

            //给予升级奖励
            getUserData().gainItemList(itemCollector.getItemList(), _context);

            //关于属性处理
            _m_pcPlayerPropertyContainer.removeModifier(oldLevelRef.player_property);
            _m_pcPlayerPropertyContainer.addModifier(_m_curLevelRef.player_property);

            getUserData().Events.OnPlayerLvlUp.onEvent(_m_curLevelRef.lvl);

            getUserData().onLogicEvent(new Event_P_PLAYER_UPGRADE(_context));

            //对于每日奖励的处理
            _checkDailyRewardOnLevelUp(oldLevelRef, _context);

            //玩家升级日志
            BM bmObj = getUSServer().getBM();
            LogPlayerLevelUpBO logBo = new LogPlayerLevelUpBO();
            logBo.setCid(bmObj, getUserData().getCid());
            logBo.setOriLevel(bmObj, oldLevelRef.lvl);
            logBo.setExp(bmObj, expCount);
            logBo.setEarnings(bmObj, getEarnings());
            logBo.setNewLevel(bmObj, _m_curLevelRef.lvl);
            logBo.setNation(bmObj, getUserData().getSdkInfo().nation);
            logBo.setDeviceOs(bmObj, getUserData().getSdkInfo().deviceType.name());
            logBo.setCreateRoleTime(bmObj, getUserData().getPlayerComponent().getBo().getCreateRoleTime());
            logBo.setCreateRoleDate(bmObj, getUserData().getPlayerComponent().getBo().getCreateRoleDate());
            CommLogDB.log(bmObj, logBo, _context);
        }finally
        {
            getUserData().unlockUser();
        }

        try
        {
            logSection(ELogSectionType.LVL);
        } catch (Exception e)
        {
            USLog.error(getUSServer(), "", e);
        }

        return Result.SUCC;
    }

    /**
     * 检查玩家VIP等级是否可以升级
     * @param _context
     */
    public void checkVipLevelUp(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try{
            long vipExpCount = getUserData().getCurrencyComponent().getItemCount(ECurrency.VIP_EXP.ordinal());

            if (_m_curVipRef == null)
            {
                USLog.error(getUSServer(), "NPPlayerComponent.checkVipLevelUp, player vip lvl ref is null, cid:{}"
                        , getUserData().getCid());
                return;
            }

            RefVip targetVipRef = null;

            RefVip nextVipRef = RefVip.getMgr().get(_m_curVipRef.vip_lvl + 1);
            while (nextVipRef != null)
            {
                if (vipExpCount < nextVipRef.vip_exp)
                    break;

                //升级
                targetVipRef = nextVipRef;

                //继续检查下一级
                nextVipRef = RefVip.getMgr().get(nextVipRef.vip_lvl + 1);
            }

            //vip等级有变化
            if (targetVipRef != null)
            {
                setParam(ENPPlayerParam.VIP_LVL, targetVipRef.vip_lvl);

                _m_pcPlayerPropertyContainer.replaceModifier(_m_curVipRef.player_property, targetVipRef.player_property);
                _m_curVipRef = targetVipRef;
            }

        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 玩家升级时检查每日奖励
     * @param _oldLevelRef 原等级配置
     * @param _context
     */
    private void _checkDailyRewardOnLevelUp(RefPlayerLevel _oldLevelRef, NPPlayerContext _context)
    {
        long lastDrawDate = getParamV(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE);
        //如果今天没有领取, 需要通过邮件补发
        if (CommonFunc.getNowTagYYYYMMDD() > lastDrawDate)
        {
            //如果没有每日奖励则直接返回
            if (_oldLevelRef.daily_reward_item.getCount() == 0)
                return;

            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().playerLvl_daily_reward_mail_id);
            NPCommon_ItemInfo itemInfo = new NPCommon_ItemInfo();
            itemInfo.setItemType(_oldLevelRef.daily_reward_item.getItemType().ordinal());
            itemInfo.setSubId(_oldLevelRef.daily_reward_item.getItemId());
            itemInfo.setCount(_oldLevelRef.daily_reward_item.getCount());
            mailData.getItemList().getItemList().add(itemInfo);
            getUserData().getMailComponent().addMail(mailData, _context);
        }else
        {
            //重置领取时间
            setParam(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE, 0);
        }
    }

    public boolean reInitLevel()
    {
        //获取玩家等级
        int level = (int) getParamMgr().getParamV(ENPPlayerParam.LEVEL);

        //设置当前等级
        RefPlayerLevel ref = RefPlayerLevel.getMgr().get(level);
        if (null == ref)
        {
            USLog.error(getUSServer(), "NPPlayerComponent.reInitLevel, player lvl ref is null, cid:{} lvl:{}"
                    , getUserData().getCid(), level);
            return false;
        }

        setCurLevel(ref);

        return true;
    }

    /**
     * 强制设置玩家等级（仅用于GM命令）
     * <p>
     * 功能：
     * 1. 直接设置玩家等级到指定值
     * 2. 同步更新所有等级相关效果，包括：
     * - 玩家属性加成（player_property）
     * - 等级参数保存
     * - 玩家经验值同步
     * - 重新计算赚速
     * - 触发等级变更事件
     * @param _targetLevel 目标等级
     * @return 是否设置成功
     * <p>
     * 线程安全：通过getUserData().lockUser()保证线程安全
     */
    public boolean forceSetLevel(int _targetLevel, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 查找目标等级配置
            RefPlayerLevel targetLevelRef = RefPlayerLevel.getMgr().get(_targetLevel);
            if (targetLevelRef == null)
            {
                USLog.error(getUSServer(), "NPPlayerComponent.forceSetLevel - ref not found: cid={}, targetLevel={}",
                        getUserData().getCid(), _targetLevel);
                return false;
            }

            // 保存旧等级和经验信息，用于日志记录
            RefPlayerLevel oldLevelRef = _m_curLevelRef;
            int oldLevel = oldLevelRef != null ? oldLevelRef.lvl : 1;

            // 移除旧等级的属性加成
            if (_m_curLevelRef != null)
            {
                _m_pcPlayerPropertyContainer.removeModifier(_m_curLevelRef.player_property);
            }

            // 更新等级配置
            _m_curLevelRef = targetLevelRef;

            // 添加新等级的属性加成
            _m_pcPlayerPropertyContainer.addModifier(_m_curLevelRef.player_property);

            // 保存等级参数
            setParam(ENPPlayerParam.LEVEL, _m_curLevelRef.lvl);

            // 同步经验值：设置为目标等级所需的经验值
            getUserData().getCurrencyComponent().setCurrencyCount(ECurrency.P_EXP.ordinal(), targetLevelRef.exp, _context);

            // 重新计算赚速，因为等级影响赚速
            _calEarnings(false);

            // 记录等级变更日志
            BM bmObj = getUSServer().getBM();
            LogPlayerLevelUpBO logBo = new LogPlayerLevelUpBO();
            logBo.setCid(bmObj, getUserData().getCid());
            logBo.setOriLevel(bmObj, oldLevel);
            logBo.setExp(bmObj, targetLevelRef.exp);
            logBo.setEarnings(bmObj, getEarnings());
            logBo.setNewLevel(bmObj, _m_curLevelRef.lvl);
            logBo.setNation(bmObj, getUserData().getSdkInfo().nation);
            logBo.setDeviceOs(bmObj, getUserData().getSdkInfo().deviceType.name());
            logBo.setCreateRoleTime(bmObj, getUserData().getPlayerComponent().getBo().getCreateRoleTime());
            logBo.setCreateRoleDate(bmObj, getUserData().getPlayerComponent().getBo().getCreateRoleDate());
            CommLogDB.log(bmObj, logBo, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 计算赚速
     */
    private void _calEarnings(boolean _isInit)
    {
        getUserData().lockUser();
        try
        {
            long oriEarnings = 0;
            oriEarnings += getUserData().getBuildingComponent().getTotalEarning();
            oriEarnings += getUserData().getChildComponent().getTotalEarning();
            oriEarnings += getParamV(ENPPlayerParam.EXTRA_EARNINGS);

            //计算最终值
            _m_earnings = (long) Math.ceil(oriEarnings * (10000 + getPropertyMgr().getValue(ENPPlayerPropertyType.EXTRA_EARNINGS_ADD_PER)) / 10000d);

            if (!_isInit)
            {
                _m_earningsChgDelegate.onEvent();

                //记录实力
                PlayerCacheFunc.updateEarnings(getUserData(), _m_earnings);

                //检查是否超过历史最大赚速
                checkExceedMaxEarningsRecord();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否超过历史最大赚速
     */
    private void checkExceedMaxEarningsRecord()
    {
        getUserData().lockUser();
        try{
            //记录玩家最大国力
            if (_m_earnings > getParamV(ENPPlayerParam.EARNINGS_MAX_RECORD))
            {
                long addValue = _m_earnings - getParamV(ENPPlayerParam.EARNINGS_MAX_RECORD);

                setParam(ENPPlayerParam.EARNINGS_MAX_RECORD, _m_earnings);

                NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MAX_EARNINGS_CHG);
                getUserData().onLogicEvent(new Event_P_MAX_EARNINGS_CHG(context, _m_earnings, addValue));

                PlayerCacheFunc.updateMaxEarnings(getUserData(), _m_earnings);

                // 检查记录TOP1玩家数据
                getUSServer().getStageGoalFirstReachMgr().recordTop1PlayerData(getUserData().getCid(), _m_earnings, getUserData().getStageGoalComponent().getDoneStep());

                // 检查赚速跑马灯触发
                getUSServer().getEarningsMarqueeMgr().checkAndTriggerMarquee(getUserData().getCid(), getName(), _m_earnings);
            }
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 标记需要计算
     */
    public void setNeedCalEarnings()
    {
        _m_earningsCalLazyDealer.setNeedDeal();
    }

    /**
     * 替换自动点击产出速度
     * @param _preSpeed
     * @param _newSpeed
     * @param _isInit
     */
    public void replaceAutoTapEarnings(long _preSpeed, long _newSpeed, boolean _isInit)
    {
        getUserData().lockUser();
        try{
            _m_autoTapEarnings -= _preSpeed;
            _m_autoTapEarnings += _newSpeed;

            //初始化的时候不进行处理，在最后统一调用结算
            if (!_isInit)
            {
                _m_autoTapEarningsChgDelegate.onEvent();
            }
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加额外产速
     * @param _value
     */
    public void addExtraEarnings(long _value)
    {
        getUserData().lockUser();
        try{
            long oriExtraEarnings = getParamV(ENPPlayerParam.EXTRA_EARNINGS);
            long newExtraEarnings = oriExtraEarnings + _value;
            setParam(ENPPlayerParam.EXTRA_EARNINGS, newExtraEarnings);

            getUserData().getPlayerComponent().setNeedCalEarnings();
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取玩家每日奖励
     * @param _context
     * @return
     */
    public Result drawDailyReward(NPPlayerContext _context)
    {
        //检查是否有每日奖励
        if (_m_curLevelRef.daily_reward_item.getCount() == 0)
            return PlayerErr.DRAW_DAILY_REWARD_NOT_EXIST;

        //检查是否已经领取
        long lastDrawDate = getParamV(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE);
        int todayTag = CommonFunc.getNowTagYYYYMMDD();
        if (todayTag <= lastDrawDate)
            return PlayerErr.DRAW_DAILY_REWARD_ALREADY_DRAW;

        //标记领取
        setParam(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE, todayTag);

        //领取奖励
        getUserData().gainItem(_m_curLevelRef.daily_reward_item, _context);

        //触发事件
        getUserData().onLogicEvent(new Event_P_DRAW_DAILY_REWARD(_context));

        return Result.SUCC;
    }

    /**
     * 领取VIP等级奖励
     * @param _rechargeReward
     * @param _vipLvl
     * @param _context
     * @return
     */
    public Result drawVipLvlReward(boolean _rechargeReward, int _vipLvl, NPPlayerContext _context)
    {
        if (_vipLvl <= 0)
            return CommErr.PARAM_ERROR;

        RefVip ref = RefVip.getMgr().get(_vipLvl);
        if (null == ref)
            return CommErr.REF_NOT_FOUND;

        if (_vipLvl > 63)
        {
            USLog.error(getUSServer()," player:{} drawVipLvlReward, vip lvl > 63, lvl:{}", getUserData().getCid(), _vipLvl);
            return CommErr.PARAM_ERROR;
        }

        ENPPlayerParam eParam = _rechargeReward ? ENPPlayerParam.HAD_DRAW_VIP_RECHARGE_REWARD_LIST : ENPPlayerParam.HAD_DRAW_VIP_REWARD_LIST;

        getUserData().lockUser();
        try
        {
            //检查VIP等级
            if (null == _m_curVipRef || _vipLvl > _m_curVipRef.vip_lvl)
                return PlayerErr.VIP_LEVEL_REWARD_NOT_MEET_REQUIRE;

            //已经领奖
            long hadDrawList = getParamV(eParam);
            if ((hadDrawList & (1L << _vipLvl)) != 0)
                return PlayerErr.VIP_LEVEL_REWARD_HAD_DRAW;

            //设置标志位
            hadDrawList |= (1L << _vipLvl);
            setParam(eParam, hadDrawList);

            //发放奖励
            if (_rechargeReward)
            {
                getUserData().gainItemList(ref.recharge_reward_list, _context);
            }else
            {
                getUserData().gainItemList(ref.gain_item_list, _context);
            }

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 设置评价内容
     * @param _content
     */
    public void setStoreReviewsContent(String _content)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		PlayerStoreReviewsBO bo = new PlayerStoreReviewsBO();
    		bo.setCid(getBM(), getUserData().getCid());
    		bo.setContent(getBM(), _content);
    		bo.insert(getBM());
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 设置商店领取记录
     * @return
     */
    public boolean setStoreReviews()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//只能领取一次
    		if(getBo().getIsStoreReviews())
    			return false;
    		
    		getBo().saveIsStoreReviews(getBM(), true);
    		return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 玩家截面数据
     * 
     * @param _sectionLogType 截面类型
     */
    public void logSection(ELogSectionType _sectionLogType)
    {
    	BM bmObj = getUSServer().getBM();
    	
        LogSectionPlayerV2BO logBo = new LogSectionPlayerV2BO();
        //截面日志类型
        logBo.setSectionType(getUSServer().getBM(), _sectionLogType.ordinal());
        //玩家
        logBo.setCid(bmObj, getUserData().getCid());
        logBo.setVillageEarning(bmObj, getUserData().getBuildingComponent().getBuildingSpeedSum());
        logBo.setChildEarning(bmObj, getUserData().getChildComponent().getBonusSum());
        logBo.setChapterStageId(bmObj, getUserData().getChapterComponent().getChapterInfo().getChapterProgress());
        logBo.setConsortNum(bmObj, getUserData().getConsortComponent().getConsortNum());
        logBo.setHeroNum(bmObj, getUserData().getHeroComponent().getHeroNum());
        logBo.setLevel(bmObj, getUserData().getPlayerComponent().getCurLvl());
        logBo.setTotalTalent(bmObj, getUserData().getHeroComponent().getTotalTalent());
        logBo.setTotalFightPower(bmObj, getUserData().getHeroComponent().getTotalPower());
        logBo.setTotalDiamondGain(bmObj, getUserData().getCurrencyComponent().getTotalGainCount(ECurrency.GEM));
        logBo.setTotalGoldGain(bmObj, getUserData().getCurrencyComponent().getTotalGainCount(ECurrency.SILVER));
        logBo.setTotalDiamondCost(bmObj, getUserData().getCurrencyComponent().getTotalConsumeCount(ECurrency.GEM));
        logBo.setTotalGoldCost(bmObj, getUserData().getCurrencyComponent().getTotalConsumeCount(ECurrency.SILVER));
        logBo.setFarmGoldGain(bmObj, getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.FRAM_COLLECT_SUM));
        logBo.setTotalPlayerExp(bmObj, getUserData().getItemCount(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal()));
        logBo.setQuestId(bmObj, getUserData().getQuestComponent().getMainQuestId());
        //主线
        logBo.setLoginDayCount(bmObj, (int) getUserData().getParam(ENPPlayerParam.LOGIN_DAY_COUNT));
        logBo.setHighestEarning(bmObj, getUserData().getParam(ENPPlayerParam.EARNINGS_MAX_RECORD));
        logBo.setFarmCollect(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.FRAM_COLLECT));
        logBo.setFarmLvl(bmObj, getUserData().getBuildingComponent().getFarmLvl());
        logBo.setTotalHeroLvl(bmObj, getUserData().getHeroComponent().getTotalHeroLevel());
        logBo.setTotalBuildingLvl(bmObj, getUserData().getBuildingComponent().getBuildingLvlSum());
        logBo.setGainEquipTimes(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.GAIN_EQUIP_TIMES));
        logBo.setConsortRandCallCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES));
        logBo.setConsortCallCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.CONSORT_CALL_TIMES));
        logBo.setChildNum(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.CHILD_GAIN_NUM));
        logBo.setTravelCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.TRAVEL_COUNT));
        logBo.setStarDinnerCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.START_DINNER_COUNT));
        logBo.setJoinDinnerCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.JOIN_DINNER_COUNT));
        logBo.setArenaAttackTimes(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.ARENA_ATTACK_TIMES));
        logBo.setArenaStationCollectTimes(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.ARENA_STATION_COLLECT_TIMES));
        logBo.setTowerPassedChapter(bmObj, getUserData().getTowerComponent().getPassedChapter());
        logBo.setRankLikeCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.RANK_FIXED_LIKE));
        //子嗣
        logBo.setYoungChildNum(bmObj, getUserData().getChildComponent().getChildMgr().getChildCount());
        logBo.setAdultChildNum(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.ADULT_GAIN_NUM));
        logBo.setChildMarryCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MARRIED_COUNT));
        logBo.setMarryChildEarning(bmObj, getUserData().getChildComponent().getAdultMgr().getBonusSum());
        logBo.setUnmarriedChildEarning(bmObj, getUserData().getChildComponent().getChildMgr().getBonusSum());
        logBo.setChildCultureCount(bmObj, (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.CHILD_TRAIN_TIMES));

        CommLogDB.log(bmObj, logBo);

        //建筑截面数据
        getUserData().getBuildingComponent().logSection(_sectionLogType);
        //伙伴截面数据
        getUserData().getHeroComponent().logSection(_sectionLogType);
        //妃子截面数据
        getUserData().getConsortComponent().logSection(_sectionLogType);
    }
}

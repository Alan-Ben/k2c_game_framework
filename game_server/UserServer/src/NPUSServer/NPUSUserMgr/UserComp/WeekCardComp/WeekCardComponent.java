package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.WeekCardObj.WeekCard_Info;
import Common.WeekCardObj.WeekCard_SettleDetailInfo;
import Common.WeekCardObj.WeekCard_SettleInfo;
import Common.WeekCardObj.WeekCard_SingleSettingInfo;
import CommonEnum.EWeekCardNPCType;
import CommonEnum.EWeekCardSettleType;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.Dealer.WeekCardDealer_ConsortRndCall;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerWeekCardBO;
import USDB.Bo.PlayerWeekCardSettingBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 挂机周卡组件
 * 主要逻辑：
 * 1、通过玩家跟GS的链接状态确认玩家的在线状态，用于计算离线期间溢出的CD值，相关方法recordStartOfflineTimeMs(离线调用)和calculateOverflowCd(上线调用)
 * 2、每个功能组件需要实现自己的AgentDealer，在里面完成溢出CD计算和溢出CD使用的逻辑
 */
public class WeekCardComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    private PlayerWeekCardBO _m_bo;
    private List<_AWeekCardDealer> _m_weekCardDealerList;
    private WeekCard_SettleInfo _m_settleInfo;

    public WeekCardComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.WEEK_CARD);

        _m_weekCardDealerList = new ArrayList<>();

        _m_weekCardDealerList.add(new WeekCardDealer_ConsortRndCall(this));
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("week_card_component_init");
        //步骤1: 加载主数据
        process.addResDelegateProcess(action -> _initMainDataFromDB(action::dealAction), "init_main_data", null, false);
        //步骤2: 加载设置数据
        process.addResDelegateProcess(action -> _initSettingDataFromDB(action::dealAction), "init_setting_data", null, false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error("WeekCardComponent init fail, cid:{}", getUserData().getCid());
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
     * 初始化主数据
     * @param _action 回调
     */
    private void _initMainDataFromDB(_ICallBackBool _action)
    {
        getUSServer().getBM().getBM(PlayerWeekCardBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerWeekCardBO>()
        {
            @Override
            public void dealSuc(PlayerWeekCardBO _bo)
            {
                _m_bo = _bo;

                _action.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    _action.onRunOver(false);
                    return;
                }

                PlayerWeekCardBO bo = new PlayerWeekCardBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.insert(getUSServer().getBM());
                _m_bo = bo;

                _action.onRunOver(true);
            }
        });
    }

    /**
     * 初始化设置数据
     * @param _action 回调
     */
    private void _initSettingDataFromDB(_ICallBackBool _action)
    {
        getUSServer().getBM().getBM(PlayerWeekCardSettingBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerWeekCardSettingBO>>()
        {
            @Override
            public void dealSuc(List<PlayerWeekCardSettingBO> _boList)
            {
                for (PlayerWeekCardSettingBO bo : _boList)
                {
                    for (_AWeekCardDealer dealer : _m_weekCardDealerList)
                    {
                        if (dealer.getType().ordinal() == bo.getSettleType())
                        {
                            dealer.setSettingBo(bo);
                            break;
                        }
                    }
                }

                _action.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _action.onRunOver(false);
            }
        });
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
    {
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

    public long getActiveTimeMs()
    {
        return _m_bo.getActiveTimeMs();
    }

    public long getExpireTimeMs()
    {
        return _m_bo.getExpireTimeMs();
    }

    public void resetTrailFlag(NPPlayerContext context)
    {
        _m_bo.saveHadUseFreeTrial(getUSServer().getBM(), false);

        //变更推送
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_063_OnWeekCardChg(makeInfoProto()));
    }

    public _AWeekCardDealer getDealer(EWeekCardSettleType _type)
    {
        for (_AWeekCardDealer dealer : _m_weekCardDealerList)
        {
            if (dealer.getType() == _type)
                return dealer;
        }
        return null;
    }

    /**
     * 在离线时，对CD进行一次计算
     */
    public void onOffline()
    {
        //如果已经过期则不处理
        if (getRemainTimeMs() <= 0)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_LOGIC_OFFLINE);

        //遍历
        _m_weekCardDealerList.forEach(dealer ->
        {
            //对CD进行一次结算
            dealer.doOfflineCdSettle(context);
            //检查是否有处理模块的设置信息
            dealer.checkSetting();
        });
    }

    /**
     * 在线时 进行一次结算
     */
    public boolean onlineSettle(long _offlineTimeMs, long _onlineTimeMs)
    {
        //如果没有周卡生效时间小于离线时间或生效时间大于在线时间, 则不处理
        if (getExpireTimeMs() < _offlineTimeMs || getActiveTimeMs() > _onlineTimeMs)
            return false;

        //如果上次离线时间小于上次上线时间, 则不处理
        if (_onlineTimeMs - _offlineTimeMs <= RefGeneral.Ref().week_card_offline_active_min_time_sec * 1000L)
            return false;

        //把开始处理时间和结束处理时间设置为上次离线时间和上次上线时间, 且限制在周卡的生效时间内
        long startSettleTimeMs = _offlineTimeMs;
        long endSettleTimeMs = Math.min(_onlineTimeMs, getExpireTimeMs());

        //如果开始处理时间大于结束处理时间, 则不处理
        if (startSettleTimeMs >= endSettleTimeMs)
            return false;

        //需要限制处理时间的长度
        if (endSettleTimeMs - startSettleTimeMs > RefGeneral.Ref().week_card_offline_hosting_max_profit_time_sec * 1000L)
            endSettleTimeMs = startSettleTimeMs + RefGeneral.Ref().week_card_offline_hosting_max_profit_time_sec * 1000L;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_LOGIC_ONLINE);
        //结算离线溢出cd
        settleOverflowCd(startSettleTimeMs, endSettleTimeMs, context);

        return _m_settleInfo != null;
    }

    /**
     * 结算离线溢出cd
     */
    public void settleOverflowCd(long _startSettleTimeMs, long _endSettleTimeMs, NPPlayerContext _context)
    {
        WeekCard_SettleInfo settleInfo = new WeekCard_SettleInfo();

        //遍历处理离线期间溢出的CD值
        for (_AWeekCardDealer dealer : _m_weekCardDealerList)
        {
            //检查对应功能是否解锁, 未解锁则不处理
            if (!dealer.isFuncUnlock())
                continue;

            WeekCard_SettleDetailInfo detailInfo = dealer.settleOverflowCd(_startSettleTimeMs, _endSettleTimeMs, _context);
            if (detailInfo == null)
                continue;

            settleInfo.addDetailList(detailInfo);
        }

        _context.getCollector().fillProtoList(settleInfo.getItemList());

        if (settleInfo.getDetailList().isEmpty())
            return;

        _m_settleInfo = settleInfo;
    }

    /**
     * 获得剩余时间
     * @return 剩余时间
     */
    public long getRemainTimeMs()
    {
        return Math.max(0, getExpireTimeMs() - CommonFunc.getNowTimeMS());
    }

    /**
     * 获得周卡时间
     * @param _sec 秒
     */
    public void gainTimeSec(int _sec, NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        boolean isFirstGain = false;

        //区分是否首次获得
        if (_m_bo.getExpireTimeMs() == 0)
        {
            _m_bo.setActiveTimeMs(getUSServer().getBM(), nowTimeMS);
            _m_bo.setExpireTimeMs(getUSServer().getBM(), nowTimeMS + _sec * 1000L);

            isFirstGain = true;
        } else
        {
            //区分是否过期
            if (nowTimeMS > _m_bo.getExpireTimeMs())
            {
                _m_bo.setActiveTimeMs(getUSServer().getBM(), nowTimeMS);
                _m_bo.setExpireTimeMs(getUSServer().getBM(), nowTimeMS + _sec * 1000L);
            } else
            {
                _m_bo.setExpireTimeMs(getUSServer().getBM(), _m_bo.getExpireTimeMs() + _sec * 1000L);
            }
        }

        _m_bo.saveAllMarked(getUSServer().getBM());

        //添加皮肤道具
        _context.getCollector().addItem(ENPItemType.WEEK_CARD, 0, _sec);

        //如果是首次获得, 需要遍历检查是否有处理模块的设置信息
        if (isFirstGain)
        {
            _m_weekCardDealerList.forEach(_AWeekCardDealer::checkSetting);
        }

        //变更推送
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_063_OnWeekCardChg(makeInfoProto()));
    }

    /**
     * 消耗周卡时间
     * @param _sec 秒
     */
    public void consumeTimeSec(int _sec, NPPlayerContext _context)
    {
        //如果已经过期则不处理
        if (getRemainTimeMs() <= 0)
            return;

        //新的过期时间
        long newExpiredTimeMs = Math.max(0, _m_bo.getExpireTimeMs() - _sec * 1000L);

        //如果消耗时间大于剩余时间, 则消耗剩余时间
        if (newExpiredTimeMs < _m_bo.getActiveTimeMs())
        {
            _m_bo.setActiveTimeMs(getUSServer().getBM(), newExpiredTimeMs);
            _m_bo.setExpireTimeMs(getUSServer().getBM(), newExpiredTimeMs);
            _m_bo.saveAllMarked(getUSServer().getBM());
        } else
        {
            _m_bo.saveExpireTimeMs(getUSServer().getBM(), newExpiredTimeMs);
        }

        //变更推送
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_063_OnWeekCardChg(makeInfoProto()));
    }

    /**
     * 构造周卡信息
     * @return 周卡信息
     */
    public WeekCard_Info makeInfoProto()
    {
        WeekCard_Info info = new WeekCard_Info();
        info.setExpireTimeTagS((int) (getExpireTimeMs() / 1000));
        info.setHadUseFreeTrial(_m_bo.getHadUseFreeTrial());
        EWeekCardNPCType npcType = EWeekCardNPCType.EWeekCardNPCType_FromInt(_m_bo.getNpcType());
        info.setNpcType(npcType == null ? EWeekCardNPCType.NONE : npcType);
        info.setNpcId(_m_bo.getNpcId());
        return info;
    }

    /**
     * 构造结算信息
     * @return 结算信息
     */
    public WeekCard_SettleInfo readSettleInfo()
    {
        WeekCard_SettleInfo settleInfo = _m_settleInfo;

        _m_settleInfo = null;

        return settleInfo;
    }

    /**
     * 构造设置协议列表
     * @return
     */
    public List<WeekCard_SingleSettingInfo> makeAllSettingProto()
    {
        List<WeekCard_SingleSettingInfo> list = new ArrayList<>();
        for (_AWeekCardDealer dealer : _m_weekCardDealerList)
        {
            list.add(dealer.makeSettingProto());
        }
        return list;
    }

    /**
     * 修改设置
     * @param _settingInfo 设置信息
     * @return 结果
     */
    public Result chgSetting(WeekCard_SingleSettingInfo _settingInfo)
    {
        _AWeekCardDealer dealer = getDealer(_settingInfo.getType());
        if (dealer == null)
        {
            USLog.error("WeekCardComponent chgSetting fail, dealer not found, cid:{} type:{}", getUserData().getCid(), _settingInfo.getType());
            return CommErr.SYS_ERR;
        }

        boolean isSuc = dealer.chgSetting(_settingInfo);
        if (!isSuc)
            return CommErr.PROTOCOL_ERR;

        return Result.SUCC;
    }

    /**
     * 修改NPC
     * @param _npcType
     * @param _npcId
     */
    public Result chgNPC(EWeekCardNPCType _npcType, long _npcId)
    {
        if (_npcType == EWeekCardNPCType.CONSORT)
        {
            ConsortInfo consort = getUserData().getConsortComponent().lookup(_npcId);
            if (consort == null)
                return ConsortErr.CONSORT_NOT_EXISTS;
        }else if (_npcType == EWeekCardNPCType.HERO)
        {
            HeroInfo heroInfo = getUserData().getHeroComponent().lookupHero(_npcId);
            if (heroInfo == null)
                return HeroErr.HERO_NOT_FOUND;
        }

        _m_bo.setNpcType(getUSServer().getBM(), _npcType.ordinal());
        _m_bo.setNpcId(getUSServer().getBM(), _npcId);
        _m_bo.saveAllMarked(getUSServer().getBM());

        //变更推送
        getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_063_OnWeekCardChg(makeInfoProto()));

        return Result.SUCC;
    }

    /**
     * 激活免费试用
     * @param _context
     * @return
     */
    public Result activeFreeTrial(NPPlayerContext _context)
    {
        //检查是否解锁
        if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().week_card_func_simple_unlock_id, getUserData(), null))
            return CommErr.SYSTEM_UNLOCK;

        //判断是否已经使用过免费试用
        if (_m_bo.getHadUseFreeTrial())
            return PlayerErr.WEEK_CARD_FREE_TRIAL_HAD_USE;

        _m_bo.saveHadUseFreeTrial(getUSServer().getBM(), true);

        //获得周卡时间
        gainTimeSec(RefGeneral.Ref().week_card_free_trial_time_sec, _context);

        return Result.SUCC;
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.WEEK_CARD;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        return getRemainTimeMs() / 1000;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return (getRemainTimeMs() / 1000) >= _count;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        gainTimeSec((int)_count, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        gainTimeSec((int)_count, _context);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }
}

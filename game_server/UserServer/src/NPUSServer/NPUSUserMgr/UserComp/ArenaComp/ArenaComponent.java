package NPUSServer.NPUSUserMgr.UserComp.ArenaComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.ArenaEnum.EArenaBuffType;
import Common.ArenaObj.Arena_BaseInfo;
import Common.ArenaObj.Arena_Info;
import Common.Common_LongList;
import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import Common.RankObj.Rank_BaseItem;
import CommonEnum.ESpecialItemType;
import GS2GC.p023_ArenaOp.GS2GC_023_005_RetRoundAttack;
import GS2GC.p023_ArenaOp.GS2GC_023_013_RetArenaAKeyAttack;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.ArenaErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Promise.SerialPromise;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Pair.WCGPair;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.RefWrap;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Arena.RefArenaBotRandom;
import NPGameRes.Refs.Arena.RefArenaBotTemplate;
import NPGameRes.Refs.Arena.RefArenaBuff;
import NPGameRes.Refs.Arena.RefArenaSelectAttackConsume;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_ARENA_INFLUENCE_CHG;
import NPUSServer.Common.Event.Events.Event_P_ARENA_OP;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.FightReport.ArenaFightReportMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Station;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerArenaBO;
import USDB.Bo.PlayerArenaBattleBO;
import USDB.Bo.PlayerArenaFightReportBO;
import USLOGDB.Bo.LogArenaUnlockBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;

public class ArenaComponent extends _ANPUserComponent
{
    private PlayerArenaBO _m_bo;
    private ArenaBattleInfo _m_battleInfo;

    private Common_LongList _m_hadSelectAttackHeroList;
    private Common_LongList _m_hadRandomAttackHeroList;
    // 已攻击过的对手CID列表（当天）
    private Common_LongList _m_hadAttackOpponentCidList;

    private ArenaFightReportMgr _m_fightReportMgr;

    public ArenaComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ARENA);
        _m_fightReportMgr = new ArenaFightReportMgr(this);

        _m_hadSelectAttackHeroList = new Common_LongList();
        _m_hadRandomAttackHeroList = new Common_LongList();
        _m_hadAttackOpponentCidList = new Common_LongList();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("ArenaComponent._init");
        process.addResDelegateProcess(action -> _initArenaInfo(action::dealAction), "arena_info_init",
                () -> USLog.error(getUSServer(), "player:{} load arena bo fail.", getUserData().getCid()), false);
        process.addResDelegateProcess(action -> _initArenaBattleInfo(action::dealAction), "arena_battle_info_init",
                () -> USLog.error(getUSServer(), "player:{} load arena_battle bo fail.", getUserData().getCid()), false);
        process.addResDelegateProcess(action -> _initArenaFightReport(action::dealAction), "arena_fight_report_init",
                () -> USLog.error(getUSServer(), "player:{} load arena_fight_report bo fail.", getUserData().getCid()), false);
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "ArenaComponent _init fail cid:{}", getUserData().getCid());
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
     * 初始化竞技场信息
     * @param _handler
     */
    private void _initArenaInfo(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerArenaBO.class).findOne("cid", getUserData().getCid(),
                new _ASelectCallback<PlayerArenaBO>()
                {
                    @Override
                    public void dealSuc(PlayerArenaBO _bo)
                    {
                        _setBo(_bo);
                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            USLog.error(getUSServer(), "ArenaComponent _initArenaInfo fail cid:{}", getUserData().getCid());
                            _handler.onRunOver(false);
                            return;
                        }

                        BM bmObj = getUSServer().getBM();

                        PlayerArenaBO bo = new PlayerArenaBO();
                        bo.setCid(bmObj, getUserData().getCid());
                        bo.insert(bmObj);

                        _setBo(bo);
                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 初始化竞技场战斗信息
     * @param _bo
     */
    private void _setBo(PlayerArenaBO _bo)
    {
        _m_bo = _bo;

        if (_bo.getHadSelectAttackHeroList() != null)
            _m_hadSelectAttackHeroList.readPackage(ByteBuffer.wrap(_bo.getHadSelectAttackHeroList()));

        if (_bo.getHadRandomAttackHeroList() != null)
            _m_hadRandomAttackHeroList.readPackage(ByteBuffer.wrap(_bo.getHadRandomAttackHeroList()));

        if (_bo.getHadAttackOpponentCidList() != null)
            _m_hadAttackOpponentCidList.readPackage(ByteBuffer.wrap(_bo.getHadAttackOpponentCidList()));
    }

    /**
     * 初始化竞技场战斗信息
     * @param _handler
     */
    private void _initArenaBattleInfo(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerArenaBattleBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerArenaBattleBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerArenaBattleBO> _boList)
                    {
                        if (_boList.isEmpty())
                        {
                            _handler.onRunOver(true);
                            return;
                        }

                        _m_battleInfo = new ArenaBattleInfo(ArenaComponent.this, _boList.get(0));
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
     * 初始化战报信息
     * @param _handler
     */
    private void _initArenaFightReport(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerArenaFightReportBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerArenaFightReportBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerArenaFightReportBO> _boList)
                    {
                        if (_boList.isEmpty())
                        {
                            _handler.onRunOver(true);
                            return;
                        }

                        _m_fightReportMgr.init(_boList);
                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //检查是否需要解锁竞技场
        checkUnlockArena(getUserData().getPlayerInitContext());
    }

    @Override
    public void dispose()
    {

    }

    private void _lock()
    {
        getUserData().lockUser();
    }

    private void _unlock()
    {
        getUserData().unlockUser();
    }

    public ArenaBattleInfo getBattleInfo()
    {
        return _m_battleInfo;
    }

    public ArenaFightReportMgr getFightReportMgr()
    {
        return _m_fightReportMgr;
    }

    /**
     * 检查是否需要重置每日次数
     */
    public void checkResetDailyData(boolean _isInit)
    {
        _lock();
        try
        {
            //检查是否需要重置每日计数
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
            if (_m_bo.getLastResetTimeMs() >= todayZeroClockMS)
                return;

            //重置数据
            _m_hadSelectAttackHeroList.getValueList().clear();
            _m_hadRandomAttackHeroList.getValueList().clear();
            _m_hadAttackOpponentCidList.getValueList().clear();

            BM bmObj = getUSServer().getBM();
            _m_bo.setHadSelectAttackNum(bmObj, 0);
            _m_bo.setHadRandomAttackNum(bmObj, 0);
            _m_bo.setHadBuyRandomAttackNum(bmObj, 0);
            _m_bo.setLastResetTimeMs(bmObj, todayZeroClockMS);
            _m_bo.setHadRandomAttackHeroList(bmObj, _m_hadRandomAttackHeroList.makePackage().array());
            _m_bo.setHadSelectAttackHeroList(bmObj, _m_hadSelectAttackHeroList.makePackage().array());
            _m_bo.setHadAttackOpponentCidList(bmObj, _m_hadAttackOpponentCidList.makePackage().array());
            _m_bo.saveAllMarked(bmObj);

            //初始化额外检查战斗数据是否过期
            if (_isInit && _m_battleInfo != null)
            {
                _m_battleInfo.discard();
                _m_battleInfo = null;
            }

            //通知客户端
            if (!_isInit)
                getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否还有选择攻击次数
     * @return
     */
    public boolean hadSelectAttackNum()
    {
        _lock();
        try{
            return _m_bo.getHadSelectAttackNum() < RefGeneral.Ref().arena_select_attack_daily_limit;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 是否解锁了竞技场
     * @return
     */
    public boolean hadUnlockArena()
    {
        return _m_bo.getHadUnlockArena();
    }

    /**
     * 清除选择攻击次数
     */
    public void resetHadSelectAttackNum()
    {
        _lock();
        try
        {
            _m_bo.saveHadSelectAttackNum(getUSServer().getBM(), 0);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加已选择攻击次数
     * @param _value
     */
    public void incHadSelectAttackNum(int _value)
    {
        _lock();
        try
        {
            _m_bo.saveHadSelectAttackNum(getUSServer().getBM(), _m_bo.getHadSelectAttackNum() + _value);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清除随机攻击次数
     */
    public void resetHadRandomAttackNum()
    {
        _lock();
        try
        {
            _m_bo.saveHadRandomAttackNum(getUSServer().getBM(), 0);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加已随机攻击次数
     * @param _value
     */
    public Result incHadRandomAttackNum(int _value)
    {
        _lock();
        try
        {
            //检查是否需要重置每日计数
            checkResetDailyData(false);

            //检查是否超过每日上限
            if (_m_bo.getHadRandomAttackNum() + _value > RefGeneral.Ref().arena_random_attack_free_limit + _m_bo.getHadBuyRandomAttackNum())
                return ArenaErr.RANDOM_ATTACK_NUM_OVER_LIMIT;

            _m_bo.saveHadRandomAttackNum(getUSServer().getBM(), _m_bo.getHadRandomAttackNum() + _value);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加已购买随机攻击次数
     * @param _value
     */
    public Result buyRandomAttackNum(int _value, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //检查是否需要重置每日计数
            checkResetDailyData(false);

            //计算上限
            int maxBuyNum = Math.min(getUserData().getHeroComponent().getHeroNum() / RefGeneral.Ref().arena_random_attack_crystal_buy_ratio,
                    RefGeneral.Ref().arena_random_attack_crystal_buy_limit);

            //检查是否超过购买上限
            if (_m_bo.getHadBuyRandomAttackNum() + _value > maxBuyNum)
                return ArenaErr.BUY_RANDOM_ATTACK_NUM_OVER_LIMIT;

            //计算需要消耗的道具
            NPItemCostCollector_nosafe costCollector = new NPItemCostCollector_nosafe();
            for (int i = 0; i < _value; i++)
            {
                NPCommonCostItem item = UsFunc.calCostPrice(getUserData(), RefGeneral.Ref().arena_random_attack_crystal_buy_time_price_id,
                        _m_bo.getHadBuyRandomAttackNum() + i);
                if (item == null)
                    return CommErr.PRICE_ERR;

                costCollector.addCostItem(item);
            }

            //检查是否有足够的道具
            List<NPCommonCostItem> costList = costCollector.getItemList();
            if (!getUserData().hasCostItemList(costList))
                return CommErr.ITEM_NOT_ENOUGH;

            if (!getUserData().spendItem(costList, _context))
                return CommErr.CONSUME_FAIL;

            _m_bo.saveHadBuyRandomAttackNum(getUSServer().getBM(), _m_bo.getHadBuyRandomAttackNum() + _value);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重置已购买随机攻击次数
     */
    public void resetHadBuyRandomAttackNum()
    {
        _lock();
        try
        {
            _m_bo.saveHadBuyRandomAttackNum(getUSServer().getBM(), 0);

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录选择攻击的英雄id
     * @param
     */
    public void addSelectAttackHeroId(long _heroId)
    {
        _lock();
        try
        {
            _m_hadSelectAttackHeroList.addValueList(_heroId);
            _m_bo.saveHadSelectAttackHeroList(getUSServer().getBM(), _m_hadSelectAttackHeroList.makePackage().array());

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重置已选择攻击的英雄id
     */
    public void resetHadSelectAttackHeroList()
    {
        _lock();
        try
        {
            _m_hadSelectAttackHeroList.getValueList().clear();
            _m_bo.saveHadSelectAttackHeroList(getUSServer().getBM(), _m_hadSelectAttackHeroList.makePackage().array());

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否已经选择过攻击的英雄id
     * @param _heroId
     * @return
     */
    public boolean hadSelectAttackHeroId(long _heroId)
    {
        _lock();
        try{
            //检查是否需要重置每日计数
            checkResetDailyData(false);

            return _m_hadSelectAttackHeroList.getValueList().contains(_heroId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 记录随机攻击的英雄id
     * @param
     */
    public void addRandomAttackHeroId(long _heroId)
    {
        _lock();
        try
        {
            _m_hadRandomAttackHeroList.addValueList(_heroId);
            _m_bo.saveHadRandomAttackHeroList(getUSServer().getBM(), _m_hadRandomAttackHeroList.makePackage().array());

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重置已随机攻击的英雄id
     */
    public void resetHadRandomAttackHeroList()
    {
        _lock();
        try
        {
            _m_hadRandomAttackHeroList.getValueList().clear();
            _m_bo.saveHadRandomAttackHeroList(getUSServer().getBM(), _m_hadRandomAttackHeroList.makePackage().array());

            //通知客户端
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录已攻击过的对手
     * 用于优化匹配算法，避免当天重复匹配到同一对手
     * @param _opponentCid 对手CID，如果为0表示机器人对手不记录
     */
    public void recordAttackOpponent(long _opponentCid)
    {
        _lock();
        try
        {
            checkResetDailyData(false);

            // 机器人对手不记录
            if (_opponentCid == 0)
                return;

            // 检查是否已经记录过（避免重复）
            if (!_m_hadAttackOpponentCidList.getValueList().contains(_opponentCid))
            {
                _m_hadAttackOpponentCidList.addValueList(_opponentCid);

                // 保存到数据库
                _m_bo.saveHadAttackOpponentCidList(getUSServer().getBM(),
                        _m_hadAttackOpponentCidList.makePackage().array());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否已经选择过随机攻击的英雄id
     */
    public boolean hadRandomAttackHeroId(long _heroId)
    {
        _lock();
        try{
            //检查是否需要重置每日计数
            checkResetDailyData(false);

            return _m_hadRandomAttackHeroList.getValueList().contains(_heroId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 随机对手
     */
    public void randomOpponent(_ICallBackT<Result> _callback)
    {
        _lock();
        try
        {
            if (!hadUnlockArena())
            {
                _callback.onRunOver(ArenaErr.HERO_NUM_NOT_ENOUGH_TO_ENTER_ARENA);
                return;
            }

            //检查是否已经有对手了
            if (_m_battleInfo != null)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_HAD_SELECTED);
                return;
            }

            SerialPromise promise = new SerialPromise(getUserData().getUserLock());

            RefWrap<Long> opponentCid = new RefWrap<>(0L);

            AtomicBoolean needRandomRobot = new AtomicBoolean(false);
            RefWrap<RefArenaBotTemplate> refBotTemplate = new RefWrap<>(null);

            //检查是否需要随机一个机器人
            promise.then(p ->
            {
                _needRandomRobot((_result, pair) ->
                {
                    if (!_result.isSucc())
                    {
                        _callback.onRunOver(_result);
                        promise.breakOut();
                        return;
                    }

                    if (pair != null)
                    {
                        needRandomRobot.set(pair.first);
                        refBotTemplate.set(pair.second);
                    }

                    promise.commit();
                });
            });
            //从排行榜中随机一个对手
            promise.then(p ->
            {
                //判断是否需要随机一个机器人, 如果需要, 则直接返回
                if (needRandomRobot.get())
                {
                    promise.commit();
                    return;
                }

                _randomOpponentFromRank((_result, _opponentCid) ->
                {
                    //如果没有匹配上, 不做处理,
                    if (_result.getCode() == ArenaErr.OPPONENT_NOT_FOUND.getCode())
                    {
                        promise.commit();
                        return;
                    }

                    if (!_result.isSucc())
                    {
                        _callback.onRunOver(_result);
                        promise.breakOut();
                        return;
                    }

                    opponentCid.set(_opponentCid);
                    promise.commit();
                });
            });
            //选择对手
            promise.then(p ->
            {
                if (needRandomRobot.get() || opponentCid.get() == 0)
                {
                    selectRobot(refBotTemplate.get(), _callback);
                }else
                {
                    selectOpponent(opponentCid.get(), EArenaAttackType.RANDOM, new _ICallBackT<Result>()
                    {
                        @Override
                        public void onRunOver(Result _result)
                        {
                            if (!_result.isSucc())
                            {
                                selectRobot(refBotTemplate.get(), _callback);
                                return;
                            }

                            _callback.onRunOver(Result.SUCC);
                        }
                    });
                }
            });
        } finally
        {
            _unlock();
        }
    }

    /**
     * 判断是否需要随机一个机器人
     * @param _callback
     */
    private void _needRandomRobot(_ICallBackResultT<WCGPair<Boolean, RefArenaBotTemplate>> _callback)
    {
        _lock();
        try
        {
            //竞技场首次体验需要返回机器人
            RefArenaBotTemplate refFirstExperienceBotTemplate = RefArenaBotTemplate.getMgr().get(RefGeneral.Ref().arena_first_experience_bot_template_id);
            if (refFirstExperienceBotTemplate != null && getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.ARENA_ATTACK_TIMES) == 0)
            {
                _callback.onRunOver(Result.SUCC, new WCGPair<>(true, refFirstExperienceBotTemplate));
                return;
            }

            RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().arena_rank_fixed_id);
            if (rankFixedInfo == null)
            {
                _callback.onRunOver(RankErr.RANK_FIXED_NOT_FOUND, null);
                return;
            }

            rankFixedInfo.makeRankBaseByKey(getUserData().getCid(), false, (_result, _rankItem) ->
            {
                if (!_result.isSucc())
                {
                    _callback.onRunOver(_result, null);
                    return;
                }

                //如果玩家未解锁排行榜, 则直接返回
                if (_rankItem.getKey() == 0)
                {
                    _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
                    return;
                }

                //按照玩家当前排名, 计算是否需要随机一个机器人
                int selfRank = _rankItem.getRank();
                RefArenaBotRandom refByRank = RefArenaBotRandom.getMgr().getRefByRank(selfRank);
                if (refByRank == null)
                {
                    _callback.onRunOver(Result.SUCC, null);
                    return;
                }

                RefArenaBotTemplate refArenaBotTemplate = RefArenaBotTemplate.getMgr().get(refByRank.template_weight_list.random());
                if (refArenaBotTemplate == null)
                {
                    _callback.onRunOver(Result.SUCC, null);
                    return;
                }

                //根据万分比概率 随机布尔值
                boolean isRandom = CommonFunc.randomInt(1, 10000) <= refByRank.weight;
                _callback.onRunOver(Result.SUCC, new WCGPair<>(isRandom, refArenaBotTemplate));
            });
        } finally
        {
            _unlock();
        }
    }

    /**
     * 从排行榜中随机一个对手
     * @param _callback
     */
    /**
     * 从排行榜中随机一个对手（优化版：优先匹配实力相近的对手）
     *
     * 执行流程：
     * 1. 查询排行榜，找到玩家自己的排名
     * 2. 计算排名区间，过滤掉当天已打过的对手
     * 3. 如果所有对手都打过了，降级到纯随机匹配
     * 4. 批量异步获取候选对手的历史最高实力
     * 5. 选择实力最接近的对手
     *
     * @param _callback 回调接口
     */
    private void _randomOpponentFromRank(_ICallBackResultT<Long> _callback)
    {
        RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().arena_rank_fixed_id);
        if (rankFixedInfo == null)
        {
            _callback.onRunOver(RankErr.RANK_FIXED_NOT_FOUND, null);
            return;
        }

        //获取排行榜的总列表
        rankFixedInfo.makeRankBaseList(false, 0, (_result, _rankBaseList) ->
        {
            if (!_result.isSucc())
            {
                _callback.onRunOver(_result, null);
                return;
            }

            //遍历找到玩家自己的排名
            int selfRank = -1;
            for (Rank_BaseItem rankItem : _rankBaseList)
            {
                if (rankItem.getKey() == getUserData().getCid())
                {
                    //此处的rank是从1开始的，所以需要减1
                    selfRank = rankItem.getRank() - 1;
                    break;
                }
            }

            //如果找不到自己的排名, 则直接返回
            if (selfRank == -1)
            {
                _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
                return;
            }

            //如果排行榜中只有自己一个人, 则直接返回
            if (_rankBaseList.size() <= 1)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_NOT_FOUND, null);
                return;
            }

            //根据自己的排名, 计算随机排名范围
            WCGPairInt randomRange = RefGeneral.Ref().arena_random_attack_match_interval;
            int minRank = Math.max(0, selfRank + randomRange.first());
            int maxRank = Math.min(_rankBaseList.size() - 1, selfRank + randomRange.second());

            //如果范围不合法, 则直接返回
            if (minRank >= maxRank)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_NOT_FOUND, null);
                return;
            }

            //截取出随机范围内的列表
            List<Rank_BaseItem> rangeList = _rankBaseList.subList(minRank, maxRank + 1);
            //移除自己
            rangeList.removeIf(_rankBase -> _rankBase.getKey() == getUserData().getCid());

            //过滤掉当天已经打过的对手
            List<Rank_BaseItem> availableList = filterAvailableOpponents(rangeList);

            //如果所有对手都打过了，降级到纯随机匹配
            if (availableList.isEmpty())
            {
                USLog.info(getUSServer(), "ArenaComponent._randomOpponentFromRank - all opponents attacked today, fallback to random: cid={}, rangeSize={}",
                        getUserData().getCid(), rangeList.size());
                // 直接从原始候选列表中随机选择
                Rank_BaseItem opponent = rangeList.get(CommonFunc.randomInt(0, rangeList.size() - 1));
                _callback.onRunOver(Result.SUCC, opponent.getKey());
                return;
            }

            final long finalSelfMaxPower = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.POWER_MAX_RECORD);

            // 批量异步获取对手的历史最高实力
            List<OpponentPowerInfo> opponentPowerList = Collections.synchronizedList(new ArrayList<>());
            AtomicInteger loadCounter = new AtomicInteger(availableList.size());

            for (Rank_BaseItem rankItem : availableList)
            {
                long opponentCid = rankItem.getKey();

                // 异步获取对手的缓存信息
                getUSServer().getPlayerCacheGetter().getDealer(PlayerInfo_CommonShow.class)
                        .getInfo(opponentCid, new HandlerTwo<Boolean, PlayerInfo_CommonShow>()
                        {
                            @Override
                            public void handle(Boolean _isSucc, PlayerInfo_CommonShow _opponentCache)
                            {
                                _lock();
                                try
                                {
                                    if (_isSucc && _opponentCache != null)
                                    {
                                        // 获取对手的历史最高实力
                                        long opponentMaxPower = _opponentCache.getMaxPower();

                                        // 如果历史最高实力为0，跳过该对手
                                        if (opponentMaxPower == 0)
                                        {
                                            USLog.info(getUSServer(),
                                                    "ArenaComponent._randomOpponentFromRank - skip opponent with zero max_power: opponentCid={}", opponentCid);
                                        } else
                                        {
                                            // 计算实力差距
                                            long powerDiff = Math.abs(finalSelfMaxPower - opponentMaxPower);
                                            opponentPowerList.add(new OpponentPowerInfo(opponentCid, opponentMaxPower, powerDiff));
                                        }
                                    } else
                                    {
                                        USLog.info(getUSServer(),
                                                "ArenaComponent._randomOpponentFromRank - opponent cache not found: opponentCid={}", opponentCid);
                                    }

                                    // 检查是否所有查询完成
                                    if (loadCounter.decrementAndGet() == 0)
                                    {
                                        // 所有查询完成，选择实力最接近的对手
                                        if (opponentPowerList.isEmpty())
                                        {
                                            // 没有找到任何有效对手，降级到随机选择
                                            USLog.error(getUSServer(),
                                                    "ArenaComponent._randomOpponentFromRank - no valid opponent found, fallback to random: cid={}",
                                                    getUserData().getCid());
                                            Rank_BaseItem randomOpponent = availableList.get(CommonFunc.randomInt(0, availableList.size() - 1));
                                            _callback.onRunOver(Result.SUCC, randomOpponent.getKey());
                                        } else
                                        {
                                            // 按实力差距排序，选择最接近的对手
                                            opponentPowerList.sort(Comparator.comparingLong(o -> o.powerDiff));
                                            OpponentPowerInfo bestMatch = opponentPowerList.get(0);

                                            USLog.info(getUSServer(),
                                                    "ArenaComponent._randomOpponentFromRank " +
                                                            "match found: cid={}, opponentCid={}, selfMaxPower={}, opponentMaxPower={}, powerDiff={}",
                                                    getUserData().getCid(), bestMatch.cid, finalSelfMaxPower, bestMatch.maxPower, bestMatch.powerDiff);

                                            _callback.onRunOver(Result.SUCC, bestMatch.cid);
                                        }
                                    }
                                } finally
                                {
                                    _unlock();
                                }
                            }
                        });
            }
        });
    }

    /**
     * 过滤出可攻击的对手列表
     * 排除掉当天已经攻击过的对手
     *
     * @param rangeList 排名区间内的对手列表
     * @return 可攻击的对手列表
     */
    private List<Rank_BaseItem> filterAvailableOpponents(List<Rank_BaseItem> rangeList)
    {
        getUserData().lockUser();
        try
        {
            checkResetDailyData(false);

            ArrayList<Long> hadAttackCidList = _m_hadAttackOpponentCidList.getValueList();
            List<Rank_BaseItem> availableList = new ArrayList<>();
            for (Rank_BaseItem rankItem : rangeList)
            {
                if (!hadAttackCidList.contains(rankItem.getKey()))
                {
                    availableList.add(rankItem);
                }
            }
            return availableList;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 对手实力信息（内部类）
     * 用于存储对手的CID、历史最高实力和实力差距
     */
    private static class OpponentPowerInfo
    {
        long cid;
        long maxPower;
        long powerDiff;

        OpponentPowerInfo(long cid, long maxPower, long powerDiff)
        {
            this.cid = cid;
            this.maxPower = maxPower;
            this.powerDiff = powerDiff;
        }
    }

    /**
     * 选择机器人
     * @param _refArenaBotTemplate
     * @param _callback
     */
    private void selectRobot(RefArenaBotTemplate _refArenaBotTemplate, _ICallBackT<Result> _callback)
    {
        _lock();
        try
        {
            //检查是否已经有对手了
            if (_m_battleInfo != null)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_HAD_SELECTED);
                return;
            }

            Result result = incHadRandomAttackNum(1);
            if (!result.isSucc())
            {
                _callback.onRunOver(result);
                return;
            }

            //根据机器人模板, 生成机器人数据
            //玩家战力
            long totalPower = getUserData().getHeroComponent().getTotalPower();
            //玩家大臣平均等级
            int avgLevel = (int) (getUserData().getHeroComponent().getTotalHeroLevel() / getUserData().getHeroComponent().getHeroNum());
            //计算机器人战力
            long robotPower = (long) (Math.ceil(totalPower * _refArenaBotTemplate.power_per / 10000d));
            //计算机器人大臣数量
            int oriRobotHeroNum = (int) Math.ceil(getUserData().getHeroComponent().getHeroNum() * _refArenaBotTemplate.hero_num_per / 10000d);
            //机器人大臣数量不能超过机器人大臣列表的数量
            int robotHeroNum = Math.min(oriRobotHeroNum,RefGeneral.Ref().arena_bot_hero_list.size());
            //随机大臣列表
            List<Long> heroIdList = new ArrayList<>();
            if (robotHeroNum == RefGeneral.Ref().arena_bot_hero_list.size())
            {
                heroIdList.addAll(RefGeneral.Ref().arena_bot_hero_list);
            }else
            {
                heroIdList.addAll(CommonFunc.getRndCount(RefGeneral.Ref().arena_bot_hero_list, robotHeroNum));
            }

            //打乱大臣列表，取前30%的大臣为高战力大臣，后70%为低战力大臣
            Collections.shuffle(heroIdList);
            //高战力大臣数量
            int highHeroNum = (int) Math.ceil(heroIdList.size() * 0.3);

            List<Long> highHeroList = heroIdList.subList(0, highHeroNum);
            List<Long> lowHeroList = heroIdList.subList(highHeroNum, heroIdList.size());

            //计算总战力
            long opponentPower = 0;

            Hero_ArenaShowList allHeroList = new Hero_ArenaShowList();
            if (!highHeroList.isEmpty())
            {
                //计算高战力大臣总战力
                long highTotalPower = (long) (Math.ceil(robotPower * _refArenaBotTemplate.power_per_pair.first() / 10000d));
                //计算高战力大臣平均战力
                long highAvgPower = (long) Math.ceil((double) highTotalPower / highHeroNum);
                //计算高战力大臣等级
                int highHeroLevel = (int) (Math.ceil(avgLevel * _refArenaBotTemplate.level_per_pair.first() / 10000d));
                //生成机器人大臣数据
                for (Long heroId : highHeroList)
                {
                    Hero_ArenaShowInfo heroInfo = new Hero_ArenaShowInfo();
                    heroInfo.setHeroId(heroId);
                    heroInfo.setPower(highAvgPower);
                    heroInfo.setLevel(highHeroLevel);
                    allHeroList.addHeroList(heroInfo);
                }
                opponentPower += highAvgPower * highHeroNum;
            }
            if (!lowHeroList.isEmpty())
            {
                //计算低战力大臣总战力
                long lowTotalPower = (long) (Math.ceil(robotPower * _refArenaBotTemplate.power_per_pair.second() / 10000d));
                //计算低战力大臣的平均战力
                long lowAvgPower = (long) Math.ceil((double) lowTotalPower / lowHeroList.size());
                //计算低战力大臣等级
                int lowHeroLevel = (int) (Math.ceil(avgLevel * _refArenaBotTemplate.level_per_pair.second() / 10000d));
                //生成机器人大臣数据
                for (Long heroId : lowHeroList)
                {
                    Hero_ArenaShowInfo heroInfo = new Hero_ArenaShowInfo();
                    heroInfo.setHeroId(heroId);
                    heroInfo.setPower(lowAvgPower);
                    heroInfo.setLevel(lowHeroLevel);
                    allHeroList.addHeroList(heroInfo);
                }
                opponentPower += lowAvgPower * lowHeroList.size();
            }

            PlayerArenaBattleBO bo = new PlayerArenaBattleBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setOpponentCid(getUSServer().getBM(), 0);
            bo.setOpponentHeroList(getUSServer().getBM(), allHeroList.makePackage().array());
            bo.setOpponentPower(getUSServer().getBM(), opponentPower);
            bo.setAttackType(getUSServer().getBM(), EArenaAttackType.RANDOM.ordinal());
            bo.setIsBot(getUSServer().getBM(), true);
            bo.insert(getUSServer().getBM());

            _m_battleInfo = new ArenaBattleInfo(ArenaComponent.this, bo);

            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(_m_battleInfo.makeProto()));

            _callback.onRunOver(Result.SUCC);
        } finally
        {
            _unlock();
        }

    }

    /**
     * 选择对手
     * @param _opponentCid 对手cid
     * @param _type
     * @param _callback
     */
    public void selectOpponent(long _opponentCid, EArenaAttackType _type, _ICallBackT<Result> _callback)
    {
        _lock();
        try
        {
            //检查是否已经有对手了
            if (_m_battleInfo != null)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_HAD_SELECTED);
                return;
            }

            //查询对手大臣信息
            getUSServer().getPlayerHeroCacheGetter().getInfo(Hero_ArenaShowList.class, _opponentCid, new HandlerTwo<Boolean, Hero_ArenaShowList>()
            {
                @Override
                public void handle(Boolean _isSucc, Hero_ArenaShowList _cacheData)
                {
                    _lock();
                    try
                    {
                        if (!_isSucc)
                        {
                            USLog.error(getUSServer(), "ArenaComponent load player hero data fail, cid:{} targetCid:{}",
                                    getUserData().getCid(), _opponentCid);
                            _callback.onRunOver(ArenaErr.OPPONENT_DATA_LOAD_FAIL);
                            return;
                        }

                        if (_cacheData.getHeroList().isEmpty())
                        {
                            USLog.error(getUSServer(), "ArenaComponent load player hero data empty, cid:{} targetCid:{}",
                                    getUserData().getCid(), _opponentCid);
                            _callback.onRunOver(ArenaErr.OPPONENT_DATA_LOAD_FAIL);
                            return;
                        }

                        if (_type == EArenaAttackType.RANDOM)
                        {
                            Result result = incHadRandomAttackNum(1);
                            if (!result.isSucc())
                            {
                                _callback.onRunOver(result);
                                return;
                            }
                        }

                        //计算对手战力
                        long opponentPower = 0;
                        for (int i = 0; i < _cacheData.getHeroList().size(); i++)
                        {
                            opponentPower += _cacheData.getHeroList().get(i).getPower();
                        }

                        //按照实力对大臣列表进行排序
                        _cacheData.getHeroList().sort((o1, o2) -> (int) (o2.getPower() - o1.getPower()));

                        PlayerArenaBattleBO bo = new PlayerArenaBattleBO();
                        bo.setCid(getUSServer().getBM(), getUserData().getCid());
                        bo.setOpponentCid(getUSServer().getBM(), _opponentCid);
                        bo.setOpponentHeroList(getUSServer().getBM(), _cacheData.makePackage().array());
                        bo.setOpponentPower(getUSServer().getBM(), opponentPower);
                        bo.setAttackType(getUSServer().getBM(), _type.ordinal());
                        bo.insert(getUSServer().getBM());

                        _m_battleInfo = new ArenaBattleInfo(ArenaComponent.this, bo);

                        getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(_m_battleInfo.makeProto()));

                        _callback.onRunOver(Result.SUCC);
                    } finally
                    {
                        _unlock();
                    }
                }
            });
        } finally
        {
            _unlock();
        }
    }

    /**
     * 选择对手（增加对机器人对手的支持，只用于引导）
     * @param _type
     * @param _callback
     */
    public void selectUsOpponent(EArenaAttackType _type, _ICallBackT<Result> _callback)
    {
        _lock();
        try
        {
            //检查是否已经有对手了
            if (_m_battleInfo != null)
            {
                _callback.onRunOver(ArenaErr.OPPONENT_HAD_SELECTED);
                return;
            }

            Hero_ArenaShowList arenaShowList = RefGeneral.Ref().arenaUsHeroList;
            if (arenaShowList.getHeroList().isEmpty())
            {
                USLog.error(getUSServer(), "ArenaComponent load us hero data empty, cid:{}",
                        getUserData().getCid());
                _callback.onRunOver(ArenaErr.OPPONENT_DATA_LOAD_FAIL);
                return;
            }

            if (_type == EArenaAttackType.RANDOM)
            {
                Result result = incHadRandomAttackNum(1);
                if (!result.isSucc())
                {
                    _callback.onRunOver(result);
                    return;
                }
            }

            //计算对手战力
            long opponentPower = 0;
            for (int i = 0; i < arenaShowList.getHeroList().size(); i++)
            {
                opponentPower += arenaShowList.getHeroList().get(i).getPower();
            }

            //按照实力对大臣列表进行排序
            arenaShowList.getHeroList().sort((o1, o2) -> (int) (o2.getPower() - o1.getPower()));

            PlayerArenaBattleBO bo = new PlayerArenaBattleBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setOpponentCid(getUSServer().getBM(), 0);
            bo.setOpponentHeroList(getUSServer().getBM(), arenaShowList.makePackage().array());
            bo.setOpponentPower(getUSServer().getBM(), opponentPower);
            bo.setAttackType(getUSServer().getBM(), _type.ordinal());
            bo.insert(getUSServer().getBM());

            _m_battleInfo = new ArenaBattleInfo(ArenaComponent.this, bo);

            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(_m_battleInfo.makeProto()));

            _callback.onRunOver(Result.SUCC);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 选择出战大臣
     */
    public Result selectBattleHero(HeroInfo _heroInfo, String _opponentBotName, boolean _isRandom, long _itemId, long _buffId, NPPlayerContext _context)
    {
        if (_m_battleInfo == null)
            return ArenaErr.OPPONENT_NOT_SELECTED;

        List<NPCommonCostItem> costList = new ArrayList<>();

        RefArenaBuff refArenaBuff = null;
        if (_buffId != 0)
        {
            //判断buff是否存在
            refArenaBuff = RefArenaBuff.getMgr().get(_buffId);
            if (refArenaBuff == null)
                return CommErr.REF_NOT_FOUND;

            //判断当前回合是否可以购买此buff
            if (!RefGeneral.Ref().arena_initial_choose_buff_list.contains(_buffId))
                return ArenaErr.CANT_BUY_ROUND_BUFF;

            costList.add(refArenaBuff.cost);
        }

        RefArenaSelectAttackConsume refItem = null;
        //如果是随机对手, 则不需要消耗指定攻击道具
        if (!_isRandom)
        {
            refItem = RefArenaSelectAttackConsume.getMgr().get(_itemId);
            if (refItem == null)
                return CommErr.REF_NOT_FOUND;

            costList.add(refItem.cost);
        }

        _lock();
        try{
            //检查是否已经选择过
            boolean hadSelect = _isRandom ? hadRandomAttackHeroId(_heroInfo.getHeroId()) : hadSelectAttackHeroId(_heroInfo.getHeroId());
            if (hadSelect)
                return ArenaErr.HERO_HAD_SELECTED;

            //检查是否超过上限
            if (!_isRandom && !getUserData().getArenaComponent().hadSelectAttackNum())
                return ArenaErr.SELECT_ATTACK_NUM_OVER_LIMIT;

            //检查是否已经选择过
            if (_isRandom)
            {
                addRandomAttackHeroId(_heroInfo.getHeroId());
            } else
            {
                addSelectAttackHeroId(_heroInfo.getHeroId());
                getUserData().getArenaComponent().incHadSelectAttackNum(1);
            }
        }finally
        {
            _unlock();
        }

        //扣除道具
        if (!costList.isEmpty())
        {
            boolean consumeSuc = getUserData().spendCostItemList(costList, _context);
            if (!consumeSuc)
                return CommErr.CONSUME_FAIL;
        }

        _m_battleInfo.selectBattleHero(_heroInfo, _opponentBotName, refItem, refArenaBuff);

        return Result.SUCC;
    }

    /**
     * 检查是否可以攻击
     * @param _opponentCid
     * @param _type
     * @param _callback
     */
    public void checkOpponentCanAttack(long _opponentCid, EArenaAttackType _type, _ICallBackT<Result> _callback)
    {
        if (_type == EArenaAttackType.RANDOM || _type == EArenaAttackType.FIGHT_BACK)
        {
            _callback.onRunOver(Result.SUCC);
        } else
        {
            RankFixedInfo rankFixedInfo = getUSServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().arena_rank_fixed_id);
            if (rankFixedInfo == null)
            {
                _callback.onRunOver(RankErr.RANK_FIXED_NOT_FOUND);
                return;
            }

            rankFixedInfo.makeRankBaseByKey(_opponentCid, false, (_result, _rankBase) ->
            {
                if (!_result.isSucc())
                {
                    _callback.onRunOver(_result);
                    return;
                }

                _callback.onRunOver(Result.SUCC);
            });
        }
    }

    /**
     * 选择buff
     * @param _buffId
     */
    public Result chooseBuff(long _buffId, NPPlayerContext _context)
    {
        if (_m_battleInfo == null)
            return ArenaErr.OPPONENT_NOT_SELECTED;

        return _m_battleInfo.chooseBuff(_buffId, _context);
    }

    /**
     * 回合战斗
     */
    public Result roundAttack(long _opponentHeroId, GS2GC_023_005_RetRoundAttack _proto, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_m_battleInfo == null)
                return ArenaErr.OPPONENT_NOT_SELECTED;

            //攻击操作
            Result attackResult = _m_battleInfo.roundAttack(_opponentHeroId,
                    _proto.getRoundResult(), _proto.getRoundReward(), _context);
            if (!attackResult.isSucc())
                return attackResult;

            //触发事件（发起战斗事件）
            Event_P_ARENA_OP evt = new Event_P_ARENA_OP(_context, 1);
            getUserData().onLogicEvent(evt);

            //进行结算
            if (_m_battleInfo.roundSettle(_proto.getBattleResult(), _context))
            {
                //清除数据
                _m_battleInfo.discard();
                _m_battleInfo = null;
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 一键攻击
     * @param _buffPrefer 优先选择的buff
     * @param _proto
     * @param _context
     * @return
     */
    public Result aKeyAttack(EArenaBuffType _buffPrefer, GS2GC_023_013_RetArenaAKeyAttack _proto, NPPlayerContext _context)
    {
        _lock();
        try{
            if (_m_battleInfo == null)
                return ArenaErr.OPPONENT_NOT_SELECTED;

            ArenaAKeyAttackDealer aKeyAttackDealer = new ArenaAKeyAttackDealer(_m_battleInfo, _buffPrefer);

            //攻击操作
            aKeyAttackDealer.aKeyAttack(_context);

            //结算操作
            Result settleResult = aKeyAttackDealer.settle(_proto.getBattleResult(), _context);
            if (!settleResult.isSucc())
                return settleResult;

            //清除数据
            _m_battleInfo.discard();
            _m_battleInfo = null;

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 删除当前玩家战斗信息
     * @return true-删除成功，false-战斗不存在
     */
    public boolean deleteBattleInfo()
    {
        _lock();
        try
        {
            if (_m_battleInfo == null)
                return false;

            _m_battleInfo.discard();
            _m_battleInfo = null;

            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_052_OnArenaBaseInfoChg(makeBaseInfo()));

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 解锁竞技场功能
     * 参考PlayerEffectDealer_S_UNLOCK_ARENA
     */
    public void checkUnlockArena(NPPlayerContext _context)
    {
        // 【GOB-5210】玩家拥有指定数量的大臣，解锁竞技场
        if (getUserData().getHeroComponent().getHeroNum() < RefGeneral.Ref().unlock_arena_when_hero_num_equals)
            return;

        _lock();
        try
        {
            // 检查是否已解锁
            if (hadUnlockArena())
                return;

            // 设置解锁标识
            _m_bo.saveHadUnlockArena(getUSServer().getBM(), true);

            //解锁产出功能
            SpecialItemDealer_Station stationDealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.ARENA_STATION, SpecialItemDealer_Station.class);
            if (stationDealer != null)
            {
                stationDealer.unlockFunc();
            }else
            {
                USLog.error(getUserData().getUSServer(), "PlayerEffectDealer_S_UNLOCK_ARENA dealEffect stationDealer is null, cid:{}", getUserData().getCid());
            }

            //获得竞技场积分
            getUserData().onLogicEvent(new Event_P_ARENA_INFLUENCE_CHG(_context, RefGeneral.Ref().arena_initial_influence));

            LogArenaUnlockBO logBo = new LogArenaUnlockBO();
            logBo.setCid(getUSServer().getBM(), getCid());
            logBo.setLevel(getUSServer().getBM(), (int) getUserData().getParam(ENPPlayerParam.LEVEL));
            logBo.setEarnings(getUSServer().getBM(), getUserData().getPlayerComponent().getEarnings());
            logBo.setPower(getUSServer().getBM(), getUserData().getHeroComponent().getTotalPower());
            CommLogDB.log(getUSServer().getBM(), logBo, _context);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造基础信息
     */
    public Arena_BaseInfo makeBaseInfo()
    {
        Arena_BaseInfo baseInfo = new Arena_BaseInfo();
        baseInfo.setHadSelectAttackNum(_m_bo.getHadSelectAttackNum());
        baseInfo.setHadRandomAttackNum(_m_bo.getHadRandomAttackNum());
        baseInfo.setHadBuyRandomAttackNum(_m_bo.getHadBuyRandomAttackNum());
        baseInfo.setLastResetTimeMs(_m_bo.getLastResetTimeMs());
        baseInfo.getHadSelectAttackHeroList().addAll(_m_hadSelectAttackHeroList.getValueList());
        baseInfo.getHadRandomAttackHeroList().addAll(_m_hadRandomAttackHeroList.getValueList());
        return baseInfo;
    }

    /**
     * 构造竞技场信息
     */
    public Arena_Info makeProto()
    {
        Arena_Info proto = new Arena_Info();
        proto.setBaseInfo(makeBaseInfo());
        if (_m_battleInfo != null)
            proto.setBattleInfo(_m_battleInfo.makeProto());
        return proto;
    }
}

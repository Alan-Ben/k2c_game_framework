package NPUSServer.NPUSUserMgr.UserComp.TowerComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.MailObj.Mail_Data;
import Common.TowerObj.Tower_OpponentInfo;
import Common.TowerObj.Tower_PosInfo;
import CommonEnum.ECurrency;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TowerErr;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Tower.RefTowerChapter;
import NPGameRes.Refs.Tower.RefTowerResearch;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;
import NPUSServer.Tower.TowerAttackResult;
import NPUSServer.Tower.TowerFloorInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerTowerBO;
import USDB.Bo.PlayerTowerChapterResearchBO;

import java.util.ArrayList;
import java.util.List;

public class TowerComponent extends _ANPUserComponent implements _IHandlerHolder
{
    //数据id
    private long _m_dbId;

    //章节ID
    private long _m_chapterId;
    //关卡内层数 从1开始
    private int _m_chapterLevel;

    //已激活研究章节ID
    private long _m_hadActiveResearchChapterId;
    //已激活研究章节内层数
    private int _m_hadActiveResearchChapterLevel;
    //已激活研究章节配置
    private RefTowerResearch _m_refTowerResearch;

    //到达最高章节ID
    private long _m_highestHadReachChapterId;
    //到达最高章节内层数
    private int _m_highestHadReachChapterLevel;

    //上次领取每日迷宫币的时间戳
    private long _m_lastDrawTowerCoinTimeMs;
    //已领取章节研究奖励 章节ID列表
    private List<Long> _m_hadDrawChapterResearchList;

    //当前金币加成（即建筑产出收益加成）
    private int _m_buildingProfitAddPer;

    private TowerStageRefObj _m_stageRefObj;

	//防守战报数据
	private TowerDefenceReportList _m_alDefenceReportList;

    public TowerComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.TOWER);

        _m_hadDrawChapterResearchList = new ArrayList<>();

        _m_alDefenceReportList = new TowerDefenceReportList(this);
    }

    public TowerDefenceReportList getDefenceReportList() {return _m_alDefenceReportList;}

    @Override
    protected void _init()
    {
        // 使用ALProcess进行多步骤初始化
        ALProcess process = ALProcess.CreateProcess("tower_component_init");

        // 步骤1: 加载主数据
        process.addResDelegateProcess(_action -> _initTowerBo(_action::dealAction),
                "init_tower_bo", null, false);

        // 步骤2: 加载已领取章节奖励数据
        process.addResDelegateProcess(_action -> _initTowerChapterResearchBo(_action::dealAction),
                "init_tower_chapter_research_bo", null, false);

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
                USLog.error(getUSServer(), "Player {} tower component init failed", getUserData().getCid());
                getUserData().setDataLoadFail();
            }
        });
    }

    private void _initTowerBo(_ICallBackBool _callback)
    {
        getUSServer().getBM().getBM(PlayerTowerBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerTowerBO>()
        {
            @Override
            public void dealSuc(PlayerTowerBO _bo)
            {
                _m_dbId = _bo.getId();
                _m_chapterId = _bo.getChapterId();
                _m_chapterLevel = _bo.getChapterLevel();
                _m_hadActiveResearchChapterId = _bo.getHadActiveResearchChapterId();
                _m_hadActiveResearchChapterLevel = _bo.getHadActiveResearchChapterLevel();
                _m_highestHadReachChapterId = _bo.getHighestHadReachChapterId();
                _m_highestHadReachChapterLevel = _bo.getHighestHadReachChapterLevel();
                _m_lastDrawTowerCoinTimeMs = _bo.getLastDrawTowerCoinTimeMs();

                _m_stageRefObj = RefTowerChapter.getMgr().getStageRefObj(_bo.getChapterId(), _bo.getChapterLevel());
                if (_m_stageRefObj == null)
                    USLog.error(getUSServer(), "player:{} tower stage ref obj init fail, not find ref.", getUserData().getCid());

                _m_refTowerResearch = RefTowerResearch.getMgr().lookupByChapterLvl(_m_hadActiveResearchChapterId, _m_hadActiveResearchChapterLevel);

                _callback.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "player:{} tower component init fail, get bo fail.", getUserData().getCid());
                    _callback.onRunOver(false);
                    return;
                }

                _m_chapterId = 1;
                _m_chapterLevel = 1;
                _m_hadActiveResearchChapterId = 1;
                _m_hadActiveResearchChapterLevel = 1;
                _m_highestHadReachChapterId = 1;
                _m_highestHadReachChapterLevel = 1;

                _m_stageRefObj = RefTowerChapter.getMgr().getStageRefObj(_m_chapterId, _m_chapterLevel);
                if (_m_stageRefObj == null)
                    USLog.error(getUSServer(), "player:{} tower stage ref obj init fail, not find ref.", getUserData().getCid());

                _m_refTowerResearch = RefTowerResearch.getMgr().lookupByChapterLvl(_m_hadActiveResearchChapterId, _m_hadActiveResearchChapterLevel);

                _callback.onRunOver(true);
            }
        });
    }

    private void _initTowerChapterResearchBo(_ICallBackBool _callback)
    {
        getUSServer().getBM().getBM(PlayerTowerChapterResearchBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerTowerChapterResearchBO>>()
        {
            @Override
            public void dealSuc(List<PlayerTowerChapterResearchBO> _boList)
            {
                for (PlayerTowerChapterResearchBO bo : _boList)
                {
                    _m_hadDrawChapterResearchList.add(bo.getChapterId());
                }
                _callback.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(false);
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
        // 如果玩家在PVP楼层，需要检查是否还在位置上
        if (_m_stageRefObj != null && !_m_stageRefObj.getRefChapter().if_pve_chapter)
        {
            TowerFloorInfo floorInfo = getUSServer().getTowerMgr().lookFloorByCid(getUserData().getCid());
            if (floorInfo == null)
            {
                onPosChg(_m_chapterId, 1, getUserData().getPlayerInitContext());
            }
        }

        //领取跨天每日迷宫币的总和
        gainDailyTowerCoin(getUserData().getPlayerInitContext());

        // 重新计算建筑产出收益加成
        recalBuildingProfitAddPer(true);

        //监听跨天处理
        getUserData().OnCrossDay.addHandler(this, new HandlerOne<Integer>()
        {
            @Override
            public void handle(Integer _nowTag)
            {
                gainDailyTowerCoin(getUserData().getPlayerInitContext());
            }
        });
    }

    @Override
    public void dispose()
    {

    }

    /**
     * 攻击指定章节和层数
     * @param _chapterId
     * @param _level
     * @param _context
     * @return
     */
    public void attackFloor(long _chapterId, int _level, NPPlayerContext _context, _ICallBackResultT<TowerAttackResult> _callback)
    {
        if (_m_chapterId == _chapterId && _m_chapterLevel == _level)
        {
            _callback.onRunOver(TowerErr.TOWER_NOT_ATTACK_FORWARD, null);
            return;
        }

        RefTowerChapter targetChapterRef = RefTowerChapter.getMgr().get(_chapterId);
        if (targetChapterRef == null)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
            return;
        }

        TowerStageRefObj targetStageRefObj = targetChapterRef.getStageRefObj(_level);
        if (targetStageRefObj == null)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
            return;
        }

        if (targetStageRefObj.getChapterStartLevel() + targetStageRefObj.getRefChapterStage().levels_in_range_count  < _level)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
            return;
        }

        if (_m_stageRefObj == null)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
            return;
        }

        // 判断当前章节和层数是否可以攻击
        if (_m_stageRefObj.getStageIndex() > targetStageRefObj.getStageIndex())
        {
            _callback.onRunOver(TowerErr.TOWER_NOT_ATTACK_FORWARD, null);
            return;
        }

        // 记录挑战前的进度
        int triggerIdBefore = _makeTriggeridFromChapterLevel(_m_chapterId, _m_chapterLevel);

        getUSServer().getTowerMgr().attackFloor(targetChapterRef, targetStageRefObj, _chapterId, _level, getUserData(), _context,
                (_result, _resultObj) ->
                {
                    getUserData().lockUser();
                    try
                    {
                        if (!_result.isSucc() || !_resultObj.isWin())
                        {
                            _callback.onRunOver(_result, _resultObj);
                            return;
                        }

                        onPosChg(_chapterId, _level, _context);

                        getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TOWER_FIGHT_SUCCESS_TIMES, 1, _context);

                        // 记录挑战后的进度（挑战成功）
                        int triggerIdFinal = _makeTriggeridFromChapterLevel(_chapterId, _level);
                        MJEventLog.logClimbTower(getUserData(), triggerIdBefore, 1, triggerIdFinal);

                        _callback.onRunOver(Result.SUCC, _resultObj);
                    } finally
                    {
                        getUserData().unlockUser();
                    }
                });
    }

    /**
     * 处理位置变化
     * @param _chapterId
     * @param _level
     * @param _context
     */
    private void onPosChg(long _chapterId, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否在同一位置
            if (_m_chapterId == _chapterId && _m_chapterLevel == _level)
                return;

            // 如果攻击成功，执行后续逻辑
            // 更新当前章节和层数
            _m_chapterId = _chapterId;
            _m_chapterLevel = _level;
            _m_stageRefObj = RefTowerChapter.getMgr().getStageRefObj(_chapterId, _level);
            if (_m_stageRefObj == null)
                USLog.error(getUSServer(), "TowerComponent onPosChg set _m_stageRefObj fail, cid:{} chapterId:{} level:{}",
                        getUserData().getCid(), _chapterId, _level);

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_061_OnTowerPosChg(makePosInfo()));

            // 检查是否到达了最高章节
            checkHadReachHighestChapter(_chapterId, _level, _context);

            // 保存数据到数据库
            _savePosData();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取跨天每日迷宫币的总和
     * @param _context
     */
    public void gainDailyTowerCoin(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_lastDrawTowerCoinTimeMs <= 0)
                return;

            if (_m_stageRefObj == null)
                return;

            //更新当前时间
            long nowTimeMS = CommonFunc.getNowTimeMS();

            //计算可以领取的天数
            int diffDays = CommonFunc.getDayDiff(_m_lastDrawTowerCoinTimeMs, nowTimeMS);
            if (diffDays <= 0)
                return;

            //计算每日迷宫币奖励
            long gainCount = _m_stageRefObj.calDailyCoinReward(_m_chapterLevel) * diffDays;

            //发送邮件领取奖励
            if (gainCount <= 0)
                return;

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            //邮件配置
            mailData.setMailRefId(RefGeneral.Ref().tower_coin_daily_reward_mail_id);
            //邮件附件
            NPCommon_ItemInfo mailItem = new NPCommon_ItemInfo();
            mailItem.setItemType(ENPItemType.CURRENCY.ordinal());
            mailItem.setSubId(ECurrency.TOWER_COIN.ordinal());
            mailItem.setCount(gainCount);
            mailData.getItemList().getItemList().add(mailItem);

            //发送邮件
            getUserData().getMailComponent().addMail(mailData, _context);

            //上次领取时间
            _m_lastDrawTowerCoinTimeMs = nowTimeMS;

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("last_draw_tower_coin_time_ms", _m_lastDrawTowerCoinTimeMs);
            getUSServer().getBM().getBM(PlayerTowerBO.class).update("id", _m_dbId, updateValue);

        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 检查是否到达了最高章节
     */
    private void checkHadReachHighestChapter(long _chapterId, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_chapterId <= _m_highestHadReachChapterId &&
                    (_chapterId != _m_highestHadReachChapterId || _level <= _m_highestHadReachChapterLevel))
                return;

            long origHighestChapterId = _m_highestHadReachChapterId;
            int origHighestChapterLevel = _m_highestHadReachChapterLevel;

            // 查询区间对象
            RefTowerChapter oriChapterRef = RefTowerChapter.getMgr().get(origHighestChapterId);
            RefTowerChapter newChapterRef = RefTowerChapter.getMgr().get(_chapterId);
            if (oriChapterRef == null || newChapterRef == null)
            {
                USLog.error(getUSServer(), "TowerComponent checkHadReachHighestChapter failed, chapter ref not found. orig: {}, new: {}",
                        getUserData().getCid(), origHighestChapterId, _chapterId);
                return;
            }

            TowerStageRefObj oriStageRefObj = oriChapterRef.getStageRefObj(origHighestChapterLevel);
            TowerStageRefObj newStageRefObj = newChapterRef.getStageRefObj(_level);
            if (oriStageRefObj == null || newStageRefObj == null)
            {
                USLog.error(getUSServer(), "TowerComponent checkHadReachHighestChapter failed, stage ref not found. orig: {}, new: {}, origLevel: {}, newLevel: {}",
                        getUserData().getCid(), origHighestChapterLevel, _level);
                return;
            }

            long towerCoinCount = 0;
            long diamondCount = 0;

            List<NPCommonCostItem> itemList = new ArrayList<>();
            // 判断是否在同一个区间
            if (oriStageRefObj == newStageRefObj)
            {
                for (int i = origHighestChapterLevel + 1; i <= _level; i++)
                {
                    towerCoinCount += oriStageRefObj.calPassedTowerCoin(i);
                    diamondCount += oriStageRefObj.getRefChapterStage().diamond_reward_per_level;
                }
            }else
            {
                for (int i = oriStageRefObj.getStageIndex(); i <= newStageRefObj.getStageIndex(); i++)
                {
                    TowerStageRefObj stageRefObj = RefGeneral.Ref().towerChapterStageList.get(i);
                    if (stageRefObj == null)
                        continue;

                    if (i == oriStageRefObj.getStageIndex())
                    {
                        // 累计当前区间的迷宫币和钻石奖励
                        for (int j = origHighestChapterLevel + 1; j < stageRefObj.getChapterStartLevel() + stageRefObj.getRefChapterStage().levels_in_range_count; j++)
                        {
                            towerCoinCount += stageRefObj.calPassedTowerCoin(j);
                            diamondCount += stageRefObj.getRefChapterStage().diamond_reward_per_level;
                        }
                    } else if (i == newStageRefObj.getStageIndex())
                    {
                        // 累计新区间的迷宫币和钻石奖励
                        for (int j = stageRefObj.getChapterStartLevel(); j <= _level; j++)
                        {
                            towerCoinCount += stageRefObj.calPassedTowerCoin(j);
                            diamondCount += stageRefObj.getRefChapterStage().diamond_reward_per_level;
                        }
                    } else
                    {
                        // 累计整个区间的迷宫币和钻石奖励
                        for (int j = stageRefObj.getChapterStartLevel(); j < stageRefObj.getChapterStartLevel() + stageRefObj.getRefChapterStage().levels_in_range_count; j++)
                        {
                            towerCoinCount += stageRefObj.calPassedTowerCoin(j);
                            diamondCount += stageRefObj.getRefChapterStage().diamond_reward_per_level;
                        }
                    }
                }
            }

            //如果原来的最高章节ID和当前章节ID不一致，需要获取章节的首达奖励
            if (origHighestChapterId != _chapterId)
            {
                for (long chapterId = _chapterId; chapterId > origHighestChapterId; chapterId--)
                {
                    RefTowerChapter refChapter = RefTowerChapter.getMgr().get(chapterId);
                    if (refChapter == null)
                    {
                        USLog.error(getUSServer(), "TowerComponent checkHadReachHighestChapter draw chapter first reach reward failed, ref not found. cid: {}, chapterId: {}",
                                getUserData().getCid(), chapterId);
                        continue;
                    }

                    // 获取章节首达奖励
                    itemList.addAll(refChapter.gain_item_list);
                }
            }

            itemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.TOWER_COIN.ordinal(), towerCoinCount));
            itemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), diamondCount));

            getUserData().gainItemList(itemList, _context);

            _m_highestHadReachChapterId = _chapterId;
            _m_highestHadReachChapterLevel = _level;

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_064_OnTowerHighestPosHadReachChg(makeHighestPosInfo()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 激活研究
     * @param _chapterId
     * @param _level
     * @param context
     * @return
     */
    public Result activeResearch(long _chapterId, int _level, NPPlayerContext context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否已经激活过该研究章节
            if (_m_hadActiveResearchChapterId > _chapterId ||
                    (_m_hadActiveResearchChapterId == _chapterId && _m_hadActiveResearchChapterLevel >= _level))
                return TowerErr.TOWER_RESEARCH_ALREADY_ACTIVE;

            return setActiveResearchPos(_chapterId, _level, context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重新计算建筑产出收益加成
     */
    private void recalBuildingProfitAddPer(boolean _isInit)
    {
        getUserData().lockUser();
        try
        {
            // 如果没有激活研究章节，则加成为0
            int totalAddPer = _m_refTowerResearch == null ? 0 : _m_refTowerResearch.building_profit_add_per;

            if (_m_buildingProfitAddPer == totalAddPer)
                return;

            _m_buildingProfitAddPer = totalAddPer;

            // 初始化的时候不处理
            if (!_isInit)
                getUserData().getBuildingComponent().recalAllBuilding();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 保存位置数据到数据库
     */
    private void _savePosData()
    {
        getUserData().lockUser();
        try
        {
            BM bmObj = getUSServer().getBM();

            if (0 == _m_dbId)
            {
                PlayerTowerBO bo = new PlayerTowerBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setChapterId(bmObj, _m_chapterId);
                bo.setChapterLevel(bmObj, _m_chapterLevel);
                bo.setHighestHadReachChapterId(bmObj, _m_highestHadReachChapterId);
                bo.setHighestHadReachChapterLevel(bmObj, _m_highestHadReachChapterLevel);
                //首次需要记录当前时间为上次领取每日迷宫币的时间戳
                long nowTimeMS = CommonFunc.getNowTimeMS();
                bo.setLastDrawTowerCoinTimeMs(bmObj, nowTimeMS);
                _m_lastDrawTowerCoinTimeMs = nowTimeMS;
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("chapter_id", _m_chapterId);
                updateValue.addValueObj("chapter_level", _m_chapterLevel);
                updateValue.addValueObj("highest_had_reach_chapter_id", _m_highestHadReachChapterId);
                updateValue.addValueObj("highest_had_reach_chapter_level", _m_highestHadReachChapterLevel);
                bmObj.getBM(PlayerTowerBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 保存研究数据到数据库
     */
    private void _saveResearchData()
    {
        getUserData().lockUser();
        try
        {
            BM bmObj = getUSServer().getBM();

            if (0 == _m_dbId)
            {
                PlayerTowerBO bo = new PlayerTowerBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setHadActiveResearchChapterId(bmObj, _m_hadActiveResearchChapterId);
                bo.setHadActiveResearchChapterLevel(bmObj, _m_hadActiveResearchChapterLevel);
                //首次需要记录当前时间为上次领取每日迷宫币的时间戳
                long nowTimeMS = CommonFunc.getNowTimeMS();
                bo.setLastDrawTowerCoinTimeMs(bmObj, nowTimeMS);
                _m_lastDrawTowerCoinTimeMs = nowTimeMS;
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("had_active_research_chapter_id", _m_hadActiveResearchChapterId);
                updateValue.addValueObj("had_active_research_chapter_level", _m_hadActiveResearchChapterLevel);
                bmObj.getBM(PlayerTowerBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public Tower_PosInfo makePosInfo()
    {
        getUserData().lockUser();
        try
        {
            return new Tower_PosInfo(_m_chapterId, _m_chapterLevel);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public Tower_PosInfo makeResearchPosInfo()
    {
        getUserData().lockUser();
        try
        {
            return new Tower_PosInfo(_m_hadActiveResearchChapterId, _m_hadActiveResearchChapterLevel);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public Tower_PosInfo makeHighestPosInfo()
    {
        getUserData().lockUser();
        try
        {
            return new Tower_PosInfo(_m_highestHadReachChapterId, _m_highestHadReachChapterLevel);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取研究章节奖励
     * @param _chapterId
     * @return
     */
    public Result drawChapterResearchReward(long _chapterId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_highestHadReachChapterId < _chapterId)
                return TowerErr.TOWER_CHAPTER_RESEARCH_NOT_DONE;

            RefTowerChapter refTowerChapter = RefTowerChapter.getMgr().get(_chapterId);
            if (refTowerChapter == null)
                return CommErr.REF_NOT_FOUND;

            getUserData().gainItemList(refTowerChapter.research_finish_reward, _context);

            PlayerTowerChapterResearchBO bo = new PlayerTowerChapterResearchBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setChapterId(getUSServer().getBM(), _chapterId);
            bo.insert(getUSServer().getBM());
            _m_hadDrawChapterResearchList.add(_chapterId);

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_063_OnTowerResearchRewardDraw(_chapterId));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造挑战列表
     * @param _callback
     */
    public void makeChallengeList(_ICallBackResultT<List<Tower_OpponentInfo>> _callback)
    {
        getUserData().lockUser();
        try
        {
            if (_m_stageRefObj == null)
            {
                _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
                return;
            }

            getUSServer().getTowerMgr().makeChallengeList(_m_stageRefObj, _m_chapterId, _m_chapterLevel,
                    getUserData().getHeroComponent().getTotalPower(), _callback);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public long getPassedChapter()
    {
        return _m_chapterId - 1;
    }

    public long getPassedTotalLevel()
    {
        getUserData().lockUser();
        try
        {
            if(null == _m_stageRefObj)
                return 0;

            return _m_stageRefObj.getTotalStartLevel() + _m_chapterLevel - 1;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public int getBuildingProfitAddPer()
    {
        return _m_buildingProfitAddPer;
    }

    /**
     * 获取已激活研究章节数量
     *
     * 计算逻辑：
     * 统计所有配置表中满足以下条件的研究章节数量：
     * 1. 章节ID < 当前已激活章节ID，或
     * 2. 章节ID == 当前已激活章节ID 且 层数 <= 当前已激活层数
     *
     * @return 已激活研究章节总数
     *
     * 线程安全：读取操作，无需加锁
     */
    public int getActiveResearchCount()
    {
        getUserData().lockUser();
        try
        {
            int count = 0;

            // 遍历所有研究章节配置
            for (RefTowerResearch ref : RefTowerResearch.getMgr().getList())
            {
                // 章节ID小于当前激活章节，或章节ID相同但层数更小
                if (ref.chapter_id < _m_hadActiveResearchChapterId ||
                    (ref.chapter_id == _m_hadActiveResearchChapterId && ref.level <= _m_hadActiveResearchChapterLevel))
                {
                    count++;
                }
            }

            return count;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public Tower_PosInfo toProto()
    {
        getUserData().lockUser();
        try
        {
            return new Tower_PosInfo(_m_chapterId, _m_chapterLevel);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 修改玩家位置
     * @param _chapterId
     * @param _chapterLevel
     * @param _context
     * @return
     */
    public Result chgPos(long _chapterId, int _chapterLevel,NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否在同一位置
            if (_m_chapterId == _chapterId && _m_chapterLevel == _chapterLevel)
                return Result.SUCC;

            // 检查章节和层数是否有效
            RefTowerChapter refChapter = RefTowerChapter.getMgr().get(_chapterId);
            if (refChapter == null)
                return CommErr.REF_NOT_FOUND;

            TowerStageRefObj stageRefObj = refChapter.getStageRefObj(_chapterLevel);
            if (stageRefObj == null)
                return CommErr.REF_NOT_FOUND;

            // 更新当前章节和层数
            _m_chapterId = _chapterId;
            _m_chapterLevel = _chapterLevel;
            _m_stageRefObj = stageRefObj;

            checkHadReachHighestChapter(_m_chapterId, _m_chapterLevel, _context);

            // 保存数据到数据库
            _savePosData();

            // 如果是PVP章节，通知爬塔管理器
            if (!refChapter.if_pve_chapter)
                getUSServer().getTowerMgr().onPvpAttackSucc(getUserData(), 0, _chapterId, _chapterLevel, true, _context);

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_061_OnTowerPosChg(makePosInfo()));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 被击败
     * @param _chapterId
     * @param _chapterLevel
     * @param _context
     */
    public void pvpBeenDefeated(long _chapterId, int _chapterLevel, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //检查是否在同一位置
            if (_m_chapterId != _chapterId || _m_chapterLevel != _chapterLevel)
                return;

            onPosChg(_m_chapterId, 1, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充已领取研究奖励列表
     * @param _hadDrawResearchRewardList
     */
    public void fillHadDrawResearchRewardList(List<Long> _hadDrawResearchRewardList)
    {
        getUserData().lockUser();
		try
		{
			_hadDrawResearchRewardList.addAll(_m_hadDrawChapterResearchList);
		} finally
		{
			getUserData().unlockUser();
		}
    }

    /**
     * 设置最高级别
     * @param _chapterId
     * @param _level
     * @param _context
     */
    public void setHighestPosHadReach(long _chapterId, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();

		try
		{
			//更新内存数据
			_m_highestHadReachChapterId = _chapterId;
            _m_highestHadReachChapterLevel = _level;
            //更新bo数据
            _saveResearchData();

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_064_OnTowerHighestPosHadReachChg(makeHighestPosInfo()));
		}
		finally
		{
			getUserData().unlockUser();
		}
    }

    /**
     * 设置激活研究进度（GM命令专用）
     *
     * 功能：直接修改玩家的激活研究章节ID和层数
     *
     * 执行流程：
     * 1. 验证配表是否存在
     * 2. 更新内存数据（章节ID、层数、配表缓存）
     * 3. 重新计算建筑产出收益加成
     * 4. 保存到数据库
     * 5. 通知客户端更新
     *
     * @param _chapterId 要设置的章节ID
     * @param _level 要设置的层数
     * @param _context 操作上下文
     * @return 操作结果
     *
     * 线程安全：通过用户级别锁保护
     */
    public Result setActiveResearchPos(long _chapterId, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查研究章节是否存在
            RefTowerResearch refTowerResearch = RefTowerResearch.getMgr().lookupByChapterLvl(_chapterId, _level);
            if (refTowerResearch == null)
                return TowerErr.TOWER_RESEARCH_NOT_FOUND;

            // 更新已激活研究章节
            _m_hadActiveResearchChapterId = _chapterId;
            _m_hadActiveResearchChapterLevel = _level;

            _m_refTowerResearch = refTowerResearch;

            // 重新计算加成
            recalBuildingProfitAddPer(false);

            // 保存数据到数据库
            _saveResearchData();

            // 更新玩家位置
            getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_062_OnTowerResearchActivePosChg(makeResearchPosInfo(), _m_buildingProfitAddPer));

            return Result.SUCC;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 将章节和层数组合成触发进度ID
     * 触发进度ID = 章节ID * 1000 + 层数
     * @param _chapterId 章节ID
     * @param _level 层数
     * @return 组合后的触发进度ID
     */
    private int _makeTriggeridFromChapterLevel(long _chapterId, int _level)
    {
        return (int)(_chapterId * 1000 + _level);
    }

    @Override
    public String toString()
    {
        return "TowerComponent{" +
                "_m_dbId=" + _m_dbId +
                ", _m_chapterId=" + _m_chapterId +
                ", _m_chapterLevel=" + _m_chapterLevel +
                ", _m_hadActiveResearchChapterId=" + _m_hadActiveResearchChapterId +
                ", _m_hadActiveResearchChapterLevel=" + _m_hadActiveResearchChapterLevel +
                ", _m_highestHadReachChapterId=" + _m_highestHadReachChapterId +
                ", _m_highestHadReachChapterLevel=" + _m_highestHadReachChapterLevel +
                ", _m_lastDrawTowerCoinTimeMs=" + _m_lastDrawTowerCoinTimeMs +
                ", _m_hadDrawChapterResearchList=" + _m_hadDrawChapterResearchList +
                ", _m_buildingProfitAddPer=" + _m_buildingProfitAddPer +
                '}';
    }
}

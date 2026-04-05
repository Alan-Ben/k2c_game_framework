package NPUSServer.Dungeon.Evening;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.DungeonObj.EveningDungeon_BossInfo;
import Common.DungeonObj.EveningDungeon_TimeInfo;
import Common.MailObj.Mail_Data;
import Common.NpChatObj.ChatObj_SystemLog;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_ItemDump;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import GS2GC.p024_DungeonOp.GS2GC_024_062_OnEveningDungeonTimeInfoChg;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.EveningDungeonErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBack;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Dungeon.Evening.RefEveningDungeonBossBloodAdd;
import NPGameRes.Refs.Dungeon.Evening.RefEveningDungeonDamageRatio;
import NPGameRes.Refs.Dungeon.Evening.RefEveningDungeonRankReward;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonBox;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefServerStartDays;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_EVENING_ATTACK;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.USLog;
import NPUSServer.USRank.USRankList;
import USDB.Bo.EveningDungeonBO;
import USLOGDB.Bo.LogEveningDungeonAttackBO;
import USLOGDB.Bo.LogEveningDungeonBossRebornBO;

import java.util.List;

public class EveningDungeonInfo
{
    private EveningDungeonMgr _m_mgr;
    private EveningDungeonBO _m_bo;
    private EveningDungeonAttackLogMgr _m_attackLogMgr;
    private boolean _m_settling = false;

    private MutexAtom _m_mutex;

    public EveningDungeonInfo(EveningDungeonMgr _mgr, EveningDungeonBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_attackLogMgr = new EveningDungeonAttackLogMgr(this);
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public EveningDungeonMgr getMgr()
    {
        return _m_mgr;
    }

    public EveningDungeonAttackLogMgr getAttackLogMgr()
    {
        return _m_attackLogMgr;
    }

    /**
     * 判断boss是否死亡
     * @return
     */
    public boolean isBossDead()
    {
        _lock();
        try
        {
            return _m_bo.getDeductedHp() >= _m_bo.getTotalHp() || _m_bo.getDefeatTimeMs() != 0;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 攻击
     * @param _userdata
     * @param _heroId
     * @return
     */
    public ResultOne<EveningDungeonAttackResult> attack(NPUSUserData _userdata, long _heroId, NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        //查询大臣
        HeroInfo heroInfo = _userdata.getHeroComponent().lookupHero(_heroId);
        if (heroInfo == null)
            return ResultOne.failed(HeroErr.HERO_NOT_FOUND);

        long roundStartTimeMs = _m_bo.getRoundStartTimeMs();

        //查询大臣是否可以使用
        boolean canUse = _userdata.getEveningDungeonComponent().getDungeonInfo().checkHeroCanUse(roundStartTimeMs, _heroId);
        if (!canUse)
            return ResultOne.failed(EveningDungeonErr.HERO_USE_REACH_LIMIT);

        //计算大臣攻击力
        long basePower = heroInfo.getPower();
        long powerAddPer = heroInfo.getPlayerBonusAddValue(EBonusPropertyType.EVENING_DUNGEON_POWER_PER);
        long power = (long) Math.ceil(basePower * (10000 + powerAddPer) / 10000d);

        EveningDungeonAttackResult data;

        _lock();
        try
        {
            //如果当前轮次不等于大臣使用的轮次，则说明boss被刷新过，则说明当前boss已经死亡
            if (roundStartTimeMs != _m_bo.getRoundStartTimeMs())
                return ResultOne.failed(EveningDungeonErr.BOSS_DEAD);

            //判断当前时间是否在轮次内
            if (nowTimeMS < _m_bo.getRoundStartTimeMs() || nowTimeMS > _m_bo.getRoundEndTimeMs())
                return ResultOne.failed(EveningDungeonErr.NOT_IN_FIGHT_TIME);

            //判断boss是否已经死亡
            if (isBossDead())
                return ResultOne.failed(EveningDungeonErr.BOSS_DEAD);

            //计算boss还有多少血量
            long leftHp = _m_bo.getTotalHp() - _m_bo.getDeductedHp();
            //计算实际可以扣除血量
            long realPower = Math.min(power, leftHp);

            //计算攻击后的数值
            long newDeductedHp = _m_bo.getDeductedHp() + realPower;
            if (newDeductedHp < _m_bo.getTotalHp())
            {
                _m_bo.saveDeductedHp(_m_mgr.getServer().getBM(), newDeductedHp);
            } else
            {
                _m_bo.setDeductedHp(_m_mgr.getServer().getBM(), _m_bo.getTotalHp());
                _m_bo.setDefeatCid(_m_mgr.getServer().getBM(), _userdata.getCid());
                _m_bo.setDefeatTimeMs(_m_mgr.getServer().getBM(), nowTimeMS);
                _m_bo.saveAllMarked(_m_mgr.getServer().getBM());
            }

            data = new EveningDungeonAttackResult();
            data.harmHp = realPower;
            data.totalHp = _m_bo.getTotalHp();
            data.isKill = isBossDead();
            data.rebornTimes = _m_bo.getRebornTimes();
            data.attackHeroId = _heroId;  // 设置攻击的英雄ID

        } finally
        {
            _unlock();
        }

        //记录大臣使用
        boolean recordSucc = _userdata.getEveningDungeonComponent().getDungeonInfo().recordHeroUse(roundStartTimeMs, _heroId);
        if (!recordSucc)
        {
            //这边应该是没问题的，但是加个日志输出
            USLog.error(_m_mgr.getServer(), "EveningDungeonInfo record hero use failed, cid:{} heroId:{} ", _userdata.getCid(), _heroId);
        }

        //结算奖励
        settleReward(_userdata, data, _context);

        //增加击杀日志
        if (data.isKill)
        {
            _m_attackLogMgr.addDefeatLog(_userdata, nowTimeMS);

            RefEveningDungeonBossBloodAdd bloodAddRef = RefEveningDungeonBossBloodAdd.getMgr().getBloodAddRef(data.rebornTimes);
            if (bloodAddRef != null && bloodAddRef.box_id != 0)
            {
                //获取宝箱配置
                RefMiddayDungeonBox refBox = RefMiddayDungeonBox.getMgr().get(bloodAddRef.box_id);
                if (refBox != null)
                {
                    //发送宝箱
                    _userdata.getUSServer().getMiddayDungeonMgr().getBoxMgr().addBox(refBox, _userdata, CommonFunc.getNowTimeMS());
                }else
                {
                    USLog.error(_m_mgr.getServer(), "EveningDungeonInfo attack refBox not found, cid:{} boxId:{}", _userdata.getCid(), bloodAddRef.box_id);
                }
            }
        }

        //增加日志
        _m_attackLogMgr.addAttackLog(_userdata, data);
        //记录伤害到排行榜
        updateHarmToRank(_userdata, data.harmHp);

        //触发事件
        Event_P_EVENING_ATTACK evt = new Event_P_EVENING_ATTACK(_context);
        _userdata.onLogicEvent(evt);

        //记录晚间副本攻击日志
        BM bmObj = _m_mgr.getServer().getBM();
        LogEveningDungeonAttackBO logBo = new LogEveningDungeonAttackBO();
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setPlayerLevel(bmObj, (int) _userdata.getParam(ENPPlayerParam.LEVEL));
        logBo.setTotalHeroPower(bmObj, _userdata.getHeroComponent().getTotalPower());
        logBo.setDamage(bmObj, data.harmHp);
        logBo.setHeroExpReward(bmObj, data.heroExp);
        logBo.setIsKilled(bmObj, data.isKill);
        CommLogDB.log(bmObj, logBo, _context);

        return ResultOne.succ(data);
    }

    /**
     * 结算奖励
     * @param _userdata
     * @param _data
     * @param _context
     */
    private void settleReward(NPUSUserData _userdata, EveningDungeonAttackResult _data, NPPlayerContext _context)
    {
        //计算奖励
        int harmRatio = (int) Math.ceil((double) _data.harmHp / _m_bo.getTotalHp() * 10000);
        //查询比率配置
        RefEveningDungeonDamageRatio refRatio = RefEveningDungeonDamageRatio.getMgr().getRatioByDamageRatio(harmRatio);
        if (refRatio != null)
        {
            //计算金币奖励
            long gold = (long) Math.ceil(refRatio.ratio * (Math.ceil(_userdata.getPlayerComponent().getEarnings() * RefGeneral.Ref().evening_dungeon_gold_profit_ratio / 10000d)) / 10000d);
            _userdata.gainItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), gold, _context);

            //计算大臣经验奖励
            long heroExp = 0;
            //获取开服时间对应的配置
            RefServerStartDays refServerStartDays = RefServerStartDays.getMgr().getRefByDay(_m_mgr.getServer().getServerStartDay());
            if (refServerStartDays != null)
            {
                heroExp = (long) Math.ceil(refRatio.ratio * refServerStartDays.evening_dungeon_base_hero_exp / 10000d);
                _userdata.gainItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), heroExp, _context);
            } else
            {
                USLog.error(_m_mgr.getServer(), "EveningDungeonInfo attack refServerStartDays not found, cid:{}", _userdata.getCid());
            }
            _data.heroExp = heroExp;

            _context.getCollector().fillProtoList(_data.attackRewardList);
        }

        //如果boss被击败需要
        if (_data.isKill)
        {
            NPPlayerContext killedContext = NPPlayerContext.createNew(ENPGameEvent.EVENING_DUNGEON_BOSS_KILLED);
            killedContext.setGuid(_context.getGuid());

            if (_data.rebornTimes == 0)
            {
                _userdata.gainItemList(RefGeneral.Ref().evening_dungeon_day_first_kill_boss_reward, killedContext);
            } else
            {
                _userdata.gainItemList(RefGeneral.Ref().evening_dungeon_day_kill_boss_reward, killedContext);
            }

            killedContext.getCollector().fillProtoList(_data.defeatRewardList);
        }
    }

    /**
     * tick
     */
    public void tick1Sec()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        if (nowTimeMS < _m_bo.getRoundStartTimeMs())
            return;

        //检查当前时间是否在轮次内
        if (nowTimeMS <= _m_bo.getRoundEndTimeMs())
        {
            //需要先初始化
            if (!tryInit())
                return;

            //检查当前boss是否需要复活
            rebornBoss(nowTimeMS);
        } else
        {
            boolean needInit = _m_bo.getRoundStartTimeMs() == -1 || _m_bo.getRoundEndTimeMs() == -1;
            //如果已经结算，则检查是否需要进入下一个轮次
            if (needInit || _m_bo.getHadSettle())
            {
                long roundStartTimeMs;
                long roundEndTimeMs;

                long beforeStartTimeTagMS = RefGeneral.Ref().evening_dungeon_start_fight_time.getBeforeFreshTimeTagMS(nowTimeMS);
                long beforeEndTimeTagMS = RefGeneral.Ref().evening_dungeon_end_fight_time.getNextFreshTimeTagMS(beforeStartTimeTagMS);
                if (nowTimeMS < beforeEndTimeTagMS)
                {
                    //使用当前时间计算当前时间所在轮的开始和结束时间
                    roundStartTimeMs = beforeStartTimeTagMS;
                    roundEndTimeMs = beforeEndTimeTagMS;
                } else
                {
                    //如果当前时间在前一天的结束时间之后，则使用当前时间
                    roundStartTimeMs = RefGeneral.Ref().evening_dungeon_start_fight_time.getNextFreshTimeTagMS(nowTimeMS);
                    roundEndTimeMs = RefGeneral.Ref().evening_dungeon_end_fight_time.getNextFreshTimeTagMS(roundStartTimeMs);
                }

                //检查时间是否合法
                if (roundStartTimeMs == -1 || roundEndTimeMs == -1)
                    return;

                //根据前一天的boss复活次数超过限制，则需要按比例增加boss血量
                int addPer = 0;
                int serverStartDay = _m_mgr.getServer().getServerStartDay();
                if (_m_bo.getRebornTimes() >= RefGeneral.Ref().evening_dungeon_boss_respawn_reference_times)
                {
                    RefServerStartDays refServerStartDays = RefServerStartDays.getMgr().getRefByDay(serverStartDay);
                    if (refServerStartDays != null)
                        addPer = refServerStartDays.evening_dungeon_boss_blood_add_per;
                }

                _lock();
                try{
                    //新的血量
                    long newBaseHp;
                    if (_m_bo.getBaseHp() == 0)
                    {
                        newBaseHp = RefGeneral.Ref().evening_dungeon_boss_initial_blood;
                    } else
                    {
                        newBaseHp = (long) Math.ceil(_m_bo.getBaseHp() * (10000 + addPer) / 10000d);
                    }

                    //计算预告时间
                    long roundPreviewTimeMs = RefGeneral.Ref().evening_dungeon_preview_fight_time.getBeforeFreshTimeTagMS(roundStartTimeMs);
                    long preRoundCloseTimeMs = RefGeneral.Ref().evening_dungeon_close_fight_time.getBeforeFreshTimeTagMS(roundPreviewTimeMs);

                    //重置boss状态
                    _m_bo.setPreRoundCloseTimeMs(_m_mgr.getServer().getBM(), preRoundCloseTimeMs);
                    _m_bo.setRoundPreviewTimeMs(_m_mgr.getServer().getBM(), roundPreviewTimeMs);
                    _m_bo.setRoundStartTimeMs(_m_mgr.getServer().getBM(), roundStartTimeMs);
                    _m_bo.setRoundEndTimeMs(_m_mgr.getServer().getBM(), roundEndTimeMs);
                    _m_bo.setRebornTimes(_m_mgr.getServer().getBM(), -1);
                    _m_bo.setBaseHp(_m_mgr.getServer().getBM(), newBaseHp);
                    _m_bo.setTotalHp(_m_mgr.getServer().getBM(), 0);
                    _m_bo.setDeductedHp(_m_mgr.getServer().getBM(), 0);
                    _m_bo.setDefeatCid(_m_mgr.getServer().getBM(), 0);
                    _m_bo.setDefeatTimeMs(_m_mgr.getServer().getBM(), 0);
                    _m_bo.setHadSettle(_m_mgr.getServer().getBM(), false);
                    _m_bo.setHadResetRank(_m_mgr.getServer().getBM(), false);
                    _m_bo.setServerStartDay(_m_mgr.getServer().getBM(), serverStartDay);
                    _m_bo.saveAllMarked(_m_mgr.getServer().getBM());
                }finally
                {
                    _unlock();
                }

                //时间变更推送
                getMgr().getServer().getUsUserMgr().broadCastMessage(new GS2GC_024_062_OnEveningDungeonTimeInfoChg(makeTimeInfo()));

                USLog.info(_m_mgr.getServer(), "EveningDungeon enter next round, previewTime:{} startTime:{}, endTime:{}",
                        _m_bo.getRoundPreviewTimeMs(), roundStartTimeMs, roundEndTimeMs);
            } else
            {
                //延迟5s开始结算
                if (nowTimeMS < _m_bo.getRoundEndTimeMs() + 5 * 1000L)
                    return;

                if (_m_settling)
                    return;

                _m_settling = true;

                //执行结算逻辑
                ALSynTaskManager.getInstance().regTask(() -> doRankSettle(() ->
                {
                    _lock();
                    try
                    {
                        _m_settling = false;
                        _m_bo.saveHadSettle(_m_mgr.getServer().getBM(), true);
                    } finally
                    {
                        _unlock();
                    }
                }));
            }
        }
    }

    /**
     * 复活boss
     * @param _nowTimeMS
     */
    private void rebornBoss(long _nowTimeMS)
    {
        _lock();
        try
        {
            //如果boss没有死亡，或者当前时间在复活时间内，或者复活次数超过限制
            if (!isBossDead()
                    || _m_bo.getRebornTimes() >= RefGeneral.Ref().evening_dungeon_boss_respawn_times_limit
                    || _nowTimeMS <= _m_bo.getDefeatTimeMs() + RefGeneral.Ref().evening_dungeon_boss_respawn_sec * 1000L)
                return;

            if (_m_bo.getBaseHp() == 0)
            {
                USLog.error(_m_mgr.getServer(), "EveningDungeonInfo rebornBoss baseHp is 0");
                return;
            }

            int nowRebornTimes = _m_bo.getRebornTimes() + 1;
            int addPer = 0;
            //根据复活次数获取血量增加比率
            RefEveningDungeonBossBloodAdd ref = RefEveningDungeonBossBloodAdd.getMgr().getBloodAddRef(nowRebornTimes);
            if (ref != null)
                addPer = ref.boss_respawn_blood_add;

            // 计算新的血量
            long newTotalHp = (long) Math.ceil(_m_bo.getBaseHp() * (10000 + addPer) / 10000d);

            long defeatCid = _m_bo.getDefeatCid();

            // 更新boss状态
            _m_bo.setRebornTimes(_m_mgr.getServer().getBM(), nowRebornTimes);
            _m_bo.setTotalHp(_m_mgr.getServer().getBM(), newTotalHp);
            _m_bo.setDeductedHp(_m_mgr.getServer().getBM(), 0);
            _m_bo.setDefeatCid(_m_mgr.getServer().getBM(), 0);
            _m_bo.setDefeatTimeMs(_m_mgr.getServer().getBM(), 0);
            _m_bo.saveAllMarked(_m_mgr.getServer().getBM());

            USLog.info(_m_mgr.getServer(), "EveningDungeon boss reborn, times:{}, newHp:{}", nowRebornTimes, newTotalHp);

            //记录晚间副本Boss复活日志
            BM bmObj = _m_mgr.getServer().getBM();
            LogEveningDungeonBossRebornBO logBo = new LogEveningDungeonBossRebornBO();
            logBo.setBaseHp(bmObj, _m_bo.getBaseHp());
            logBo.setBossHp(bmObj, newTotalHp);
            logBo.setRebornTimes(bmObj, nowRebornTimes);
            logBo.setServerStartDay(bmObj, _m_bo.getServerStartDay());
            CommLogDB.log(bmObj, logBo);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 尝试初始化排行榜
     * @return
     */
    private boolean tryInit()
    {
        //如果已经初始化，直接返回成功
        if (_m_bo.getHadResetRank())
            return true;

        //先移除之前的排行榜
        if (_m_bo.getRankInstanceId() != 0)
        {
            _m_mgr.getServer().getRankListMgr().closeRankList(_m_bo.getRankInstanceId());
            _m_bo.saveRankInstanceId(_m_mgr.getServer().getBM(), 0);
        }

        //如果没有排行榜配置不处理
        if (RefGeneral.Ref().evening_dungeon_rank_id == 0)
            return false;

        //创建排行榜
        long rankInstanceId = _m_mgr.getServer().getRankListMgr().createUsRank(RefGeneral.Ref().evening_dungeon_rank_id, 0);
        if (rankInstanceId == 0)
        {
            USLog.error(_m_mgr.getServer(), "EveningDungeonInfo checkInitRank create rank failed, rankId:{}", RefGeneral.Ref().evening_dungeon_rank_id);
            return false;
        }

        _m_bo.setRankInstanceId(_m_mgr.getServer().getBM(), rankInstanceId);
        _m_bo.setHadResetRank(_m_mgr.getServer().getBM(), true);
        _m_bo.saveAllMarked(_m_mgr.getServer().getBM());

        //清空攻击日志
        getAttackLogMgr().clearLog();

        //聊天用户数据
        NPCommon_ChatPlayerContent userProto = new NPCommon_ChatPlayerContent();
        //聊天内容数据
        ChatObj_SystemLog proto = new ChatObj_SystemLog();
        proto.setLogType(RefGeneral.Ref().evening_dungeon_system_log_id);
        //对全服聊天频道发送消息
        ChatRoomApi.sendUsRoomSysMsg(getMgr().getServer()
                , ENPChatMsgType.SYSTEM_LOG
                , userProto.makePackage()
                , proto.makePackage()
                , null);

        return true;
    }

    /**
     * 执行排行榜结算逻辑
     */
    public void doRankSettle(_ICallBack _callback)
    {
        //如果没有排行榜实例ID，则直接返回
        if (_m_bo.getRankInstanceId() == 0)
        {
            USLog.error(_m_mgr.getServer(), "EveningDungeonInfo doRankSettle rankInstanceId is 0");
            _callback.onRunOver();
            return;
        }

        USRankList rankList = _m_mgr.getServer().getRankListMgr().lookup(_m_bo.getRankInstanceId());
        if (rankList == null)
        {
            USLog.error(_m_mgr.getServer(), "EveningDungeonInfo doRankSettle rankList is null, rankInstanceId:{}",
                    _m_bo.getRankInstanceId());
            return;
        }

        //dump排行榜
        List<Rank_ItemDump> rankItemDumpList =
                rankList.dumpRankObjListWithLimitRankByUs(
                        usId -> _m_mgr.getServer().getServerTypeId() == usId,
                        RefEveningDungeonRankReward.getMgr().getRankRewardMaxRank());

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.EVENING_DUNGEON_RANK_SETTLE);

        //遍历发奖
        for (Rank_ItemDump rankItem : rankItemDumpList)
        {
            RefEveningDungeonRankReward refReward = RefEveningDungeonRankReward.getMgr().getRefByRank(rankItem.getRank());
            if (refReward == null)
            {
                USLog.error(_m_mgr.getServer(), "EveningDungeonInfo doRankSettle refReward not found, cid:{} rank:{}", rankItem.getKey(), rankItem.getRank());
                return;
            }

            //发送邮件
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().evening_dungeon_rank_reward_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(refReward.reward_item_list));
            MailSystem.addMail(_m_mgr.getServer(), rankItem.getKey(), mailData, context);
        }

        _callback.onRunOver();
    }

    /**
     * 记录伤害到排行榜
     * @param _userdata
     * @param _realPower
     */
    private void updateHarmToRank(NPUSUserData _userdata, long _realPower)
    {
        USRankList rankList = _m_mgr.getServer().getRankListMgr().lookup(_m_bo.getRankInstanceId());
        if (rankList == null)
        {
            USLog.error(_m_mgr.getServer(), "EveningDungeonInfo updateHarmToRank rankList is null, rankInstanceId:{} cid:{} harm:{}",
                    _m_bo.getRankInstanceId(), _userdata.getCid(), _realPower);
            return;
        }

        rankList.onScoreChg(_userdata.getCid(), 0, _realPower);
    }

    /**
     * 构造排行榜
     * @param _num
     * @return
     */
    public List<Rank_BaseItem> makeRankList(int _num)
    {
        if (CommonFunc.getNowTimeMS() < _m_bo.getRoundStartTimeMs())
            return null;

        USRankList rankList = _m_mgr.getServer().getRankListMgr().lookup(_m_bo.getRankInstanceId());
        if (rankList == null)
            return null;

        return rankList.makeRankObjBaseList(_num);
    }

    /**
     * 构造排行榜信息
     * @param _cid
     * @return
     */
    public Rank_BaseItem makeRankItemByPlayer(long _cid)
    {
        if (CommonFunc.getNowTimeMS() < _m_bo.getRoundStartTimeMs())
            return null;

        USRankList rankList = _m_mgr.getServer().getRankListMgr().lookup(_m_bo.getRankInstanceId());
        if (rankList == null)
            return null;

        return rankList.makeRankObjBaseByKey(_cid);
    }

    /**
     * 设置boss血量
     * @param _blood
     */
    public void cmdChgBossBlood(long _blood)
    {
        _lock();
        try{
            _m_bo.saveDeductedHp(_m_mgr.getServer().getBM(), Math.min(_m_bo.getTotalHp() - _blood, _m_bo.getTotalHp()));
        }finally
        {
            _unlock();
        }
    }

    /**
     * 马上复活boss
     */
    public void cmdRebornBoss()
    {
        _lock();
        try{
            _m_bo.saveDefeatTimeMs(_m_mgr.getServer().getBM(), CommonFunc.getNowTimeMS() - RefGeneral.Ref().evening_dungeon_boss_respawn_sec * 1000L);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 重置次数
     */
    public void cmdResetRebornTimes()
    {
        _lock();
        try{
            _m_bo.setRebornTimes(_m_mgr.getServer().getBM(), -1);
            _m_bo.setDefeatTimeMs(_m_mgr.getServer().getBM(), CommonFunc.getNowTimeMS() - RefGeneral.Ref().evening_dungeon_boss_respawn_sec * 1000L);
            _m_bo.saveAllMarked(_m_mgr.getServer().getBM());
        }finally
        {
            _unlock();
        }
    }

    /**
     * 修改boss时间
     * @param _preview
     * @param _start
     * @param _end
     * @param _close
     */
    public boolean cmdChgBossTime(int _preview, int _start, int _end, int _close)
    {
        NPRefreshTimeObj previewTime = new NPRefreshTimeObj();
        previewTime.parseFromString("REF_CLOCK:" + _preview);
        NPRefreshTimeObj startTime = new NPRefreshTimeObj();
        startTime.parseFromString("REF_CLOCK:" + _start);
        NPRefreshTimeObj endTime = new NPRefreshTimeObj();
        endTime.parseFromString("REF_CLOCK:" + _end);
        NPRefreshTimeObj closeTime = new NPRefreshTimeObj();
        closeTime.parseFromString("REF_CLOCK:" + _close);

        long nowTimeMS = CommonFunc.getNowTimeMS();

        long roundStartTimeMs;
        long roundEndTimeMs;

        long beforeStartTimeTagMS = startTime.getBeforeFreshTimeTagMS(nowTimeMS);
        long beforeEndTimeTagMS = endTime.getNextFreshTimeTagMS(beforeStartTimeTagMS);
        if (nowTimeMS < beforeEndTimeTagMS)
        {
            //使用当前时间计算当前时间所在轮的开始和结束时间
            roundStartTimeMs = beforeStartTimeTagMS;
            roundEndTimeMs = beforeEndTimeTagMS;
        } else
        {
            //如果当前时间在前一天的结束时间之后，则使用当前时间
            roundStartTimeMs = startTime.getNextFreshTimeTagMS(nowTimeMS);
            roundEndTimeMs = endTime.getNextFreshTimeTagMS(roundStartTimeMs);
        }

        //检查时间是否合法
        if (roundStartTimeMs == -1 || roundEndTimeMs == -1)
            return false;

        //计算预告时间
        long roundPreviewTimeMs = previewTime.getBeforeFreshTimeTagMS(roundStartTimeMs);
        long preRoundCloseTimeMs = closeTime.getBeforeFreshTimeTagMS(roundPreviewTimeMs);

        _lock();
        try
        {
            _m_bo.setPreRoundCloseTimeMs(_m_mgr.getServer().getBM(), preRoundCloseTimeMs);
            _m_bo.setRoundPreviewTimeMs(_m_mgr.getServer().getBM(), roundPreviewTimeMs);
            _m_bo.setRoundStartTimeMs(_m_mgr.getServer().getBM(), roundStartTimeMs);
            _m_bo.setRoundEndTimeMs(_m_mgr.getServer().getBM(), roundEndTimeMs);
            _m_bo.saveAllMarked(_m_mgr.getServer().getBM());
        } finally
        {
            _unlock();
        }

        //时间变更推送
        getMgr().getServer().getUsUserMgr().broadCastMessage(new GS2GC_024_062_OnEveningDungeonTimeInfoChg(makeTimeInfo()));

        return true;
    }

    /**
     * 构造时间信息
     * @return
     */
    public EveningDungeon_TimeInfo makeTimeInfo()
    {
        EveningDungeon_TimeInfo info = new EveningDungeon_TimeInfo();
        info.setPreCloseTimeMs(_m_bo.getPreRoundCloseTimeMs());
        info.setPreviewTimeMs(_m_bo.getRoundPreviewTimeMs());
        info.setStartTimeMs(_m_bo.getRoundStartTimeMs());
        info.setEndTimeMs(_m_bo.getRoundEndTimeMs());
        return info;
    }

    /**
     * 构造boss信息
     * @return
     */
    public EveningDungeon_BossInfo makeBossInfo()
    {
        EveningDungeon_BossInfo info = new EveningDungeon_BossInfo();
        info.setWave(_m_bo.getRebornTimes());
        info.setDeductedHp(_m_bo.getDeductedHp());
        info.setTotalHp(_m_bo.getTotalHp());
        info.setBeDefeatTimeMs(_m_bo.getDefeatTimeMs());
        info.setDefeatCid(_m_bo.getDefeatCid());
        return info;
    }

    @Override
    public String toString()
    {
        return "eveningDungeon " +
                "preview: " + CommonFunc.getTimeStringMs(_m_bo.getRoundPreviewTimeMs()) +
                ", start: " + CommonFunc.getTimeStringMs(_m_bo.getRoundStartTimeMs()) +
                ", end: " + CommonFunc.getTimeStringMs(_m_bo.getRoundEndTimeMs()) +
                ", end: " + CommonFunc.getTimeStringMs(_m_bo.getRoundEndTimeMs()) +
                ", rebornTimes: " + _m_bo.getRebornTimes() +
                ", baseHp: " + _m_bo.getBaseHp() +
                ", totalHp: " + _m_bo.getTotalHp() +
                ", deductedHp: " + _m_bo.getDeductedHp() +
                ", defeatCid: " + _m_bo.getDefeatCid() +
                ", defeatTimeMs: " + CommonFunc.getTimeStringMs(_m_bo.getDefeatTimeMs()) +
                ", hadSettle: " + _m_bo.getHadSettle() +
                ", hadResetRank: " + _m_bo.getHadResetRank() +
                ", rankInstanceId: " + _m_bo.getRankInstanceId();
    }
}

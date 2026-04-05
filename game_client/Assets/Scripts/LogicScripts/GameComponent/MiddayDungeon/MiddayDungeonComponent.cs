using System;
using System.Collections.Generic;
using ALPackage;
using Common.DungeonObj;
using Common.GuildObj;
using CommonEnum;
using GC2GS.p024_DungeonOp;
using GC2GS.p032_GuildOp;
using GS2GC.p002_InitOp;
using GS2GC.p024_DungeonOp;
using GS2GC.p032_GuildOp;
using JetBrains.Annotations;

namespace GOE
{
    public class MiddayDungeonComponent : _ANPBasicPlayerComponent
    {
        // 轮次信息 ,使用轮次信息的时候需要先判断是否是新的一轮，重置轮次信息
        // 这个参数是玩家上一次打的轮次的时间戳，用于判断是否是新的一轮，并重置boss信息
        private long _m_roundStartTimeMS;// 当前或者上一轮的开始时间戳
        private MiddayDungeonBossInfo _m_bossInfo;// Boss信息
        private bool _m_hasRoundInfoReset = false;// 是否已经重置过
        [NotNull]private Dictionary<long, int> _m_selfFightHeroDic = new Dictionary<long, int>();// 已战斗过的英雄列表
        [NotNull]private List<long> _m_hasBorrowGuildCid = new List<long>();
        
        // 下面这个三个参数，一轮结束就会刷新上一轮的
        private long _m_startTimeMs;// 当前所在的轮次开始时间戳
        private long _m_previewTimeMs;// 预告时间戳
        private long _m_endTimeMs;// 结束时间戳
        
        private EMiddayDungeonActivityState _m_activityState;//活动状态
        private ALCommonEnableTaskController _m_tickActionMonoTask;//任务
        private MiddayDungeonLocalPushDealer _m_localPushDealer;//午间活动开始本地推送

        public MiddayDungeonComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.MIDDAY_DUNGEON; } }
        public override ENPPlayerCompType[] dependCompList { get { return new []{ ENPPlayerCompType.BASIC_INFO, ENPPlayerCompType.HERO, ENPPlayerCompType.PLAYER_BUFF, ENPPlayerCompType.INN, ENPPlayerCompType.BUILDING, ENPPlayerCompType.GUILD, ENPPlayerCompType.MUSEUM, ENPPlayerCompType.TREASURE_HUNT}; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        
        public bool isOpen
        {
            get
            {
                return _m_startTimeMs < FpsAndPingMgr.instance.serverTimeTag && FpsAndPingMgr.instance.serverTimeTag < _m_endTimeMs;
            }
        }
        public bool isPreview
        {
            get
            {
                return _m_previewTimeMs < FpsAndPingMgr.instance.serverTimeTag && FpsAndPingMgr.instance.serverTimeTag < _m_startTimeMs;
            }
        }

        public bool isEndShow
        {
            get
            {
                return _m_endTimeMs < FpsAndPingMgr.instance.serverTimeTag || FpsAndPingMgr.instance.serverTimeTag < _m_previewTimeMs;
            }
        }
        
        public long startTimeMs
        {
            get
            {
                return _m_startTimeMs;
            }
        }

        public long endTimeMs
        {
            get
            {
                return _m_endTimeMs;
            }
        }

        //活动状态
        public EMiddayDungeonActivityState activityState => _m_activityState;


        public MiddayDungeonBossInfo bossInfo
        {
            get
            {
                _checkResetRoundInfo();
                return _m_bossInfo;
            }
        }

        private void _checkResetRoundInfo()
        {
            if (!_m_hasRoundInfoReset && _m_roundStartTimeMS != _m_startTimeMs)
            {
                _m_bossInfo?.reset();
                _m_selfFightHeroDic.Clear();
                _m_hasBorrowGuildCid.Clear();
                _m_hasRoundInfoReset = true;
            }
        }

        public override void presendInitProtocol()
        {
            _m_bossInfo = new MiddayDungeonBossInfo();
            _m_hasRoundInfoReset = false;
            //请求初始化
            _reqMiddayDungeonInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            _m_tickActionMonoTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 1f);
            //注册本地推送
            _m_localPushDealer = new MiddayDungeonLocalPushDealer();
            LocalPushMgr.instance.regDealer(_m_localPushDealer);
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_tickActionMonoTask.setDisable();
            //注销本地推送
            LocalPushMgr.instance.unRegDealer(_m_localPushDealer);
            _m_localPushDealer = null;
        }
        
        /// <summary>
        /// 任务执行方法
        /// </summary>
        private void _tick()
        {
            _refreshActivityState();//刷新当前活动状态
        }
        
        private void _refreshRedTip()
        {
            bool showRedTip = false;
            if (isOpen)
            {
                showRedTip = true;
                if (bossInfo == null || GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(bossInfo.wave) == null)
                    showRedTip = false;
                if(getSelfMatchHero(0) == null)
                    showRedTip = false;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MIDDAY_DUNGEON_ENTRY, showRedTip ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新轮次信息，如果轮次变化了，重置boss信息，已战斗大臣信息，已借用联盟大臣信息
        /// </summary>
        public void refreshRoundInfo()
        {
            _checkResetRoundInfo();
        }

        /// <summary>
        /// 获取大臣剩余战斗次数，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public int getHeroLeftFightTimes(long _heroId)
        {
            long battleExtraTimes = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES, getHeroIdJudgeUnionBonus(_heroId));
            int fightNum = 0;
            _m_selfFightHeroDic.TryGetValue(_heroId, out fightNum);
            
            return 1 + (int)battleExtraTimes - fightNum;
        }
        /// <summary>
        /// 获取大臣剩余战斗次数，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public int getHeroLeftAndMaxFightTimes(long _heroId, out int _maxFightTimes)
        {
            long battleExtraTimes = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES, getHeroIdJudgeUnionBonus(_heroId));
            int fightNum = 0;
            _m_selfFightHeroDic.TryGetValue(_heroId, out fightNum);
            _maxFightTimes = 1 + (int)battleExtraTimes;
            return _maxFightTimes - fightNum;
        }
        
        /// <summary>
        /// 获取联盟成员借用大臣是否使用，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public bool getGuildIsUsed(long _cid)
        {
            return _m_hasBorrowGuildCid.Contains(_cid);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_bossPower"></param>
        /// <returns></returns>
        public HeroInfo getSelfMatchHero(long _bossPower)
        {
            refreshRoundInfo();
            List<HeroInfo> heroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(heroList);
            heroList.Sort(sortHero);
            HeroInfo selectHero = null;
            foreach (var heroInfo in heroList)
            {
                if(heroInfo == null) continue;
                if(getHeroLeftFightTimes(heroInfo.id) <= 0)
                    continue;
                // 如果当前选中大臣为空，并且还有次数，则选中当前大臣
                if (selectHero == null)
                {
                    selectHero = heroInfo;
                }
                if(selectHero.power > heroInfo.power && heroInfo.power > _bossPower)
                    selectHero = heroInfo;
            }

            return selectHero;
        }
        
        /// <summary>
        /// 按照大臣战力排序，有战斗次数的排在无战斗次数的前面，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int sortHero(_IHeroCardShow a, _IHeroCardShow b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            if (getHeroLeftFightTimes(b.id) <= 0 && getHeroLeftFightTimes(a.id) > 0)
                return -1;
            
            if (getHeroLeftFightTimes(a.id) <= 0 && getHeroLeftFightTimes(b.id) > 0)
                return 1;
            
            return b.power.CompareTo(a.power);
        }
        
        /// <summary>
        /// 按照战力排序，有战斗次数的排在无战斗次数的前面，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        public int sortGuildHero(_IHeroCardShow _a, _IHeroCardShow _b)
        {
            MiddayDungeonGuildHeroInfo a = _a as MiddayDungeonGuildHeroInfo;
            MiddayDungeonGuildHeroInfo b = _b as MiddayDungeonGuildHeroInfo;
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            if (getGuildIsUsed(b.cid) && !getGuildIsUsed(a.cid))
                return -1;
            if (getGuildIsUsed(a.cid) && !getGuildIsUsed(b.cid))
                return 1;
            return b.power.CompareTo(a.power);
        }
        
        
        /// <summary>
        /// 刷新当前活动状态
        /// </summary>
        private void _refreshActivityState()
        {
            long nowServerTime = FpsAndPingMgr.instance.serverTimeTag;//当前服务器时间
            EMiddayDungeonActivityState nState;//上一次的活动状态
            if(nowServerTime > _m_previewTimeMs && nowServerTime < _m_startTimeMs)
                nState = EMiddayDungeonActivityState.PREVIEW;
            else if (_m_startTimeMs < nowServerTime && nowServerTime < _m_endTimeMs)
                nState = EMiddayDungeonActivityState.ONGOING;
            else
                nState = EMiddayDungeonActivityState.END;
            
            // 若活动状态发生变化, 发出对应消息
            if (nState != _m_activityState)
            {
                // 如果状态变了，则更新状态
                _m_activityState = nState;
                // 刷新一下Condition
                GCommon.reloadCustomLoadPrefab();
                WinMsg.SendMsg(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG);
                if(_m_activityState == EMiddayDungeonActivityState.END)
                    WinMsg.SendMsg(WinMsgType.ON_MIDDAY_DUNGEON_STATE_END);
            }
        }
        
        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqMiddayDungeonInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_045_ReqMiddayDungeonInit());
        }
        
        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retMiddayDungeonInit(GS2GC_002_045_RetMiddayDungeonInit _msg)
        {
            if(_msg == null)
                return;
            _updateTimeInfo(_msg.getTimeInfo());
            _updateMiddayDungeonInfo(_msg.getInfo());
            _refreshRedTip();
            setInitDone();
        }
        
        private void _updateMiddayDungeonInfo(MiddayDungeon_Info _info)
        {
            if (_info == null)
                return;
            _m_roundStartTimeMS = _info.getRoundStartTimeMS();
            _m_bossInfo?.update(_info.getBossInfo());
            _m_selfFightHeroDic.Clear();
            foreach (var fightHero in _info.getHadFightHeroList())
            {
                if (fightHero != null) _m_selfFightHeroDic[fightHero.getHeroId()] = fightHero.getNum();
            }
            _m_hasBorrowGuildCid.Clear();
            foreach (var borrowCid in _info.getBorrowCidList())
            {
                _m_hasBorrowGuildCid.Add(borrowCid);
            }

            _m_hasRoundInfoReset = false;
            _checkResetRoundInfo();
        }

        private void _updateTimeInfo(MiddayDungeon_TimeInfo _timeInfo)
        {
            if (_timeInfo == null) return;
            _m_startTimeMs = _timeInfo.getStartTimeMs();
            _m_previewTimeMs = _timeInfo.getPreviewTimeMs();
            _m_endTimeMs = _timeInfo.getEndTimeMs();
            _refreshActivityState();
        }
        
        /// <summary>
        /// 请求午间副本攻击
        /// </summary>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonAttack(long _heroId, Action<bool, MiddayDungeon_SettleInfo> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_001_ReqMiddayDungeonAttack(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_001_RetMiddayDungeonAttack>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc, _msg.getSettleInfo());
                }, null, false));
        }

        /// <summary>
        /// 请求午间副本借助大臣攻击
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonBorrowAttack(long _cid, long _heroId, Action<bool, MiddayDungeon_SettleInfo> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_002_ReqMiddayDungeonBorrowAttack(_cid, _heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_002_RetMiddayDungeonBorrowAttack>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc, _msg.getSettleInfo());
                }, null, false));
        }
        
        /// <summary>
        /// 请求午间副本宝箱列表
        /// </summary>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonBoxList(Common.DungeonEnum.EDungeonBoxType _boxType, Action<bool, List<MiddayDungeonBoxInfo>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_003_ReqMiddayDungeonBoxList(_boxType),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_003_RetMiddayDungeonBoxList>((_isSuc, _msg) =>
                {
                    List<MiddayDungeonBoxInfo> boxList = new List<MiddayDungeonBoxInfo>();
                    foreach (MiddayDungeon_BoxInfo box in _msg.getBoxList())
                    {
                        boxList.Add(new MiddayDungeonBoxInfo(box));
                    }
                    _callback?.Invoke(_isSuc, boxList);
                }, null, false));
        }
        
        /// <summary>
        /// 请求午间副本宝箱列表
        /// </summary>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonDrawBox(long _dbId, long _expireTs, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_004_ReqMiddayDungeonDrawBox(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_004_RetMiddayDungeonDrawBox>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        AccountSettingMgr.instance.middayDungeonSaver.setHasOpen(_dbId, _expireTs);
                    }
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 请求午间副本宝箱是否可以领取
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonBoxCanDraw(long _dbId, long _expireTs, Action<bool, bool, int> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_005_ReqMiddayDungeonBoxCanDraw(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_005_RetMiddayDungeonBoxCanDraw>((_isSuc, _msg) =>
                {
                    // 如果已经领完则置为无效
                    if(_isSuc && _msg.getRemainDrawCount()<=0)
                        AccountSettingMgr.instance.middayDungeonSaver.setHasInValid(_dbId, _expireTs);

                    _callback?.Invoke(_isSuc, _msg != null && _msg.getCanDraw(), _msg.getRemainDrawCount());
                }, null, false));
        }
        
        /// <summary>
        /// 查询午间副本宝箱领取记录
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqMiddayDungeonBoxDrawRecord(long _dbId, Action<bool, List<MiddayDungeon_DrawRecord>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_006_ReqMiddayDungeonBoxDrawRecord(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_006_RetMiddayDungeonBoxDrawRecord>(
                    (_isSuc, _msg) =>
                    {
                        if (_msg != null) _callback?.Invoke(_isSuc, _msg.getDrawRecordList());
                    }, null, false));
        }
        /// <summary>
        /// 午间副本信息更新
        /// </summary>
        /// <param name="_msg"></param>
        public void onMiddayDungeonInfoChg(GS2GC_024_051_OnMiddayDungeonInfoChg _msg)
        {
            if (_msg != null)
                _updateMiddayDungeonInfo(_msg.getInfo());
            
            _refreshRedTip();
        }
        
        
        public void onMiddayDungeonTimeInfoChg(GS2GC_024_052_OnMiddayDungeonTimeInfoChg _msg)
        {
            if (_msg != null)
            {
                _updateTimeInfo(_msg.getInfo());
                _checkResetRoundInfo();
            }
            
            _refreshRedTip();
        }
        #endregion
        
        
        
        /// <summary>
        /// 获取JudgeUnionBonus
        /// </summary>
        /// <returns></returns>
        private JudgeUnionBonus getHeroIdJudgeUnionBonus(long _heroId)
        {
            JudgeUnionBonusPart judgePart = new JudgeUnionBonusPart();
            judgePart.filterType = EBonusFilterType.HERO_ID;
            judgePart.id = _heroId;
            JudgeUnionBonus judgeUnionBonus = new JudgeUnionBonus(judgePart);
            return judgeUnionBonus;
        }
    }
}
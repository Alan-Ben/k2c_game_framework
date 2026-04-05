using System;
using System.Collections.Generic;
using ALPackage;
using Common.ArenaEnum;
using Common.ArenaObj;
using GC2GS.p023_ArenaOp;
using GS2GC.p002_InitOp;
using GS2GC.p023_ArenaOp;

namespace GOE
{
    /// <summary>
    /// 竞技场组件
    /// </summary>
    public class ArenaComponent : _ANPBasicPlayerComponent
    {
        //竞技场信息
        private ArenaInfo _m_arenaInfo;
        //竞技场战斗信息
        private ArenaBattleInfo _m_arenaBattleInfo;
        //名人榜信息列表
        private List<ArenaCelebrityInfo> _m_lCelebrityList;
        //名人榜机器人信息列表（引导用）
        private List<ArenaCelebrityInfo> _m_lBotCelebrityList;
        //上次选择谈判的名人榜玩家CID
        private long _m_lLastSelectCelebrityCid;
        //上次选择谈判的名人榜玩家名字
        private string _m_lLastSelectCelebrityName;

        //构造函数
        public ArenaComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = {};
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.ARENA; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 竞技场信息
        /// </summary>
        public ArenaInfo arenaInfo { get { return _m_arenaInfo; } }
        /// <summary>
        /// 竞技场战斗信息
        /// </summary>
        public ArenaBattleInfo arenaBattleInfo { get { return _m_arenaBattleInfo; } }
        /// <summary>
        /// 名人榜信息列表
        /// </summary>
        public List<ArenaCelebrityInfo> celebrityList 
        {
            get
            {
                //如果有机器人列表，返回机器人列表
                if (_m_lBotCelebrityList != null && _m_lBotCelebrityList.Count > 0)
                    return _m_lBotCelebrityList;
                return _m_lCelebrityList;
            }
        }
        /// <summary>
        /// 上次选择谈判的名人榜玩家CID
        /// </summary>
        public long lastSelectCelebrityCid { get { return _m_lLastSelectCelebrityCid; } set { _m_lLastSelectCelebrityCid = value; } }
        /// <summary>
        /// 上次选择谈判的名人榜玩家名字
        /// </summary>
        public string lastSelectCelebrityName { get { return _m_lLastSelectCelebrityName; } set { _m_lLastSelectCelebrityName = value; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqArenaInit();
        }

        protected override void _dealInit()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            //发生跨天
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("ArenaComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            //发生跨天
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_arenaInfo = null;
            _m_arenaBattleInfo = null;
            _m_lCelebrityList = null;
            _m_lBotCelebrityList = null;
        }

        /// <summary>
        /// 是否正在战斗中
        /// </summary>
        /// <returns></returns>
        public bool isInBattle()
        {
            //数据不为空，并且对手CID大于0或者是NPC
            return _m_arenaBattleInfo != null && (_m_arenaBattleInfo.opponentCid > 0 || _m_arenaBattleInfo.opponentCid <= 0 && _m_arenaBattleInfo.opponentIsBot);
        }

        /// <summary>
        /// 获取战斗中选择的伙伴id
        /// </summary>
        /// <returns></returns>
        public long getBattleSelectHeroId()
        {
            if (!isInBattle())
                return 0;

            return _m_arenaBattleInfo.heroId;
        }

        /// <summary>
        /// 是否可以随机战斗
        /// </summary>
        /// <returns></returns>
        public bool canRendomAttack()
        {
            return _m_arenaInfo != null && _m_arenaInfo.getCurLeftAttackCount() > 0;
        }

        /// <summary>
        /// 是否可以指定战斗
        /// </summary>
        /// <returns></returns>
        public bool canSelectAttack()
        {
            return _m_arenaInfo != null && _m_arenaInfo.hadSelectAttackNum < GRefdataCoreMgr.instance.npGeneral.arena_select_attack_daily_limit;
        }

        /// <summary>
        /// 选中的伙伴是否可以派遣进行战斗
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_isSelectAttack"></param>
        /// <returns></returns>
        public bool canSelectHeroAttack(long _heroId, bool _isSelectAttack)
        {
            if (_m_arenaInfo == null)
                return false;

            if (_isSelectAttack)
                return _m_arenaInfo.hadSelectAttackHeroList != null && _m_arenaInfo.hadSelectAttackHeroList.Contains(_heroId);
            else
                return _m_arenaInfo.hadRandomAttackHeroList != null && _m_arenaInfo.hadRandomAttackHeroList.Contains(_heroId);
        }

        /// <summary>
        /// 设置名人榜机器人信息
        /// </summary>
        public void setCelebrityOneBot()
        {
            //记录机器人名称
            string botName = AccountSettingMgr.instance.accountSetting.arenaFightBotName;
            if (string.IsNullOrEmpty(botName))
            {
                botName = GRefdataCoreMgr.instance.getRandomInitName();
                AccountSettingMgr.instance.accountSetting.setArenaFightBotName(botName);
            }

            Arena_CelebrityRankInfo botInfo = new Arena_CelebrityRankInfo();
            botInfo.setDbId(0);
            botInfo.setAttackerCid(0);
            botInfo.setAttackerName(botName);
            botInfo.setDefenderName(GRefdataCoreMgr.instance.getRandomInitName());
            botInfo.setDefeatHeroNum(10);
            botInfo.setIsSelectAttack(false);
            botInfo.setTimeMs(FpsAndPingMgr.instance.serverTimeTag - 3600000);//前一小时
            ArenaCelebrityInfo info = new ArenaCelebrityInfo(botInfo, true);
            if(_m_lBotCelebrityList == null)
                _m_lBotCelebrityList = new List<ArenaCelebrityInfo>();
            _m_lBotCelebrityList.Clear();
            _m_lBotCelebrityList.Add(info);
        }

        /// <summary>
        /// 清空名人榜机器人信息
        /// </summary>
        public void clearCelebrityBot()
        {
            _m_lBotCelebrityList?.Clear();
            _m_lBotCelebrityList = null;
        }

        //发生跨天时的处理
        private void _onCrossDay()
        {
            //发生跨天时，竞技场数据检查是否需要重置
            _m_arenaInfo?.checkNeedResetData();

            _refreshCountRedTip();
        }

        //刷新次数红点
        private void _refreshCountRedTip()
        {
            if (_m_arenaInfo == null)
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ARENA_FIGHT_COUNT, 0);
                return;
            }

            //设置竞技场次数红点
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ARENA_FIGHT_COUNT, _m_arenaInfo.getCurLeftAttackCount());
        }

        #region S2C

        /// <summary>
        /// 竞技场初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retArenaInit(GS2GC_002_025_RetArenaInit _msg)
        {
            if (_msg == null || _msg.getInfo() == null)
                return;

            _m_arenaInfo = new ArenaInfo(_msg.getInfo().getBaseInfo());
            _m_arenaBattleInfo = new ArenaBattleInfo(_msg.getInfo().getBattleInfo());

            //检查是否需要重置数据
            _m_arenaInfo.checkNeedResetData();

            //刷新次数红点
            _refreshCountRedTip();

            setInitDone();
        }

        /// <summary>
        /// 竞技场战斗信息变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onArenaBattleInfoChg(GS2GC_023_051_OnArenaBattleInfoChg _msg)
        {
            if (_msg == null)
                return;

            if (_m_arenaBattleInfo == null)
                _m_arenaBattleInfo = new ArenaBattleInfo(_msg.getBattleInfo());
            else
                _m_arenaBattleInfo.updateInfo(_msg.getBattleInfo());

            //竞技场战斗信息变更
            WinMsg.SendMsg(WinMsgType.ON_ARENA_BATTLE_INFO_CHG);
        }

        /// <summary>
        /// 竞技场基础数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onArenaBaseInfoChg(GS2GC_023_052_OnArenaBaseInfoChg _msg)
        {
            if (_msg == null)
                return;

            if (_m_arenaInfo == null)
                _m_arenaInfo = new ArenaInfo(_msg.getBaseInfo());
            else
                _m_arenaInfo.updateInfo(_msg.getBaseInfo());

            //刷新次数红点
            _refreshCountRedTip();

            //竞技场基础信息变更
            WinMsg.SendMsg(WinMsgType.ON_ARENA_BASE_INFO_CHG);
        }

        /// <summary>
        /// 竞技场战斗结束
        /// </summary>
        public void onArenaBattleReset()
        {
            _m_arenaBattleInfo = null; 
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求竞技场初始化
        /// </summary>
        public void reqArenaInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_025_ReqArenaInit());
        }

        /// <summary>
        /// 请求随机攻击
        /// </summary>
        /// <param name="_callback"></param>
        public void reqRandomAttackPlayer(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_001_ReqRandomAttackPlayer(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_001_RetRandomAttackPlayer>((_isSuc, _msg) =>
                    {
                        //清空机器人名字
                        AccountSettingMgr.instance.accountSetting.setArenaFightBotName("");
                        //清空机器人等级
                        AccountSettingMgr.instance.accountSetting.setArenaFightBotLevel(0);
                        _callback?.Invoke(_isSuc);
                    }));
        }

        /// <summary>
        /// 请求随机攻击选择出战大臣
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_opponentBotName">机器人对手名称，用于记录在名人榜上</param>
        /// <param name="_callback"></param>
        public void reqRandomAttackSelectHero(long _heroId, string _opponentBotName, long _buffId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_002_ReqRandomAttackSelectHero(_heroId, _opponentBotName, _buffId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_002_RetRandomAttackSelectHero>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        /// <summary>
        /// 请求指定攻击选择出战大臣
        /// </summary>
        /// <param name="_opponentCid"></param>
        /// <param name="_itemId"></param>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqSelectAttackSelectHero(long _opponentCid, long _itemId, long _heroId, long _buffId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_003_ReqSelectAttackSelectHero(_opponentCid, _itemId, _heroId, _buffId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_003_RetSelectAttackSelectHero>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        /// <summary>
        /// 请求选择临时增益
        /// </summary>
        /// <param name="_buffId"></param>
        /// <param name="_callback"></param>
        public void reqChooseBuff(long _buffId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_004_ReqChooseBuff(_buffId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_004_RetChooseBuff>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        /// <summary>
        /// 请求回合进攻
        /// </summary>
        /// <param name="_opponentHeroId"></param>
        /// <param name="_callback"></param>
        public void reqRoundAttack(long _opponentHeroId, Action<bool, GS2GC_023_005_RetRoundAttack> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_005_ReqRoundAttack(_opponentHeroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_005_RetRoundAttack>((_isSuc, _msg) => _callback?.Invoke(_isSuc, _msg)));
        }

        /// <summary>
        /// 请求反击选择出战大臣
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqFightBackSelectHero(long _dbId, long _itemId, long _heroId, long _buffId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_006_ReqFightBackSelectHero(_dbId, _itemId, _heroId, _buffId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_006_RetFightBackSelectHero>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        /// <summary>
        /// 请求名人榜排行
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqCelebrityRank(long _dbId, Action<List<ArenaCelebrityInfo>> _callback = null)
        {
            //如果有机器人列表，直接返回机器人列表
            if (_m_lBotCelebrityList != null && _m_lBotCelebrityList.Count > 0)
            {
                _callback?.Invoke(_m_lBotCelebrityList);
                return;
            }

            NPGSClientListener.sendRequestByLog(new GC2GS_023_007_ReqCelebrityRank(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_007_RetCelebrityRank>((_isSuc, _msg) =>
                {
                    if(_msg == null)
                        return;

                    if (_m_lCelebrityList == null)
                        _m_lCelebrityList = new List<ArenaCelebrityInfo>();
                    else
                        _m_lCelebrityList.Clear();

                    for (int i = 0; i < _msg.getRankList().Count; i++)
                    {
                        _m_lCelebrityList.Add(new ArenaCelebrityInfo(_msg.getRankList()[i]));
                    }
                    //排序 时间倒序
                    _m_lCelebrityList?.Sort((_a, _b) => -(_a.timeMs.CompareTo(_b.timeMs)));

                    _callback?.Invoke(_m_lCelebrityList);

                    WinMsg.SendMsg(WinMsgType.ON_GET_ARENA_CELEBRITY_LIST);
                }));
        }

        /// <summary>
        /// 请求竞技场战报
        /// </summary>
        /// <param name="_callback"></param>
        public void reqArenaBattleReport(Action<List<Arena_BattleReport>> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_010_ReqArenaBattleReport(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_010_RetArenaBattleReport>((_isSuc, _msg) => _callback?.Invoke(_msg?.getReportList())));
        }

        /// <summary>
        /// 请求竞技场反击数据
        /// </summary>
        /// <param name="_callback"></param>
        public void reqArenaFightBackData(Action<List<Arena_FightBackInfo>> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_011_ReqArenaFightBackData(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_011_RetArenaFightBackData>((_isSuc, _msg) => _callback?.Invoke(_msg?.getFightBackList())));
        }

        /// <summary>
        /// 请求竞技场购买随机攻击次数
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_callback"></param>
        public void reqArenaBuyRandomAttack(int _num, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_012_ReqArenaBuyRandomAttack(_num),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_012_RetArenaBuyRandomAttack>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        /// <summary>
        /// 请求一键攻击
        /// </summary>
        /// <param name="_buffType"></param>
        /// <param name="_callback"></param>
        public void reqArenaAKeyAttack(EArenaBuffType _buffType, Action<bool, GS2GC_023_013_RetArenaAKeyAttack> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_013_ReqArenaAKeyAttack(_buffType),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_013_RetArenaAKeyAttack>((_isSuc, _msg) => _callback?.Invoke(_isSuc, _msg)));
        }

        /// <summary>
        /// 请求攻击系统内定账号选择出战大臣
        /// </summary>
        /// <param name="_itemId"></param>
        /// <param name="_heroId"></param>
        /// <param name="_buffId"></param>
        /// <param name="_callback"></param>
        public void reqSysSelectAttackSelectHero(long _itemId, long _heroId, long _buffId, Action<bool> _callback = null)
        {
            //清空机器人信息
            _m_lBotCelebrityList?.Clear();
            _m_lBotCelebrityList = null;

            NPGSClientListener.sendRequestByLog(new GC2GS_023_014_ReqSysSelectAttackSelectHero(_itemId, _heroId, _buffId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_014_RetSysSelectAttackSelectHero>((_isSuc, _msg) => _callback?.Invoke(_isSuc)));
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using CommonEnum;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExplorePvPLog : _ANPGGUIBasicWnd<GGUIMonoMarsExplorePvPLog>
    {
        public enum ELogListType
        {
            MyLog,
            GuildLog,
        }


        [NotNull] public static GGUIWndMarsExplorePvPLog instance { get { return _g_instance ??= new GGUIWndMarsExplorePvPLog(); } }
        private static GGUIWndMarsExplorePvPLog _g_instance;


        private GGUISubWndMarsExplorePvPLogGrid _m_logGrid;
        private int _m_refreshSerialize;
        private NPGGUIWndCommonTab _m_myLogToggle;
        private NPGGUIWndCommonTab _m_guildLogToggle;
        private ELogListType _m_currentListType;


        public GGUIWndMarsExplorePvPLog()
            : base(EALUIWndLayer.ADDITION)
        {
            _m_currentListType = ELogListType.MyLog;
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsExplorePvPLog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExplorePvPLog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_logGrid?.showWnd();
            _m_myLogToggle?.showWnd();
            _m_guildLogToggle?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_logGrid?.hideWnd();
            _m_myLogToggle?.hideWnd();
            _m_guildLogToggle?.hideWnd();

            _m_refreshSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_logGrid?.resetWnd();
            _m_myLogToggle?.resetWnd();
            _m_guildLogToggle?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_logGrid?.discard();
            _m_logGrid = null;
            if (_m_myLogToggle != null)
            {
                _m_myLogToggle.clickDelegate -= _onMyLogTabClick;
                _m_myLogToggle.discard();
                _m_myLogToggle = null;
            }
            if (_m_guildLogToggle != null)
            {
                _m_guildLogToggle.clickDelegate -= _onGuildLogTabClick;
                _m_guildLogToggle.discard();
                _m_guildLogToggle = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLogGrid != null)
                _m_logGrid = new GGUISubWndMarsExplorePvPLogGrid(wnd.monoLogGrid);
            if (wnd.toggleMyLog != null)
            {
                _m_myLogToggle = new NPGGUIWndCommonTab(wnd.toggleMyLog);
                _m_myLogToggle.clickDelegate += _onMyLogTabClick;
            }
            if (wnd.toggleGuildLog != null)
            {
                _m_guildLogToggle = new NPGGUIWndCommonTab(wnd.toggleGuildLog);
                _m_guildLogToggle.clickDelegate += _onGuildLogTabClick;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_refreshSerialize = ALSerializeOpMgr.next();
            int loadSerialize = _m_refreshSerialize;

            _m_myLogToggle?.setSelected(_m_currentListType == ELogListType.MyLog);
            _m_guildLogToggle?.setSelected(_m_currentListType == ELogListType.GuildLog);

            bool hasGuild = NPPlayer.instance.guildComp.isJoinGuild();
            wnd.setHasGuild(hasGuild);

            ALUGUICommon.setLabelTxt(wnd.txtLogNumTip, TextTranslate.instance.getLanguage(TransKeyConst.mars_explorePvPLogNum_num, GRefdataCoreMgr.instance.npGeneral.mars_explore_pvp_record_save_limit));

            wnd.setState(true, true);
            _m_logGrid?.refreshWnd(null);

            if (_m_currentListType == ELogListType.MyLog)
            {
                _refreshMyLog(loadSerialize);
            }
            else
            {
                _refreshGuildLog(loadSerialize);
            }
        }

        private void _refreshMyLog(int _loadSerialize)
        {
            NPPlayer.instance.marsComp.exploreSubComponent.reqPvPLog((_idxList) =>
            {
                if (_loadSerialize != _m_refreshSerialize)
                    return;

                List<Mars_ExplorePVPLogIdx> idxList = _idxList;
                List<_ALogData> logDataList = new List<_ALogData>(idxList?.Count ?? 0);
                if (idxList != null)
                {
                    for (int i = idxList.Count - 1; i >= 0; i--)
                    {
                        _ALogData logData = _ALogData.createLogData(idxList[i]);
                        if (logData != null)
                            logDataList.Add(logData);
                    }
                }

                _m_logGrid?.refreshWnd(logDataList);
                wnd?.setState(false, idxList is not { Count: > 0 });
            });
        }

        private void _refreshGuildLog(int _loadSerialize)
        {
            NPPlayer.instance.guildComp.reqGuildBattleReport((_reportList) =>
            {
                if (_loadSerialize != _m_refreshSerialize)
                    return;

                List<_ALogData> logDataList = new List<_ALogData>(_reportList?.Count ?? 0);
                if (_reportList != null)
                {
                    for (int i = _reportList.Count - 1; i >= 0; i--)
                    {
                        _ALogData logData = _ALogData.createLogData(_reportList[i]);
                        if (logData != null)
                            logDataList.Add(logData);
                    }
                }

                _m_logGrid?.refreshWnd(logDataList);
                wnd?.setState(false, _reportList is not { Count: > 0 });
            });
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_PVP_LOG);
        }
        private void _onMyLogTabClick(bool _)
        {
            _setToggleType(ELogListType.MyLog);
        }
        private void _onGuildLogTabClick(bool _)
        {
            _setToggleType(ELogListType.GuildLog);
        }
        private void _setToggleType(ELogListType _type)
        {
            if (_m_currentListType == _type)
                return;
            _m_currentListType = _type;
            refreshWnd();
        }


        public abstract class _ALogData
        {
            public struct BattleSideData
            {
                [NotNull] public string name;
                public NPGTextureIndex avatar;
                public long oriSoldierNum;
                public long hurtSoldierNum;
                public long singleSoldierPower;
                public NPCommonSimplePlayerInfo playerInfo;
            }
            
            
            private readonly long _m_createdAt;
            protected readonly bool _m_isGuildLog;


            protected _ALogData(long _createdAt, bool _isGuildLog)
            {
                _m_createdAt = _createdAt;
                _m_isGuildLog = _isGuildLog;
            }


            public abstract bool isSuccess { get; }
            public abstract NPGTextureIndex icon { get; }
            public abstract string title { get; }
            public abstract bool needShowDetailBtn { get; }
            public abstract NPCommonCostItem showItem { get; }


            public long getCreatedAt() { return _m_createdAt; }
            
            public abstract void getDesc(Action<string> _descComplete);
            public abstract void getMySideData(Action<BattleSideData> _callback);
            public abstract void getEnemySideData(Action<BattleSideData> _callback);

            protected static string _getNameWithGuildTag(NPCommon.PlayerInfo_IconShow _info)
            {
                if (_info == null)
                    return string.Empty;
                if (string.IsNullOrEmpty(_info.getGuildSimpleName()))
                    return _info.getPlayerName();
                return TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOccupantName,
                    _info.getGuildSimpleName(), _info.getPlayerName());
            }


            public static _ALogData createLogData(EMarsExplorePVPLogType _logType, long _createdAt, byte[] _logData, bool _isGuildLog)
            {
                switch (_logType)
                {
                    case EMarsExplorePVPLogType.MINE_ATTACK:
                        return new MineAttackLogData(_createdAt, _logData, _isGuildLog);
                    case EMarsExplorePVPLogType.MINE_DEFEND:
                        return new MineDefendLogData(_createdAt, _logData, _isGuildLog);
                    case EMarsExplorePVPLogType.BATTLE_EVENT:
                        return new BattleEventLogData(_createdAt, _logData, _isGuildLog);
                    case EMarsExplorePVPLogType.BOSS_BATTLE_EVENT:
                        return new BossBattleEventLogData(_createdAt, _logData, _isGuildLog);
                    case EMarsExplorePVPLogType.MINE_COLLECT_COMPLETE:
                        return new MineCollectCompleteLogData(_createdAt, _logData, _isGuildLog);
                    default:
                        ALLog.Error("Unsupported log type: " + _logType);
                        return null;
                }
            }

            public static _ALogData createLogData(Mars_ExplorePVPLogIdx _logIdx)
            {
                if (_logIdx == null)
                    return null;
                return createLogData(_logIdx.getLogType(), _logIdx.getCreatedAt(), _logIdx.getExData(), false);
            }

            public static _ALogData createLogData(Mars_GuildBattleReportIdx _reportIdx)
            {
                if (_reportIdx == null)
                    return null;
                return createLogData(_reportIdx.getLogType(), _reportIdx.getCreatedAt(), _reportIdx.getLogData(), true);
            }
        }
        public class MineAttackLogData : _ALogData
        {
            [NotNull] private readonly Mars_MineAttackLog _m_attackLog;
            private readonly MarsExploreMineRefObj _m_mineRefObj;

            private bool _m_opponentInfoGetComplete;
            private bool _m_startGetOpponentInfo;
            private Action<NPCommon.PlayerInfo_IconShow> _m_opponentInfoCallbackList;
            private NPCommon.PlayerInfo_IconShow _m_opponentInfo;

            private bool _m_allyInfoGetComplete;
            private bool _m_startGetAllyInfo;
            private Action<NPCommon.PlayerInfo_IconShow> _m_allyInfoCallbackList;
            private NPCommon.PlayerInfo_IconShow _m_allyInfo;


            public MineAttackLogData(long _createdAt, byte[] _logData, bool _isGuildLog) : base(_createdAt, _isGuildLog)
            {
                _m_attackLog = new Mars_MineAttackLog();
                if(_logData != null)
                    _m_attackLog.readPackage(_logData);
                _m_mineRefObj = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_attackLog.getMineRefId());
            }


            public override bool isSuccess { get { return _m_attackLog.getIsSucc(); } }
            public override NPGTextureIndex icon { get { return _m_mineRefObj?.icon; } }
            public override string title
            {
                get
                {
                    string key = isSuccess ? TransKeyConst.mars_explore_resAtkSucTitle : TransKeyConst.mars_explore_resAtkFailTitle;
                    return TextTranslate.instance.getLanguage(key);
                }
            }
            public override bool needShowDetailBtn { get { return true; } }
            public override NPCommonCostItem showItem { get { return null; } }


            private void _getOpponentPlayerInfo([NotNull] Action<NPCommon.PlayerInfo_IconShow> _callback)
            {
                if (_m_opponentInfoGetComplete)
                {
                    _callback.Invoke(_m_opponentInfo);
                    return;
                }

                _m_opponentInfoCallbackList += _callback;

                if (_m_startGetOpponentInfo)
                    return;

                _m_startGetOpponentInfo = true;

                GC2GS_004_011_ReqSomeOnePlayerBriefInfo req = new GC2GS_004_011_ReqSomeOnePlayerBriefInfo();
                req.setCid(_m_attackLog.getDefender().getCid());

                NPGSClientListener.sendRequestByLog(req,
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        _m_opponentInfo = _msg?.getPlayerBrief();
                        _m_opponentInfoGetComplete = true;

                        Action<NPCommon.PlayerInfo_IconShow> callback = _m_opponentInfoCallbackList;
                        _m_opponentInfoCallbackList = null;
                        callback?.Invoke(_m_opponentInfo);
                    }));
            }

            private void _getAllyPlayerInfo([NotNull] Action<NPCommon.PlayerInfo_IconShow> _callback)
            {
                if (_m_allyInfoGetComplete)
                {
                    _callback.Invoke(_m_allyInfo);
                    return;
                }

                _m_allyInfoCallbackList += _callback;

                if (_m_startGetAllyInfo)
                    return;

                _m_startGetAllyInfo = true;

                GC2GS_004_011_ReqSomeOnePlayerBriefInfo req = new GC2GS_004_011_ReqSomeOnePlayerBriefInfo();
                req.setCid(_m_attackLog.getAttacker().getCid());

                NPGSClientListener.sendRequestByLog(req,
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        _m_allyInfo = _msg?.getPlayerBrief();
                        _m_allyInfoGetComplete = true;

                        Action<NPCommon.PlayerInfo_IconShow> callback = _m_allyInfoCallbackList;
                        _m_allyInfoCallbackList = null;
                        callback?.Invoke(_m_allyInfo);
                    }));
            }


            public override void getDesc(Action<string> _descComplete)
            {
                if (!_m_isGuildLog)
                {
                    _getOpponentPlayerInfo((_opponentInfo) =>
                    {
                        string key = isSuccess ? TransKeyConst.mars_explore_resAtkSuc : TransKeyConst.mars_explore_resAtkFail;
                        string desc = TextTranslate.instance.getLanguage(key, _opponentInfo?.getPlayerName());
                        _descComplete?.Invoke(desc);
                    });
                    return;
                }

                _getAllyPlayerInfo((_allyInfo) =>
                {
                    _getOpponentPlayerInfo((_enemyInfo) =>
                    {
                        string enemyName = _getNameWithGuildTag(_enemyInfo);
                        string allyName = _allyInfo?.getPlayerName() ?? string.Empty;
                        string desc = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreLog_guildAtkOther_desc, enemyName, allyName);
                        _descComplete?.Invoke(desc);
                    });
                });
            }


            public override void getMySideData([NotNull] Action<BattleSideData> _callback)
            {
                Common.MarsObj.Mars_ExploreBattlePlayerInfo myInfo = _m_attackLog.getAttacker();
                BattleSideData data = new BattleSideData
                {
                    name = NPPlayer.instance.playerInfo.PlayerName,
                    avatar = NPPlayer.instance.playerInfo.curIcon?.baseData?.icon,
                    oriSoldierNum = myInfo.getOriSoldierNum(),
                    hurtSoldierNum = myInfo.getHurtSoldierNum(),
                    singleSoldierPower = myInfo.getSingleSoldierPower(),
                    playerInfo = new NPCommonSimplePlayerInfo(NPPlayer.instance.playerInfo, NPPlayer.instance.titleComp.curTitle, true,GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP))
                };
                _callback.Invoke(data);
            }


            public override void getEnemySideData([NotNull] Action<BattleSideData> _callback)
            {
                _getOpponentPlayerInfo((_opponentInfo) =>
                {
                    Mars_ExploreBattlePlayerInfo enemyInfo = _m_attackLog.getDefender();
                    BattleSideData data = new BattleSideData
                    {
                        name = _opponentInfo?.getPlayerName() ?? string.Empty,
                        avatar = GCommon.getItemTexIcon(ENPItemType.ICON, _m_opponentInfo?.getIconId() ?? 0),
                        oriSoldierNum = enemyInfo.getOriSoldierNum(),
                        hurtSoldierNum = enemyInfo.getHurtSoldierNum(),
                        singleSoldierPower = enemyInfo.getSingleSoldierPower(),
                        playerInfo = new NPCommonSimplePlayerInfo(_opponentInfo)
                    };
                    _callback.Invoke(data);
                });
            }
        }
        public class MineDefendLogData : _ALogData
        {
            [NotNull] private readonly Mars_MineDefenceLog _m_defendLog;
            private readonly MarsExploreMineRefObj _m_mineRefObj;

            private bool _m_opponentInfoGetComplete;
            private bool _m_startGetOpponentInfo;
            private Action<NPCommon.PlayerInfo_IconShow> _m_opponentInfoCallbackList;
            private NPCommon.PlayerInfo_IconShow _m_opponentInfo;

            private bool _m_allyInfoGetComplete;
            private bool _m_startGetAllyInfo;
            private Action<NPCommon.PlayerInfo_IconShow> _m_allyInfoCallbackList;
            private NPCommon.PlayerInfo_IconShow _m_allyInfo;


            public MineDefendLogData(long _createdAt, byte[] _logData, bool _isGuildLog) : base(_createdAt, _isGuildLog)
            {
                _m_defendLog = new Mars_MineDefenceLog();
                if (_logData != null)
                    _m_defendLog.readPackage(_logData);
                _m_mineRefObj = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_defendLog.getMineRefId());
            }


            public override bool isSuccess { get { return _m_defendLog.getIsSucc(); } }
            public override NPGTextureIndex icon { get { return _m_mineRefObj?.icon; } }
            public override string title
            {
                get
                {
                    string key = isSuccess ? TransKeyConst.mars_explore_resDefSucTitle : TransKeyConst.mars_explore_resDefFailTitle;
                    return TextTranslate.instance.getLanguage(key);
                }
            }
            public override bool needShowDetailBtn { get { return true; } }
            public override NPCommonCostItem showItem { get { return null; } }


            private void _getOpponentPlayerInfo([NotNull] Action<NPCommon.PlayerInfo_IconShow> _callback)
            {
                if (_m_opponentInfoGetComplete)
                {
                    _callback.Invoke(_m_opponentInfo);
                    return;
                }

                _m_opponentInfoCallbackList += _callback;

                if (_m_startGetOpponentInfo)
                    return;

                _m_startGetOpponentInfo = true;

                GC2GS_004_011_ReqSomeOnePlayerBriefInfo req = new GC2GS_004_011_ReqSomeOnePlayerBriefInfo();
                req.setCid(_m_defendLog.getAttacker().getCid());

                NPGSClientListener.sendRequestByLog(req,
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        _m_opponentInfo = _msg?.getPlayerBrief();
                        _m_opponentInfoGetComplete = true;

                        Action<NPCommon.PlayerInfo_IconShow> callback = _m_opponentInfoCallbackList;
                        _m_opponentInfoCallbackList = null;
                        callback?.Invoke(_m_opponentInfo);
                    }));
            }

            private void _getAllyPlayerInfo([NotNull] Action<NPCommon.PlayerInfo_IconShow> _callback)
            {
                if (_m_allyInfoGetComplete)
                {
                    _callback.Invoke(_m_allyInfo);
                    return;
                }

                _m_allyInfoCallbackList += _callback;

                if (_m_startGetAllyInfo)
                    return;

                _m_startGetAllyInfo = true;

                GC2GS_004_011_ReqSomeOnePlayerBriefInfo req = new GC2GS_004_011_ReqSomeOnePlayerBriefInfo();
                req.setCid(_m_defendLog.getDefender().getCid());

                NPGSClientListener.sendRequestByLog(req,
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        _m_allyInfo = _msg?.getPlayerBrief();
                        _m_allyInfoGetComplete = true;

                        Action<NPCommon.PlayerInfo_IconShow> callback = _m_allyInfoCallbackList;
                        _m_allyInfoCallbackList = null;
                        callback?.Invoke(_m_allyInfo);
                    }));
            }


            public override void getDesc(Action<string> _descComplete)
            {
                if (!_m_isGuildLog)
                {
                    _getOpponentPlayerInfo((_opponentInfo) =>
                    {
                        string key = isSuccess ? TransKeyConst.mars_explore_resDefSuc : TransKeyConst.mars_explore_resDefFail;
                        string desc = TextTranslate.instance.getLanguage(key, _opponentInfo?.getPlayerName());
                        _descComplete?.Invoke(desc);
                    });
                    return;
                }

                _getAllyPlayerInfo((_allyInfo) =>
                {
                    _getOpponentPlayerInfo((_enemyInfo) =>
                    {
                        string enemyName = _getNameWithGuildTag(_enemyInfo);
                        string allyName = _allyInfo?.getPlayerName() ?? string.Empty;
                        string desc = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreLog_otherAtkGuild_desc, enemyName, allyName);
                        _descComplete?.Invoke(desc);
                    });
                });
            }
            public override void getMySideData([NotNull] Action<BattleSideData> _callback)
            {
                Common.MarsObj.Mars_ExploreBattlePlayerInfo myInfo = _m_defendLog.getDefender();
                BattleSideData data = new BattleSideData
                {
                    name = NPPlayer.instance.playerInfo.PlayerName,
                    avatar = NPPlayer.instance.playerInfo.curIcon?.baseData?.icon,
                    oriSoldierNum = myInfo.getOriSoldierNum(),
                    hurtSoldierNum = myInfo.getHurtSoldierNum(),
                    singleSoldierPower = myInfo.getSingleSoldierPower(),
                    playerInfo = new NPCommonSimplePlayerInfo(NPPlayer.instance.playerInfo, NPPlayer.instance.titleComp.curTitle, true,GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP))
                };
                _callback.Invoke(data);
            }
            public override void getEnemySideData([NotNull] Action<BattleSideData> _callback)
            {
                _getOpponentPlayerInfo((_opponentInfo) =>
                {
                    Common.MarsObj.Mars_ExploreBattlePlayerInfo enemyInfo = _m_defendLog.getAttacker();
                    BattleSideData data = new BattleSideData
                    {
                        name = _opponentInfo?.getPlayerName() ?? string.Empty,
                        avatar = GCommon.getItemTexIcon(ENPItemType.ICON, _m_opponentInfo?.getIconId() ?? 0),
                        oriSoldierNum = enemyInfo.getOriSoldierNum(),
                        hurtSoldierNum = enemyInfo.getHurtSoldierNum(),
                        singleSoldierPower = enemyInfo.getSingleSoldierPower(),
                        playerInfo = new NPCommonSimplePlayerInfo(_opponentInfo)
                    };
                    _callback.Invoke(data);
                });
            }
        }
        public class BattleEventLogData : _ALogData
        {
            [NotNull] private readonly Mars_BattleEventLog _m_eventLog;
            private readonly MarsExploreEventBattleRefObj _m_battleRefObj;


            public BattleEventLogData(long _createdAt, byte[] _logData, bool _isGuildLog) : base(_createdAt, _isGuildLog)
            {
                _m_eventLog = new Mars_BattleEventLog();
                if (_logData != null)
                    _m_eventLog.readPackage(_logData);
                _m_battleRefObj = GRefdataCoreMgr.instance.marsExploreEventBattleRefCore.getRef(_m_eventLog.getEventRefId());
            }


            public override bool isSuccess { get { return _m_eventLog.getIsSucc(); } }
            public override NPGTextureIndex icon { get { return _m_battleRefObj?.icon; } }
            public override string title
            {
                get
                {
                    string key = isSuccess ? TransKeyConst.mars_explore_eventBattleSucTitle : TransKeyConst.mars_explore_eventBattleFailTitle;
                    return TextTranslate.instance.getLanguage(key);
                }
            }
            public override bool needShowDetailBtn { get { return true; } }
            public override NPCommonCostItem showItem { get { return null; } }


            public override void getDesc(Action<string> _descComplete)
            {
                string key = isSuccess ? TransKeyConst.mars_explore_eventSuc : TransKeyConst.mars_explore_eventFail;
                _descComplete?.Invoke(TextTranslate.instance.getLanguage(key, TextTranslate.instance.getLanguage(_m_battleRefObj?.name)));
            }
            public override void getMySideData(Action<BattleSideData> _callback)
            {
                Mars_ExploreBattlePlayerInfo myInfo = _m_eventLog.getAttacker();
                BattleSideData data = new BattleSideData
                {
                    name = NPPlayer.instance.playerInfo.PlayerName,
                    avatar = NPPlayer.instance.playerInfo?.curIcon?.baseData?.icon,
                    oriSoldierNum = myInfo.getOriSoldierNum(),
                    hurtSoldierNum = myInfo.getHurtSoldierNum(),
                    singleSoldierPower = myInfo.getSingleSoldierPower(),
                    playerInfo = new NPCommonSimplePlayerInfo(NPPlayer.instance.playerInfo, NPPlayer.instance.titleComp.curTitle, true,GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP))
                };
                _callback?.Invoke(data);
            }
            public override void getEnemySideData(Action<BattleSideData> _callback)
            {
                Mars_ExploreBattleNPCInfo npcInfo = _m_eventLog.getNpc();
                BattleSideData data = new BattleSideData
                {
                    name = TextTranslate.instance.getLanguage(_m_battleRefObj?.name),
                    avatar = _m_battleRefObj?.icon,
                    oriSoldierNum = npcInfo.getOriSoldierNum(),
                    hurtSoldierNum = npcInfo.getHurtSoldierNum(),
                    singleSoldierPower = npcInfo.getSingleSoldierPower(),
                    playerInfo = null
                };
                _callback?.Invoke(data);
            }
        }
        public class BossBattleEventLogData : _ALogData
        {
            [NotNull] private readonly Mars_BattleEventLog _m_eventLog;
            private readonly MarsExploreEventBossRefObj _m_bossRefObj;


            public BossBattleEventLogData(long _createdAt, byte[] _logData, bool _isGuildLog) : base(_createdAt, _isGuildLog)
            {
                _m_eventLog = new Mars_BattleEventLog();
                if (_logData != null)
                    _m_eventLog.readPackage(_logData);
                _m_bossRefObj = GRefdataCoreMgr.instance.marsExploreEventBossRefCore.getRef(_m_eventLog.getEventRefId());
            }


            public override bool isSuccess { get { return _m_eventLog.getIsSucc(); } }
            public override NPGTextureIndex icon { get { return _m_bossRefObj?.icon; } }
            public override string title
            {
                get
                {
                    string key = isSuccess ? TransKeyConst.mars_explore_bossBattleSucTitle : TransKeyConst.mars_explore_bossBattleFailTitle;
                    return TextTranslate.instance.getLanguage(key);
                }
            }
            public override bool needShowDetailBtn { get { return true; } }
            public override NPCommonCostItem showItem { get { return null; } }


            public override void getDesc(Action<string> _descComplete)
            {
                string key = isSuccess ? TransKeyConst.mars_explore_eventSuc : TransKeyConst.mars_explore_eventFail;
                _descComplete?.Invoke(TextTranslate.instance.getLanguage(key, TextTranslate.instance.getLanguage(_m_bossRefObj?.name, _m_bossRefObj?.name_parms)));
            }
            public override void getMySideData([NotNull] Action<BattleSideData> _callback)
            {
                Mars_ExploreBattlePlayerInfo myInfo = _m_eventLog.getAttacker();
                BattleSideData data = new BattleSideData
                {
                    name = NPPlayer.instance.playerInfo.PlayerName,
                    avatar = NPPlayer.instance.playerInfo.curIcon?.baseData?.icon,
                    oriSoldierNum = myInfo.getOriSoldierNum(),
                    hurtSoldierNum = myInfo.getHurtSoldierNum(),
                    singleSoldierPower = myInfo.getSingleSoldierPower(),
                    playerInfo = new NPCommonSimplePlayerInfo(NPPlayer.instance.playerInfo, NPPlayer.instance.titleComp.curTitle, true,GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP))
                };
                _callback.Invoke(data);
            }
            public override void getEnemySideData([NotNull] Action<BattleSideData> _callback)
            {
                Mars_ExploreBattleNPCInfo npcInfo = _m_eventLog.getNpc();
                BattleSideData data = new BattleSideData
                {
                    name = TextTranslate.instance.getLanguage(_m_bossRefObj?.name, _m_bossRefObj?.name_parms),
                    avatar = _m_bossRefObj?.icon,
                    oriSoldierNum = npcInfo.getOriSoldierNum(),
                    hurtSoldierNum = npcInfo.getHurtSoldierNum(),
                    singleSoldierPower = npcInfo.getSingleSoldierPower(),
                    playerInfo = null
                };
                _callback.Invoke(data);
            }
        }
        public class MineCollectCompleteLogData : _ALogData
        {
            [NotNull] private readonly Mars_MineCollectLog _m_collectLog;
            private readonly MarsExploreMineRefObj _m_mineRefObj;
            private readonly NPCommonCostItem _m_showItem;
            

            public MineCollectCompleteLogData(long _createdAt, byte[] _logData, bool _isGuildLog) : base(_createdAt, _isGuildLog)
            {
                _m_collectLog = new Mars_MineCollectLog();
                if (_logData != null)
                    _m_collectLog.readPackage(_logData);
                _m_mineRefObj = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_collectLog.getMineRefId());
                _m_showItem = new NPCommonCostItem(_m_mineRefObj?.res_type, _m_collectLog.getCollectNum());
            }


            public override bool isSuccess { get { return true; } }
            public override NPGTextureIndex icon { get { return _m_mineRefObj?.icon; } }
            public override string title
            {
                get
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.mars_explore_mineCollectCompleteTitle);
                }
            }
            public override bool needShowDetailBtn { get { return false; } }
            public override NPCommonCostItem showItem { get { return _m_showItem; } }


            public override void getDesc(Action<string> _descComplete)
            {
                MarsExploreTeamInfo teamInfo = NPPlayer.instance.marsComp.exploreSubComponent.getTeamInfoById(_m_collectLog.getTeamId());
                string desc = TextTranslate.instance.getLanguage(TransKeyConst.mars_explore_resEnd, teamInfo?.name ?? string.Empty);
                _descComplete?.Invoke(desc);
            }
            public override void getMySideData([NotNull] Action<BattleSideData> _callback)
            {
                _callback.Invoke(new BattleSideData());
            }
            public override void getEnemySideData([NotNull] Action<BattleSideData> _callback)
            {
                _callback.Invoke(new BattleSideData());
            }
        }
    }
}

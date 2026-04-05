using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星探索-联盟矿分享列表窗口
    /// </summary>
    public class GGUIWndMarsExploreMineShare : _ANPGGUIBasicWnd<GGUIMonoMarsExploreMineShare>
    {
        public enum EMineShareListType
        {
            MyMine,
            GuildMine,
        }
        
        
        [NotNull] public static GGUIWndMarsExploreMineShare instance { get { return _g_instance ??= new GGUIWndMarsExploreMineShare(); } }
        private static GGUIWndMarsExploreMineShare _g_instance;


        /// <summary>矿分享列表Grid</summary>
        private GGUISubWndMarsExploreMineShareGrid _m_itemGrid;
        /// <summary>刷新序列号，用于控制异步请求的有效性</summary>
        private int _m_refreshSerialize;
        /// <summary>表现数据列表</summary>
        [ItemNotNull, NotNull] private readonly List<_AMineShareItemViewData> _m_myMineViewDataList;
        [ItemNotNull, NotNull] private readonly List<_AMineShareItemViewData> _m_guildMineViewDataList;
        /// <summary>是否需要刷新Grid</summary>
        private bool _m_needRefreshGrid;
        /// <summary>定时检查矿有效期的任务控制器</summary>
        private ALCommonEnableTaskController _m_checkExpireTask;
        /// <summary>我的矿的切换按钮</summary>
        private NPGGUIWndCommonTab _m_myMineToggle;
        /// <summary>联盟矿的切换按钮</summary>
        private NPGGUIWndCommonTab _m_guildMineToggle;
        /// <summary>当前显示的矿分享列表类型</summary>
        private EMineShareListType _m_currentListType;


        public GGUIWndMarsExploreMineShare()
            : base(EALUIWndLayer.ADDITION)
        {
            _m_myMineViewDataList = new List<_AMineShareItemViewData>();
            _m_guildMineViewDataList = new List<_AMineShareItemViewData>();
            _m_needRefreshGrid = false;
            _m_currentListType = EMineShareListType.MyMine;
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreMineShare.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreMineShare.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_itemGrid?.showWnd();
            _m_myMineToggle?.showWnd();
            _m_guildMineToggle?.showWnd();
            
            refreshWnd();

            // 启动定时任务，每秒检查矿的有效期
            _m_checkExpireTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_checkMineExpire, 1f);
        }
        protected override void _onHideWnd()
        {
            _m_itemGrid?.hideWnd();
            _m_myMineToggle?.hideWnd();
            _m_guildMineToggle?.hideWnd();

            _m_refreshSerialize = ALSerializeOpMgr.next();

            // 停止定时任务
            _m_checkExpireTask.setDisable();
        }
        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
            _m_myMineToggle?.resetWnd();
            _m_guildMineToggle?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_itemGrid?.discard();
            _m_itemGrid = null;
            if (_m_myMineToggle != null)
            {
                _m_myMineToggle.clickDelegate -= _onMyMineTabClick;
                _m_myMineToggle.discard();
                _m_myMineToggle = null;
            }
            if (_m_guildMineToggle != null)
            {
                _m_guildMineToggle.clickDelegate -= _onGuildMineTabClick;
                _m_guildMineToggle?.discard();
                _m_guildMineToggle = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemGrid != null)
                _m_itemGrid = new GGUISubWndMarsExploreMineShareGrid(wnd.monoItemGrid);
            if (wnd.toggleMyMine != null)
            {
                _m_myMineToggle = new NPGGUIWndCommonTab(wnd.toggleMyMine);
                _m_myMineToggle.clickDelegate += _onMyMineTabClick;
            }
            if (wnd.toggleGuildMine != null)
            {
                _m_guildMineToggle = new NPGGUIWndCommonTab(wnd.toggleGuildMine);
                _m_guildMineToggle.clickDelegate += _onGuildMineTabClick;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        public void refreshWnd(EMineShareListType _type)
        {
            if (_m_currentListType == _type)
                return;

            _m_currentListType = _type;
            refreshWnd();
        }
        /// <summary>
        /// 刷新窗口，请求联盟矿分享列表
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDataCountDesc,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMinShareMaxCount_num, GRefdataCoreMgr.instance.npGeneral.mars_mine_guild_share_limit));

            // 生成新的刷新序列号
            int refreshSerialize = _m_refreshSerialize = ALSerializeOpMgr.next();
            bool hasGuild = NPPlayer.instance.guildComp.isJoinGuild();
            wnd.setLoading(true);
            wnd.setHasGuild(hasGuild || _m_currentListType == EMineShareListType.MyMine); // 如果页签是自己的矿也认为是有联盟（不显示加入联盟的提示）
            _m_itemGrid?.refreshWnd(null);
            _m_myMineToggle?.setSelected(_m_currentListType == EMineShareListType.MyMine);
            _m_guildMineToggle?.setSelected(_m_currentListType == EMineShareListType.GuildMine);
            _m_myMineViewDataList.Clear();
            _m_guildMineViewDataList.Clear();

            // 获取自己的矿信息
            NPPlayer.instance.marsComp.exploreSubComponent.doEachMine(_mineInfo =>
                _m_myMineViewDataList.Add(new MyMineShareItemViewData(_mineInfo, this)));
            
            // 如果有加入联盟就请求联盟的矿分享列表
            if (hasGuild)
            {
                // 请求联盟矿分享列表
                NPPlayer.instance.marsComp.exploreSubComponent.reqSharedMine((_mineList) =>
                {
                    // 检查序列号，确保是最新的请求
                    if (refreshSerialize != _m_refreshSerialize)
                        return;

                    // 获取分享列表
                    List<Guild_MineShareInfo> mineShareList = _mineList;

                    if (mineShareList != null)
                    {
                        // 为每个分享信息创建表现数据对象（此时不加载详细信息）
                        for (int i = mineShareList.Count - 1; i >= 0; i--)
                        {
                            Guild_MineShareInfo shareInfo = mineShareList[i];
                            if (shareInfo == null)
                                continue;
                            
                            if (shareInfo.getFinderCid() == NPPlayer.instance.playerInfo.CID)
                            {
                                long mineInstance = shareInfo.getMineInstanceId();
                                foreach (_AMineShareItemViewData myMineViewData in _m_myMineViewDataList)
                                {
                                    MyMineShareItemViewData forUnit = myMineViewData as MyMineShareItemViewData;
                                    if (forUnit == null)
                                        continue;
                                    
                                    if (forUnit.mineInstanceId == mineInstance)
                                    {
                                        forUnit.setShareInfo(shareInfo);
                                        break;
                                    }
                                }
                                continue;
                            }
                            
                            _m_guildMineViewDataList.Add(new GuildMineShareItemViewData(shareInfo, this));
                        }
                    }

                    // 刷新Grid，详细信息会在GridItem显示时按需加载
                    wnd.setLoading(false);
                    _m_itemGrid?.refreshWnd(_m_currentListType == EMineShareListType.MyMine ? _m_myMineViewDataList : _m_guildMineViewDataList);
                });
            }
            else
            {
                wnd.setLoading(false);
                _m_itemGrid?.refreshWnd(_m_currentListType == EMineShareListType.MyMine ? _m_myMineViewDataList : _m_guildMineViewDataList);
            }
        }
        public void refreshGrid()
        {
            if (_m_needRefreshGrid)
                return;

            _m_needRefreshGrid = true;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                _m_needRefreshGrid = false;
                if (wnd == null || !_m_bIsShow)
                    return;

                _m_itemGrid?.refreshWnd(_m_currentListType == EMineShareListType.MyMine ? _m_myMineViewDataList : _m_guildMineViewDataList);
            });
        }

        /// <summary>
        /// 检查矿的有效期，移除过期的矿
        /// </summary>
        private void _checkMineExpire()
        {
            long currentTime = FpsAndPingMgr.instance.serverTimeTag;
            byte hasExpired = 0; // 用位来表示哪个类型的矿有过期

            // 从后向前遍历，方便移除元素
            for (int i = _m_myMineViewDataList.Count - 1; i >= 0; i--)
            {
                _AMineShareItemViewData viewData = _m_myMineViewDataList[i];
                
                // 检查矿的有效期是否已过
                if (currentTime >= viewData.getMineEndShowMs())
                {
                    _m_myMineViewDataList.RemoveAt(i);
                    hasExpired |= 1 << (int)EMineShareListType.MyMine;
                }
            }
            for (int i = _m_guildMineViewDataList.Count - 1; i >= 0; i--)
            {
                _AMineShareItemViewData viewData = _m_guildMineViewDataList[i];

                // 检查矿的有效期是否已过
                if (currentTime >= viewData.getMineEndShowMs())
                {
                    _m_guildMineViewDataList.RemoveAt(i);
                    hasExpired |= 1 << (int)EMineShareListType.GuildMine;
                }
            }

            // 如果当前显示的类型有矿过期，刷新Grid
            if ((hasExpired & (1 << (int)_m_currentListType)) != 0)
                refreshGrid();
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_MINE_SHARE);
        }
        private void _onMyMineTabClick(bool _)
        {
            _setToggleType(EMineShareListType.MyMine);
        }
        private void _onGuildMineTabClick(bool _)
        {
            _setToggleType(EMineShareListType.GuildMine);
        }
        private void _setToggleType(EMineShareListType _type)
        {
            _m_currentListType = _type;
            _m_myMineToggle?.setSelected(_m_currentListType == EMineShareListType.MyMine);
            _m_guildMineToggle?.setSelected(_m_currentListType == EMineShareListType.GuildMine);
            if (wnd != null)
            {
                bool hasGuild = NPPlayer.instance.guildComp.isJoinGuild();
                wnd.setHasGuild(hasGuild || _m_currentListType == EMineShareListType.MyMine); // 如果页签是自己的矿也认为是有联盟（不显示加入联盟的提示）
            }
            refreshGrid();
        }


        public abstract class _AMineShareItemViewData
        {
            /// <summary>所属窗口引用</summary>
            [NotNull] private readonly GGUIWndMarsExploreMineShare _m_wnd;
            /// <summary>矿的动态数据（异步加载）</summary>
            private Mars_MineDynamic _m_mineDynamic;
            /// <summary>矿的配置数据（异步加载）</summary>
            private MarsExploreMineRefObj _m_mineRef;
            /// <summary>发现者的玩家简要信息（异步加载）</summary>
            private PlayerInfo_IconShow _m_finderInfo;
            /// <summary>占领者的玩家简要信息（异步加载）</summary>
            private PlayerInfo_IconShow _m_occupierInfo;
            /// <summary>是否被其他玩家攻击的标记</summary>
            private bool _m_hadAttackByOthersTag;
            /// <summary>加载序列号，用于控制异步请求的有效性</summary>
            private int _m_loadSerialize;
            /// <summary>是否正在加载中</summary>
            private bool _m_isLoading;
            /// <summary>是否已经开始加载（防止重复请求）</summary>
            private bool _m_isLoadStarted;
            /// <summary>是否已经加载完成</summary>
            private bool _m_isLoadCompleted;
            /// <summary>数据变化的回调列表（支持多个回调）</summary>
            private Action _m_onDataChangedCallbacks;
            /// <summary>待完成的异步加载请求数量</summary>
            private int _m_pendingLoadCount;
            
            
            protected _AMineShareItemViewData([NotNull] GGUIWndMarsExploreMineShare _wnd)
            {
                _m_wnd = _wnd;
                _m_isLoading = false;
                _m_isLoadStarted = false;
                _m_isLoadCompleted = false;
                _m_onDataChangedCallbacks = null;
                _m_pendingLoadCount = 0;
            }
            
            
            /// <summary>矿实分享数据</summary>
            public abstract Guild_MineShareInfo shareInfo { get; }
            /// <summary>矿的动态数据</summary>
            public Mars_MineDynamic mineDynamic { get { return _m_mineDynamic; } }
            /// <summary>矿的配置数据</summary>
            public MarsExploreMineRefObj mineRef { get { return _m_mineRef; } }
            /// <summary>发现者的玩家简要信息</summary>
            public PlayerInfo_IconShow finderInfo { get { return _m_finderInfo; } }
            /// <summary>占领者的玩家简要信息</summary>
            public PlayerInfo_IconShow occupierInfo { get { return _m_occupierInfo; } }
            /// <summary>是否被其他玩家攻击的标记</summary>
            public bool isAttackedByOthers { get { return _m_hadAttackByOthersTag; } }
            /// <summary>是否正在加载中</summary>
            public bool isLoading { get { return _m_isLoading; } }
            /// <summary>发现者是否为好友</summary>
            public bool isFriend { get { return _m_occupierInfo != null && NPPlayer.instance.friendsComp.isFriend(_m_occupierInfo.getCid()); } }
            /// <summary>发现者是否为同联盟成员</summary>
            public bool isAlliance
            {
                get
                {
                    if (_m_occupierInfo == null)
                        return false;
                    long guildId = _m_occupierInfo.getGuildId();
                    return NPPlayer.instance.guildComp.guildInfo is { guildId: long myGuildId } && myGuildId == guildId && guildId > 0;
                }
            }
            /// <summary>发现者是否为其他联盟成员</summary>
            public bool isOtherAlliance
            {
                get
                {
                    if (_m_occupierInfo == null)
                        return false;

                    long guildId = _m_occupierInfo.getGuildId();
                    return guildId > 0 && !isAlliance;
                }
            }
            public long remainNum
            {
                get
                {
                    if (_m_mineDynamic == null)
                        return 0;

                    long result = _m_mineDynamic.getRemainNum();
                    result = Mathf.FloorToInt(result - (FpsAndPingMgr.instance.serverTimeTag - _m_mineDynamic.getOccupiedMs()) * _m_mineDynamic.getCollectSpeed() / 1000f);

                    return Math.Max(0, result);
                }
            }
            
            
            /// <summary>
            /// 加载数据（按需加载，GridItem显示时调用）
            /// 如果已加载完成则直接调用回调，否则添加到等待队列
            /// </summary>
            /// <param name="_onDataChanged">数据加载完成的回调</param>
            public void loadData(Action _onDataChanged)
            {
                // 如果已经加载完成，直接调用回调
                if (_m_isLoadCompleted)
                {
                    _onDataChanged?.Invoke();
                    return;
                }

                // 添加回调到列表
                if (_onDataChanged != null)
                    _m_onDataChangedCallbacks += _onDataChanged;

                // 如果已经开始加载，直接返回（防止重复请求）
                if (_m_isLoadStarted)
                    return;

                // 标记为已开始加载
                _m_isLoadStarted = true;
                _m_isLoading = true;
                _loadData();
            }
            
            public abstract long getMineEndShowMs();
            public abstract void forwardCollectMine(long _teamId);


            protected abstract void _reqMineDynamicData(Action<bool, Mars_MineDynamic> _complete, Action<int> _failed);
            protected abstract long _getFinderCid();
            
            
            /// <summary>
            /// 加载矿的详细信息（私有方法）
            /// </summary>
            private void _loadData()
            {
                int loadSerialize = _m_loadSerialize = ALSerializeOpMgr.next();

                _m_pendingLoadCount++;
                
                // 请求矿的动态数据
                _reqMineDynamicData((_isSuc, _data) =>
                {
                    if (loadSerialize != _m_loadSerialize)
                        return;

                    if (_isSuc && _data != null)
                    {
                        _m_mineDynamic = _data;
                        if (_m_mineDynamic != null)
                        {
                            // 通过refId获取矿的配置数据
                            _m_mineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_mineDynamic.getRefId());
                            // 加载占领者信息（如果有）
                            _loadOccupierInfo(loadSerialize);
                        }

                        // 加载发现者信息
                        _loadFinderInfo(loadSerialize);

                        // 加载是否被其他玩家攻击的标记
                        _reqAttackByOthersFlag(loadSerialize);
                    }

                    _checkLoadComplete(loadSerialize);
                }, _errorCode =>
                {
                    if (loadSerialize != _m_loadSerialize)
                        return;

                    if (_errorCode == ErrorCodeConst.MARS_MINE_SHARE_EXPIRED)
                    {
                        if (_m_wnd._m_myMineViewDataList.Remove(this) && _m_wnd._m_currentListType == EMineShareListType.MyMine)
                            _m_wnd.refreshGrid();
                        if (_m_wnd._m_guildMineViewDataList.Remove(this) && _m_wnd._m_currentListType == EMineShareListType.GuildMine)
                            _m_wnd.refreshGrid();
                        return;
                    }
                        
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                });
            }
            /// <summary>
            /// 加载发现者的玩家简要信息
            /// </summary>
            private void _loadFinderInfo(int _loadSerialize)
            {
                // 增加待加载计数（无论同步还是异步都要计数）
                _m_pendingLoadCount++;

                long finderCid = _getFinderCid();
                // 如果是自己，直接使用本地数据
                if (finderCid == NPPlayer.instance.playerInfo.CID)
                {
                    _m_finderInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                    _checkLoadComplete(_loadSerialize);
                    return;
                }

                // 请求其他玩家的简要信息
                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(finderCid),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        if (_loadSerialize != _m_loadSerialize)
                            return;

                        if (_isSuc && _msg != null)
                        {
                            _m_finderInfo = _msg.getPlayerBrief();
                        }

                        _checkLoadComplete(_loadSerialize);
                    }));
            }
            /// <summary>
            /// 加载占领者的玩家简要信息
            /// </summary>
            private void _loadOccupierInfo(int _loadSerialize)
            {
                if (_m_mineDynamic == null)
                    return;

                long occupierCid = _m_mineDynamic.getOccupiedCid();
                // 如果没有占领者，直接返回
                if (occupierCid == 0)
                    return;

                // 增加待加载计数（无论同步还是异步都要计数）
                _m_pendingLoadCount++;

                // 如果占领者是自己，直接使用本地数据
                if (occupierCid == NPPlayer.instance.playerInfo.CID)
                {
                    _m_occupierInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                    _checkLoadComplete(_loadSerialize);
                    return;
                }

                // 请求占领者的简要信息
                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(occupierCid),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                    {
                        if (_loadSerialize != _m_loadSerialize)
                            return;

                        if (_isSuc && _msg != null)
                        {
                            _m_occupierInfo = _msg.getPlayerBrief();
                        }

                        _checkLoadComplete(_loadSerialize);
                    }));
            }
            private void _reqAttackByOthersFlag(int _loadSerialize)
            {
                if (shareInfo == null)
                    return;

                // 增加待加载计数（无论同步还是异步都要计数）
                _m_pendingLoadCount++;

                NPGSClientListener.sendRequestByLog(new GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag(shareInfo.getId()),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag>((_isSuc, _msg) =>
                    {
                        if (_loadSerialize != _m_loadSerialize)
                            return;

                        _m_hadAttackByOthersTag = _msg?.getHadAttackByOthersTag() ?? false;
                        _checkLoadComplete(_loadSerialize);
                    }));
            }
            /// <summary>
            /// 检查加载是否完成（每个异步请求完成后调用）
            /// </summary>
            private void _checkLoadComplete(int _loadSerialize)
            {
                if (_loadSerialize != _m_loadSerialize)
                    return;

                // 减少待加载计数
                _m_pendingLoadCount--;

                // 只有当所有异步请求都完成时，才标记为加载完成
                if (_m_pendingLoadCount <= 0)
                {
                    _m_isLoading = false;
                    _m_isLoadCompleted = true;
                    _notifyDataChanged();
                }
            }
            /// <summary>
            /// 通知所有已注册的回调，数据已更新
            /// </summary>
            private void _notifyDataChanged()
            {
                Action callback = _m_onDataChangedCallbacks;
                callback?.Invoke();
                // 清空回调列表，因为已经通知过了
                _m_onDataChangedCallbacks = null;
            }
        }
        /// <summary>
        /// 矿分享列表项的表现数据类
        /// 负责按需异步加载矿的详细信息和玩家信息
        /// </summary>
        public class GuildMineShareItemViewData : _AMineShareItemViewData
        {
            /// <summary>原始的分享信息（来自服务端）</summary>
            [NotNull] private readonly Guild_MineShareInfo _m_shareInfo;


            public GuildMineShareItemViewData([NotNull] Guild_MineShareInfo _shareInfo, [NotNull] GGUIWndMarsExploreMineShare _wnd)
                : base(_wnd)
            {
                _m_shareInfo = _shareInfo;
            }


            /// <summary>该矿是否为自己的矿</summary>
            public bool isMyMine { get { return _m_shareInfo.getFinderCid() == NPPlayer.instance.playerInfo.CID; } }
            /// <summary>原始的分享信息</summary>
            [NotNull] public override Guild_MineShareInfo shareInfo { get { return _m_shareInfo; } }


            
            public override void forwardCollectMine(long _teamId)
            {
                long shareMineDbId = _m_shareInfo.getId();
                NPGSClientListener.sendRequestByLog(new GC2GS_041_018_ReqGuildMateForwardCollectMine(shareMineDbId, _teamId, false, false),
                    new CommonRequestCallbackProtocolDealer<GS2GC_041_018_RetGuildMateForwardCollectMine>((_retMsg) =>
                    {
                    }, (_errCode) =>
                    {
                        string tips = string.Empty;
                        bool needShowConfirm = false;
                        if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_OCCUPY)
                        {
                            tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerOccupyTip_none);
                            needShowConfirm = true;
                        }
                        else if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_FORWARD)
                        {
                            tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerForwardTip_none);
                            needShowConfirm = true;
                        }

                        if (needShowConfirm)
                        {
                            NPMesMgr.instance.showTwoBtnMes(tips,
                                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                                null,
                                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                                () => NPGSClientListener.sendMsgByLog(new GC2GS_041_018_ReqGuildMateForwardCollectMine(shareMineDbId, _teamId, true, true)));
                        }
                    }));
            }
            public override long getMineEndShowMs()
            {
                return _m_shareInfo.getMineEndShowMs();
            }
            protected override void _reqMineDynamicData(Action<bool, Mars_MineDynamic> _complete, Action<int> _failed)
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_041_019_ReqGuildShareMineInfo(_m_shareInfo.getId()),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_019_RetGuildShareMineInfo>((_isSuc, _msg) =>
                            _complete?.Invoke(_isSuc, _msg?.getInfo())
                        , _failed, false));
            }
            protected override long _getFinderCid()
            {
                return _m_shareInfo.getFinderCid();
            }
        }
        public class MyMineShareItemViewData : _AMineShareItemViewData
        {
            /// <summary>原始的矿信息（来自本地数据）</summary>
            [NotNull] private readonly MarsExploreMineInfo _m_mineInfo;
            /// <summary>分享信息（来自服务端）</summary>
            private Guild_MineShareInfo _m_shareInfo;
            
            
            public MyMineShareItemViewData([NotNull] MarsExploreMineInfo _mineInfo, [NotNull] GGUIWndMarsExploreMineShare _wnd) 
                : base(_wnd)
            {
                _m_mineInfo = _mineInfo;
            }
            
            
            /// <summary>矿实例ID</summary>
            public long mineInstanceId { get { return _m_mineInfo.instanceId; } }
            /// <summary>原始的分享信息</summary>
            public override Guild_MineShareInfo shareInfo { get { return _m_shareInfo; } }


            public void setShareInfo(Guild_MineShareInfo _shareInfo)
            {
                _m_shareInfo = _shareInfo;
            }
            public override long getMineEndShowMs()
            {
                return _m_mineInfo.endTime;
            }
            public override void forwardCollectMine(long _teamId)
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, _m_mineInfo.instanceId, false, false), 
                    new CommonRequestCallbackProtocolDealer<GS2GC_041_010_RetForwardCollectMine>((_retMsg) => {
                            
                    }, (_errCode) =>
                    {
                        string tips = string.Empty;
                        bool needShowConfirm = false;
                        if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_OCCUPY)
                        {
                            tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerOccupyTip_none);
                            needShowConfirm = true;
                        }
                        else if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_FORWARD)
                        {
                            tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerForwardTip_none);
                            needShowConfirm = true;
                        }

                        if (needShowConfirm)
                        {
                            NPMesMgr.instance.showTwoBtnMes(tips,
                                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                                null,
                                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                                () => NPGSClientListener.sendMsgByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, _m_mineInfo.instanceId, true, true)));
                        }
                    }));
            }
            protected override void _reqMineDynamicData(Action<bool, Mars_MineDynamic> _complete, Action<int> _failed)
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_041_011_ReqMarsMineInfo(_m_mineInfo.instanceId),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_011_RetMarsMineInfo>((_isSuc, _msg) =>
                            _complete?.Invoke(_isSuc, _msg?.getInfo())
                        , _failed, false));
            }
            protected override long _getFinderCid()
            {
                return NPPlayer.instance.playerInfo.CID;
            }
        }
    }
}
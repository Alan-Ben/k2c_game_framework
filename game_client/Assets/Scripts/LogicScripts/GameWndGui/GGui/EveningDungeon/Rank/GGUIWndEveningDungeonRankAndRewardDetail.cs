using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜和奖励详情窗口
    /// </summary>
    public class GGUIWndEveningDungeonRankAndRewardDetail : _ANPGGUIBasicResBarWnd<GGUIMonoEveningDungeonRankAndRewardDetail>
    {
        private static GGUIWndEveningDungeonRankAndRewardDetail _g_instance;
        public static GGUIWndEveningDungeonRankAndRewardDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndEveningDungeonRankAndRewardDetail();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndEveningDungeonRankAndRewardDetailTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndEveningDungeonRankAndRewardDetailTab _m_wSelectTabWnd;
        //排行详情前几名信息附加窗口
        private List<GGUIWndEveningDungeonRankTopPlayerInfo> _m_lTopPlayerInfoList;
        //排行榜列表
        private List<EveningDungeonRankInfo> _m_lRankShowInfoList;
        // 自己排行数据
        private Common.RankObj.Rank_BaseItem _m_SelfRankShowInfo;
        //显示序列号
        private long _m_lShowSerialize;
        //奖励页面
        private GGUIWndEveningDungeonRankRewardPage _m_wRewardPage;
        //排行榜页面
        private GGUIWndEveningDungeonRankPage _m_wRankPage;

        public GGUIWndEveningDungeonRankAndRewardDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonRankAndRewardDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonRankAndRewardDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_SEC_TICK, _onSecTick);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_SEC_TICK, _onSecTick);

            _hideAllPage();
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndEveningDungeonRankTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.hideWnd();
                }
            }

            _m_SelfRankShowInfo = null;
            
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndEveningDungeonRankAndRewardDetailTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }

            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndEveningDungeonRankTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.resetWnd();
                }
            }

            _m_wSelectTabWnd = null;

            _m_wRewardPage?.resetWnd();
            _m_wRankPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndEveningDungeonRankAndRewardDetailTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndEveningDungeonRankTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.discard();
                }
                _m_lTopPlayerInfoList.Clear();
            }

            _m_wRewardPage?.discard();
            _m_wRewardPage = null;
            _m_wRankPage?.discard();
            _m_wRankPage = null;

            _m_wSelectTabWnd = null;

            _m_lRankShowInfoList?.Clear();
            _m_lRankShowInfoList = null;
            _m_SelfRankShowInfo = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndEveningDungeonRankAndRewardDetailTab>();
            if (null != wnd.monoTabList)
            {
                GGUIEveningDungeonRankAndRewardDetailTabMono tempTabMono = null;
                GGUIWndEveningDungeonRankAndRewardDetailTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndEveningDungeonRankAndRewardDetailTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            _m_lTopPlayerInfoList = new List<GGUIWndEveningDungeonRankTopPlayerInfo>();
            if (wnd.monoTopPlayerList != null)
            {
                for (int i = 0; i < wnd.monoTopPlayerList.Count; i++)
                {
                    _m_lTopPlayerInfoList.Add(new GGUIWndEveningDungeonRankTopPlayerInfo(wnd.monoTopPlayerList[i]));
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_selectTabType"></param>
        public void setInfo(EEveningDungeonRankAndRewardDetailTabType _selectTabType)
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //设置页签
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;
            foreach (GGUIWndEveningDungeonRankAndRewardDetailTab itemTab in _m_lTabWndList)
            {
                if (itemTab?.tabType == _selectTabType)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }

            //刷新窗口
            _refreshRank(true, true);
            _refreshActivityStateShow();
            _onCDRefresh();
        }

        /// <summary>
        /// 获取当前选中页签类型
        /// </summary>
        /// <returns></returns>
        public EEveningDungeonRankAndRewardDetailTabType getCurSelectTabType()
        {
            if (_m_wSelectTabWnd != null)
                return _m_wSelectTabWnd.tabType;
            else
                return EEveningDungeonRankAndRewardDetailTabType.REWARD;
        }

        /// <summary>
        /// 刷新排行相关数据
        /// </summary>
        /// <param name="_needRefreshBeforeReqData">是否需要在请求数据前先刷新一次窗口</param>
        /// <param name="_needResetRankInfo">是否需要重置排行数据</param>
        private void _refreshRank(bool _needRefreshBeforeReqData, bool _needResetRankInfo)
        {
            if (_needRefreshBeforeReqData)
            {
                _refreshTopPlayer();
                _refreshSelfInfo();
                _refreshTabPageShow();
            }
            
            List<EveningDungeonRankInfo> oldRankInfoList = _m_lRankShowInfoList;
            long serialize = _m_lShowSerialize;

            //请求排行数据
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonRankList((_msg) =>
            {
                if (wnd == null || !isShow || _m_lShowSerialize != serialize)
                    return;
                
                _m_lRankShowInfoList = new List<EveningDungeonRankInfo>();
                EveningDungeonRankInfo rankInfo = null;
                Common.RankObj.Rank_BaseItem rankBaseItem = null;

                if (_msg != null && _msg.getRankList() != null)
                {
                    for (int i = 0; i < _msg.getRankList().Count; i++)
                    {
                        rankBaseItem = _msg.getRankList()[i];
                        if(rankBaseItem == null)
                            continue;

                        if (_needResetRankInfo)
                        {
                            rankInfo = oldRankInfoList?.SafeGet(i);
                            rankInfo?.update(rankBaseItem);
                        }
                        else
                        {
                            rankInfo = oldRankInfoList?.FindAndRemove((_info) =>
                            {
                                if (_info != null && _info.cid == rankBaseItem.getKey())
                                    return true;

                                return false;
                            });
                            rankInfo?.updateRankInfo(rankBaseItem.getRank(), rankBaseItem.getScore());
                        }

                        if (rankInfo == null)
                            rankInfo = new EveningDungeonRankInfo(rankBaseItem);
                        
                        _m_lRankShowInfoList.Add(rankInfo);
                    }
                }

                _m_lRankShowInfoList.Sort((_a,_b)=>_a.rankSortId.CompareTo(_b.rankSortId));
                _m_SelfRankShowInfo = _msg?.getSelfRankItem();
                
                _refreshTopPlayer();
                _refreshSelfInfo();
                _refreshTabPageShow();
            });
        }

        //刷新前几名玩家
        private void _refreshTopPlayer()
        {
            if (wnd == null || _m_lTopPlayerInfoList == null)
                return;

            for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
            {
                _m_lTopPlayerInfoList[i]?.showWnd();
                _m_lTopPlayerInfoList[i]?.setInfo(_m_lRankShowInfoList?.SafeGet(i));
            }
        }

        //刷新自己的排名信息
        private void _refreshSelfInfo()
        {
            if (wnd == null)
                return;

            int selfRank = _m_SelfRankShowInfo?.getRank() ?? 0;
            //设置排名
            if (selfRank > 0)
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myRank_str, selfRank));
            else
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myRank_str, TransKeyConst.eveningDungeon_notOnTheRankingList_none));

            //设置分数
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myScore_str_num, 
                GCommon.getValueFormatStr(EValueFormatType.NORMAL, _m_SelfRankShowInfo?.getScore() ?? 0)));
        }

        /// <summary>
        /// 刷新不同活动状态显示物体
        /// </summary>
        private void _refreshActivityStateShow()
        {
            if(wnd == null || wnd.showStateList == null)
                return;
            
            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            GGUIEveningDungeonActivityStateShow stateShow = null;
            foreach (var item in wnd.showStateList)
            {
                if(item == null)
                    continue;

                if (item.activityState != activityState)
                    ALUGUICommon.setGameObjEnable(item.goShowList, false);
                else
                    stateShow = item;
            }
            if(stateShow != null)
                ALUGUICommon.setGameObjEnable(stateShow.goShowList, true);
        }
        
        //倒计时刷新
        private void _onCDRefresh()
        {
            if (wnd == null)
                return;

            //当前状态结束时间
            long curStateFinishTimeMs = 0;
            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            //当前状态文本颜色
            Color curTextColor = wnd.getCDTextColor(activityState);
            //翻译key
            string transKey = null;

            switch (activityState)
            {
                case EEveningDungeonActivityState.ONGOING:
                    curStateFinishTimeMs = NPPlayer.instance.eveningDungeonComp.endTimeMs;
                    transKey = TransKeyConst.eveningDungeon_playing_str;
                    break;
                case EEveningDungeonActivityState.END:
                    curStateFinishTimeMs = NPPlayer.instance.eveningDungeonComp.preCloseTimeMs;
                    transKey = TransKeyConst.eveningDungeon_settling_str;
                    break;
                // case EEveningDungeonActivityState.CLOSE:
                //     curStateFinishTimeMs = NPPlayer.instance.eveningDungeonComp.previewTimeMs;
                //     transKey = TransKeyConst.eveningDungeon_preparing_str;
                //     break;
                case EEveningDungeonActivityState.PREVIEW:
                    curStateFinishTimeMs = NPPlayer.instance.eveningDungeonComp.startTimeMs;
                    transKey = TransKeyConst.eveningDungeon_preparing_str;
                    break;
            }

            long leftTimeMs = curStateFinishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            ALUGUICommon.setLabelTxt(wnd.txtCD, GCommon.addColorForRichText(TextTranslate.instance.getLanguage(transKey, TimeUtil.millisecondsToTime_hms(leftTimeMs)), curTextColor));
        }


        #region 页签处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndEveningDungeonRankAndRewardDetailTab _tabItemWnd)
        {
            if (wnd == null || wnd.monoTabList == null || null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            //设置显隐
            for (int i = 0; i < wnd.monoTabList.Count; i++)
            {
                if (wnd.monoTabList[i].tabType == _m_wSelectTabWnd.tabType)
                {
                    ALUGUICommon.setGameObjEnable(wnd.monoTabList[i].goClickHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.monoTabList[i].goClickShowList, true);
                    break;
                }
            }

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EEveningDungeonRankAndRewardDetailTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EEveningDungeonRankAndRewardDetailTabType.REWARD:
                    _showRewardPage();
                    break;
                case EEveningDungeonRankAndRewardDetailTabType.RANK:
                    _showRankPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wRewardPage?.hideWnd();
            _m_wRankPage?.hideWnd();
        }

        //显示奖励页面
        private void _showRewardPage()
        {
            if (wnd == null)
                return;
            
            if (_m_wRewardPage != null)
            {
                _m_wRewardPage.showWnd();
                _m_wRewardPage.setInfo(_m_SelfRankShowInfo?.getRank() ?? 0);
            }
            else
            {
                _m_wRewardPage = new GGUIWndEveningDungeonRankRewardPage(_getPageAssetPathByType(EEveningDungeonRankAndRewardDetailTabType.REWARD), wnd.pageParent);
                _m_wRewardPage.load(() =>
                {
                    if (_m_wRewardPage == null)
                        return;

                    _m_wRewardPage.showWnd();
                    _m_wRewardPage.setInfo(_m_SelfRankShowInfo?.getRank() ?? 0);
                });
            }
        }

        //显示排行榜页面
        private void _showRankPage()
        {
            if (wnd == null)
                return;

            if (_m_wRankPage != null)
            {
                _m_wRankPage.showWnd();
                _m_wRankPage.setInfo(_m_lRankShowInfoList);
            }
            else
            {
                _m_wRankPage = new GGUIWndEveningDungeonRankPage(_getPageAssetPathByType(EEveningDungeonRankAndRewardDetailTabType.RANK), wnd.pageParent);
                _m_wRankPage.load(() =>
                {
                    if (_m_wRankPage == null)
                        return;

                    _m_wRankPage.showWnd();
                    _m_wRankPage.setInfo(_m_lRankShowInfoList);
                });
            }
        }

        private void _refreshTabPageShow()
        {
            if(_m_wRankPage != null && _m_wRankPage.isShow)
                _m_wRankPage.setInfo(_m_lRankShowInfoList);
            
            if(_m_wRewardPage != null && _m_wRewardPage.isShow)
                _m_wRewardPage.setSelfRank(_m_SelfRankShowInfo?.getRank() ?? 0);
        }
        
        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EEveningDungeonRankAndRewardDetailTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return null;

            foreach (GGUIEveningDungeonRankAndRewardDetailTabMono mono in wnd.monoTabList)
            {
                if (mono != null && mono.tabType == _type)
                    return mono.tabAssetPathInfo;
            }

            return null;
        }

        #endregion

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_RANK_AND_REWARD_DETAIL);
        }

        #endregion

        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            _refreshRank(false, false);
            _refreshActivityStateShow();
        }

        private void _onSecTick()
        {
            _onCDRefresh();
        }

        #endregion
    }
}
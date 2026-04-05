using ALPackage;
using System.Collections.Generic;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜详情主界面
    /// </summary>
    public abstract class _AGGUIWndRankRushDetailBase<T> : _ANPGGUIBasicResBarWnd<T> where T: GGUIMonoRankRushDetailBase
    {
        //冲榜信息
        private ActivityRankRushInfo _m_rankRushInfo;
        //页签列表
        private List<GGUIWndRankRushDetailTab> _m_lTabWndList;
        //当前选中的页签
        protected GGUIWndRankRushDetailTab _m_wSelectTabWnd;
        //冲榜详情前几名信息附加窗口
        private List<GGUIWndRankRushTopPlayerInfo> _m_lTopPlayerInfoList;
        //排行榜列表
        private List<NPRankCommonShowInfo> _m_lRankShowInfoList;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        //显示序列号
        private long _m_lShowSerialize;
        //奖励页面
        private GGUIWndRankRushDetailRewardPage _m_wRewardPage;
        //排行榜页面
        private GGUIWndRankRushDetailRankPage _m_wRankPage;
        //自己的排名
        private long _m_lSelfRank;
        //是否需要展示礼包按钮
        private bool _m_bNeedShowGiftBtn;

        public _AGGUIWndRankRushDetailBase() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);

            _hideAllPage();
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankRushTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.hideWnd();
                }
            }

            _m_tcTickTaskController.setDisable();

            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lSelfRank = 0;
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndRankRushDetailTab tempTabItem = null;
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
                foreach (GGUIWndRankRushTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
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
                GGUIWndRankRushDetailTab tempTabItem = null;
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
                foreach (GGUIWndRankRushTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.discard();
                }
                _m_lTopPlayerInfoList.Clear();
            }

            _m_wRewardPage?.discard();
            _m_wRewardPage = null;
            _m_wRankPage?.discard();
            _m_wRankPage = null;

            _m_tcTickTaskController.setDisable();

            _m_wSelectTabWnd = null;

            resetRankList();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnServerListDetail, _onClickServerListDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnRankTip, _onClickRankingTip);
            ALUGUICommon.uncombineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGift, _onClickGift);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndRankRushDetailTab>();
            if (null != wnd.monoTabList)
            {
                GGUIRankRushDetailTabMono tempTabMono = null;
                GGUIWndRankRushDetailTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndRankRushDetailTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            _m_lTopPlayerInfoList = new List<GGUIWndRankRushTopPlayerInfo>();
            if (wnd.monoTopPlayerList != null)
            {
                for (int i = 0; i < wnd.monoTopPlayerList.Count; i++)
                {
                    _m_lTopPlayerInfoList.Add(new GGUIWndRankRushTopPlayerInfo(wnd.monoTopPlayerList[i]));
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnServerListDetail, _onClickServerListDetail);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnRankTip, _onClickRankingTip);
            ALUGUICommon.combineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGift, _onClickGift);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_selectTabType"></param>
        public void setInfo(ActivityRankRushInfo _info, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn)
        {
            _m_rankRushInfo = _info;
            _m_bNeedShowGiftBtn = _needShowGiftBtn;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lSelfRank = 0;

            //设置已读
            _m_rankRushInfo?.setIsReadNew();

            //设置页签
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;
            foreach (GGUIWndRankRushDetailTab itemTab in _m_lTabWndList)
            {
                if (itemTab?.tabType == _selectTabType)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }

            //设置倒计时任务
            _m_tcTickTaskController.setDisable();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onCDRefresh, 1f);

            //刷新窗口
            _refreshWnd();
        }

        /// <summary>
        /// 获取当前选中页签类型
        /// </summary>
        /// <returns></returns>
        public ERankRushDetailTabType getCurSelectTabType()
        {
            if (_m_wSelectTabWnd != null)
                return _m_wSelectTabWnd.tabType;
            else
                return ERankRushDetailTabType.REWARD;
        }

        /// <summary>
        /// 重置排行榜数据
        /// </summary>
        public void resetRankList()
        {
            _m_lRankShowInfoList?.Clear();
            _m_lRankShowInfoList = null;
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshCrossServer();
            _refreshTopPlayer();
            _refreshSelfInfo();
            _refreshGiftBtnState();
        }

        //刷新跨服相关展示
        private void _refreshCrossServer()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null)
                return;

            //先置空显示
            ALUGUICommon.setLabelTxt(wnd.txtServerList, "");

            //如果是跨服活动，显示区服列表
            if (_m_rankRushInfo.activityInfo.isCross)
            {
                //获取区服名称列表
                List<string> nameList = new List<string>();
                GCommon.getServerNameListByServerIdList(_m_rankRushInfo.activityInfo.usIdList, _list =>
                {
                    if (_list == null || _list.Count == 0)
                        return;

                    nameList.AddRange(_list);
                    //设置显示
                    ALUGUICommon.setLabelTxt(wnd.txtServerList, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_serverList_str, string.Join("、", nameList.ToArray())));
                });
            }

            //设置跨服活动相关显示
            ALUGUICommon.setGameObjEnable(wnd.goCrossShowList, _m_rankRushInfo.activityInfo.isCross);
            ALUGUICommon.setGameObjEnable(wnd.goCrossHideList, !_m_rankRushInfo.activityInfo.isCross);
        }

        //刷新前几名玩家
        private void _refreshTopPlayer()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_lTopPlayerInfoList == null)
                return;

            //先重置显示
            for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
            {
                _m_lTopPlayerInfoList[i]?.showWnd();
                _m_lTopPlayerInfoList[i]?.setInfo(null);
            }

            //判断是否有数据，没有的话请求一下数据
            if (_m_lRankShowInfoList == null || _m_lRankShowInfoList.Count == 0)
            {
                long serialize = _m_lShowSerialize;
                NPPlayer.instance.commonActivityComp.reqActivityRankBaseList(_m_rankRushInfo.activityInfo.instanceId, _m_rankRushInfo.activityRankRushRefObj.rank_id, _m_rankRushInfo.activityInfo.isCross,
                    _msg =>
                    {
                        if (_msg == null || wnd == null || !isShow || _m_lShowSerialize != serialize)
                            return;

                        if (_m_lRankShowInfoList == null)
                            _m_lRankShowInfoList = new List<NPRankCommonShowInfo>();
                        _m_lRankShowInfoList.Clear();

                        for (int i = 0; i < _msg.getBaseItemlist().Count; i++)
                        {
                            NPRankCommonShowInfo rankCommonShowInfo = new NPRankCommonShowInfo(_msg.getBaseItemlist()[i], _m_rankRushInfo.activityInfo.isCross);
                            rankCommonShowInfo.setRankId(_m_rankRushInfo.activityRankRushRefObj.rank_id, 0);
                            _m_lRankShowInfoList.Add(rankCommonShowInfo);
                            _m_lRankShowInfoList.Sort((_a,_b)=>_a.rankSortId.CompareTo(_b.rankSortId));
                        }

                        for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
                        {
                            _m_lTopPlayerInfoList[i]?.showWnd();
                            _m_lTopPlayerInfoList[i]?.setInfo(_m_lRankShowInfoList.Count > i ? _m_lRankShowInfoList[i] : null);
                        }

                        //如果正在排行榜列表 刷新一下
                        if (_m_wSelectTabWnd != null && _m_wSelectTabWnd.tabType == ERankRushDetailTabType.RANK)
                            _showRankPage();
                    });
            }
            else
            {
                for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
                {
                    _m_lTopPlayerInfoList[i]?.showWnd();
                    _m_lTopPlayerInfoList[i]?.setInfo(_m_lRankShowInfoList.Count > i ? _m_lRankShowInfoList[i] : null);
                }
            }

            //冲榜名称
            ALUGUICommon.setLabelTxt(wnd.txtRankRushName, TextTranslate.instance.getLanguage(_m_rankRushInfo.rankRefObj?.nameStr));
            //获取途径按钮，没配置不显示
            bool showAccessBtn = _m_rankRushInfo.activityRankRushRefObj.access_id_list != null && _m_rankRushInfo.activityRankRushRefObj.access_id_list.Count > 0;
            ALUGUICommon.setGameObjEnable(wnd.btnAccess, showAccessBtn);
        }

        //刷新自己的排名信息
        private void _refreshSelfInfo()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_lTopPlayerInfoList == null || _m_rankRushInfo.rankRefObj == null)
                return;

            long serialize = _m_lShowSerialize;

            //设置默认显示状态
            ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myRank_str, ""));
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myScore_str_num, _m_rankRushInfo.rankRefObj.score_name, ""));
            ALUGUICommon.setGameObjEnable(wnd.btnRankTip, !string.IsNullOrEmpty(_m_rankRushInfo.activityRankRushRefObj.ranking_tip));
            ALUGUICommon.setLabelTxt(wnd.txtInitialValue, "");
            ALUGUICommon.setGameObjEnable(wnd.goNoInititalValueHideList, _m_rankRushInfo.activityRankRushRefObj.haveMaxRecordValue);
            NPCommonGetStatInfo.setStat(wnd.getRewardStateInfoList, ENPCommonGetStat.CAN_NOT_GET);

            //根据状态显示不同信息
            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAYING:
                case EActivityState.SETTLING:
                    //请求自己的排名信息
                    NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(_m_rankRushInfo.activityInfo.instanceId, _m_rankRushInfo.activityRankRushRefObj.rank_id, NPPlayer.instance.playerInfo.CID, _m_rankRushInfo.activityInfo.isCross,
                        _msg =>
                        {
                            if (_msg == null || _msg.getBaseItem() == null || wnd == null || !isShow || _m_lShowSerialize != serialize)
                                return;

                            _setSelfShowInfo(_msg.getBaseItem().getRank(), _msg.getBaseItem().getScore(), false);
                        });
                    break;
                case EActivityState.REWARDING:
                    _m_rankRushInfo.getSettleInfo(_settleInfo =>
                    {
                        if (wnd == null || !isShow || _m_lShowSerialize != serialize)
                            return;

                        if(_settleInfo != null)
                            _setSelfShowInfo(_settleInfo.getRank(), _settleInfo.getScore(), _settleInfo.getHadDraw());
                        else
                            _setSelfShowInfo(0, 0, false);
                    });
                    break;
            }
        }

        //刷新礼包按钮状态
        private void _refreshGiftBtnState()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            //是否有配置礼包
            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_rankRushInfo.activityInfo.activityId);
            bool haveGiftPack = activityMainRef != null && 
                                ((activityMainRef.cash_gift_pack_group_id_list != null && activityMainRef.cash_gift_pack_group_id_list.Count > 0) || activityMainRef.crystal_gift_pack_group_id > 0);

            //根据状态显示不同信息
            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAYING:
                    ALUGUICommon.setGameObjEnable(wnd.btnGift, _m_bNeedShowGiftBtn && haveGiftPack);
                    break;
                default:
                    ALUGUICommon.setGameObjEnable(wnd.btnGift, false);
                    //不是在进行中状态，关闭礼包界面
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_GIFT_PACK);
                    break;
            }
        }

        //设置自己显示信息
        private void _setSelfShowInfo(long _selfRank, long _score, bool _hadDraw)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.rankRefObj == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            //记录自己的排名
            _m_lSelfRank = _selfRank;

            //设置排名
            if (_selfRank > 0)
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myRank_str, _selfRank));
            else
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myRank_str, TransKeyConst.rankRush_notOnTheRankingList_none));

            //设置分数
            string scoreStr = GCommon.getValueFormatStr(_m_rankRushInfo.rankRefObj.process_num_format, _score);
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myScore_str_num, _m_rankRushInfo.rankRefObj.score_name, scoreStr));

            //设置按钮状态
            ENPCommonGetStat rewardState = ENPCommonGetStat.NONE;
            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAN:
                case EActivityState.PLAYING:
                case EActivityState.SETTLING:
                    rewardState = ENPCommonGetStat.CAN_NOT_GET;
                    break;
                case EActivityState.REWARDING:
                    if (_selfRank > 0)
                    {
                        if (_hadDraw)
                            rewardState = ENPCommonGetStat.HAS_GET;
                        else
                            rewardState = ENPCommonGetStat.CAN_GET;
                    }
                    else
                        rewardState = ENPCommonGetStat.CAN_NOT_GET;
                    break;
                default:
                    rewardState = ENPCommonGetStat.CAN_NOT_GET;
                    break;
            }
            NPCommonGetStatInfo.setStat(wnd.getRewardStateInfoList, rewardState);

            //设置冲榜初始值
            if (_m_rankRushInfo.activityRankRushRefObj.haveMaxRecordValue)
            {
                long initialValue = NPPlayer.instance.commonActivityComp.getRankRushRecordInitialValue(_m_rankRushInfo, _score);
                string initialValueStr = GCommon.getValueFormatStr(_m_rankRushInfo.rankRefObj.process_num_format, initialValue);
                ALUGUICommon.setLabelTxt(wnd.txtInitialValue, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_myScore_str_num, _m_rankRushInfo.activityRankRushRefObj.initial_value_desc, initialValueStr));
            }

            //刷新奖励显示状态
            _refreshRewardPageBySelfRank(_selfRank);
        }

        //倒计时刷新
        private void _onCDRefresh()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            //当前状态结束时间
            long curStateFinishTimeMs = 0;
            //当前状态文本颜色
            Color curTextColor = wnd.getCDTextColor(_m_rankRushInfo.activityInfo.activityState);
            //翻译key
            string transKey = null;

            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAYING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.endTimeMs;
                    transKey = TransKeyConst.rankRush_detailShowPlaying_str;
                    break;
                case EActivityState.SETTLING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.settleTimeMs;
                    transKey = TransKeyConst.rankRush_detailShowSettling_str;
                    break;
                case EActivityState.REWARDING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.closeTimeMs;
                    transKey = TransKeyConst.rankRush_detailShowGettingReward_str;
                    break;
            }

            long leftTimeMs = curStateFinishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            ALUGUICommon.setLabelTxt(wnd.txtCD, GCommon.addColorForRichText(TextTranslate.instance.getLanguage(transKey, TimeUtil.millisecondsToTime_dhms(leftTimeMs)), curTextColor));
        }


        #region 页签处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndRankRushDetailTab _tabItemWnd)
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
        private void _refreshTabView(ERankRushDetailTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case ERankRushDetailTabType.REWARD:
                    _showRewardPage();
                    break;
                case ERankRushDetailTabType.RANK:
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
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null)
                return;
            
            if (_m_wRewardPage != null)
            {
                _m_wRewardPage.showWnd();
                _m_wRewardPage.setInfo(_m_rankRushInfo.activityRankRushRefObj.rank_id, _m_lSelfRank);
            }
            else
            {
                _m_wRewardPage = new GGUIWndRankRushDetailRewardPage(_getPageAssetPathByType(ERankRushDetailTabType.REWARD), wnd.pageParent);
                _m_wRewardPage.load(() =>
                {
                    if (_m_wRewardPage == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null)
                        return;

                    _m_wRewardPage.showWnd();
                    _m_wRewardPage.setInfo(_m_rankRushInfo.activityRankRushRefObj.rank_id, _m_lSelfRank);
                });
            }
        }

        //显示排行榜页面
        private void _showRankPage()
        {
            if (wnd == null || _m_rankRushInfo == null)
                return;

            if (_m_wRankPage != null)
            {
                _m_wRankPage.showWnd();
                _m_wRankPage.setInfo(_m_rankRushInfo.activityRankRushRefObj, _m_lRankShowInfoList);
            }
            else
            {
                _m_wRankPage = new GGUIWndRankRushDetailRankPage(_getPageAssetPathByType(ERankRushDetailTabType.RANK), wnd.pageParent);
                _m_wRankPage.load(() =>
                {
                    if (_m_wRankPage == null || _m_rankRushInfo == null)
                        return;

                    _m_wRankPage.showWnd();
                    _m_wRankPage.setInfo(_m_rankRushInfo.activityRankRushRefObj, _m_lRankShowInfoList);
                });
            }
        }

        //根据自己排名刷新奖励显示状态
        private void _refreshRewardPageBySelfRank(long _rank)
        {
            if(_m_wRewardPage != null && _m_wRewardPage.isShow)
                _m_wRewardPage.setSelfRank(_rank);
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(ERankRushDetailTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return null;

            foreach (GGUIRankRushDetailTabMono mono in wnd.monoTabList)
            {
                if (mono?.tabType == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(mono.tabSubWndAssetId);
            }

            return null;
        }

        #endregion

        #region 点击事件

        //点击领奖
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_rankRushInfo == null || 
                _m_rankRushInfo.activityInfo == null ||
                _m_rankRushInfo.activityRankRushRefObj == null)
                return;

            if (_m_rankRushInfo.activityInfo.activityState != EActivityState.REWARDING)
            {
                //未到领奖时间
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.rankRush_isNotAwardTime_none);
                return;
            }

            _m_rankRushInfo.getSettleInfo(_settleInfo =>
            {
                if (_settleInfo == null || _settleInfo.getRank() <= 0)
                {
                    //未满足领奖资格
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.rankRush_notSatisfiedGetReward_none);
                    return;
                }

                if (_settleInfo.getHadDraw())
                {
                    //已领取
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.rankRush_buttonAlreadyReceiveName_none);
                    return;
                }

                long serialize = _m_lShowSerialize;
                NPPlayer.instance.commonActivityComp.reqDrawActivityRankReward(_m_rankRushInfo.activityInfo.instanceId, _m_rankRushInfo.activityRankRushRefObj.rank_id,
                    (_isSuc) =>
                    {
                        if (wnd == null || !isShow || _m_lShowSerialize != serialize)
                            return;

                        if (_isSuc)
                        {
                            //设置为已领取
                            _m_rankRushInfo.setIsGetReward(true);
                            _refreshSelfInfo();
                        }
                    });
            });
        }

        //点击排行榜提示
        private void _onClickRankingTip(GameObject _go)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null)
                return;

            //没有配置不显示
            if (string.IsNullOrEmpty(_m_rankRushInfo.activityRankRushRefObj.ranking_tip))
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                TextTranslate.instance.getLanguage(_m_rankRushInfo.activityRankRushRefObj.ranking_tip),
                (RectTransform)_go.transform, wnd.tipIntervalX, wnd.tipIntervalY));
        }

        //点击获取途径
        private void _onClickAccess(GameObject _go)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndRankRushAccess.instance, () =>
            {
                GGUIWndRankRushAccess.instance.showWnd();
                GGUIWndRankRushAccess.instance.setInfo(_m_rankRushInfo.activityInfo.instanceId, _m_rankRushInfo.activityRankRushRefObj.access_id_list);
            }, UINodeTagConst.C_RANK_RUSH_ACCESS);
        }

        //点击礼包按钮
        private void _onClickGift(GameObject _go)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndActivityGiftPack.instance, () =>
            {
                GGUIWndActivityGiftPack.instance.showWnd();
                GGUIWndActivityGiftPack.instance.setInfo(_m_rankRushInfo.activityInfo.activityId);
            }, UINodeTagConst.C_ACTIVITY_GIFT_PACK);
        }

        //点击区服列表详情
        private void _onClickServerListDetail(GameObject _go)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null)
                return;

            //获取区服名称列表
            GCommon.getServerNameListByServerIdList(_m_rankRushInfo.activityInfo.usIdList, _list =>
            {
                if (_list == null || _list.Count == 0)
                    return;

                //打开区服列表界面
                QueueMgr.instance.AddNode(new GNodeCommonToolTip_ServerList(3914, _list, (RectTransform)_go.transform, wnd.serverListInterval));
            });
        }

        //点击关闭
        protected abstract void _onClickClose(GameObject _go);

        #endregion

        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long instanceId = (long)_objects[1];
            EActivityState oriState = (EActivityState)_objects[2];
            EActivityState curState = (EActivityState)_objects[3];

            if (_m_rankRushInfo != null && _m_rankRushInfo.activityInfo != null && _m_rankRushInfo.activityInfo.instanceId == instanceId)
            {
                //如果是关闭或者可丢弃状态，直接关闭界面
                if (curState == EActivityState.CLOSED || curState == EActivityState.CAN_DISCARD)
                {
                    //活动已结束
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.rankRush_activityAlreadyClose_none);
                    _onClickClose(null);
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_GIFT_PACK);
                }
                else
                {
                    //重置数据，重新获得
                    if (oriState == EActivityState.PLAYING && curState != EActivityState.PLAYING)
                    {
                        //如果从进行中变成其他状态，重置排行榜数据，重新请求
                        _m_lRankShowInfoList?.Clear();
                        _m_lRankShowInfoList = null;
                    }
                    _refreshWnd();
                }
            }
        }

        #endregion
    }
}

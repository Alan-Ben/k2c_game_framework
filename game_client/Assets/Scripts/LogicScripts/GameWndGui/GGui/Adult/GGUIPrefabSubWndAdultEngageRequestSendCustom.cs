using System;
using System.Collections.Generic;
using ALPackage;
using Common.ChildObj;
using GS2GC.p004_PlayerOp;
using GS2GC.p014_ChildOp;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndAdultEngageRequestSendCustom : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoAdultEngageRequestSendCustom>
    {
        private GGUISubWndAdultEngageRequestSendCustomContainer _m_recommendContainer;
        private NPGGUIWndPlayerIcon _m_searchingResultPlayerInfoWnd;
        private NPGGUIWndCommonToggleEx _m_recommendToggle;
        private NPGGUIWndCommonToggleEx _m_friendToggle;

        private long _m_searchingPlayerCid;
        private AdultInfo _m_myAdultInfo;
        private int _m_refreshSerialize;
        private bool _m_isShowFriendList;
        
        
        public GGUIPrefabSubWndAdultEngageRequestSendCustom(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageRequestSendCustom.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageRequestSendCustom.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        { 
            _m_recommendContainer?.showWnd();
            _m_searchingResultPlayerInfoWnd?.showWnd();
            _m_recommendToggle?.showWnd();
            _m_friendToggle?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        { 
            _m_recommendContainer?.hideWnd();
            _m_searchingResultPlayerInfoWnd?.hideWnd();
            _m_recommendToggle?.hideWnd();
            _m_friendToggle?.hideWnd();
        }
        protected override void _onReset()
        { 
            _m_recommendContainer?.resetWnd();
            _m_searchingResultPlayerInfoWnd?.resetWnd();
            _m_recommendToggle?.resetWnd();
            _m_friendToggle?.resetWnd();
        }
        protected override void _onDiscard()
        { 
            _m_recommendContainer?.discard();
            _m_recommendContainer = null;
            _m_searchingResultPlayerInfoWnd?.discard();
            _m_searchingResultPlayerInfoWnd = null;
            _m_recommendToggle?.discard();
            _m_recommendToggle = null;
            _m_friendToggle?.discard();
            _m_friendToggle = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSearch, _onSearchBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnEngageSearchResult, _onEngageSearchResultBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoRecommendContainer != null)
                _m_recommendContainer = new GGUISubWndAdultEngageRequestSendCustomContainer(wnd.monoRecommendContainer);
            if (wnd.monoSearchResult != null)
                _m_searchingResultPlayerInfoWnd = new NPGGUIWndPlayerIcon(wnd.monoSearchResult);
            if (wnd.monoRecommendToggle != null)
            {
                _m_recommendToggle = new NPGGUIWndCommonToggleEx(wnd.monoRecommendToggle);
                _m_recommendToggle.clickDelegate += _onRecommendToggleClick;
            }
            if (wnd.monoFriendToggle != null)
            {
                _m_friendToggle = new NPGGUIWndCommonToggleEx(wnd.monoFriendToggle);
                _m_friendToggle.clickDelegate += _onFriendToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSearch, _onSearchBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnEngageSearchResult, _onEngageSearchResultBtnClick);
        }


        public void refreshWnd(AdultInfo _adultInfo)
        {
            _m_myAdultInfo = _adultInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_myAdultInfo == null || _m_recommendContainer == null)
                return;

            _m_isShowFriendList = true;
            _refreshToggleState();
            refreshList();
            wnd.setSearchingResultState(_m_searchingPlayerCid != 0);
        }
        public void refreshList()
        {
            if (wnd == null || !_m_bIsShow || _m_myAdultInfo == null)
                return;

            wnd.setLoadingShow(true);
            int serialize = _m_refreshSerialize = ALSerializeOpMgr.next();
            if (_m_isShowFriendList)
            {
                NPPlayer.instance.friendsComp.getFriendsDataList(_friendDataList =>
                {
                    if (serialize != _m_refreshSerialize || wnd == null || _friendDataList == null)
                        return;

                    wnd.setLoadingShow(false);
                    List<NPCommonSimplePlayerInfo> playerList = new List<NPCommonSimplePlayerInfo>();
                    foreach (PlayerFriendItemData friendData in _friendDataList)
                    {
                        if (friendData?.playerInfo == null)
                            continue;

                        playerList.Add(friendData.playerInfo);
                    }

                    _m_recommendContainer?.refreshWnd(_m_myAdultInfo, playerList);
                });
            }
            else
            {
                NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_012_ReqGetRecommendPlayerList(_m_myAdultInfo.id),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_012_RetGetRecommendPlayerList>(
                        (_isSuc, _msg) =>
                        {
                            if (serialize != _m_refreshSerialize || _msg == null)
                                return;

                            List<NPCommonSimplePlayerInfo> playerList = new List<NPCommonSimplePlayerInfo>();
                            List<Adult_PoolBaseInfo> recommendList = _msg.getMatchList();
                            HashSet<long> cidSet = new HashSet<long>(); // 用来去重，这边服务端偷懒，就客户端来吧
                            
                            ALStepCounter stepCounter = new ALStepCounter();
                            stepCounter.chgTotalStepCount(recommendList.Count + 1);
                            stepCounter.regAllDoneDelegate(() =>
                            {
                                if (serialize != _m_refreshSerialize || wnd == null)
                                    return;
                                
                                wnd.setLoadingShow(false);
                                _m_recommendContainer?.refreshWnd(_m_myAdultInfo, playerList);
                            });
                            
                            foreach (Adult_PoolBaseInfo serverInfo in recommendList)
                            {
                                if (serverInfo == null || cidSet.Contains(serverInfo.getApplyCid()))
                                {
                                    stepCounter.addDoneStepCount();
                                    continue;
                                }

                                cidSet.Add(serverInfo.getApplyCid());
                                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(serverInfo.getApplyCid()),
                                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>(
                                        (_isSuc2, _msg2) =>
                                        {
                                            if (_isSuc2)
                                                playerList.Add(new NPCommonSimplePlayerInfo(_msg2.getPlayerBrief()));
                                            
                                            stepCounter.addDoneStepCount();
                                        }));
                            }
                            
                            stepCounter.addDoneStepCount();
                        }));
            }
        }
        

        private void _onRecommendToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (!_m_isShowFriendList)
                return;

            _m_isShowFriendList = false;
            _refreshToggleState(true);
            refreshList();
        }
        private void _onFriendToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_isShowFriendList)
                return;

            _m_isShowFriendList = true;
            _refreshToggleState(true);
            refreshList();
        }
        private void _refreshToggleState(bool _showAnim = false)
        {
            if (_showAnim)
            {
                _m_recommendToggle?.setSelected(!_m_isShowFriendList);
                _m_friendToggle?.setSelected(_m_isShowFriendList);
            }
            else
            {
                _m_recommendToggle?.setState(!_m_isShowFriendList);
                _m_friendToggle?.setState(_m_isShowFriendList);
            }
        }
        private void _onSearchBtnClick(GameObject _obj)
        {
            if (wnd == null || wnd.iptPlayerId == null || !long.TryParse(wnd.iptPlayerId.text, out long playerCid))
                return;

            _m_searchingPlayerCid = 0;
            wnd.setSearchingResultState(false);
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(playerCid),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>(
                    (_isSuc, _msg) =>
                    {
                        if (_isSuc)
                        {
                            wnd.setSearchingResultState(true);
                            _m_searchingPlayerCid = playerCid;
                            _m_searchingResultPlayerInfoWnd?.setPlayerInfo(new NPCommonSimplePlayerInfo(_msg.getPlayerBrief()));
                        }
                    }));
        }
        private void _onEngageSearchResultBtnClick(GameObject _obj)
        {
            if (_m_searchingPlayerCid == 0 || _m_myAdultInfo == null)
                return;

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_013_ReqApplyToPlayer(_m_searchingPlayerCid, _m_myAdultInfo.id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_013_RetApplyToPlayer>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);

                    if (_msg != null && _msg.getIsPlayerRefuseAllRequest())
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_engagedTargetRefuseAllRequest_none);
                        return;
                    }
                    
                    if (_isSuc)
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_REQUEST_SEND);
                }));
        }
    }
}
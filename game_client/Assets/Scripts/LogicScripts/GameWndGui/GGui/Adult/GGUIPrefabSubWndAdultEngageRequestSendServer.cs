using System;
using System.Collections.Generic;
using ALPackage;
using Common.ChildObj;
using GS2GC.p014_ChildOp;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndAdultEngageRequestSendServer : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoAdultEngageRequestSendServer>
    {
        private GGUISubWndAdultEngageRequestSendServerContainer _m_recommendContainer;
        private GGUISubWndChildInfo _m_adultInfoWnd;
        private NPGGUIWndCommonToggleEx _m_engageLimitToggle;//联谊限制勾选Toggle

        private AdultInfo _m_myAdultInfo;
        private int _m_refreshSerialize;
        
        
        public GGUIPrefabSubWndAdultEngageRequestSendServer(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageRequestSendServer.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageRequestSendServer.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_recommendContainer?.showWnd();
            _m_adultInfoWnd?.showWnd();
            _m_engageLimitToggle?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        { 
            _m_recommendContainer?.hideWnd();
            _m_adultInfoWnd?.hideWnd();
            _m_engageLimitToggle?.hideWnd();
        }
        protected override void _onReset()
        { 
            _m_recommendContainer?.resetWnd();
            _m_adultInfoWnd?.resetWnd();
            _m_engageLimitToggle?.resetWnd();
        }
        protected override void _onDiscard()
        { 
            _m_recommendContainer?.discard();
            _m_recommendContainer = null;
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;
            _m_engageLimitToggle?.discard();
            _m_engageLimitToggle = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnRefresh, _onRefreshBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRandom, _onRandomBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoRecommendContainer != null)
                _m_recommendContainer = new GGUISubWndAdultEngageRequestSendServerContainer(wnd.monoRecommendContainer);
            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            if (wnd.toggleEngageLimit != null)
            {
                _m_engageLimitToggle = new NPGGUIWndCommonToggleEx(wnd.toggleEngageLimit);
                _m_engageLimitToggle.clickDelegate = _onEngageLimitToggleClick;
                // 从本地存储读取初始状态
                bool isEnabled = AccountSettingMgr.instance.childSaver?.getEngageLimitEnabled() ?? true;
                _m_engageLimitToggle.setSelected(isEnabled, true, false);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnRefresh, _onRefreshBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRandom, _onRandomBtnClick);
        }


        public void refreshWnd(AdultInfo _adultInfo)
        {
            _m_myAdultInfo = _adultInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_myAdultInfo == null || _m_recommendContainer == null)
                return;
            
            wnd.setLoadingShow(true);
            int serialize = _m_refreshSerialize;
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_012_ReqGetRecommendPlayerList(_m_myAdultInfo.id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_012_RetGetRecommendPlayerList>(
                    (_isSuc, _msg) =>
                    {
                        if (serialize != _m_refreshSerialize || wnd == null || _msg == null)
                            return;
                        
                        wnd.setLoadingShow(false);
                        List<Adult_PoolBaseInfo> serverList = _msg.getMatchList();
                        List<PoolSimpleAdultInfo> recommendList = serverList.ConvertAll(_serverInfo => new PoolSimpleAdultInfo(_serverInfo.getApplyAdultId(), _serverInfo.getApplyCid(), _serverInfo.getBonus(), _serverInfo.getMinBonus()));
                        _m_recommendContainer?.refreshWnd(_m_myAdultInfo, recommendList);
                    }));
            
            _m_adultInfoWnd?.refreshWnd(_m_myAdultInfo);
        }
        
        
        private void _onRefreshBtnClick(GameObject _obj)
        {
            if (wnd == null || _m_myAdultInfo == null || _m_recommendContainer == null)
                return;
            
            wnd.setLoadingShow(true);
            int serialize = _m_refreshSerialize;
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_012_ReqGetRecommendPlayerList(_m_myAdultInfo.id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_012_RetGetRecommendPlayerList>(
                    (_isSuc, _msg) =>
                    {
                        if (serialize != _m_refreshSerialize || wnd == null || _msg == null)
                            return;
                        
                        wnd.setLoadingShow(false);
                        List<Adult_PoolBaseInfo> serverList = _msg.getMatchList();
                        List<PoolSimpleAdultInfo> recommendList = serverList.ConvertAll(_serverInfo => new PoolSimpleAdultInfo(_serverInfo.getApplyAdultId(), _serverInfo.getApplyCid(), _serverInfo.getBonus(), _serverInfo.getMinBonus()));
                        _m_recommendContainer?.refreshWnd(_m_myAdultInfo, recommendList);
                    }));
        }
        private void _onRandomBtnClick(GameObject _obj)
        {
            if (_m_myAdultInfo == null)
                return;

            Action<long> afterDealLimit = (_limitEarning) =>
            {
                int serialize = MainCameraMono.selfInstance.openAllInputMask();
                NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_014_ReqApplyToGroup(_m_myAdultInfo.id, _limitEarning),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_014_RetApplyToGroup>((_isSuc, _msg) =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(serialize);
                        if (_isSuc)
                            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_REQUEST_SEND);
                    }));
            };
            
            if (AccountSettingMgr.instance.childSaver?.getEngageLimitEnabled() ?? false)
            {
                GGUISubWndAdultTeamUpRequirementSetting.instance.refreshWnd(_m_myAdultInfo.earnings, afterDealLimit);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUISubWndAdultTeamUpRequirementSetting.instance, () =>
                {
                    GGUISubWndAdultTeamUpRequirementSetting.instance.showWnd();
                }, UINodeTagConst.C_ADULT_TEAM_UP_REQUIREMENT_SETTING);
            }
            else
            {
                afterDealLimit(0);
            }
        }

        /// <summary>
        /// 联谊限制Toggle点击事件
        /// </summary>
        private void _onEngageLimitToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;

            // 切换选中状态
            bool newState = !_toggle.isOn;
            _toggle.setSelected(newState);

            // 保存到本地存储
            AccountSettingMgr.instance.childSaver?.setEngageLimitEnabled(newState);
        }
    }
}
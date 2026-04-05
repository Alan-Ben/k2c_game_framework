using ALPackage;
using GS2GC.p014_ChildOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendCustomContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoAdultEngageRequestSendCustomContainerItem>
    {
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        
        private AdultInfo _m_myAdultInfo;
        private NPCommonSimplePlayerInfo _m_simpleInfo;
        
        
        public GGUISubWndAdultEngageRequestSendCustomContainerItem(GGUIMonoAdultEngageRequestSendCustomContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        { 
            _m_playerInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        { 
            _m_playerInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        { 
            _m_playerInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        { 
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEngage, _onEngageBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoPlayerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.monoPlayerInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnEngage, _onEngageBtnClick);
        }
        

        public void refreshWnd(AdultInfo _myAdultInfo, NPCommonSimplePlayerInfo _simpleInfo)
        {
            _m_myAdultInfo = _myAdultInfo;
            _m_simpleInfo = _simpleInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_simpleInfo == null)
                return;

            _m_playerInfoWnd?.setPlayerInfo(_m_simpleInfo);
        }
        
        
        private void _onEngageBtnClick(GameObject _obj)
        {
            if (_m_myAdultInfo == null || _m_simpleInfo == null)
                return;
            
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_013_ReqApplyToPlayer(_m_simpleInfo.cid, _m_myAdultInfo.id),
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
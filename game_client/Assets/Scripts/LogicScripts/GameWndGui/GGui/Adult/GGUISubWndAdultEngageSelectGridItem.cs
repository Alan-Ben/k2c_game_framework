using ALPackage;
using GS2GC.p014_ChildOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageSelectGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAdultEngageSelectGridItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;
        private AdultInfo _m_targetAdultInfo;
        private AdultInfo _m_myAdultInfo;
        
        
        public GGUISubWndAdultEngageSelectGridItem(GGUIMonoAdultEngageSelectGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_adultInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_adultInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(AdultInfo _targetAdultInfo, AdultInfo _myAdult)
        {
            _m_targetAdultInfo = _targetAdultInfo;
            _m_myAdultInfo = _myAdult;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_myAdultInfo == null)
                return;
            
            _m_adultInfoWnd?.refreshWnd(_m_myAdultInfo);
        }
        
        
        private void _onSelectBtnClick(GameObject _obj)
        {
            if (_m_targetAdultInfo == null || _m_myAdultInfo == null)
                return;

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_011_ReqAgreeToMeApply(_m_myAdultInfo.id, _m_targetAdultInfo.id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_011_RetAgreeToMeApply>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    if (_isSuc)
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_SELECT);
                }));
        }
    }
}
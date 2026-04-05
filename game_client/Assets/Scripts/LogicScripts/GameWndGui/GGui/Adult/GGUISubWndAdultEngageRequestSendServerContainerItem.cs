using ALPackage;
using GS2GC.p014_ChildOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendServerContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoAdultEngageRequestSendServerContainerItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;

        private AdultInfo _m_myAdultInfo;
        private PoolSimpleAdultInfo _m_simpleInfo;
        private int _m_refreshSerialize;
        
        
        public GGUISubWndAdultEngageRequestSendServerContainerItem(GGUIMonoAdultEngageRequestSendServerContainerItem _wnd) 
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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEngage, _onEngageBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnEngage, _onEngageBtnClick);

            refreshWnd();
        }


        public void refreshWnd(AdultInfo _myAdultInfo, PoolSimpleAdultInfo _simpleInfo)
        {
            _m_myAdultInfo = _myAdultInfo;
            _m_simpleInfo = _simpleInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_simpleInfo == null)
                return;

            int serialize = _m_refreshSerialize;
            wnd.setLoadingShow(true);
            _m_simpleInfo.getAdultInfo(_info =>
            {
                if (serialize != _m_refreshSerialize)
                    return;

                if (wnd == null)
                    return;
                
                wnd.setLoadingShow(false);
                _m_adultInfoWnd?.refreshWnd(_info);
            });
            
            
            ALUGUICommon.setGameObjEnable(wnd.hasLowerEarningsLimitShow, _m_simpleInfo.minBonus > 0);
            ALUGUICommon.setGameObjEnable(wnd.hasLowerEarningsLimitHide, _m_simpleInfo.minBonus <= 0);
            wnd.setEarningsColor(_m_myAdultInfo != null && _m_myAdultInfo.earnings >= _m_simpleInfo.minBonus);
            string limitLowerEarningsLargeStr = _m_simpleInfo.minBonus.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD);
            ALUGUICommon.setLabelTxt(wnd.txtLimitLowerEarnings, string.IsNullOrEmpty(wnd.limitLowerEarningsKey) ? limitLowerEarningsLargeStr : 
                TextTranslate.instance.getLanguage(wnd.limitLowerEarningsKey, limitLowerEarningsLargeStr));
        }


        private void _onEngageBtnClick(GameObject _obj)
        {
            if (_m_simpleInfo == null || _m_myAdultInfo == null || wnd == null)
                return;

            if (_m_myAdultInfo.earnings < _m_simpleInfo.minBonus)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(wnd.onSelfChildEarningsLessThanLimitTip);
                return;
            }
            
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_015_ReqAgreeApplyGroup(_m_myAdultInfo.id, _m_simpleInfo.adultId, _m_simpleInfo.cid), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_015_RetAgreeApplyGroup>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    if (_isSuc)
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_REQUEST_SEND);
                }));
        }
    }
}
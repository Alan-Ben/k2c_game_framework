using System.Collections.Generic;
using ALPackage;
using Common.ChildEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestReceiveGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAdultEngageRequestReceiveGridItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;

        private AdultEngageRequestInfo _m_requestInfo;
        private int _m_refreshSerialize;
        private int _m_agreeSerialize;


        public GGUISubWndAdultEngageRequestReceiveGridItem(GGUIMonoAdultEngageRequestReceiveGridItem _wnd) 
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
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_agreeSerialize = ALSerializeOpMgr.next();
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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnAgree, _onAgreeBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRefuse, _onRefuseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnAgree, _onAgreeBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRefuse, _onRefuseBtnClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(AdultEngageRequestInfo _requestInfo)
        {
            _m_requestInfo = _requestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_requestInfo == null)
                return;
            
            wnd.setLoadingShow(true);
            int serialize = _m_refreshSerialize;
            _m_requestInfo.getAdultInfo(_adultInfo =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                wnd.setLoadingShow(false);
                _m_adultInfoWnd?.refreshWnd(_adultInfo);
            });
            ALUGUICommon.setLabelTxt(wnd.txtEnableTime, TimeUtil.millisecondsToTime_hms((_m_requestInfo.expiredTimeS - FpsAndPingMgr.instance.serverTimeTagS) * 1000));
        }
        public void refreshTime()
        {
            if (wnd == null || _m_requestInfo == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtEnableTime, TimeUtil.millisecondsToTime_hms((_m_requestInfo.expiredTimeS - FpsAndPingMgr.instance.serverTimeTagS) * 1000));
        }
        

        private void _onAgreeBtnClick(GameObject _obj)
        {
            _m_agreeSerialize = ALSerializeOpMgr.next();
            if (_m_requestInfo == null)
                return;

            int serialize = _m_refreshSerialize;
            int agreeSerialize = _m_agreeSerialize;
            _m_requestInfo.getAdultInfo(_otherAdultInfo =>
            {
                if (serialize != _m_refreshSerialize || agreeSerialize != _m_agreeSerialize)
                    return;

                List<UnmarriedInfo> unmarriedInfoList = NPPlayer.instance.childComp.getUnmarriedChildList();
                List<AdultInfo> myAdultList = new List<AdultInfo>();
                
                foreach (UnmarriedInfo unmarriedInfo in unmarriedInfoList)
                {
                    if (unmarriedInfo.status == EAdultStatus.NONE && _otherAdultInfo.canMarryWith(unmarriedInfo.adultInfo))
                        myAdultList.Add(unmarriedInfo.adultInfo);
                }
                
                if (myAdultList.Count == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.adult_noSuitableAdultToMarry_none);
                    return;
                }
                    
                GGUIWndAdultEngageSelect.instance.refreshWnd(_otherAdultInfo, myAdultList);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultEngageSelect.instance, GGUIWndAdultEngageSelect.instance.showWnd, UINodeTagConst.C_ADULT_ENGAGE_SELECT);
            });
        }
        private void _onRefuseBtnClick(GameObject _obj)
        {
            if (_m_requestInfo == null)
                return;
            
            NPGSClientListener.sendMsgByLog(GSWriter_014_ChildOp.make_009_ReqRefuseToMeApply(_m_requestInfo.adultId));
        }
    }
}
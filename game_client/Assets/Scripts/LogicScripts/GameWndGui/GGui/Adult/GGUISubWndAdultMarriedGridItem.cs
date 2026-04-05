using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultMarriedGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAdultMarriedGridItem>
    {
        private GGUISubWndChildInfo _m_myChildInfo;
        private GGUISubWndChildInfo _m_otherChildInfo;

        private MarriedInfo _m_marriedInfo;
        [NotNull] private readonly ALStepCounter _m_stepCounter;
        private int _m_refreshSerialize;


        public GGUISubWndAdultMarriedGridItem(GGUIMonoAdultMarriedGridItem _wnd) 
            : base(_wnd)
        {
            _m_stepCounter = new ALStepCounter();
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_myChildInfo?.showWnd();
            _m_otherChildInfo?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_myChildInfo?.hideWnd();
            _m_otherChildInfo?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_myChildInfo?.resetWnd();
            _m_otherChildInfo?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_myChildInfo?.discard();
            _m_otherChildInfo?.discard();
            _m_myChildInfo = null;
            _m_otherChildInfo = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnMyChildDetail, _onMyChildDetailBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnOtherChildDetail, _onOtherChildDetailBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoMyChildInfo != null)
                _m_myChildInfo = new GGUISubWndChildInfo(wnd.monoMyChildInfo);
            if (wnd.monoOtherChildInfo != null)
                _m_otherChildInfo = new GGUISubWndChildInfo(wnd.monoOtherChildInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnMyChildDetail, _onMyChildDetailBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnOtherChildDetail, _onOtherChildDetailBtnClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(MarriedInfo _marriedInfo)
        {
            _m_marriedInfo = _marriedInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_marriedInfo == null)
                return;
            
            int serialize = _m_refreshSerialize;
            AdultInfo myAdultInfo = null;
            AdultInfo otherAdultInfo = null;
            int marriedTime = 0;
            
            wnd.setLoadingShow(true);
            _m_stepCounter.resetAll();
            _m_stepCounter.chgTotalStepCount(3);
            _m_stepCounter.regAllDoneDelegate(() =>
            {
                if (wnd == null || serialize != _m_refreshSerialize || myAdultInfo == null || otherAdultInfo == null)
                    return;
                
                wnd.setLoadingShow(false);
                _m_myChildInfo?.refreshWnd(myAdultInfo);
                _m_otherChildInfo?.refreshWnd(otherAdultInfo);
                ALUGUICommon.setLabelTxt(wnd.txtTotalEarnings, TextTranslate.instance.getLanguage(TransKeyConst.adult_marriedAdultRarnings_num, (myAdultInfo.earnings + otherAdultInfo.earnings).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
                ALUGUICommon.setLabelTxt(wnd.txtMarriedTime, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCSeconds(marriedTime)));
            });
            
            _m_marriedInfo.getMyAdultInfo(_info =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                myAdultInfo = _info;
                _m_stepCounter.addDoneStepCount();
            });
            _m_marriedInfo.getOtherAdultInfo(_info =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                otherAdultInfo = _info;
                _m_stepCounter.addDoneStepCount();
            });
            _m_marriedInfo.getMarriedTime(_time =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                marriedTime = _time;
                _m_stepCounter.addDoneStepCount();
            });
        }
        
        
        private void _onMyChildDetailBtnClick(GameObject _obj)
        {
            if (_m_marriedInfo == null)
                return;
            
            int serialize = _m_refreshSerialize;
            _m_marriedInfo.getMyAdultInfo(_adultInfo =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                GGUIWndAdultMarriedDetail.instance.refreshWnd(_adultInfo, _obj.transform as RectTransform, wnd.detailInterval);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultMarriedDetail.instance, GGUIWndAdultMarriedDetail.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_ADULT_MARRIED_DETAIL_TOOL_TIP, true, false);
            });
        }
        private void _onOtherChildDetailBtnClick(GameObject _obj)
        {
            if (_m_marriedInfo == null)
                return;
            
            int serialize = _m_refreshSerialize;
            _m_marriedInfo.getOtherAdultInfo(_adultInfo =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                GGUIWndAdultMarriedDetail.instance.refreshWnd(_adultInfo, _obj.transform as RectTransform, wnd.detailInterval);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultMarriedDetail.instance, GGUIWndAdultMarriedDetail.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_ADULT_MARRIED_DETAIL_TOOL_TIP, true, false);
            });
        }
    }
}
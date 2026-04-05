using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultMarrySuccess : _ATALBasicUIWnd<GGUIMonoAdultMarrySuccess>
    {
        [NotNull] public static GGUIWndAdultMarrySuccess instance { get { return _g_instance ??= new GGUIWndAdultMarrySuccess(); } }
        private static GGUIWndAdultMarrySuccess _g_instance;


        private GGUISubWndChildInfo _m_myChildInfoWnd;
        private GGUISubWndChildInfo _m_otherChildInfoWnd;
        private NPGGUIWndCommonItem _m_marryItemWnd;

        private MarriedInfo _m_marriedInfo;
        private NPCommonCostItem _m_rewardItem;
        [NotNull] private readonly ALStepCounter _m_stepCounter;
        private int _m_refreshSerialize;
        
        
        public GGUIWndAdultMarrySuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_stepCounter = new ALStepCounter();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultMarrySuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultMarrySuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_myChildInfoWnd?.showWnd();
            _m_otherChildInfoWnd?.showWnd();
            _m_marryItemWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_myChildInfoWnd?.hideWnd();
            _m_otherChildInfoWnd?.hideWnd();
            _m_marryItemWnd?.hideWnd();
        }
        protected override void _onReset()
        { 
            _m_myChildInfoWnd?.resetWnd();
            _m_otherChildInfoWnd?.resetWnd();
            _m_marryItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        { 
            _m_myChildInfoWnd?.discard();
            _m_otherChildInfoWnd?.discard();
            _m_marryItemWnd?.discard();
            _m_myChildInfoWnd = null;
            _m_otherChildInfoWnd = null;
            _m_marryItemWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoMarryItem != null)
                _m_marryItemWnd = new NPGGUIWndCommonItem(wnd.monoMarryItem);
            if (wnd.monoMyChildInfo != null)
                _m_myChildInfoWnd = new GGUISubWndChildInfo(wnd.monoMyChildInfo);
            if (wnd.monoOtherChildInfo != null)
                _m_otherChildInfoWnd = new GGUISubWndChildInfo(wnd.monoOtherChildInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd(MarriedInfo _marriedInfo, NPCommonCostItem _rewardItem)
        {
            _m_marriedInfo = _marriedInfo;
            _m_rewardItem = _rewardItem;
            refreshWnd();
        }
        public void refreshWnd()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            if (wnd == null || !_m_bIsShow || _m_marriedInfo == null)
                return;
            
            _m_marryItemWnd?.setItem(_m_rewardItem);

            int serialize = _m_refreshSerialize;
            AdultInfo myAdultInfo = null;
            AdultInfo otherAdultInfo = null;
            long marriedTime = 0;
            
            wnd.setLoadingShow(true);
            _m_stepCounter.resetAll();
            _m_stepCounter.chgTotalStepCount(3);
            _m_stepCounter.regAllDoneDelegate(() =>
            {
                if (serialize != _m_refreshSerialize)
                    return;
                
                wnd.setLoadingShow(false);
                wnd.refreshBubble();
                _m_myChildInfoWnd?.refreshWnd(myAdultInfo);
                _m_otherChildInfoWnd?.refreshWnd(otherAdultInfo);
                ALUGUICommon.setLabelTxt(wnd.txtDate, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCSeconds(marriedTime)));
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
        
        
        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}
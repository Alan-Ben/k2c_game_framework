using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPlayerDailyRewardPreview : _ANPGGUIBasicWnd<GGUIMonoPlayerDailyRewardPreview>
    {
        [NotNull] public static GGUIWndPlayerDailyRewardPreview instance { get { return _g_instance ??= new GGUIWndPlayerDailyRewardPreview(); } }
        private static GGUIWndPlayerDailyRewardPreview _g_instance;
        
        
        private NPGGUIWndCommonItemContainer _m_rewardContainer; 
        private List<NPCommonCostItem> _m_rewardItemList;
        
        
        public GGUIWndPlayerDailyRewardPreview() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoPlayerDailyRewardPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerDailyRewardPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_rewardContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_rewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_rewardContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_rewardContainer?.discard();
            _m_rewardContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoRewardContainer != null)
                _m_rewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd(List<NPCommonCostItem> _rewardItemList)
        {
            _m_rewardItemList = _rewardItemList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_rewardContainer?.showItemList(_m_rewardItemList);
        }
        

        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}
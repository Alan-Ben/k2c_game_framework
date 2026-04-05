using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerResearch : _ATALBasicUIWnd<GGUIMonoTowerResearch>
    {
        private static GGUIWndTowerResearch _g_instance = new GGUIWndTowerResearch();
    
        public static GGUIWndTowerResearch instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerResearch();
                return _g_instance;
            }
        }
        private GGUIWndTowerResearchItemContainer _m_chapterContainer; //研究项容器
        
        public GGUIWndTowerResearch() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoTowerResearch.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerResearch.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_TOWER_ACTIVE_POS_CHG, _refreshProcess);
        }


        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TOWER_ACTIVE_POS_CHG, _refreshProcess);
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            _m_chapterContainer?.discard();
            _m_chapterContainer = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.chapterContainer != null)
                _m_chapterContainer = new GGUIWndTowerResearchItemContainer(wnd.chapterContainer);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _refreshProcess();

            _m_chapterContainer?.showWnd();
            _m_chapterContainer?.showItemList(GRefdataCoreMgr.instance.towerChapterRefCore.refList);
        }
        
        
        private void _refreshProcess()
        {
            ALUGUICommon.setLabelTxt(wnd.txtProcess,
                TextTranslate.instance.getLanguage(TransKeyConst.tower_research_title_process_str,
                    NPPlayer.instance.towerComp.towerEarningAddPer / 100f));
        }
    }
}
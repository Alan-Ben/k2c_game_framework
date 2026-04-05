using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 图鉴主页面
    /// </summary>
    public class GGUIWndTreasureHuntCatalogMain : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntCatalogMain>
    {
        private static GGUIWndTreasureHuntCatalogMain _g_instance;
        public static GGUIWndTreasureHuntCatalogMain instance { get { return _g_instance ??= new GGUIWndTreasureHuntCatalogMain(); } }

        private NPGGUIWndProgress _m_wOreCatalogProgress;
        private NPGGUIWndProgress _m_wTreasureCatalogProgress;
        private NPGGUIWndProgress _m_wCompositeCatalogProgress;

        public GGUIWndTreasureHuntCatalogMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCatalogMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCatalogMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreCatalogProgress != null)
                _m_wOreCatalogProgress = new NPGGUIWndProgress(wnd.monoOreCatalogProgress);

            if (wnd.monoTreasureCatalogProgress != null)
                _m_wTreasureCatalogProgress = new NPGGUIWndProgress(wnd.monoTreasureCatalogProgress);

            if (wnd.monoCompositeCatalogProgress != null)
                _m_wCompositeCatalogProgress = new NPGGUIWndProgress(wnd.monoCompositeCatalogProgress);

            ALUGUICommon.combineBtnClick(wnd.btnOreCatalog, _onClickOreCatalog);
            ALUGUICommon.combineBtnClick(wnd.btnTreasureCatalog, _onClickTreasureCatalog);
            ALUGUICommon.combineBtnClick(wnd.btnCompositeCatalog, _onClickCompositeCatalog);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnOreCatalog, _onClickOreCatalog);
                ALUGUICommon.uncombineBtnClick(wnd.btnTreasureCatalog, _onClickTreasureCatalog);
                ALUGUICommon.uncombineBtnClick(wnd.btnCompositeCatalog, _onClickCompositeCatalog);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }

            _m_wOreCatalogProgress?.discard();
            _m_wOreCatalogProgress = null;

            _m_wTreasureCatalogProgress?.discard();
            _m_wTreasureCatalogProgress = null;

            _m_wCompositeCatalogProgress?.discard();
            _m_wCompositeCatalogProgress = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreCatalogProgress?.hideWnd();
            _m_wTreasureCatalogProgress?.hideWnd();
            _m_wCompositeCatalogProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreCatalogProgress?.resetWnd();
            _m_wTreasureCatalogProgress?.resetWnd();
            _m_wCompositeCatalogProgress?.resetWnd();
        }

        private void _refreshWnd()
        {
            int totalOreCount = GRefdataCoreMgr.instance.treasureHuntOreRefCore.refList.Count;
            int gotOreCount = NPPlayer.instance.treasureHuntComponent.gotOreInfoList.Count;
            if (_m_wOreCatalogProgress != null)
            {
                _m_wOreCatalogProgress.showWnd();
                _m_wOreCatalogProgress.setProgress(gotOreCount, totalOreCount, EValueFormatType.NORMAL);
            }
            
            int totalTreasureCount = GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.refList.Count;
            int gotTreasureCount = NPPlayer.instance.treasureHuntComponent.gotTreasureInfoList.Count;
            if (_m_wTreasureCatalogProgress != null)
            {
                _m_wTreasureCatalogProgress.showWnd();
                _m_wTreasureCatalogProgress.setProgress(gotTreasureCount, totalTreasureCount, EValueFormatType.NORMAL);
            }
            
            int totalCompositeCatalogCount = NPPlayer.instance.treasureHuntComponent.compositeCatalogInfoList.Count;
            int gotCompositeCatalogCount = NPPlayer.instance.treasureHuntComponent.getCollectedCompositeCount();
            if (_m_wCompositeCatalogProgress != null)
            {
                _m_wCompositeCatalogProgress.showWnd();
                _m_wCompositeCatalogProgress.setProgress(gotCompositeCatalogCount, totalCompositeCatalogCount, EValueFormatType.NORMAL);
            }
        }
        
        /// <summary>
        /// 点击矿石图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickOreCatalog(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeTreasureHuntOreCatalog());
        }

        /// <summary>
        /// 点击奇物图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickTreasureCatalog(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeTreasureHuntTreasureCatalog());
        }

        /// <summary>
        /// 点击组合图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCompositeCatalog(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeTreasureHuntCompositeCatalog());
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_CATALOG_MAIN);
        }
    }
}
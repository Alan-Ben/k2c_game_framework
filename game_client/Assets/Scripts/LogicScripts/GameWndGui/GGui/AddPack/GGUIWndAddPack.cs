using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAddPack : _ANPGGUIBasicWnd<GGUIMonoAddPack>
    {
        public static GGUIWndAddPack instance { get { return _g_instance ??= new GGUIWndAddPack(); } }
        private static GGUIWndAddPack _g_instance;


        private GGUIWndAddPackInstallerContainer _m_installerContainer;
        

        private GGUIWndAddPack() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAddPack.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAddPack.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_installerContainer?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_installerContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_installerContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_installerContainer?.discard();
            _m_installerContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnAbortAll, _onBtnAbortAllClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.installerContainer != null)
            {
                _m_installerContainer = new GGUIWndAddPackInstallerContainer(wnd.installerContainer);
                _m_installerContainer.setShowData(AddPackMgr.instance.installerList);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnAbortAll, _onBtnAbortAllClicked);
        }
        
        
        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onBtnAbortAllClicked(GameObject _)
        {
            foreach (AddPackInstaller installer in AddPackMgr.instance.installerList)
            {
                if (installer.download == null)
                    continue;
                
                if (installer.download.isDownloading)
                    installer.download.abortDownload();
            }

            _onBtnCloseClicked(null);
        }
    }
}
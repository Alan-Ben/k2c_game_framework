using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民窗口
    /// </summary>
    public class GGUIWndMarsResident : _ANPGGUIBasicWnd<GGUIMonoMarsResident>
    {
        private static GGUIWndMarsResident _g_instance;
        public static GGUIWndMarsResident instance { get { return _g_instance ??= new GGUIWndMarsResident(); } }
        
        private GGUIWndMarsResidentTabList _m_wMarsResidentTabList;

        public GGUIWndMarsResident() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsResident.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsResident.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onCloseBtnClick);
            
            if (wnd.tabList != null)
                _m_wMarsResidentTabList = new GGUIWndMarsResidentTabList(wnd.tabList);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onCloseBtnClick);
            }
            
            _m_wMarsResidentTabList?.discard();
            _m_wMarsResidentTabList = null;
        }

        protected override void _onShowWnd()
        {
            _m_wMarsResidentTabList?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wMarsResidentTabList?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wMarsResidentTabList?.resetWnd();
        }
        
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_RESIDENT);
        }
    }
}
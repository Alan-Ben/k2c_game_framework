using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 博物馆珍宝激活成功窗口
    /// </summary>
    public class GGUIWndMuseumItemGet : _ATALBasicUIWnd<GGUIMonoMuseumItemGet>
    {
        [NotNull] public static GGUIWndMuseumItemGet instance { get { return _g_instance ??= new GGUIWndMuseumItemGet(); } }
        private static GGUIWndMuseumItemGet _g_instance;


        private MuseumItemInfo _m_itemInfo;
        private NPGGuiWndTexture _m_iconTexture;


        public GGUIWndMuseumItemGet()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMuseumItemGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMuseumItemGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconTexture?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconTexture?.discard();
            _m_iconTexture = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgMuseumItemIcon != null)
                _m_iconTexture = new NPGGuiWndTexture(wnd.imgMuseumItemIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd([NotNull] MuseumItemInfo _itemInfo)
        {
            _m_itemInfo = _itemInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_itemInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtMuseumItemName, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtMuseumItemDesc, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.desc));
            _m_iconTexture?.setTexture(_m_itemInfo.refObj.icon);
        }


        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MUSEUM_ITEM_GET);
        }
    }
}
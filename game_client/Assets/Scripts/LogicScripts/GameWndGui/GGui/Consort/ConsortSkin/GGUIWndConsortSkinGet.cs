using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤获取弹窗
    /// </summary>
    public class GGUIWndConsortSkinGet : _ANPGGUIBasicWnd<GGUIMonoConsortSkinGet>
    {
        private static GGUIWndConsortSkinGet _g_instance;
        public static GGUIWndConsortSkinGet instance { get { return _g_instance ??= new GGUIWndConsortSkinGet(); } }
        
        private GConsortSkinInfo _m_iConsortSkinInfo;
        
        private GGUIWndConsortSkinItem _m_wSkinItem;
        
        public GGUIWndConsortSkinGet() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortSkinGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortSkinGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoSkinItem != null)
                _m_wSkinItem = new GGUIWndConsortSkinItem(wnd.monoSkinItem);
        }
        
        protected override void _onDiscard()
        {
            _m_wSkinItem?.discard();
            _m_wSkinItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSkinItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSkinItem?.resetWnd();
        }

        public void setData(GConsortSkinInfo _skinInfo)
        {
            _m_iConsortSkinInfo = _skinInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortSkinInfo == null)
                return;

            if (string.IsNullOrEmpty(wnd.txtGainSkinTipKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainSkinTip, TextTranslate.instance.getLanguage(GCommon.getItemName(ENPItemType.CONSORT_SKIN, _m_iConsortSkinInfo.skinId)));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtGainSkinTip,
                    TextTranslate.instance.getLanguage(wnd.txtGainSkinTipKey,
                        GCommon.getItemName(ENPItemType.CONSORT_SKIN, _m_iConsortSkinInfo.skinId)));
            }

            if (_m_wSkinItem != null)
            {
                _m_wSkinItem.showWnd();
                _m_wSkinItem.setInfo(_m_iConsortSkinInfo.skinRefObj);
            }
        }
    }
}
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndBusinessBuildingProductGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoBusinessBuildingProductGridItem>
    {
        private BusinessBuildingProductRefObj _m_productRef;
        private NPGGuiWndTexture _m_productIcon;
        private NPGGuiWndTexture _m_productIcon2;
        private float _m_outputProgressTime;
        

        public GGUISubWndBusinessBuildingProductGridItem(GGUIMonoBusinessBuildingProductGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_productIcon?.showWnd();
            _m_productIcon2?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_productIcon?.hideWnd();
            _m_productIcon2?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_productIcon?.discardTexture();
            _m_productIcon2?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_productIcon?.discard();
            _m_productIcon2?.discard();
            _m_productIcon = null;
            _m_productIcon2 = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgProductIcon != null)
                _m_productIcon = new NPGGuiWndTexture(wnd.imgProductIcon);
            if (wnd.imgProductIcon2 != null)
                _m_productIcon2 = new NPGGuiWndTexture(wnd.imgProductIcon2);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(BusinessBuildingProductRefObj _productRef)
        {
            _m_productRef = _productRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_productRef == null)
                return;
            
            _m_productIcon?.setTexture(_m_productRef.icon);
            _m_productIcon2?.setTexture(_m_productRef.icon);
            if (wnd.sldOutputProgress != null)
            {
                wnd.sldOutputProgress.minValue = 0;
                wnd.sldOutputProgress.maxValue = _m_productRef.fake_output_time;
            }
            
            _refreshProgress();
        }
        public void update()
        {
            if (wnd == null || _m_productRef == null || !_m_bIsShow || !isShow)
                return;

            float progressTime = _m_outputProgressTime;
            _refreshProgress();

            if (wnd.transOutputPoint != null && progressTime > _m_outputProgressTime)
            {
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.uiCamera, wnd.transOutputPoint.position);
                NPGTextureIndex expIcon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.SILVER);
                string text = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_productRef.fake_output_count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                NPGUIAddSceneCenterTip.instance.showIconTextTip(expIcon, text, wnd.outputTipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); });
            }
        }


        private void _refreshProgress()
        {
            if (wnd == null || _m_productRef == null)
                return;

            _m_outputProgressTime = Mathf.Repeat(Time.time + _m_productRef.fake_output_time_offset, _m_productRef.fake_output_time);
            if (wnd.sldOutputProgress != null)
                wnd.sldOutputProgress.value = _m_outputProgressTime;
        }
    }
}
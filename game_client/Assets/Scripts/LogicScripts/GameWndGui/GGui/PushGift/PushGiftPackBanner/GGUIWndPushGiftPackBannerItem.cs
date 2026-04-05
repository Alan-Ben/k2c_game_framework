using ALPackage;

namespace GOE
{
    /// <summary>
    /// 推送礼包BannerItem窗口
    /// </summary>
    public class GGUIWndPushGiftPackBannerItem : _ATNPGGUIWndSelectContainerItem<GGUIMonoPushGiftPackBannerItem>
    {
        // 当前推送礼包信息
        private PushGiftPackInfo _m_pushGiftPackInfo;
        
        // Banner图片纹理组件
        private NPGGuiWndTexture _m_wBannerImage;
        
        /// <summary>
        /// 推送礼包信息
        /// </summary>
        public PushGiftPackInfo pushGiftPackInfo => _m_pushGiftPackInfo;
        
        public GGUIWndPushGiftPackBannerItem(GGUIMonoPushGiftPackBannerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
            _m_wBannerImage?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wBannerImage?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_wBannerImage?.discard();
            _m_wBannerImage = null;
            
            _m_pushGiftPackInfo = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;
            
            // 构建Banner图片纹理组件
            if (wnd.bannerImage != null)
                _m_wBannerImage = new NPGGuiWndTexture(wnd.bannerImage);
        }
        
        /// <summary>
        /// 设置推送礼包信息
        /// </summary>
        /// <param name="_pushGiftPackInfo">推送礼包信息</param>
        public void setInfo(PushGiftPackInfo _pushGiftPackInfo)
        {
            _m_pushGiftPackInfo = _pushGiftPackInfo;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            if (_m_pushGiftPackInfo == null)
                return;
            
            // 获取推送礼包配表数据
            PushGiftPackRefObj pushGiftPackRef = _m_pushGiftPackInfo.pushGiftPackRefObj;
            if (pushGiftPackRef == null)
                return;
            
            // 刷新Banner图片
            if (_m_wBannerImage != null)
            {
                _m_wBannerImage.showWnd();
                _m_wBannerImage.setTexture(pushGiftPackRef.banner_img);
            }
            
            // 刷新礼包名称文本
            GiftPackRefObj giftPackRef = pushGiftPackRef.giftPackRefObj;
            string giftPackName = string.Empty;
            if (giftPackRef != null)
            {
                giftPackName = TextTranslate.instance.getLanguage(giftPackRef.name, giftPackRef.name_args);
            }

            if (wnd.txtPushGiftNameList != null)
            {
                foreach (var txtPushGiftName in wnd.txtPushGiftNameList)
                {
                    ALUGUICommon.setLabelTxt(txtPushGiftName, giftPackName);
                }
            }
            if (wnd.tmpPushGiftNameList != null)
            {
                foreach (var tmpPushGiftName in wnd.tmpPushGiftNameList)
                {
                    ALUGUICommon.setLabelTxt(tmpPushGiftName, giftPackName);
                }
            }
        }
    }
}
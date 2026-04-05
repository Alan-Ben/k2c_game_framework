using ALPackage;

namespace GOE
{
    /// <summary>
    /// 玩家buff item显示
    /// </summary>
    public class GGUIWndPlayerBuffItem : _ANPGGUIBasicSubWnd<GGUIMonoPlayerBuffItem>
    {
        private NPPlayerBuffInfo _m_iBuffInfo; // buff信息
        
        private NPGGuiWndTexture _m_wBuffIcon; // buff图标窗口
        
        public GGUIWndPlayerBuffItem(GGUIMonoPlayerBuffItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            // 初始化buff图标窗口
            if (wnd.buffIcon != null)
                _m_wBuffIcon = new NPGGuiWndTexture(wnd.buffIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_iBuffInfo = null;
            
            // 销毁buff图标窗口
            _m_wBuffIcon?.discard();
            _m_wBuffIcon = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏buff图标
            _m_wBuffIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置buff图标
            _m_wBuffIcon?.discardTexture();
        }
        
        /// <summary>
        /// 设置buff数据
        /// </summary>
        /// <param name="buffInfo">buff信息</param>
        public void setData(NPPlayerBuffInfo buffInfo)
        {
            _m_iBuffInfo = buffInfo;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            // 添加空值检查
            if (wnd == null || !isShow || _m_iBuffInfo == null || _m_iBuffInfo.refObj == null)
                return;

            string txtBuffNameKey = string.IsNullOrEmpty(wnd.txtBuffNameKey) ? TransKeyConst.common_value : wnd.txtBuffNameKey;
            // 设置buff名称
            ALUGUICommon.setLabelTxt(wnd.txtBuffName, TextTranslate.instance.getLanguage(txtBuffNameKey, _m_iBuffInfo.refObj.name));
            
            string txtBuffDescKey = string.IsNullOrEmpty(wnd.txtBuffDescKey) ? TransKeyConst.common_value : wnd.txtBuffDescKey;
            // 设置buff描述
            ALUGUICommon.setLabelTxt(wnd.txtBuffDesc, TextTranslate.instance.getLanguage(txtBuffDescKey, _m_iBuffInfo.refObj.desc));
            
            string txtBuffLayerKey = string.IsNullOrEmpty(wnd.txtBuffLayerKey) ? TransKeyConst.common_value : wnd.txtBuffLayerKey;
            // 设置buff层数
            ALUGUICommon.setLabelTxt(wnd.txtBuffLayer, TextTranslate.instance.getLanguage(txtBuffLayerKey, _m_iBuffInfo.layer));
            
            // 设置buff图标
            if (_m_wBuffIcon != null)
            {
                _m_wBuffIcon.showWnd();
                _m_wBuffIcon.setTexture(_m_iBuffInfo.refObj.icon);
            }
        }
    }
}
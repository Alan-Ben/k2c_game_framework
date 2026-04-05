using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就页签item
    /// </summary>
    public class GGUIWndAchieveTypeTabContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoAchieveTypeTabContainerItem, GGUIWndAchieveTypeTabContainerItem>
    {
        //成就类型数据
        private AchieveTypeRefObj _m_typeRef;
        //图标
        private NPGGuiWndTexture _m_wIcon;

        /// <summary>
        /// 成就类型数据
        /// </summary>
        public AchieveTypeRefObj typeRef { get { return _m_typeRef; } }

        public GGUIWndAchieveTypeTabContainerItem(GGUIMonoAchieveTypeTabContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(AchieveTypeRefObj _type)
        {
            _m_typeRef = _type;
            _refreshWnd();
            refreshRedTip();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            if (wnd == null || _m_typeRef == null)
                return;

            //刷新红点
            _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_m_typeRef?.red_tip_id ?? 0);
            bool haveRedTip = redTipNode != null && redTipNode.needShow();
            ALUGUICommon.setGameObjEnable(wnd.goRedTip, haveRedTip);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_typeRef == null)
                return;

            //刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_typeRef.icon);
            }

            //刷新名字
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_typeRef.title));

        }
    }
}

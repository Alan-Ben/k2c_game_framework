using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴带半身像和sp_icon的品质背景的item
    /// </summary>
    public class GGUIWndHeroSpIconItem : _ATALBasicUISubWnd<GGUIMonoHeroSpIconItem>
    {
        //伙伴图标
        private NPGGuiWndTexture _m_wIcon;
        //伙伴图标背景
        private GGuiWndSprite _m_wSpIconBg;
        //伙伴配置信息
        private _IHeroCardShow _m_heroShowData;

        public GGUIWndHeroSpIconItem(GGUIMonoHeroSpIconItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wSpIconBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wSpIconBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            _m_wSpIconBg?.discard();
            _m_wSpIconBg = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            if (null != wnd.imgIcon)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgSpIconBg != null)
                _m_wSpIconBg = new GGuiWndSprite(wnd.imgSpIconBg);
        }

        /// <summary>
        /// 显示伙伴item
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void setInfo(_IHeroCardShow _heroInfo)
        {
            _m_heroShowData = _heroInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_heroShowData || null == _m_heroShowData.heroRefObj)
                return;

            //设置伙伴名称
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_heroShowData.heroRefObj.transName);

            //设置伙伴半身像
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_heroShowData.getCardImage());

            //设置伙伴特殊图片背景
            if (_m_wSpIconBg != null)
            {
                _m_wSpIconBg.showWnd();
                _m_wSpIconBg.setTexture(GCommon.getItemQualitySpIcon(ENPItemType.HERO, _m_heroShowData.id));
            }

            //实力
            ALUGUICommon.setLabelTxt(wnd.txtPower, _m_heroShowData.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
    }
}

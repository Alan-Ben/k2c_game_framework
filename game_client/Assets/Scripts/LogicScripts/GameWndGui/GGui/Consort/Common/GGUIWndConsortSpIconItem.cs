using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 家人带半身像和sp_icon的品质背景的item
    /// </summary>
    public class GGUIWndConsortSpIconItem : _ATALBasicUISubWnd<GGUIMonoConsortSpIconItem>
    {
        //家人图标
        private NPGGuiWndTexture _m_wIcon;
        //家人图标背景
        private GGuiWndSprite _m_wSpIconBg;
        //家人配置信息
        private _IConsortShowInfo _m_consortShowData;

        public GGUIWndConsortSpIconItem(GGUIMonoConsortSpIconItem _wnd) : base(_wnd)
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
        /// 显示家人item
        /// </summary>
        /// <param name="_consortInfo"></param>
        public void setInfo(_IConsortShowInfo _consortInfo)
        {
            _m_consortShowData = _consortInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_consortShowData)
                return;

            //设置家人名称
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_consortShowData.consortTransName);

            //设置家人半身像
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_consortShowData.consortSkinShowInfo?.consortCardImage);

            //设置家人特殊图片背景
            if (_m_wSpIconBg != null)
            {
                _m_wSpIconBg.showWnd();
                _m_wSpIconBg.setTexture(GCommon.getItemQualitySpIcon(ENPItemType.CONSORT, _m_consortShowData.consortId));
            }
        }
    }
}

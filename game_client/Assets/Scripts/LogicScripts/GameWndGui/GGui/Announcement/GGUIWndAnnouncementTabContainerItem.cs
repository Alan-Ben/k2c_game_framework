
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 运营公告页签列表item
    /// </summary>
    public class GGUIWndAnnouncementTabContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoAnnouncementTabContainerItem, GGUIWndAnnouncementTabContainerItem>
    {
        //运营公告页签图标
        private NPGGuiWndTexture _m_wTabIcon;
        //当前item下标
        private int _m_itemIdx;
        //运营公告信息
        private AnnouncementShowInfo _m_showInfo;


        /// <summary>
        /// 运营公告展示信息
        /// </summary>
        public AnnouncementShowInfo showInfo { get => _m_showInfo; }
        /// <summary>
        /// 当前下标
        /// </summary>
        public int itemIdx { get => _m_itemIdx; }

        public GGUIWndAnnouncementTabContainerItem(GGUIMonoAnnouncementTabContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
            _m_wTabIcon?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_wTabIcon?.discard();
            _m_wTabIcon = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (null == wnd)
                return;

            if (null != wnd.imgIcon)
                _m_wTabIcon = new NPGGuiWndTexture(wnd.imgIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_showInfo"></param>
        public void setInfo(AnnouncementShowInfo _showInfo, int _index)
        {
            _m_itemIdx = _index;
            _m_showInfo = _showInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中相关显示
        /// </summary>
        /// <param name="_isSelect"></param>
        public override void setSelectShow(bool _isSelect)
        {
            base.setSelectShow(_isSelect);
            //如果选中设置已读
            if(_isSelect)
                _m_showInfo?.setIsRead();

            //刷新红点
            _refreshRedTip();
        }

        /// <summary>
        /// 刷新图标
        /// </summary>
        public void refreshIcon()
        {
            if (wnd == null || _m_showInfo == null)
                return;

            //先设置默认图片
            AnnouncementTabRefObj tabRef = GRefdataCoreMgr.instance.announcementTabRefCore.getRef(_m_showInfo.defaultIconId);
            if (tabRef != null)
            {
                _m_wTabIcon?.showWnd();
                _m_wTabIcon?.setTexture(tabRef.tab_image);
            }

            //读取运营后台配置的图片
            _m_showInfo.getIconImageBytes(_imageByte =>
            {
                if (wnd == null || !isShow || _imageByte == null)
                    return;

                //有获取到图片数据，转换成图片展示
                int imgWidth = 0;
                int imgHeight = 0;
                if (wnd.imgIcon != null && wnd.imgIcon.rectTransform != null && wnd.imgIcon.rectTransform.rect != null)
                {
                    imgWidth = (int)wnd.imgIcon.rectTransform.rect.width;
                    imgHeight = (int)wnd.imgIcon.rectTransform.rect.height;
                }
                Texture2D downloadTexture = GCommon.getTextureByBytes(imgWidth, imgHeight, _imageByte);
                if (downloadTexture != null)
                {
                    _m_wTabIcon?.showWnd();
                    _m_wTabIcon?.setTexture(downloadTexture);
                }
            });
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_showInfo == null)
                return;

            //获取公告内容
            AnnouncementContent announcementContent = _m_showInfo.getCurLanguageContent();
            //设置文本
            if (announcementContent != null)
                ALUGUICommon.setLabelTxt(wnd.txtContent, announcementContent.icon_text);

            //刷新图标
            refreshIcon();
            //刷新红点
            _refreshRedTip();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (wnd == null || _m_showInfo == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, !_m_showInfo.isRead());
        }
    }
}

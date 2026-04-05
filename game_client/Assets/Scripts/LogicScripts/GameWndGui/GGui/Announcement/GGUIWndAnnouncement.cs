using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 运营公告弹窗
    /// </summary>
    public class GGUIWndAnnouncement : _ATALBasicUIWnd<GGUIMonoAnnouncement>
    {
        private static GGUIWndAnnouncement _g_instance;
        public static GGUIWndAnnouncement instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndAnnouncement();
                }
                return _g_instance;
            }
        }

        //页签列表
        private GGUIWndAnnouncementTabContainer _m_wTabContainer;
        //banner图片
        private NPGGuiWndTexture _m_wBannerImage;
        //当前选中的页签
        private GGUIWndAnnouncementTabContainerItem _m_wCurSelectItem;
        //公告列表
        List<AnnouncementShowInfo> _m_announcementList;

        public GGUIWndAnnouncement() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoAnnouncement.assetPath; }
        protected override string _monoObjName { get => GGUIMonoAnnouncement.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        

        protected override void _onShowWnd()
        {
            _setInfo();

            //记录已经展示
            AnnouncementMgr.instance.setIsShow();
        }

        protected override void _onHideWnd()
        {
            if (_m_wTabContainer != null)
                _m_wTabContainer.hideWnd();

            if (_m_wBannerImage != null)
                _m_wBannerImage.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wTabContainer != null)
                _m_wTabContainer.resetWnd();

            if (_m_wBannerImage != null)
                _m_wBannerImage.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;

            _m_wBannerImage?.discard();
            _m_wBannerImage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.uncombineBtnClick(wnd.btnIKnow, _onClickTKnow);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_announcementList = new List<AnnouncementShowInfo>();

            if (null != wnd.imgBanner)
                _m_wBannerImage = new NPGGuiWndTexture(wnd.imgBanner);

            if (null != wnd.consortItemContainer)
            {
                _m_wTabContainer = new GGUIWndAnnouncementTabContainer(wnd.consortItemContainer);
                _m_wTabContainer.onSelectItemChg += _onClickTabItem;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.combineBtnClick(wnd.btnIKnow, _onClickTKnow);
        }

        /// <summary>
        /// 重置选中的item
        /// </summary>
        public void resetSelect()
        {
            _m_wCurSelectItem = null;
        }

        //设置信息
        private void _setInfo()
        {
            if (_m_announcementList == null)
                _m_announcementList = new List<AnnouncementShowInfo>();
            _m_announcementList.Clear();

            //获取运营公告列表
            AnnouncementMgr.instance.getValidAnnouncementList(_m_announcementList);

            //刷新页签列表
            _refreshContainer();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBanner();
            _refreshContent();
        }

        //刷新banner图片
        private void _refreshBanner()
        {
            if (wnd == null || _m_wCurSelectItem == null || _m_wCurSelectItem.showInfo == null || _m_wCurSelectItem.showInfo == null)
                return;

            //先设置默认图片，没有的话隐藏
            AnnouncementBannerRefObj tabRef = GRefdataCoreMgr.instance.announcementBannerRefCore.getRef(_m_wCurSelectItem.showInfo.defaultBannerId);
            if (tabRef != null)
            {
                _m_wBannerImage?.showWnd();
                _m_wBannerImage?.setTexture(tabRef.banner_image);
            }
            else
            {
                _m_wBannerImage?.hideWnd();
            }

            //读取运营后台配置的图片
            _m_wCurSelectItem.showInfo.getBannerImageBytes(_imageByte =>
            {
                if (wnd == null || !isShow || _imageByte == null)
                    return;

                //有获取到图片数据，转换成图片展示
                int imgWidth = 0;
                int imgHeight = 0;
                if (wnd.imgBanner != null && wnd.imgBanner.rectTransform != null && wnd.imgBanner.rectTransform.rect != null)
                {
                    imgWidth = (int)wnd.imgBanner.rectTransform.rect.width;
                    imgHeight = (int)wnd.imgBanner.rectTransform.rect.height;
                }
                Texture2D downloadTexture = GCommon.getTextureByBytes(imgWidth, imgHeight, _imageByte);
                if (downloadTexture != null)
                {
                    _m_wBannerImage?.showWnd();
                    _m_wBannerImage?.setTexture(downloadTexture);
                }
            });
        }

        //刷新运营公告页签列表
        private void _refreshContainer()
        {
            if (wnd == null)
                return;

            //控制没有公告显隐
            ALUGUICommon.setGameObjEnable(wnd.noAnnouncementShowList, _m_announcementList.Count <= 0);
            ALUGUICommon.setGameObjEnable(wnd.noAnnouncementHideList, _m_announcementList.Count > 0);

            //列表设置
            if (_m_wTabContainer != null)
            {
                //这里记录上次选择的id，避免跳转回来后重置选择
                long customId = 0;
                if (_m_wCurSelectItem != null && _m_wCurSelectItem.showInfo != null)
                    customId  = _m_wCurSelectItem.showInfo.id;
                _m_wCurSelectItem = null;

                _m_wTabContainer.showWnd();
                _m_wTabContainer.showItemList(_m_announcementList, customId);
            }
        }

        //刷新显隐状态
        private void _refreshContent()
        {
            if (wnd == null || _m_wCurSelectItem == null || _m_wCurSelectItem.showInfo == null)
                return;

            AnnouncementContent curContent = _m_wCurSelectItem.showInfo.getCurLanguageContent();
            if (curContent == null)
                return;

            //设置标题、文本、页数
            ALUGUICommon.setLabelTxt(wnd.txtTitle, curContent.title);
            ALUGUICommon.setLabelTxt(wnd.txtContent, curContent.content);
            ALUGUICommon.setLabelTxt(wnd.txtTabPage, TextTranslate.instance.getLanguage(TransKeyConst.common_pageNum_num_num, _m_wCurSelectItem.itemIdx + 1, _m_wTabContainer?.totalTabCount));

            //设置按钮
            ALUGUICommon.setGameObjEnable(wnd.webUrlJumpShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.gameInsideJumpShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.noJumpShowList, false);

            if (!string.IsNullOrEmpty(curContent.redirect_url))
                ALUGUICommon.setGameObjEnable(wnd.webUrlJumpShowList, true);//外链前往
            else
            {
                AnnouncementJumpRefObj jumpRef = GRefdataCoreMgr.instance.announcementJumpRefCore.getRef(curContent.redirect_to_game);
                if(jumpRef != null && jumpRef.jump_effect_str != null && !jumpRef.jump_effect_str.isEmpty && (jumpRef.jump_condition == null || jumpRef.jump_condition.isEmpty || jumpRef.jump_condition.IsEnable(null)))
                    ALUGUICommon.setGameObjEnable(wnd.gameInsideJumpShowList, true);//内链前往
                else
                    ALUGUICommon.setGameObjEnable(wnd.noJumpShowList, true);//我知道了
            }

            //将内容文本滚动到顶部
            if (wnd.scContent != null)
                wnd.scContent.verticalNormalizedPosition = 1;
        }

        #region 点击按钮

        //点击页签
        private void _onClickTabItem(GGUIWndAnnouncementTabContainerItem _item)
        {
            if (_m_wCurSelectItem == _item)
                return;

            _m_wCurSelectItem = _item;
            _refreshWnd();
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ANNOUNCEMENT_NODE);
        }

        //点击前往
        private void _onClickGoTo(GameObject _go)
        {
            if (_m_wCurSelectItem == null || _m_wCurSelectItem.showInfo == null)
            {
                //联系客服了解更多!
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.announcement_contactCustomerServiceTip_none);
                return;
            }

            AnnouncementContent curContent = _m_wCurSelectItem.showInfo.getCurLanguageContent();
            if (curContent == null)
            {
                //联系客服了解更多!
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.announcement_contactCustomerServiceTip_none);
                return;
            }

            if (!string.IsNullOrEmpty(curContent.redirect_url))
            {
                //跳转外链
                GCommon.openURL(curContent.redirect_url);
            }
            else
            {
                //游戏内效果跳转
                AnnouncementJumpRefObj jumpRef = GRefdataCoreMgr.instance.announcementJumpRefCore.getRef(curContent.redirect_to_game);
                if (jumpRef == null || jumpRef.jump_effect_str == null || jumpRef.jump_effect_str.isEmpty)
                {
                    //联系客服了解更多!
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.announcement_contactCustomerServiceTip_none);
                    return;
                }

                //条件通过执行跳转效果
                if (jumpRef.jump_condition == null || jumpRef.jump_condition.isEmpty || jumpRef.jump_condition.IsEnable(null))
                    jumpRef.jump_effect_str.dealEffect();
            }
        }

        //点击我知道了
        private void _onClickTKnow(GameObject _go)
        {
            long nextId = 0;
            if (_m_announcementList != null && _m_announcementList.Count > 0)
            {
                for (int i = 0; i < _m_announcementList.Count; i++)
                {
                    if (_m_announcementList[i] != null && !_m_announcementList[i].isRead())
                    {
                        nextId = _m_announcementList[i].id;
                        break;
                    }
                }
            }

            //如果有未读的则选中未读的公告，否则直接关闭窗口
            if (nextId != 0)
            {
                if(_m_wTabContainer != null)
                    _m_wTabContainer.setSelectByAnnouncementId(nextId);
            }
            else
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ANNOUNCEMENT_NODE);
            }
        }

        #endregion
    }
}
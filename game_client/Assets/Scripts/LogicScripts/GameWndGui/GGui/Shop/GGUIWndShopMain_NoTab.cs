using UnityEngine;
using ALPackage;

namespace GOE
{
    // 商店主界面
    public class GGUIWndShopMain_NoTab : _ANPGGUIBasicWnd<GGUIMonoShopMain_NoTab>
    {
        private static GGUIWndShopMain_NoTab _g_instance;

        public static GGUIWndShopMain_NoTab instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShopMain_NoTab();
                return _g_instance;
            }
        }

        //视图列表
        private GGUIWndShopNormalPage _m_shopNormalPageList;
        private long _m_currentShopMainRefId;

        protected GGUIWndShopMain_NoTab()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        /// <summary>
        /// show 动画是否只播放一次
        /// </summary>
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoShopMain_NoTab.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoShopMain_NoTab.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;
        }

        protected override void _onHideWnd()
        {
            if (null == wnd)
                return;
            _m_shopNormalPageList?.hideWnd();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_shopNormalPageList?.discard();
            _m_shopNormalPageList = null;
            _m_currentShopMainRefId = 0;
        }

        /// <summary>
        /// 设置显示的商店
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(long _shopMainRefId, long _targetShopId)
        {
            //样式一致则不处理
            if (_m_currentShopMainRefId == _shopMainRefId)
            {
                _m_shopNormalPageList?.showWnd();
                return;
            }

            _m_currentShopMainRefId = _shopMainRefId;
            ShopMainRefObj shopMainRef = GRefdataCoreMgr.instance.shopMainRefCore.getRef(_shopMainRefId);
            if (shopMainRef == null)
                return;

            //如果没有达到解锁条件 弹窗tip
            if (!shopMainRef.unlock_cond.IsEnable(null))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TextTranslate.instance.getLanguage(shopMainRef.unlock_cond_desc, shopMainRef.unlock_cond_desc_args));
                return;
            }
            //刷新
            _refreshPage(_targetShopId);
        }

        /// <summary>
        /// 刷新显示处理
        /// </summary>
        private void _refreshPage(long _targetShopId = 0)
        {
            _m_shopNormalPageList?.discard();
            _m_shopNormalPageList = null;
            
            _m_shopNormalPageList =  _getShopNormalPage(_m_currentShopMainRefId);
            //如果无对象则创建对象
            _m_shopNormalPageList?.load(() =>
            {
                _m_shopNormalPageList?.showWnd();
                _m_shopNormalPageList?.setShopMainId(_m_currentShopMainRefId, _targetShopId);
            });
        }

        /// <summary>
        /// 根据商店配置获取UI实例
        /// </summary>
        private GGUIWndShopNormalPage _getShopNormalPage(long _shopMainRefId)
        {
            ShopMainRefObj shopMainRef = GRefdataCoreMgr.instance.shopMainRefCore.getRef(_shopMainRefId);
            if (wnd == null || shopMainRef == null)
                return null;

            return new GGUIWndShopNormalPage(shopMainRef.no_tab_ui_path_id, wnd.pageParentPos);
        }

        #region 点击事件

        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Shop.C_MAIN_SHOP_NO_TAB_NODE);
        }

        #endregion
    }
}
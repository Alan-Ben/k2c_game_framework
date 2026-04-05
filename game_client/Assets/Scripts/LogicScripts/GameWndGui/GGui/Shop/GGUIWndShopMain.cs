using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 商店主界面
    public class GGUIWndShopMain : _ANPGGUIBasicWnd<GGUIMonoShopMain>
    {

        private static GGUIWndShopMain _g_instance;
        public static GGUIWndShopMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShopMain();
                return _g_instance;
            }
        }

        //当前选中的shop_main表id
        private long _m_eCurrentShopMainId;
        //tab 列表
        private List<GGUIWndShopTab> _m_tabList;
        //当前选中的tab
        private GGUIWndShopTab _m_curSelectTab;

        //用于管理4个子窗口的容器
        private ALEmptyContainerScene _m_cSubTabWndContainer;
        //视图列表
        private List<GGUIWndShopNormalPage> _m_shopNormalPageList;
        // //需要在隐藏页面时才刷新红点的商店id
        // private long _m_lPageHideRefreshRedTipShopId;
        protected GGUIWndShopMain()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /// <summary>
        /// show 动画是否只播放一次
        /// </summary>
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoShopMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoShopMain.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //创建容器Scene
            _m_cSubTabWndContainer = new ALEmptyContainerScene(0);
            _m_cSubTabWndContainer.enterScene();

            //创建tab
            _m_tabList = new List<GGUIWndShopTab>();
            if (null != wnd.monoTabList)
            {
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    GGUIMonoShopTab tabMono = wnd.monoTabList[i];
                    if (null == tabMono)
                        continue;

                    GGUIWndShopTab tabWnd = new GGUIWndShopTab(tabMono);
                    tabWnd.refreshWnd(false);
                    tabWnd.onClickTab += _tabDidClick;

                    _m_tabList.Add(tabWnd);
                }
            }

            _m_shopNormalPageList = new List<GGUIWndShopNormalPage>();

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;

            //设置默认商店
            if (_m_eCurrentShopMainId == 0)
                _m_eCurrentShopMainId = wnd.defaultShopMainRefId;

            _refreshAll();

            //播放选中动画
            for (int i = 0; i < _m_tabList.Count; i++)
            {
                GGUIWndShopTab tab = _m_tabList[i];
                if (null == tab)
                    continue;
                
                tab.playIsShowAni(true);
            }

            WinMsg.RegisterMsg(WinMsgType.SHOP_CHG, _onShopChg);//商店变更
            WinMsg.RegisterMsg(WinMsgType.ON_SHOP_RED_TIP_CHG, _onShopRedTipChg);//商店红点变更
        }

        protected override void _onHideWnd()
        {
            if (null == wnd)
                return;

            if (null != _m_cSubTabWndContainer)
                _m_cSubTabWndContainer.hideScene(null);

            // _m_lPageHideRefreshRedTipShopId = 0;
            WinMsg.UnregisterMsg(WinMsgType.SHOP_CHG, _onShopChg);//商店变更
            WinMsg.UnregisterMsg(WinMsgType.ON_SHOP_RED_TIP_CHG, _onShopRedTipChg);//商店红点变更
        }

        protected override void _onReset()
        {

        }

        protected override void _onDiscard()
        {
            _m_eCurrentShopMainId = 0;
            _m_cSubTabWndContainer?.quitScene();
            _m_cSubTabWndContainer = null;

            //由于窗口是添加到容器_m_cSubTabWndContainer管理，因此这里窗口直接重置即可
            _m_shopNormalPageList?.Clear();
            _m_shopNormalPageList = null;

            if (null != _m_tabList)
            {
                foreach (GGUIWndShopTab shopTab in _m_tabList)
                {
                    shopTab?.discard();
                }
            }
            _m_tabList?.Clear();
            _m_tabList = null;

            if (null != wnd)
                ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        /// <summary>
        /// 设置显示的页签
        /// </summary>
        /// <param name="_type"></param>
        public bool setInfo(long _shopMainRefId, long _targetShopId, bool _refreshRedTipWhenPageHide = false)
        {
            // if (_refreshRedTipWhenPageHide)
            //     _m_lPageHideRefreshRedTipShopId = _shopRefId;
            // else
            //     _m_lPageHideRefreshRedTipShopId = 0;

            //样式一致则不处理
            if (_m_eCurrentShopMainId == _shopMainRefId)
                return false;

            ShopMainRefObj shopMainRef = GRefdataCoreMgr.instance.shopMainRefCore.getRef(_shopMainRefId);
            if (null == shopMainRef)
                return false;

            //如果没有达到解锁条件 弹窗tip
            if (!shopMainRef.unlock_cond.IsEnable(null))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TextTranslate.instance.getLanguage(shopMainRef.unlock_cond_desc, shopMainRef.unlock_cond_desc_args));
                return false;
            }

            _m_eCurrentShopMainId = _shopMainRefId;

            //刷新
            _refreshAll(_targetShopId);
            return true;
        }

        /// <summary>
        /// 根据下标获取商店item的RectTransform
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public RectTransform getShopItemRectTransformByIndex(int _index)
        {
            if(_m_cSubTabWndContainer == null)
                return null;

            GGUIWndShopNormalPage curPage = _m_cSubTabWndContainer.curShowWnd as GGUIWndShopNormalPage;
            if (curPage == null)
                return null;

            return curPage.getShopItemRectTransformByIndex(_index);
        }

        /// <summary>
        /// 刷新显示处理
        /// </summary>
        /// <param name="_type"></param>
        private void _refreshAll(long _targetShopId = 0)
        {
            if (null == wnd)
                return;

            //显示当前数据
            _refreshTab();
            _refreshPage(_targetShopId);
        }

        private void _refreshPage(long _targetShopId = 0)
        {
            GGUIWndShopNormalPage page = _getShopNormalPage(_m_eCurrentShopMainId);
            GGUIMonoShopTab tabMono = _getShopTabMono(_m_eCurrentShopMainId);
            //如果无对象则创建对象
            if (null == page)
            {
                if (null != wnd && null != tabMono)
                {
                    if (null == wnd.pageParentPos)
                    {
                        Debug.LogError("NPGGUIWndShopMain上 父节点没挂载");
                        return;
                    }

                    ShopMainRefObj shopMainRefObj = GRefdataCoreMgr.instance.shopMainRefCore.getRef(tabMono.shopMainRefId);
                    if (null == shopMainRefObj)
                    {
                        Debug.LogError($"商店主表里找不到id={tabMono.shopMainRefId}的配置");
                        return;
                    }
                    page = new GGUIWndShopNormalPage(shopMainRefObj.ui_path_id, wnd.pageParentPos);
                    _m_shopNormalPageList?.Add(page);
                }
            }

            _m_cSubTabWndContainer?.showMainWnd(page, () =>
            {
                if(tabMono != null)
                    page?.setShopMainId(tabMono.shopMainRefId, _targetShopId);
            });
        }

        private void _refreshTab()
        {
            for (int i = 0; i < _m_tabList.Count; i++)
            {
                if(_m_tabList[i] == null)
                    continue;

                GGUIWndShopTab tab = _m_tabList[i];
                tab.refreshWnd(tab.shopMainRefId == _m_eCurrentShopMainId);
                if (tab.shopMainRefId == _m_eCurrentShopMainId)
                    _m_curSelectTab = tab;

                // //如果当前选中的不是特殊红点，隐藏红点
                // if (_m_eCurrentShopMainId != _m_lPageHideRefreshRedTipShopId && _m_eCurrentShopMainId == tab.shopRefId)
                // {
                //     tab.showRedTipNum(0);
                //     continue;
                // }
                //
                // //设置红点
                // NPShopRefObj shopRef = GRefdataCoreMgr.instance.shopMap.getRef(tab.shopRefId);
                // if(shopRef == null)
                //     continue;
                // _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(shopRef.red_tip_id);
                // tab.showRedTipNum(redTipNode == null ? 0 : (int) redTipNode.getCount());
            }
        }

        /// <summary>
        /// 获取展示过的商店page
        /// </summary>
        private GGUIWndShopNormalPage _getShopNormalPage(long _shopMainRefId)
        {
            GGUIWndShopNormalPage temp = null;
            for (int i = 0; i < _m_shopNormalPageList.Count; i++)
            {
                temp = _m_shopNormalPageList[i];
                if (null == temp)
                    continue;
                if (temp.shopMainRefId == _shopMainRefId)
                    return temp;
            }
            return null;
        }

        /// <summary>
        /// 获取窗口中某一类型的TabMono
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        private GGUIMonoShopTab _getShopTabMono(long _shopMainRefId)
        {
            if (null != wnd.monoTabList)
            {
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    GGUIMonoShopTab tabMono = wnd.monoTabList[i];
                    if (tabMono.shopMainRefId == _shopMainRefId)
                        return tabMono;
                }
            }
            return null;
        }

        #region 点击事件

        //点击tab
        private void _tabDidClick(GGUIWndShopTab _tab)
        {
            //点击同个页签
            if (_m_curSelectTab != null && _tab != null && _tab.shopMainRefId == _m_curSelectTab.shopMainRefId)
                return;

            //进入一个操作节点
            QueueMgr.instance.addNode_OnlyOp(() => { 
                bool isSuc = setInfo(_tab.shopMainRefId, 0);
                if (!isSuc)
                    return;

                //切换成功再执行
                //重置红点
                if (_m_curSelectTab != null)
                {
                    // _m_curSelectTab.showRedTipNum(0);
                    _m_curSelectTab.playIsSelectAni(false);
                }

                // _tab.showRedTipNum(0);
                _tab.playIsSelectAni(true);
                _m_curSelectTab = _tab;
                
            }, UINodeTagConst.C_OP_Shop_Tab);
        }

        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Shop.C_MAIN_SHOP_NODE);
        }
        #endregion


        #region 消息事件

        //商店变更
        private void _onShopChg(params object[] _objects)
        {
            //处理当前选中的商店更新时展示红点
            // if (_objects == null || _objects.Length == 0)
            //     return;

            // long shopId = (long)_objects[0];
            // if (shopId > 0 && shopId == _m_eCurrentShopMainId)
            // {
            //     _m_lPageHideRefreshRedTipShopId = shopId;
            //     _m_curSelectTab?.showRedTipNum(1);
            // }
        }

        //商店红点数据变更
        private void _onShopRedTipChg(params object[] _objects)
        {
            //处理其他页签商店更新展示红点
            // if (_objects == null || _objects.Length == 0)
            //     return;
            //
            // long shopId = (long) _objects[0];
            // if (_m_tabList == null)
            //     return;
            //
            // for (int i = 0; i < _m_tabList.Count; i++)
            // {
            //     if (_m_tabList[i] != null && _m_tabList[i].shopRefId == shopId && _m_tabList[i].redTipNum == 0 && _m_eCurrentShopMainId != shopId)
            //     {
            //         NPShopRefObj shopRef = GRefdataCoreMgr.instance.shopMap.getRef(shopId);
            //         if (shopRef == null)
            //             break;
            //
            //         _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(shopRef.red_tip_id);
            //         _m_tabList[i].showRedTipNum(redTipNode == null ? 0 : (int)redTipNode.getCount());
            //         break;
            //     }
            // }
        }

        #endregion
    }
}

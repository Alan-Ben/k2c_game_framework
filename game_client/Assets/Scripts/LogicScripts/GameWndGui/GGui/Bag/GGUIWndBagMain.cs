using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包(页签+物品列表)
    public class GGUIWndBagMain : _ANPGGUIBasicResBarWnd<GGUIMonoBagMain>
    {
        // 单例
        private static GGUIWndBagMain _m_gInstance;
        public static GGUIWndBagMain instance
        {
            get
            {
                if (_m_gInstance == null)
                    _m_gInstance = new GGUIWndBagMain();
                return _m_gInstance;
            }
        }


        //页签列表
        private List<GGUIWndBagMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndBagMainTab _m_wSelectTabWnd;
        //背包界面
        private GGUIWndBagPage _m_bagPage;
        //可使用背包界面
        private GGUIWndBagPage _m_useBagPage;
        //道具合成界面
        private GGUIWndBagConvertPage _m_itemConvertPage;

        protected GGUIWndBagMain()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoBagMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagMain.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onItemChg);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onItemChg);
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;

            _hideAllPage();
        }

        protected override void _onReset()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndBagMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.resetWnd();
                }
            }

            if (_m_bagPage != null)
                _m_bagPage.resetWnd();

            if (_m_useBagPage != null)
                _m_useBagPage.resetWnd();

            if (_m_itemConvertPage != null)
                _m_itemConvertPage.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lTabWndList != null)
            {
                GGUIWndBagMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;

            if (_m_bagPage != null)
                _m_bagPage.discard();
            _m_bagPage = null;

            if (_m_useBagPage != null)
                _m_useBagPage.discard();
            _m_useBagPage = null;

            if (_m_itemConvertPage != null)
                _m_itemConvertPage.discard();
            _m_itemConvertPage = null;

            ALUGUICommon.uncombineBtnClick(wnd.returnBtn, _onReturnBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndBagMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIBagMainTabMono tempTabMono = null;
                GGUIWndBagMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndBagMainTab(tempTabMono.monoTab, tempTabMono.tabType, tempTabMono.resPathId);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            _m_wSelectTabWnd = null;

            ALUGUICommon.combineBtnClick(wnd.returnBtn, _onReturnBtnClick);
        }


        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_lTabWndList == null)
                return;

            for (int i = 0; i < _m_lTabWndList.Count; i++)
            {
                if (_m_lTabWndList[i] != null && _m_lTabWndList[i].tabType == wnd.defaultTab)
                {
                    _m_lTabWndList[i].setSelected(true);
                    _m_wSelectTabWnd = _m_lTabWndList[i];
                    _refreshTabView(wnd.defaultTab);
                }
            }
        }

        /// <summary>
        /// 设置选中tab
        /// </summary>
        public void setSelectTab(EBagMainTabMonoType _tabView)
        {
            if (_m_lTabWndList == null || (_m_wSelectTabWnd != null && _m_wSelectTabWnd.tabType == _tabView))
                return;
            
            _m_wSelectTabWnd?.setSelected(false);
            
            for (int i = 0; i < _m_lTabWndList.Count; i++)
            {
                if (_m_lTabWndList[i] != null && _m_lTabWndList[i].tabType == _tabView)
                {
                    _m_lTabWndList[i].setSelected(true);
                    _m_wSelectTabWnd = _m_lTabWndList[i];
                    _refreshTabView(_tabView);
                    break;
                }
            }
        }
        
        //点击切换页签按钮
        private void _onTabSelect(GGUIWndBagMainTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EBagMainTabMonoType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EBagMainTabMonoType.USE_ITEM:
                    _showUseBagPage();
                    break;
                case EBagMainTabMonoType.BAG_ITEM:
                    _showBagPage();
                    break;
                case EBagMainTabMonoType.BAG_CONVERT:
                    _showItemConvertPage();
                    break;
            }

            //设置红点已读
            _setReadRedTip();
        }
         
        //关闭所有页面
        private void _hideAllPage()
        {
            if (null != _m_bagPage)
                _m_bagPage.hideWnd();

            if (null != _m_useBagPage)
                _m_useBagPage.hideWnd();

            if (null != _m_itemConvertPage)
                _m_itemConvertPage.hideWnd();
        }

        //显示背包物品页面
        private void _showBagPage()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_bagPage != null)
            {
                _m_bagPage.showWnd();
            }
            else
            {
                _m_bagPage = new GGUIWndBagPage(_m_wSelectTabWnd.resPathId, wnd.pageParent, EBagMainTabMonoType.BAG_ITEM);
                _m_bagPage.load(_m_bagPage.showWnd);
            }
        }

        //显示可使用背包物品页面
        private void _showUseBagPage()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_useBagPage != null)
            {
                _m_useBagPage.showWnd();
            }
            else
            {
                _m_useBagPage = new GGUIWndBagPage(_m_wSelectTabWnd.resPathId, wnd.pageParent, EBagMainTabMonoType.USE_ITEM);
                _m_useBagPage.load(_m_useBagPage.showWnd);
            }
        }

        //显示合成页面
        private void _showItemConvertPage()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_itemConvertPage != null)
            {
                _m_itemConvertPage.showWnd();
            }
            else
            {
                _m_itemConvertPage = new GGUIWndBagConvertPage(_m_wSelectTabWnd.resPathId, wnd.pageParent);
                _m_itemConvertPage.load(_m_itemConvertPage.showWnd);
            }
        }

        //设置红点已读
        private void _setReadRedTip()
        {
            if (_m_wSelectTabWnd == null)
                return;

            //设置当前页签红点已读
            switch (_m_wSelectTabWnd.tabType)
            {
                case EBagMainTabMonoType.BAG_ITEM:
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_PAGE, 0);
                    break;
                case EBagMainTabMonoType.BAG_CONVERT:
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_CONVERT_PAGE, 0);
                    break;
                case EBagMainTabMonoType.USE_ITEM:
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_CAN_USE_PAGE, 0);
                    break;
            }
        }

        // 响应返回按钮点击事件
        private void _onReturnBtnClick(GameObject _btn)
        {
            // 返回时，清空小红点
            BagRedTipMgr.instance.clear();

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_BagNode);
        }
        
        public RectTransform getUseBagPageItemRectTransform(EGGUIMonoBagItemGridTargetItemType _targetItemType, string _paramsStr)
        {
            if (_m_useBagPage == null || !_m_useBagPage.isShow)
            {
                Debug.LogError($"[GGUIWndBagMain] 可使用背包页签未选中，无法获取物品RectTransform，类型: {_targetItemType}, 参数: {_paramsStr}");
                return null;
            }
            
            return _m_useBagPage.getRectTransformByTargetItemType(_targetItemType, _paramsStr);
        }

        /// <summary>
        /// 获取使用物品bar使用按钮的RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getUseBagPageUseSimpleBarUseBtnRectTransform()
        {
            if (_m_useBagPage == null || !_m_useBagPage.isShow)
            {
                Debug.LogError($"[GGUIWndBagMain] 可使用背包页签未选中，无法获取使用物品Bar使用按钮RectTransform");
                return null;
            }
            
            return _m_useBagPage.getUseBagPageUseSimpleBarUseBtnRectTransform();
        }

        /// <summary>
        /// 物品变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onItemChg(params object[] _objs)
        {
            //设置红点已读
            _setReadRedTip();
        }
    }
}

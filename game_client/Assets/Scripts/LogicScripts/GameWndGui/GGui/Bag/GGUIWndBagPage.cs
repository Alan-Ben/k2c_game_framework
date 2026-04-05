using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包(页签+物品列表)
    public class GGUIWndBagPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoBagPage>
    {
        private long _m_resPathId;

        // 选中的页签类型
        private EBagMainTabMonoType _m_eTabType;

        // 当前选中的二级页签
        private GGUIWndBagSecondTab _m_wSelectSecondTabItem = null;

        // 当前选中的一级页签
        private GGUIWndBagFirstTab _m_wSelectFirstTabItem = null;

        // 所有一级页签
        private List<GGUIWndBagFirstTab> _m_lTabList = null;

        // 物品容器
        private GGUIWndBagItemGrid _m_wItemGrid = null;

        public GGUIWndBagPage(long _resPathId, Transform _parent, EBagMainTabMonoType _tabType)
        : base(_parent)
        {
            _m_resPathId = _resPathId;
            _m_eTabType = _tabType;
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_resPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_resPathId); } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //隐藏一个页签时的Go
            if (null != wnd.oneTabHideGo && (null == wnd.monoTabList || (wnd.monoTabList != null && wnd.monoTabList.Count <= 1)))
                ALUGUICommon.setGameObjDisable(wnd.oneTabHideGo);

            // 创建grid对象
            if (wnd.itemGrid != null)
                _m_wItemGrid = new GGUIWndBagItemGrid(wnd.itemGrid);

            // 创建一级页签对象
            if (wnd.monoTabList != null)
            {
                _m_lTabList = new List<GGUIWndBagFirstTab>();
                GGUIBagTabFirstMono tempTabMono = null;
                GGUIWndBagFirstTab tempTabWnd = null;
                for (int i = 0, count = wnd.monoTabList.Count; i < count; i++)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;

                    tempTabWnd = new GGUIWndBagFirstTab(tempTabMono.monoTab, tempTabMono.secondMonoList, tempTabMono.itemTypeList, _onSecondTabSelect);

                    //隐藏二级页签
                    tempTabWnd.setSecondTabListEnable(false);
                    _m_lTabList.Add(tempTabWnd);

                    // 注册页签点击事
                    tempTabWnd.onClickTab = _onFirstTabSelect;

                    if (i == 0)
                    {
                        _onFirstTabSelect(tempTabWnd);
                    }
                }
            }
        }

        // 显示窗体
        protected override void _onShowWnd()
        {
            // 注册事件
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onAddItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onRemoveItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);

            if (_m_wItemGrid != null)
            {
                _m_wItemGrid.showWnd();
            }

            if (null != _m_wSelectFirstTabItem)
            {
                //先取消原来的选择
                GGUIWndBagFirstTab tab = _m_wSelectFirstTabItem;
                _m_wSelectFirstTabItem.setSelected(false);
                _m_wSelectFirstTabItem.setSecondTabListEnable(false);
                _m_wSelectFirstTabItem = null;
                _onFirstTabSelect(tab);
            }
            else if (null != _m_lTabList && _m_lTabList.Count > 0)
            {
                _onFirstTabSelect(_m_lTabList[0]);
            }

            // 刷新页签小红点
            refreshTabRedTip();

        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onAddItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onRemoveItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);

            if (_m_wItemGrid != null)
                _m_wItemGrid.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wItemGrid != null)
                _m_wItemGrid.resetWnd();
        }

        // 释放窗体
        protected override void _onDiscard()
        {
            if (_m_wItemGrid != null)
            {
                _m_wItemGrid.discard();
                _m_wItemGrid = null;
            }

            if (_m_lTabList != null)
            {
                for (int i = 0, count = _m_lTabList.Count; i < count; i++)
                {
                    _m_lTabList[i].discard();
                }
                _m_lTabList.Clear();
                _m_lTabList = null;
            }

            _m_wSelectFirstTabItem = null;
            _m_wSelectSecondTabItem = null;
        }

        /************
        * 点击一级页签按钮
        **/
        private void _onFirstTabSelect(GGUIWndBagFirstTab _tabItemWnd)
        {
            if (null == _tabItemWnd || (null != _m_wSelectFirstTabItem && _tabItemWnd == _m_wSelectFirstTabItem))
                return;

            //取消原来的选择
            if (null != _m_wSelectFirstTabItem)
            {
                _m_wSelectFirstTabItem.setSelected(false);
                _m_wSelectFirstTabItem.setSecondTabListEnable(false);
            }

            //设置新对象
            _m_wSelectFirstTabItem = _tabItemWnd;

            if (null != _m_wSelectFirstTabItem)
            {
                _m_wSelectFirstTabItem.setSelected(true);
                _m_wSelectFirstTabItem.setSecondTabListEnable(true);

            }

            //默认选中第一个二级页签
            if (null != _tabItemWnd.secondWndList && _tabItemWnd.secondWndList.Count > 0)
                _onSecondTabSelect(_tabItemWnd.secondWndList[0]);
            else
            {
                _m_wSelectSecondTabItem = null;
                // 显示物品列表
                if (_m_wItemGrid != null)
                {
                    _m_wItemGrid.setItemList(_tabItemWnd.itemTypeList, _m_eTabType);
                }
            }
        }

        /// <summary>
        /// 点击二级页签按钮
        /// </summary>
        /// <param name="_tabItemWnd"></param>
        private void _onSecondTabSelect(GGUIWndBagSecondTab _tabItemWnd)
        {
            if (null == _tabItemWnd || (null != _m_wSelectSecondTabItem && _m_wSelectSecondTabItem == _tabItemWnd))
                return;

            //取消原来的选择
            if (null != _m_wSelectSecondTabItem)
                _m_wSelectSecondTabItem.setSelected(false);

            //设置新对象
            _m_wSelectSecondTabItem = _tabItemWnd;

            // 该页签小红点清零
            BagRedTipMgr.instance.clearTabRedTipNum(_tabItemWnd.secondItemMono.secondItemTypeList);

            // 刷新页签小红点
            refreshTabRedTip();

            if (null != _m_wSelectSecondTabItem)
            {
                _m_wSelectSecondTabItem.setSelected(true);

                // 显示物品列表
                if (_m_wItemGrid != null && null != _m_wSelectSecondTabItem.secondItemMono)
                {
                    if (_m_wSelectSecondTabItem.secondItemMono.isShowTypeBar)
                    {
                        _m_wItemGrid.setItemListWithBar(_m_wSelectSecondTabItem.secondItemMono.secondItemTypeList, _m_wSelectSecondTabItem.secondItemMono.typeBarUiPathId);
                    }
                    else
                    {
                        _m_wItemGrid.setItemList(_m_wSelectSecondTabItem.secondItemMono.secondItemTypeList, _m_eTabType);
                    }
                }
            }
        }

        // 刷新物品
        private void _onUpdateItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow || _item == null)
                return;

            // 判断是否需要更新
            if (_needUpdateItem(_item) && _m_wItemGrid != null)
                _m_wItemGrid.updateItem(_item);
        }

        // 移除物品
        private void _onRemoveItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow || _item == null)
                return;

            // 判断是否需要更新
            if (_needUpdateItem(_item) && _m_wItemGrid != null)
                _m_wItemGrid.removeItem(_item);

            // 更新页签小红点
            refreshTabRedTip();
        }

        // 添加物品
        private void _onAddItem(params object[] _objs)
        {
            BagItem _item = (BagItem)_objs[0];
            if (!isShow)
                return;

            // 判断是否需要更新
            if (_needUpdateItem(_item) && _m_wItemGrid != null)
                _m_wItemGrid.addItem(_item);

            // 更新页签小红点
            refreshTabRedTip();
        }

        // 是否需要更新物品
        private bool _needUpdateItem(BagItem _item)
        {
            if (_item == null || _item.itemRefObj == null || _m_wSelectFirstTabItem == null)
                return false;

            // 若物品属于当前分类，则需更新
            return _m_wSelectFirstTabItem.itemTypeList.Contains(_item.bagItemType);
        }

        // 刷新页签小红点
        public void refreshTabRedTip()
        {
            GGUIWndBagFirstTab tab = null;
            for (int i = 0, count = _m_lTabList.Count; i < count; i++)
            {
                tab = _m_lTabList[i];
                if (null == tab)
                    continue;


                tab.refreshRedTip();
            }
        }

        public RectTransform getRectTransformByTargetItemType(EGGUIMonoBagItemGridTargetItemType _targetItemType, string _paramsStr)
        {
            return _m_wItemGrid?.getRectTransformByTargetItemType(_targetItemType, _paramsStr);
        }

        /// <summary>
        /// 获取使用物品bar使用按钮的RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getUseBagPageUseSimpleBarUseBtnRectTransform()
        {
            return _m_wItemGrid?.getUseBagPageUseSimpleBarUseBtnRectTransform();
        }
    }
}

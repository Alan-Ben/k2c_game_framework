using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店界面
    /// </summary>
    public class GGuiWndRegularEventShop : _AHotfixBaseWnd<GGUIMonoRegularEventShop>
    {
        //活动id
        private long _m_lActivityId;
        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //页签列表
        private List<GGuiWndRegularEventShopTab> _m_lTabWndList;
        //当前选中的页签
        private GGuiWndRegularEventShopTab _m_wSelectTabWnd;
        //商店列表
        private GGuiWndRegularEventShopContainer _m_wShopContainer;
        //仓库列表
        private GGuiWndRegularEventWarehouseContainer _m_wWarehouseContainer;
        //点击使用按钮回调
        private Action<RegularEventShopItemRefObj> _m_aOnUseItem;

        /// <summary>
        /// 点击使用按钮回调
        /// </summary>
        public Action<RegularEventShopItemRefObj> onUseItem { get { return _m_aOnUseItem; } set { _m_aOnUseItem = value; } }

        protected override string _monoAssetPath { get { return GCommon.getActivityPrefabSkinAssetPath(_m_lActivityId,HotfixUINodeTagConst.REGULAR_EVENT_SHOP); } }
        protected override string _monoObjName { get { return GCommon.getActivityPrefabSkinObjName(_m_lActivityId, HotfixUINodeTagConst.REGULAR_EVENT_SHOP); } }
        public override bool needDiscardOnSwitch { get { return true; } }

        public GGuiWndRegularEventShop(long _activityId) : base(EALUIWndLayer.ADDITION)
        {
            _m_lActivityId = _activityId;
        }

        protected override void _onShowWnd()
        {
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_CHG, _onShopChg);
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_REFRESH, _onShopChg);

            //取第一个活动
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_CHG, _onShopChg);
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_REFRESH, _onShopChg);

            _hideAllPage();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGuiWndRegularEventShopTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            if (_m_lTabWndList != null)
            {
                GGuiWndRegularEventShopTab tempTabItem = null;
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

            _m_aOnUseItem = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            //初始化页签列表
            _m_lTabWndList = new List<GGuiWndRegularEventShopTab>();
            GGuiWndRegularEventShopTab tempItem = null;

            if (hotfixWnd.monoTabList != null)
            {
                for (int i = 0; i < hotfixWnd.monoTabList.Count; i++)
                {
                    if(hotfixWnd.monoTabList[i] == null)
                        continue;
                    //创建页签对象
                    tempItem = new GGuiWndRegularEventShopTab(hotfixWnd.monoTabList[i].monoTab, hotfixWnd.monoTabList[i].tabTypeStr);
                    //初始化默认未选中
                    tempItem.setSelected(false);
                    //绑定点击事件
                    tempItem.onClickButton += _onTabSelect;
                    _m_lTabWndList.Add(tempItem);
                }
            }

            if(hotfixWnd.monoShopContainer != null)
                _m_wShopContainer = new GGuiWndRegularEventShopContainer(hotfixWnd.monoShopContainer);

            if (hotfixWnd.monoWarehouseContainer != null)
            {
                _m_wWarehouseContainer = new GGuiWndRegularEventWarehouseContainer(hotfixWnd.monoWarehouseContainer);
                _m_wWarehouseContainer.onUseItem += _onClickUse;
            }

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (hotfixWnd == null)
                return;

            //刷新默认页签
            _refreshPageWnd();
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd()
        {
            if (null == wnd)
                return;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                //设置默认选中页签
                string defaultType = RegularEventShopTabType.SHOP;
                //仓库是否有道具可使用
                bool haveItem = false;
                HotfixRefdataCoreMgr.instance.regularEventShopItemRefCore.dealAllRef(_ref =>
                {
                    if (!haveItem  && _ref != null && _ref.activity_id == _m_lActivityId && _ref.item != null && GCommon.isItemEnough(_ref.item.getItemType(), _ref.item.subId, 1, false))
                        haveItem = true;
                });
                if (haveItem)
                    defaultType = RegularEventShopTabType.WAREHOUSE;

                foreach (GGuiWndRegularEventShopTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabTag == defaultType)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                foreach (GGuiWndRegularEventShopTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabTag == _m_wSelectTabWnd.tabTag)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabTag);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGuiWndRegularEventShopTab _tabItemWnd)
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
                _refreshTabView(_m_wSelectTabWnd.tabTag);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(string _tabView)
        {
            _hideAllPage();

            switch (_tabView?.ToUpperInvariant())
            {
                case RegularEventShopTabType.WAREHOUSE://仓库
                    _m_wWarehouseContainer?.showWnd();
                    _m_wWarehouseContainer?.showItemList(_m_lActivityId);
                    break;
                case RegularEventShopTabType.SHOP://商店
                    _m_wShopContainer?.showWnd();
                    _m_wShopContainer?.showItemList(_m_lActivityId);
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wShopContainer?.hideWnd();
            _m_wWarehouseContainer?.hideWnd();
        }

        #endregion

        /// <summary>
        /// 点击使用按钮回调
        /// </summary>
        /// <param name="_itemRef"></param>
        private void _onClickUse(RegularEventShopItemRefObj _itemRef)
        {
            Action<RegularEventShopItemRefObj> _onClickUse = _m_aOnUseItem;
            _m_aOnUseItem = null;

            //退出界面，执行回调
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.REGULAR_EVENT_SHOP);

            if (_onClickUse != null)
                _onClickUse(_itemRef);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.REGULAR_EVENT_SHOP);
        }

        //商店刷新
        private void _onShopChg(params object[] _args)
        {
            if (_args == null || _args.Length == 0)
                return;

            long instanceId = (long) _args[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != instanceId)
                return;

            if (_m_wSelectTabWnd != null && _m_wSelectTabWnd.tabTag == RegularEventShopTabType.SHOP)
                _refreshTabView(_m_wSelectTabWnd.tabTag);
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 万能活动仓库container
    /// </summary>
    public class GGuiWndRegularEventWarehouseContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoRegularEventWarehouseContainer, GGuiWndRegularEventWarehouseContainerItem>
    {
        //窗口容器
        protected List<GGuiWndRegularEventWarehouseContainerItem> _m_lItemList;
        //点击使用按钮回调
        private Action<RegularEventShopItemRefObj> _m_aOnUseItem;

        /// <summary>
        /// 点击使用按钮回调
        /// </summary>
        public Action<RegularEventShopItemRefObj> onUseItem { get { return _m_aOnUseItem; } set { _m_aOnUseItem = value; } }

        public GGuiWndRegularEventWarehouseContainer(GGUIHotfixCommonMono containerWndMono) : base(containerWndMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_aOnUseItem = null;
        }

        protected override GGuiWndRegularEventWarehouseContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            GGuiWndRegularEventWarehouseContainerItem itemWnd = new GGuiWndRegularEventWarehouseContainerItem(_itemMono);
            itemWnd.onUseItem += _onClickUse;
            return itemWnd;
        }

        protected override void _onWndInitDoneHotfix()
        {
            _m_lItemList = new List<GGuiWndRegularEventWarehouseContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void showItemList(long _activityId)
        {
            if (_m_lItemList == null || hotfixWnd == null)
                return;

            //获取道具列表
            List<RegularEventShopItemRefObj> regularEventShopItemRefList = new List<RegularEventShopItemRefObj>();
            HotfixRefdataCoreMgr.instance.regularEventShopItemRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.activity_id == _activityId && _ref.item != null &&
                    GCommon.isItemEnough(_ref.item.getItemType(), _ref.item.subId, 1, false))
                    regularEventShopItemRefList.Add(_ref);
            });
            regularEventShopItemRefList.Sort(_sortList);

            GGuiWndRegularEventWarehouseContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < regularEventShopItemRefList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(regularEventShopItemRefList[count]);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            ALUGUICommon.setGameObjEnable(hotfixWnd.goEmptyShowList, regularEventShopItemRefList.Count == 0);
        }

        //排序，品质从低到高，id从小到大
        private int _sortList(RegularEventShopItemRefObj _a, RegularEventShopItemRefObj _b)
        {
            if (_a == null || _b == null || _a.item == null || _b.item == null)
                return 0;

            EQuality qualityA = GCommon.getItemQuality(_a.item.getItemType(), _a.item.subId);
            EQuality qualityB = GCommon.getItemQuality(_b.item.getItemType(), _b.item.subId);

            if (qualityA != qualityB)
                return qualityA.CompareTo(qualityB);

            return _a.id.CompareTo(_b.id);
        }

        /// <summary>
        /// 点击使用按钮回调
        /// </summary>
        /// <param name="_itemRef"></param>
        private void _onClickUse(RegularEventShopItemRefObj _itemRef)
        {
            if (_m_aOnUseItem != null)
                _m_aOnUseItem(_itemRef);
        }
    }
}
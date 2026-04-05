using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 万能活动商店container
    /// </summary>
    public class GGuiWndRegularEventShopContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoRegularEventShopContainer, GGuiWndRegularEventShopContainerItem>
    {
        //窗口容器
        protected List<GGuiWndRegularEventShopContainerItem> _m_lItemList;

        public GGuiWndRegularEventShopContainer(GGUIHotfixCommonMono containerWndMono) : base(containerWndMono)
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
        }

        protected override GGuiWndRegularEventShopContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGuiWndRegularEventShopContainerItem(_itemMono);
        }

        protected override void _onWndInitDoneHotfix()
        {
            _m_lItemList = new List<GGuiWndRegularEventShopContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void showItemList(long _activityId)
        {
            if (_m_lItemList == null)
                return;

            //获取商品列表
            List<RegularEventShopItem> shopItemList = new List<RegularEventShopItem>();
            RegularEventShop regularEventShop = HotfixNPPlayer.instance.regularEventComponent.getRegularEventShopByActivityId(_activityId);
            if (regularEventShop != null)
                regularEventShop.getCanBuyRegularEventShopItemList(shopItemList);
            shopItemList.Sort(_sortList);

            GGuiWndRegularEventShopContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < shopItemList.Count; i++)
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
                itemWnd.setInfo(shopItemList[count]);
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
        }

        //排序， 免费>付费，id从小到大
        private int _sortList(RegularEventShopItem _a, RegularEventShopItem _b)
        {
            if (_a == null || _b == null || _a.regularEventShopItemRef == null || _b.regularEventShopItemRef == null)
                return 0;

            if (_a.isFree.CompareTo(_b.isFree) != 0)
                return -(_a.isFree.CompareTo(_b.isFree));

            return _a.regularEventShopItemRef.id.CompareTo(_b.regularEventShopItemRef.id);
        }
    }
}
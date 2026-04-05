using ALPackage;
using Common;
using NPEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    // 普通商店容器
    public class GGUIWndShopNormalPageGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoShopNormalPageGridItem, GGUIMonoShopNormalPageGrid, GGUIWndShopNormalPageGridItem>, _IScrollerSmoothMovable
    {
        //商品列表
        private List<NPPlayerShopItem> _m_shopItemList;
        //滚动序列号
        private long _m_lScrollMoveSerialize;

        //构造函数
        public GGUIWndShopNormalPageGrid(GGUIMonoShopNormalPageGrid _containerMono) : base(_containerMono)
        {
            _m_shopItemList = new List<NPPlayerShopItem>();
            initWnd();
        }

        #region override方法
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;
            WinMsg.RegisterMsg(WinMsgType.SHOP_ITEM_CHG, _shopItemChg);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_SHOP_ITEM, _simulateClickShopItem);

        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SHOP_ITEM_CHG, _shopItemChg);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_SHOP_ITEM, _simulateClickShopItem);
            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_shopItemList.Clear();

        }

        // 创建对象
        protected override GGUIWndShopNormalPageGridItem _createItemWnd(GGUIMonoShopNormalPageGridItem _itemMono)
        {
            // 创建对象
            GGUIWndShopNormalPageGridItem gridItem = new GGUIWndShopNormalPageGridItem(_itemMono);
            return gridItem;
        }

        // 刷新grid item 对象
        protected override void _onRefreshItemWnd(GGUIWndShopNormalPageGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx >= _m_shopItemList.Count)
                return;
           
            // 刷新
            NPPlayerShopItem item = _m_shopItemList[_itemIdx];

            _itemMono.setItem(item,_itemIdx % wnd.perLineItemCount == 0);
        }
        #endregion

        public void refresh(NPPlayerShop _shop)
        {
            if (null == _shop)
                return;

            _m_shopItemList.Clear();
            _shop.getShopItemList(_m_shopItemList);
            _m_shopItemList.Sort(_sortBySortId);
            setItemCount(_m_shopItemList.Count);

            //调用子窗体动画
            hideWnd();
            showWnd();
        }

        /// <summary>
        /// 根据下标获取商店item的RectTransform
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public RectTransform getShopItemRectTransformByIndex(int _index)
        {
            if (_index < 0)
                return null;

            RectTransform targetRect = null;
            refreshAllItem((_itemWnd, _idx) =>
            {
                if (_itemWnd != null && _idx == _index && targetRect == null)
                    targetRect = _itemWnd.rectTransform;
            });
            return targetRect;
        }

        /// <summary>
        /// 获取商店item
        /// </summary>
        /// <param name="_shopItemId"></param>
        /// <returns></returns>
        public GGUIWndShopNormalPageGridItem getShopItemByShopItemId(long _shopItemId)
        {
            if (_shopItemId <= 0)
                return null;

            GGUIWndShopNormalPageGridItem targetItem = null;

            refreshAllItem((_itemWnd, _idx) =>
            {
                if (_itemWnd == null || _itemWnd.shopItem == null || _itemWnd.shopItem.shopItemRefId != _shopItemId)
                    return;

                targetItem = _itemWnd;
            });

            return targetItem;
        }

        /// <summary>
        /// 滚动到指定商店item的位置
        /// </summary>
        /// <param name="_shopItemId"></param>
        /// <param name="_moveTime"></param>
        public void scrollMoveToTargetShopItem(long _shopItemId, float _moveTime)
        {
            if (_shopItemId <= 0)
                return;

            int index = -1;
            for (int i = 0; i < _m_shopItemList.Count; i++)
            {
                if (_m_shopItemList[i].shopItemRefId == _shopItemId)
                {
                    index = i;
                    break;
                }
            }

            scrollMoveToIndex(index, _moveTime);
        }

        /// <summary>
        /// 滚动到指定下标的位置
        /// </summary>
        /// <param name="_index">下标</param>
        /// <param name="_smoothTime">平滑移动时间，默认0.25秒</param>
        /// <param name="_complete">移动完成回调</param>
        public void scrollMoveToIndex(int _index, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null || _index < 0)
            {
                _complete?.Invoke();
                return;
            }

            long serializeId = _m_lScrollMoveSerialize = ALSerializeOpMgr.next();

            // 获取伙伴所在位置
            Vector2 itemPos = getItemPos(_index);

            // 计算可移动区域高度
            float canMoveHeight = wnd.gridAreaMaskObj == null ? allHeight : allHeight - wnd.gridAreaMaskObj.rect.height;

            // 计算目标垂直位置比率
            float tmpHeight = canMoveHeight - (-itemPos.y);
            float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);

            // 使用平滑移动任务
            new ScrollerEaseMoveTaskVertical(this, verticalRate, EaseType.OutSine, _smoothTime, () =>
            {
                // 检查序列化ID是否匹配，避免多次调用冲突
                if (serializeId != _m_lScrollMoveSerialize)
                    return;

                // 移动完成后刷新一次所有item
                forceRefreshAllItem();
                _complete?.Invoke();
            }).deal();
        }

        //单个商品变动
        private void _shopItemChg(params object[] _objs)
        {
            forceRefreshAllItem();
            // if (null == wnd || null == _objs || _objs.Length == 0)
            //     return;
            //
            // long instanceId = (long)_objs[0];
            // NPPlayerShopItem shopItem = null;
            // for (int i = 0; i < _m_shopItemList.Count; i++)
            // {
            //     shopItem = _m_shopItemList[i];
            //     
            //     if (null == shopItem || shopItem.instanceId != instanceId)
            //         continue;
            //     forceRefreshItem(i);
            //     break;
            // }
        }

        /// <summary>
        /// 模拟点击购买，按下标匹配
        /// </summary>
        /// <param name="_objects"></param>
        private void _simulateClickShopItem(params object[] _objects)
        {
            if (null == _objects || _objects.Length <= 0)
                return;

            long index = (long)_objects[0];
            if(index < 0)
                return;

            refreshAllItem((_item, _index) =>
            {
                if(_index == index)
                    _item?.simulateClickShopItem();
            });
        }

        private int _sortBySortId(NPPlayerShopItem _x, NPPlayerShopItem _y)
        {
            if (_x == null || _y == null)
                return 0;

            //比较售空 未售罄>已售罄
            bool xIsSellOut = _x.lastBuyCount == 0;
            bool yIsSellOut = _y.lastBuyCount == 0;
            int res = xIsSellOut.CompareTo(yIsSellOut);
            if (res != 0)
                return res;

            //已解锁的放前面
            bool isConditionEnableX = null == _x.shopItemGruopRef || (_x.shopItemGruopRef.buy_condition == null) || _x.shopItemGruopRef.buy_condition.IsEnable(null);
            bool isConditionEnableY = null == _y.shopItemGruopRef || (_y.shopItemGruopRef.buy_condition == null) || _y.shopItemGruopRef.buy_condition.IsEnable(null);
            res = isConditionEnableX.CompareTo(isConditionEnableY);
            if (res != 0)
                return -res;
            if (!isConditionEnableX && !isConditionEnableY)
            {
                //比较sortId 排序ID小到大
                res = _x.sortId.CompareTo(_y.sortId);
                return res;
            }
            
            if (isConditionEnableX && isConditionEnableY)
            {
                //免费 > 付费
                bool isFreeX = _x.getCostItem() == null;
                bool isFreeY = _y.getCostItem() == null;
                res = isFreeX.CompareTo(isFreeY);
                if (res != 0)
                    return -res;

                //比较推荐 推荐>未推荐
                res = _x.isRecommend.CompareTo(_y.isRecommend);
                if (res != 0)
                    return -res;
            
                bool isDiscountX = _x.discountRefId > 0;
                bool isDiscountY = _y.discountRefId > 0;
                //有折扣 > 没折扣
                res = isDiscountX.CompareTo(isDiscountY);
                if (res != 0)
                    return -res;
                //比较折扣 折扣高>低,值越大折扣越低
                res = _x.discount.CompareTo(_y.discount);
                if (res != 0)
                    return res;

                //比较品质 品质高>低
                if (_x.gainItem == null || _y.gainItem == null)
                    return 0;
                res = _x.gainItem.getQuality().CompareTo(_y.gainItem.getQuality());
                if (res != 0)
                    return -res;

                //比较sortId 排序ID小到大
                res = _x.sortId.CompareTo(_y.sortId);
                return res;
            }

            return 0;
        }

        public ScrollRect scrollRect { get { return wnd == null ? null : wnd.scrollRect; } }
        public long serialize { get { return _m_lScrollMoveSerialize; } }
    }
}

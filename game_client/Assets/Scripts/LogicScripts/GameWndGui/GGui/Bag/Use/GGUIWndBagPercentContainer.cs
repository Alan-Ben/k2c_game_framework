using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包概率物品列表
    public class GGUIWndBagPercentContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoBagPercentContainerItem, GGUIMonoBagPercentContainer, GGUIWndBagPercentContainerItem>
    {
        //窗口容器
        protected List<GGUIWndBagPercentContainerItem> _m_lItemList;

        public GGUIWndBagPercentContainer(GGUIMonoBagPercentContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            //初始化
            _m_lItemList = new List<GGUIWndBagPercentContainerItem>();
        }

        protected override void _onShowWnd()
        {

        }


        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                GGUIWndBagPercentContainerItem temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.resetWnd();
                }
                _m_lItemList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
            {
                GGUIWndBagPercentContainerItem temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.discard();
                }
                _m_lItemList.Clear();
                _m_lItemList = null;
            }
        }


        protected override GGUIWndBagPercentContainerItem _createItemWnd(GGUIMonoBagPercentContainerItem _itemMono)
        {
            // 创建对象
            return new GGUIWndBagPercentContainerItem(_itemMono);
        }
        public void refreshWindow(BagItem _bagItem)
        {
            if (null == _bagItem)
                return;
            if (null == _bagItem.itemRewardRefObj)
            {
                ALLog.Error($"can not find {_bagItem.itemId}'s NPSORewardRefObj!");
                return;
            }
                
            GGUIWndBagPercentContainerItem itemWnd = null;
            int count = 0;
            int totalWeight = 0;
            if (_bagItem.itemRewardRefObj.show_pro_list != null)
            {
                for (int i = 0; i < _bagItem.itemRewardRefObj.show_pro_list.Count; i++)
                {
                    totalWeight += _bagItem.itemRewardRefObj.show_pro_list[i];
                }
            }
            //遍历玩家数据
            for (int i = 0; i < _bagItem.itemRewardRefObj.show_item_list.Count; i++)
            {
                //取数据
                NPCommonCostItem costItem = _bagItem.itemRewardRefObj.show_item_list[i];
                if (null == costItem)
                    continue;

                //判断该道具类型是否需要展示
                if (!GCommon.itemCanShowInRewardPreview(costItem.getItemType(), costItem.subId))
                    continue;

                CommonItemData itemData = new CommonItemData(costItem);
                //取出百分比
                int percent = 0;
                if (count < _bagItem.itemRewardRefObj.show_pro_list.Count)
                   percent = _bagItem.itemRewardRefObj.show_pro_list[count];
                //如果容器内部个数不足则新增视图
                if (count >= _m_lItemList.Count)
                {
                     itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                {
                    itemWnd = _m_lItemList[count];
                }
                itemWnd.showWnd();
                itemWnd.refreshItem(itemData, percent, totalWeight);
                count++;
            }
            //隐藏容器中多余的视图
            for (int j = count; j < _m_lItemList.Count; j++)
            {
                _m_lItemList[j].hideWnd();
            }
        }
    }
}

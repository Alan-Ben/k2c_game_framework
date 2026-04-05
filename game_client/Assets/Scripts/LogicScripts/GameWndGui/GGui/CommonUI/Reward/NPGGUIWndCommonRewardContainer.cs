using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 通用奖励容器 展示reward_item_list
    public class NPGGUIWndCommonRewardContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoCommonItem, NPGGUIMonoCommonRewardContainer, NPGGUIWndCommonItem>
    {
        //item 复用列表
        private List<NPGGUIWndCommonItem> _m_lItemList;

        public NPGGUIWndCommonRewardContainer(NPGGUIMonoCommonRewardContainer _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<NPGGUIWndCommonItem>();

            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

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
            _m_lItemList.Clear();
            _m_lItemList = null;
        }

        protected override NPGGUIWndCommonItem _createItemWnd(NPGGUIMonoCommonItem _itemMono)
        {
            // 创建对象
            return new NPGGUIWndCommonItem(_itemMono);
        }

        public void setRewardList(List<NPCommonCostItem> _list)
        {
            if (null == _list )
                return;

            NPGGUIWndCommonItem itemWnd = null;
            NPCommonCostItem temp = null;
            //逐个添加Item
            int itemIdx = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;

                //如果是mail,quest类型不做展示
                if (temp.item.itemType == NPEnum.ENPItemType.MAIL
                    || temp.item.itemType == NPEnum.ENPItemType.QUEST)
                    continue;

                if (itemIdx < _m_lItemList.Count)
                {
                    itemWnd = _m_lItemList[itemIdx];
                }
                else
                {
                    itemWnd = addItemWnd();
                    if (null != itemWnd)
                        _m_lItemList.Add(itemWnd);
                }
                //累加索引
                itemIdx++;

                if (null != itemWnd)
                {
                    itemWnd.showWnd();
                    itemWnd.setItem(temp);
                }
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= itemIdx; j--)
            {
                itemWnd = _m_lItemList[j];
                itemWnd.hideWnd();
            }

            _scrollRectMoveToWndSetting();
        }

        private void _scrollRectMoveToWndSetting()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                if(wnd == null || wnd.ScrollRectMoveTypeList == null || !isShow)
                    return;

                foreach (EScrollRectMoveType scrollRectMoveType in wnd.ScrollRectMoveTypeList)
                {
                    switch (scrollRectMoveType)
                    {
                        case EScrollRectMoveType.TOP:
                            moveToTop();
                            break;
                    
                        case EScrollRectMoveType.LEFT:
                            moveToLeft();
                            break;
                    
                        case EScrollRectMoveType.RIGHT:
                            moveToRight();
                            break;
                    
                        case EScrollRectMoveType.BOTTOM:
                            moveToBottom();
                            break;
                    }
                }
            });
        }
    }

}

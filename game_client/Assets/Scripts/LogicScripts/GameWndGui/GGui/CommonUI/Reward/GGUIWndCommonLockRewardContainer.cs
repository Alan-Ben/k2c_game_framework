using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    // 通用奖励容器（带锁定状态） 展示reward_item_list
    public class GGUIWndCommonLockRewardContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoCommonLockRewardContainerItem, GGUIMonoCommonLockRewardContainer, GGUIWndCommonLockRewardContainerItem>
    {
        //item 复用列表
        private List<GGUIWndCommonLockRewardContainerItem> _m_lItemList;

        public GGUIWndCommonLockRewardContainer(GGUIMonoCommonLockRewardContainer _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<GGUIWndCommonLockRewardContainerItem>();

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

        protected override GGUIWndCommonLockRewardContainerItem _createItemWnd(GGUIMonoCommonLockRewardContainerItem _itemMono)
        {
            // 创建对象
            return new GGUIWndCommonLockRewardContainerItem(_itemMono);
        }

        public void setRewardList(List<NPCommonCostItem> _list, ECommonLockRewardType _type = ECommonLockRewardType.NONE)
        {
            if (_list == null)
                return;

            List<_IItem> itemList = new List<_IItem>();
            itemList.AddRange(_list);
            setRewardList(itemList, _type);
        }

        public void setRewardList(List<_IItem> _list, ECommonLockRewardType _type = ECommonLockRewardType.NONE)
        {
            if (null == _list)
                return;

            GGUIWndCommonLockRewardContainerItem itemWnd = null;
            _IItem temp = null;
            //逐个添加Item
            int itemIdx = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;

                //如果是mail,quest 类型不做展示
                if (temp.getItemType() == NPEnum.ENPItemType.MAIL
                    || temp.getItemType() == NPEnum.ENPItemType.QUEST)
                    continue;

                //判断该道具类型是否需要展示
                if (!GCommon.itemCanShowInRewardPreview(temp.getItemType(), temp.subId))
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
                    itemWnd.setItem(temp, _type);
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

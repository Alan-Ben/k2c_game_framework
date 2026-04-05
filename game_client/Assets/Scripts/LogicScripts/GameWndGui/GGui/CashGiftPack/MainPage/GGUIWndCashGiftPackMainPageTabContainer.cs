using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 现金礼包页签item容器
    /// </summary>
    public class GGUIWndCashGiftPackMainPageTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoCashGiftPackMainPageTabContainerItem, GGUIMonoCashGiftPackMainPageTabContainer, GGUIWndCashGiftPackMainPageTabContainerItem>
    {
        //item列表
        protected List<GGUIWndCashGiftPackMainPageTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndCashGiftPackMainPageTabContainerItem _m_wCurSelectItem;
        //点击item事件
        private Action<GGUIWndCashGiftPackMainPageTabContainerItem, bool> _m_aOnClickItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndCashGiftPackMainPageTabContainerItem, bool> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndCashGiftPackMainPageTabContainer(GGUIMonoCashGiftPackMainPageTabContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndCashGiftPackMainPageTabContainerItem _createItemWnd(GGUIMonoCashGiftPackMainPageTabContainerItem _itemMono)
        {
            GGUIWndCashGiftPackMainPageTabContainerItem item = new GGUIWndCashGiftPackMainPageTabContainerItem(_itemMono);
            item.onClickItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);
        }

        protected override void _onHideWnd()
        {
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
            resetSelect();
            moveToLeft();
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;

            _m_wCurSelectItem = null;
            _m_aOnClickItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lItemList = new List<GGUIWndCashGiftPackMainPageTabContainerItem>();
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<GiftPackGroupRefObj> _refList)
        {
            if (_refList == null || _m_lItemList == null)
                return;

            GiftPackGroupRefObj lastSelectRef = _m_wCurSelectItem?.giftPackGroupRef;
            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = null;
            GGUIWndCashGiftPackMainPageTabContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _refList.Count; i++)
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
                itemWnd.setInfo(_refList[i], count == 0);
                count++;

                //判断当前选中的item是否在列表中，有则选中
                if (lastSelectRef != null && lastSelectRef.id == _refList[i].id)
                    _onClickItem(itemWnd, false);
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            //如果当前选中的item为空，默认选中第一个有红点的item
            if (_m_wCurSelectItem == null && _m_lItemList.Count > 0)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    if (_m_lItemList[i] != null && _m_lItemList[i].haveRedTip)
                    {
                        _onClickItem(_m_lItemList[i], true);
                        break;
                    }
                }
            }

            //如果当前选中的item为空，则选中第一个
            if (_m_wCurSelectItem == null && _m_lItemList.Count > 0)
                _onClickItem(_m_lItemList[0], true);

            //列表为空显隐
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _refList.Count <= 0);

            //延迟一帧刷新左右红点tip
            wnd?.sideRedTipInfo?.hideRedTipGo();
            ALCommonActionMonoTask.addNextFrameTask(() => { _onScrollRectValueChg(Vector2.one); });
        }

        /// <summary>
        /// 重置选择
        /// </summary>
        public void resetSelect()
        {
            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = null;
        }

        //点击选中item
        private void _onClickItem(GGUIWndCashGiftPackMainPageTabContainerItem _item, bool _moveToTop)
        {
            if (wnd == null || _item == null)
                return;

            if (_m_wCurSelectItem == null || (_m_wCurSelectItem.giftPackGroupRef?.id != _item.giftPackGroupRef?.id))
            {
                _m_wCurSelectItem?.setSelect(false);
                _m_wCurSelectItem = _item;
                _m_wCurSelectItem.setSelect(true);

                _m_aOnClickItem?.Invoke(_m_wCurSelectItem, _moveToTop);
            }

            //设置超出的item移动到里面
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item.rectTransform, rectTransform, (RectTransform)wnd.itemContainer?.transform);
        }

        //列表滚动事件
        private void _onScrollRectValueChg(Vector2 _arg)
        {
            //刷新列表左右两边红点提示
            wnd?.sideRedTipInfo?.refreshContainerSideRedTip(_m_lItemList, rectTransform);
        }

        #region 点击事件

        /// <summary>
        /// 点击左侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lItemList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lItemList, rectTransform, true);

            if (targetIndex > -1 && _m_lItemList.Count > targetIndex)
                _onClickItem(_m_lItemList[targetIndex], true);
        }

        /// <summary>
        /// 点击右侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lItemList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lItemList, rectTransform, false);

            if (targetIndex > -1 && _m_lItemList.Count > targetIndex)
                _onClickItem(_m_lItemList[targetIndex], true);
        }

        #endregion
    }
}

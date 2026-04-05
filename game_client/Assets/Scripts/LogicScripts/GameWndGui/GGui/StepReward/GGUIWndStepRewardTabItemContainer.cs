using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndStepRewardTabItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoStepRewardTabItem,GGUIMonoStepRewardTabItemContainer,GGUIWndStepRewardTabItem>
    {
        public List<GGUIWndStepRewardTabItem> _m_lItemGroupList;//子控件列表
        
        private Action<GGUIWndStepRewardTabItem> _m_onClickItemCallBack = null;
		
        public GGUIWndStepRewardTabItemContainer(GGUIMonoStepRewardTabItemContainer _containerMono,  Action<GGUIWndStepRewardTabItem> _onClickItemCallBack) : base(_containerMono)
        {
            _m_onClickItemCallBack = _onClickItemCallBack;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);
        }

        protected override void _onHideWnd()
        {
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
        }

        protected override void _onReset()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndStepRewardTabItem>();
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        protected override GGUIWndStepRewardTabItem _createItemWnd(GGUIMonoStepRewardTabItem _itemMono)
        {
            GGUIWndStepRewardTabItem itemWnd = new GGUIWndStepRewardTabItem(_itemMono, _onItemClick);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<ActivityStepRewardInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            ActivityStepRewardInfo tempData = null;
            GGUIWndStepRewardTabItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[i];
                }

                tempItemWnd.setInfo(tempData, count == 0);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();

            //延迟一帧刷新左右红点tip
            wnd?.sideRedTipInfo?.hideRedTipGo();
            ALCommonActionMonoTask.addNextFrameTask(() => { _onScrollRectValueChg(Vector2.one); });
        }
        
        public void setSelect(ActivityStepRewardInfo _info, bool _needMoveItem)
        {
            if (_m_lItemGroupList == null)
                return;

            foreach (var item in _m_lItemGroupList)
            {
                if (item != null)
                {
                    bool isSelected = item.stepRewardInfo == _info;
                    item.setSelected(isSelected);
                    if (isSelected && _needMoveItem)
                    {
                        //将item左右移动到容器范围内
                        GCommon.setContainerMoveItemWithinRangeInHorizontal(item?.rectTransform, rectTransform, (RectTransform)wnd?.itemContainer?.transform);
                    }
                }
            }
        }
        
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_item"></param>
        private void _onItemClick(GGUIWndStepRewardTabItem _item)
        {
            _m_onClickItemCallBack?.Invoke(_item);

            //将item左右移动到容器范围内
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item?.rectTransform, rectTransform, (RectTransform)wnd?.itemContainer?.transform);
        }

        //列表滚动事件
        private void _onScrollRectValueChg(Vector2 _arg)
        {
            //刷新列表左右两边红点提示
            wnd?.sideRedTipInfo?.refreshContainerSideRedTip(_m_lItemGroupList, rectTransform);
        }

        #region 点击事件

        /// <summary>
        /// 点击左侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lItemGroupList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lItemGroupList, rectTransform, true);

            if (targetIndex > -1 && _m_lItemGroupList.Count > targetIndex)
                _onItemClick(_m_lItemGroupList[targetIndex]);
        }

        /// <summary>
        /// 点击右侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lItemGroupList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lItemGroupList, rectTransform, false);

            if (targetIndex > -1 && _m_lItemGroupList.Count > targetIndex)
                _onItemClick(_m_lItemGroupList[targetIndex]);
        }

        #endregion
    }
}

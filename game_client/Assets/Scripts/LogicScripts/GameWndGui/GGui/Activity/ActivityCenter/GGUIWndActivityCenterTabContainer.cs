using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动中心页签列表
    /// </summary>
    public class GGUIWndActivityCenterTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoActivityCenterTabContainerItem, GGUIMonoActivityCenterTabContainer, GGUIWndActivityCenterTabContainerItem>
    {
        //item列表
        protected List<GGUIWndActivityCenterTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndActivityCenterTabContainerItem _m_wCurSelectItem;
        //当前选择的活动中心配置
        private ActivityCenterRefObj _m_curSelectActivityCenterRef;
        //点击item事件
        private Action<GGUIWndActivityCenterTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndActivityCenterTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndActivityCenterTabContainer(GGUIMonoActivityCenterTabContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndActivityCenterTabContainerItem _createItemWnd(GGUIMonoActivityCenterTabContainerItem _itemMono)
        {
            GGUIWndActivityCenterTabContainerItem item = new GGUIWndActivityCenterTabContainerItem(_itemMono);
            item.onClickItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);
        }

        protected override void _onHideWnd()
        {
            foreach (GGUIWndActivityCenterTabContainerItem tabItem in _m_lItemList)
            {
                tabItem?.hideWnd();
            }
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
            _m_curSelectActivityCenterRef = null;
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

            _m_lItemList = new List<GGUIWndActivityCenterTabContainerItem>();
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<ActivityCenterRefObj> _refList)
        {
            if (wnd == null || _refList == null || _m_lItemList == null)
                return;

            GGUIWndActivityCenterTabContainerItem itemWnd = null;
            GGUIWndActivityCenterTabContainerItem firstItem = null;
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
                itemWnd.setInfo(_refList[i]);
                itemWnd.setSelect(false);
                count++;

                //如果未选中或者已选中但是不再展示时，则选中第一个
                if ((_m_wCurSelectItem == null ||
                     (_m_wCurSelectItem.activityCenterRef != null &&
                      _m_wCurSelectItem.activityCenterRef.show_condition != null &&
                      !_m_wCurSelectItem.activityCenterRef.show_condition.isEmpty &&
                      !_m_wCurSelectItem.activityCenterRef.show_condition.IsEnable(null)))
                    && i == 0)
                {
                    firstItem = itemWnd;
                    _m_wCurSelectItem = null;
                }

                //该页面入口是否有红点
                bool haveRedTip = false;
                if (_refList[i] != null && _refList[i].red_tip_id > 0)
                {
                    _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_refList[i].red_tip_id);
                    haveRedTip = redTipNode != null && redTipNode.needShow();
                }

                //如果未选中过item，且该item有红点，则设置为选中该item
                if (_m_wCurSelectItem == null && haveRedTip)
                {
                    _m_wCurSelectItem = itemWnd;
                    _m_curSelectActivityCenterRef = _refList[i];
                }
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            //如果未选中，设置选中第一个item
            if (_m_wCurSelectItem == null && firstItem != null)
            {
                _m_wCurSelectItem = firstItem;
                _m_curSelectActivityCenterRef = _m_wCurSelectItem?.activityCenterRef;
            }

            //判断原本选中的item是否数据变更了，变更了需要重新选中原本数据对应的item
            if (_m_curSelectActivityCenterRef != null && 
                _m_wCurSelectItem != null && 
                _m_wCurSelectItem.activityCenterRef != null && 
                _m_curSelectActivityCenterRef.id != _m_wCurSelectItem.activityCenterRef.id)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    if (_m_curSelectActivityCenterRef.id == _m_lItemList[i]?.activityCenterRef?.id)
                    {
                        _m_wCurSelectItem = _m_lItemList[i];
                        break;
                    }
                }
            }

            //设置选中状态，并执行选中回调
            _m_wCurSelectItem?.setSelect(true);
            _m_aOnClickItem?.Invoke(_m_wCurSelectItem);

            //延迟一帧刷新左右红点tip
            wnd?.sideRedTipInfo?.hideRedTipGo();
            ALCommonActionMonoTask.addNextFrameTask(() => { _onScrollRectValueChg(Vector2.one); });
        }

        /// <summary>
        /// 设置选中item
        /// </summary>
        /// <param name="_activityCenterId"></param>
        public void setSelectItem(long _activityCenterId)
        {
            if (_m_wCurSelectItem != null && 
                _m_wCurSelectItem.activityCenterRef != null &&
                _m_wCurSelectItem.activityCenterRef.id == _activityCenterId)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && 
                    _m_lItemList[i].activityCenterRef != null &&
                    _m_lItemList[i].activityCenterRef.id == _activityCenterId)
                {
                    _onClickItem(_m_lItemList[i]);
                    break;
                }
            }
        }

        //点击选中item
        private void _onClickItem(GGUIWndActivityCenterTabContainerItem _item)
        {
            if (_item == null)
                return;

            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem.setSelect(true);
            _m_curSelectActivityCenterRef = _item.activityCenterRef;

            _m_aOnClickItem?.Invoke(_m_wCurSelectItem);
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

            if(targetIndex > -1 && _m_lItemList.Count > targetIndex)
                _onClickItem(_m_lItemList[targetIndex]);
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
                _onClickItem(_m_lItemList[targetIndex]);
        }

        #endregion
    }
}

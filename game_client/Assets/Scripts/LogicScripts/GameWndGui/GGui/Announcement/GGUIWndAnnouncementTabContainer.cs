using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 运营公告页签列表，选中会居中显示
    /// </summary>
    public class GGUIWndAnnouncementTabContainer: _ATNPGGUIWndSingleChoiceContainer<GGUIMonoAnnouncementTabContainerItem,GGUIMonoAnnouncementTabContainer,GGUIWndAnnouncementTabContainerItem>
    {
        private List<GGUIWndAnnouncementTabContainerItem> _m_lItemGroupList;//子控件列表
        private Tweener _m_tweener;//dotweener的列表移动处理


        /// <summary>
        /// 页签总数
        /// </summary>
        public int totalTabCount { get { return _m_lItemGroupList == null ? 0 : _m_lItemGroupList.Count; } }

        public GGUIWndAnnouncementTabContainer(GGUIMonoAnnouncementTabContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscardEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
            
            if (wnd != null && wnd.scrollRect != null)
            {
                wnd.commonScrollRect.onStartDragDelegate -= _onDragStart;
                wnd.commonScrollRect.onEndDragDelegate -= _onDragEnd;
            }
            
            _m_tweener?.Kill();
            _m_tweener = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndAnnouncementTabContainerItem>();
            if (wnd.commonScrollRect != null)
            {
                wnd.commonScrollRect.onStartDragDelegate += _onDragStart;
                wnd.commonScrollRect.onEndDragDelegate += _onDragEnd;
            }
        }

        protected override GGUIWndAnnouncementTabContainerItem _createItemWnd(GGUIMonoAnnouncementTabContainerItem _itemMono)
        {
            return new GGUIWndAnnouncementTabContainerItem(_itemMono);
        }

        /// <summary>
        /// 选中的item发生了变化
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected override void _onSelectItemChg(GGUIWndAnnouncementTabContainerItem _itemWnd)
        {
            base._onSelectItemChg(_itemWnd);
            _moveItemToCenter(_itemWnd);
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(List<AnnouncementShowInfo> _showInfoList, long _customSelectId = 0)
        {
            if (_showInfoList == null || _showInfoList.Count == 0)
                return;

            AnnouncementShowInfo tempData = null;
            GGUIWndAnnouncementTabContainerItem tempItemWnd = null;
            GGUIWndAnnouncementTabContainerItem selectedItemWnd = null;
            GGUIWndAnnouncementTabContainerItem unReadItem = null;
            GGUIWndAnnouncementTabContainerItem customItem = null;
            int count = 0;
            for (int i = 0; i < _showInfoList.Count; ++i)
            {
                tempData = _showInfoList[i];
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
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.setInfo(tempData, count);
                //默认选中第一个
                if (count == 0)
                    selectedItemWnd = tempItemWnd;
                //记录第一个未读item
                if (unReadItem == null && !tempData.isRead())
                    unReadItem = tempItemWnd;
                //记录指定的item
                if (_customSelectId != 0 && _customSelectId == tempData.id)
                    customItem = tempItemWnd;
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            //如果没有指定的选择item并且有未读公告，则选中未读公告
            if (unReadItem != null && customItem == null)
                selectedItemWnd = unReadItem;
            else if (customItem != null)
                selectedItemWnd = customItem;

            _refreshContentLayout(selectedItemWnd);
        }

        /// <summary>
        /// 根据运营公告id选中item
        /// </summary>
        /// <param name="_id"></param>
        public void setSelectByAnnouncementId(long _id)
        {
            if (_m_lItemGroupList == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null && _m_lItemGroupList[i].showInfo != null && _m_lItemGroupList[i].showInfo.id == _id)
                {
                    setSelectItem(_m_lItemGroupList[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout( GGUIWndAnnouncementTabContainerItem _selectedItemWnd)
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
                _moveItemToCenter(_selectedItemWnd, false);
                
                setSelectItem(_selectedItemWnd);
            });
        }

        /// <summary>
        /// 移动某个item到中间
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_isSmooth"></param>
        private void _moveItemToCenter(GGUIWndAnnouncementTabContainerItem _itemWnd, bool _isSmooth = true)
        {
            //获取滑动的范围
            RectTransform itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
            float curItemX = itemContainer.anchoredPosition.x + _itemWnd.rectTransform.localPosition.x + _itemWnd.rectTransform.rect.width / 2;
            float targetX = wnd.areaMaskObj.anchoredPosition.x + wnd.areaMaskObj.rect.width / 2;
            float mergeX = targetX - curItemX;
            if (null != wnd && null != itemContainer)
            {
                _m_tweener?.Kill();
                _m_tweener = null;

                Vector2 anchoredPosition = itemContainer.anchoredPosition;
                float nowHorizontal = anchoredPosition.x;
                float newHorizontal = nowHorizontal;
                float endX = nowHorizontal + mergeX;
                if (_isSmooth)
                {
                    _m_tweener = DOTween.To(_value => newHorizontal = _value, nowHorizontal, endX, 0.3f).OnUpdate(() =>
                    {
                        itemContainer.anchoredPosition = new Vector2(newHorizontal, anchoredPosition.y);
                    });
                }
                else
                {
                    itemContainer.anchoredPosition = new Vector2(endX, anchoredPosition.y);
                }
            }
        }

        /// <summary>
        /// 拖拽开始
        /// </summary>
        /// <param name="_eventData"></param>
        private void _onDragStart(PointerEventData _eventData)
        {
            _m_tweener?.Kill();
            _m_tweener = null;
        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="_eventData"></param>
        private void _onDragEnd(PointerEventData _eventData)
        {
            //获取滑动的范围
            RectTransform itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
            float targetX = wnd.areaMaskObj.anchoredPosition.x + wnd.areaMaskObj.rect.width / 2;
            float diff = float.MaxValue;
            GGUIWndAnnouncementTabContainerItem moveToItem = null;
            //计算最接近中间的item
            foreach (GGUIWndAnnouncementTabContainerItem item in _m_lItemGroupList)
            {
                if (null == item)
                    continue;
                float curItemX = itemContainer.anchoredPosition.x + item.rectTransform.localPosition.x + item.rectTransform.rect.width / 2;
                float mergeX = Mathf.Abs(targetX - curItemX);
                if (diff > mergeX)
                {
                    diff = mergeX;
                    moveToItem = item;
                }
            }

            if (null != moveToItem)
            {
                setSelectItem(moveToItem);
            }
        }
    }
}
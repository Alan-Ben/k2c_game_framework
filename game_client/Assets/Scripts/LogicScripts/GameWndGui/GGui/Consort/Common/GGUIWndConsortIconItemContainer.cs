using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using NPEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 情人item容器，选中会居中显示，往两边逐渐缩小
    /// </summary>
    public class GGUIWndConsortIconItemContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoConsortIconItem,GGUIMonoConsortIconItemContainer,GGUIWndConsortIconItem>
    {
        public List<GGUIWndConsortIconItem> _m_lItemGroupList;//子控件列表
        private Tweener _m_tweener;

        public GGUIWndConsortIconItemContainer(GGUIMonoConsortIconItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        
        protected override GGUIWndConsortIconItem _createItemWnd(GGUIMonoConsortIconItem _itemMono)
        {
            return new GGUIWndConsortIconItem(_itemMono);
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
            
            if (wnd != null && wnd.commonScrollRect != null)
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

            _m_lItemGroupList = new List<GGUIWndConsortIconItem>();
            if (wnd.commonScrollRect != null)
            {
                wnd.commonScrollRect.onStartDragDelegate += _onDragStart;
                wnd.commonScrollRect.onEndDragDelegate += _onDragEnd;
            }
        }

        private void _onDragStart(PointerEventData _eventData)
        {
            _m_tweener?.Kill();
            _m_tweener = null;
        }

        private void _onDragEnd(PointerEventData _eventData)
        {
            //获取滑动的范围
            RectTransform itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
            float targetX =  wnd.areaMaskObj.anchoredPosition.x + wnd.areaMaskObj.rect.width / 2;
            float diff = float.MaxValue;
            GGUIWndConsortIconItem moveToItem = null;
            //计算最接近中间的item
            foreach (GGUIWndConsortIconItem item in _m_lItemGroupList)
            {
                if(null == item)
                    continue;
                float curItemX =  itemContainer.anchoredPosition.x + item.rectTransform.localPosition.x + item.rectTransform.rect.width / 2;
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

        protected override void _onSelectItemChg(GGUIWndConsortIconItem _itemWnd)
        {
            base._onSelectItemChg(_itemWnd);
            if (wnd.scaleList.Count == 0)
                return;
            if (_itemWnd != null)
            {
                int _diffIdx;
                foreach (GGUIWndConsortIconItem item in _m_lItemGroupList)
                {
                    if(null == item)
                        continue;
                    _diffIdx = Mathf.Abs(item.itemIdx - _itemWnd.itemIdx);
                    _diffIdx = Mathf.Min(wnd.scaleList.Count - 1, _diffIdx);
                    item.setScale(wnd.scaleList[_diffIdx]);
                }
            }

            if(wnd != null && wnd.needMoveSelectItemCenter)
                _moveItemToCenter(_itemWnd);
        }

        /// <summary>
        /// 移动某个item到中间
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_isSmooth"></param>
        private void _moveItemToCenter(GGUIWndConsortIconItem _itemWnd, bool _isSmooth = true)
        {
            //获取滑动的范围
            RectTransform itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
            float curItemX =  itemContainer.anchoredPosition.x + _itemWnd.rectTransform.localPosition.x + _itemWnd.rectTransform.rect.width / 2;
            float targetX =  wnd.areaMaskObj.anchoredPosition.x + wnd.areaMaskObj.rect.width / 2;
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
                        itemContainer.anchoredPosition = new Vector2(newHorizontal ,anchoredPosition.y);
                    });
                }
                else
                {
                    itemContainer.anchoredPosition = new Vector2(endX ,anchoredPosition.y);
                }
            }
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_defaultConsortId">默认item</param>
        public void showItemList(List<_IConsortShowInfo> _itemDataList, long _defaultConsortId = 0)
        {
            if (_itemDataList == null)
                return;

            _IConsortShowInfo tempData = null;
            GGUIWndConsortIconItem tempItemWnd = null;
            GGUIWndConsortIconItem selectedItemWnd = null;
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
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.setInfo(tempData, count);
                if (_defaultConsortId == tempData.consortId)
                {
                    selectedItemWnd = tempItemWnd;
                }
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout(selectedItemWnd);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_type"></param>
        /// <param name="_defaultConsortId">默认item</param>
        public void showItemList(List<_IConsortShowInfo> _itemDataList, EGameCommonUnlockType _type, long _defaultConsortId = 0)
        {
            if (_itemDataList == null)
                return;
            List<_IConsortShowInfo> finalDataList = new List<_IConsortShowInfo>();
            _IConsortShowInfo tempData = null;
            EGameCommonUnlockType itemUnlockType = EGameCommonUnlockType.LOCK;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if(tempData == null || tempData.unlockType != _type)
                    continue;
                
                finalDataList.Add(tempData);
            }
            
            showItemList(finalDataList, _defaultConsortId);
        }
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout( GGUIWndConsortIconItem _selectedItemWnd)
        {
            if(_selectedItemWnd == null)
                return;
            
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
                
                if(wnd.needMoveSelectItemCenter)
                    _moveItemToCenter(_selectedItemWnd, false);
                
                setSelectItem(_selectedItemWnd);
            });
        }
    }
}
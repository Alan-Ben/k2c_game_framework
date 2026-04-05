using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;


namespace GOE
{
    public enum EScrollToItemType
    {
        Center,
        NearestEdge,
        TOP_OR_RIGHT,
        BOTTOM_OR_LEFT,
    }

    public abstract class _ANPGGUIBasicGridSubWnd<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _AALUGUIBasicGridSubWnd<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>, _IScrollerSmoothMovable
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _TALUGUIMonoGridWnd<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        private int _m_serialize;
        
        
        protected _ANPGGUIBasicGridSubWnd(_T_CONTAINER_MONO _wnd)
                : base(_wnd)
        {
        }


        public override void hideWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();
            
            base.hideWnd();
        }


        public void MoveIfCantSeeItem(int _itemIndex, EScrollToItemType _type = EScrollToItemType.Center, bool _needFade = true)
        {
            if (wnd == null)
                return;
            if (wnd.itemTemplate == null)
                return;
            if (wnd.scrollRect == null)
                return;

            RectTransform itemMask = wnd.gridAreaMaskObj;
            RectTransform itemContainer = wnd.gridAreaUIObj;
            if (itemMask == null || itemContainer == null)
                return;

            ALGUIListLayoutStyle layoutStyle = wnd.layoutStyle;
            float itemSpace = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL ? wnd.spaceSize.x : wnd.spaceSize.y;
            float paddingBefore = wnd.paddingForSide.x;
            float itemSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL ? wnd.itemTemplate.width : wnd.itemTemplate.height;

            float itemPos = paddingBefore + _itemIndex * (itemSize + itemSpace);

            float viewportSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? itemMask.rect.width
                : itemMask.rect.height;
            float contentSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? itemContainer.rect.width
                : itemContainer.rect.height;

            float scrollableRange = contentSize - viewportSize;
            if (scrollableRange <= 0)
                return;

            float currentScrollPos = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? wnd.scrollRect.horizontalNormalizedPosition * scrollableRange
                : (1 - wnd.scrollRect.verticalNormalizedPosition) * scrollableRange;

            float visibleMin = currentScrollPos;
            float visibleMax = currentScrollPos + viewportSize;

            float itemMin = itemPos;
            float itemMax = itemPos + itemSize;

            bool isVisible = itemMin >= visibleMin && itemMax <= visibleMax;
            if (isVisible)
                return;

            float targetScrollPos;
            if (_type == EScrollToItemType.Center)
            {
                targetScrollPos = itemPos + itemSize * 0.5f - viewportSize * 0.5f;
            }
            else if(_type == EScrollToItemType.TOP_OR_RIGHT)
            {
                targetScrollPos = itemPos;
            }
            else if(_type == EScrollToItemType.BOTTOM_OR_LEFT)
            {
                targetScrollPos = itemMax - viewportSize;
            }
            else
            {
                if (itemMax < visibleMin)
                {
                    targetScrollPos = itemPos;
                }
                else if (itemMin > visibleMax)
                {
                    targetScrollPos = itemMax - viewportSize;
                }
                else
                {
                    float distToStart = Mathf.Abs(itemMin - visibleMin);
                    float distToEnd = Mathf.Abs(itemMax - visibleMax);
                    targetScrollPos = distToStart < distToEnd ? itemPos : itemMax - viewportSize;
                }
            }

            targetScrollPos = Mathf.Clamp(targetScrollPos, 0, scrollableRange);

            float targetNormalizeValue = targetScrollPos / scrollableRange;
            if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                targetNormalizeValue = 1 - targetNormalizeValue;
            
            _m_serialize = ALSerializeOpMgr.next();
            if (_needFade)
            {
                if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                    new ScrollerSmoothMoveTaskVertical(this, targetNormalizeValue).deal();
                else
                    new ScrollerSmoothMoveTaskHorizontal(this, targetNormalizeValue).deal();
            }
            else 
            {
                if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                    wnd.scrollRect.verticalNormalizedPosition = targetNormalizeValue;
                else
                    wnd.scrollRect.horizontalNormalizedPosition = targetNormalizeValue;
            }
        }
        
        public void MoveItem(int _itemIndex, EScrollToItemType _type = EScrollToItemType.Center, bool _needFade = true, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }
            if (wnd.itemTemplate == null)
            {
                _complete?.Invoke();
                return;
            }
            if (wnd.scrollRect == null)
            {
                _complete?.Invoke();
                return;
            }

            RectTransform itemMask = wnd.gridAreaMaskObj;
            RectTransform itemContainer = wnd.gridAreaUIObj;
            if (itemMask == null || itemContainer == null)
            {
                _complete?.Invoke();
                return;
            }

            ALGUIListLayoutStyle layoutStyle = wnd.layoutStyle;
            float itemSpace = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL ? wnd.spaceSize.x : wnd.spaceSize.y;
            float paddingBefore = wnd.paddingForSide.x;
            float itemSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL ? wnd.itemTemplate.width : wnd.itemTemplate.height;

            float itemPos = paddingBefore + _itemIndex * (itemSize + itemSpace);

            float viewportSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? itemMask.rect.width
                : itemMask.rect.height;
            float contentSize = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? itemContainer.rect.width
                : itemContainer.rect.height;

            float scrollableRange = contentSize - viewportSize;
            if (scrollableRange <= 0)
            {
                _complete?.Invoke();
                return;
            }

            float currentScrollPos = layoutStyle == ALGUIListLayoutStyle.HORIZONTAL
                ? wnd.scrollRect.horizontalNormalizedPosition * scrollableRange
                : (1 - wnd.scrollRect.verticalNormalizedPosition) * scrollableRange;

            float visibleMin = currentScrollPos;
            float visibleMax = currentScrollPos + viewportSize;

            float itemMin = itemPos;
            float itemMax = itemPos + itemSize;

            float targetScrollPos;
            if (_type == EScrollToItemType.Center)
            {
                targetScrollPos = itemPos + itemSize * 0.5f - viewportSize * 0.5f;
            }
            else if(_type == EScrollToItemType.TOP_OR_RIGHT)
            {
                targetScrollPos = itemPos;
            }
            else if(_type == EScrollToItemType.BOTTOM_OR_LEFT)
            {
                targetScrollPos = itemMax - viewportSize;
            }
            else
            {
                if (itemMax < visibleMin)
                {
                    targetScrollPos = itemPos;
                }
                else if (itemMin > visibleMax)
                {
                    targetScrollPos = itemMax - viewportSize;
                }
                else
                {
                    float distToStart = Mathf.Abs(itemMin - visibleMin);
                    float distToEnd = Mathf.Abs(itemMax - visibleMax);
                    targetScrollPos = distToStart < distToEnd ? itemPos : itemMax - viewportSize;
                }
            }

            targetScrollPos = Mathf.Clamp(targetScrollPos, 0, scrollableRange);

            float targetNormalizeValue = targetScrollPos / scrollableRange;
            if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                targetNormalizeValue = 1 - targetNormalizeValue;
            
            _m_serialize = ALSerializeOpMgr.next();
            if (_needFade)
            {
                if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                    new ScrollerSmoothMoveTaskVertical(this, targetNormalizeValue, _smoothTime, _complete).deal();
                else
                    new ScrollerSmoothMoveTaskHorizontal(this, targetNormalizeValue, _smoothTime, _complete).deal();
            }
            else 
            {
                if (layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                    wnd.scrollRect.verticalNormalizedPosition = targetNormalizeValue;
                else
                    wnd.scrollRect.horizontalNormalizedPosition = targetNormalizeValue;
                
                _complete?.Invoke();
            }
        }
        
        ScrollRect _IScrollerSmoothMovable.scrollRect { get { return wnd == null ? null : wnd.scrollRect; } }
        long _IScrollerSmoothMovable.serialize { get { return _m_serialize; } }
    }
}

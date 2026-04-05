using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoEquipmentContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsBuildingInfoEquipmentContainerItem, GGUIMonoMarsBuildingInfoEquipmentContainer, GGUISubWndMarsBuildingInfoEquipmentContainerItem>
    {
        [ItemNotNull, NotNull] private readonly List<MarsBuildingEquipmentInfo> _m_sortedEquipmentList;
        private readonly Action<MarsBuildingEquipmentInfo> _m_onSelectionChangedCallback;
        
        private GGUISubWndMarsBuildingInfoEquipmentContainerItem _m_currentSelectedItem;
        
        
        public GGUISubWndMarsBuildingInfoEquipmentContainer(GGUIMonoMarsBuildingInfoEquipmentContainer _containerMono, Action<MarsBuildingEquipmentInfo> _onSelectionChanged = null)
            : base(_containerMono)
        {
            _m_onSelectionChangedCallback = _onSelectionChanged;
            _m_sortedEquipmentList = new List<MarsBuildingEquipmentInfo>();
            initWnd();
        }


        public MarsBuildingEquipmentInfo currentSelected { get { return _m_currentSelectedItem?.equipmentInfo; } }


        protected override GGUISubWndMarsBuildingInfoEquipmentContainerItem _createItemWnd(GGUIMonoMarsBuildingInfoEquipmentContainerItem _itemMono)
        {
            return new GGUISubWndMarsBuildingInfoEquipmentContainerItem(_itemMono, _onItemSelected);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsBuildingInfoEquipmentContainerItem _itemWnd, int _index)
        {
            if (_index >= 0 && _index < _m_sortedEquipmentList.Count)
            {
                MarsBuildingEquipmentInfo equipmentInfo = _m_sortedEquipmentList[_index];
                bool isSelected = _itemWnd == _m_currentSelectedItem;
                _itemWnd.refreshWnd(equipmentInfo, isSelected);
            }
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo)
        {
            _m_sortedEquipmentList.Clear();
            if (_buildingInfo?.equipmentData.equipmentList != null)
            {
                _m_sortedEquipmentList.AddRange(_buildingInfo.equipmentData.equipmentList);
                // _m_sortedEquipmentList.Sort((_a, _b) =>
                // {
                //     // unlocked equipment first (true > false)
                //     if (_a.isUnlock != _b.isUnlock)
                //         return _a.isUnlock ? -1 : 1;
                //     return 0;
                // });
                
                // remove locked equipment
                for (int i = _m_sortedEquipmentList.Count - 1; i >= 0; i--)
                {
                    MarsBuildingEquipmentInfo equipmentInfo = _m_sortedEquipmentList[i];
                    if (!equipmentInfo.isUnlock)
                        _m_sortedEquipmentList.RemoveAt(i);
                }
            }

            refreshWnd(_m_sortedEquipmentList.Count);
            int firstUpgradableIndex = -1;
            for (int i = 0; i < _m_sortedEquipmentList.Count; i++)
            {
                MarsBuildingEquipmentInfo equipmentInfo = _m_sortedEquipmentList[i];
                if (equipmentInfo.isUnlock && !equipmentInfo.isLevelMax && !equipmentInfo.isLevelLimit)
                {
                    firstUpgradableIndex = i;
                    break;
                }
            }

            GGUISubWndMarsBuildingInfoEquipmentContainerItem defaultSelectItem = getItem(firstUpgradableIndex) ?? getItem(0);
            if (defaultSelectItem != null)
                _onItemSelected(defaultSelectItem);
        }
        public bool trySwitchToNextUpgradableEquipment()
        {
            if (_m_sortedEquipmentList.Count == 0)
                return false;
                
            // find next upgradable equipment after current selection
            int currentIndex = -1;
            if (_m_currentSelectedItem != null)
            {
                if (_m_currentSelectedItem.equipmentInfo is { isLevelLimit: false })
                    return false;
                
                for (int i = 0; i < _m_sortedEquipmentList.Count; i++)
                {
                    if (_m_sortedEquipmentList[i] == _m_currentSelectedItem.equipmentInfo)
                    {
                        currentIndex = i;
                        break;
                    }
                }
            }
            
            // search for next upgradable equipment starting from current + 1
            for (int i = 0; i < _m_sortedEquipmentList.Count; i++)
            {
                int checkIndex = (currentIndex + 1 + i) % _m_sortedEquipmentList.Count;
                MarsBuildingEquipmentInfo equipmentInfo = _m_sortedEquipmentList[checkIndex];
                
                if (equipmentInfo.isUnlock && !equipmentInfo.isLevelMax && !equipmentInfo.isLevelLimit)
                {
                    GGUISubWndMarsBuildingInfoEquipmentContainerItem targetItem = getItem(checkIndex);
                    if (targetItem != null)
                    {
                        _onItemSelected(targetItem);
                        moveIfCantSee(targetItem);
                        return true;
                    }
                }
            }
            
            return false;
        }


        private void _onItemSelected(GGUISubWndMarsBuildingInfoEquipmentContainerItem _itemWnd)
        {
            if (_m_currentSelectedItem == _itemWnd)
                return;

            _m_currentSelectedItem = _itemWnd;
            refreshAllItem();
            _m_onSelectionChangedCallback?.Invoke(currentSelected);
        }
        public void moveIfCantSee(GGUISubWndMarsBuildingInfoEquipmentContainerItem _item)
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.scrollRect == null || wnd.scrollRect.viewport == null || wnd.scrollRect.content == null)
                return;

            if (_item == null || _item.rectTransform == null)
                return;

            RectTransform itemTrans = _item.rectTransform;

            Vector3[] itemCorners = new Vector3[4];
            Vector3[] viewportCorners = new Vector3[4];
            itemTrans.GetWorldCorners(itemCorners);
            wnd.scrollRect.viewport.GetWorldCorners(viewportCorners);

            float itemMinX = itemCorners[0].x;
            float itemMaxX = itemCorners[2].x;
            float viewportMinX = viewportCorners[0].x;
            float viewportMaxX = viewportCorners[2].x;

            bool canSeeItem = itemMinX >= viewportMinX && itemMaxX <= viewportMaxX;
            if (canSeeItem)
                return;

            float contentWidth = wnd.scrollRect.content.rect.width;
            float viewportWidth = wnd.scrollRect.viewport.rect.width;
            float scrollableWidth = contentWidth - viewportWidth;

            if (scrollableWidth <= 0)
                return;

            float itemLocalPosX = itemTrans.localPosition.x;
            float itemCenterOffsetX = itemLocalPosX + itemTrans.rect.center.x;
            float targetScrollPosX = itemCenterOffsetX - viewportWidth * 0.5f;

            float targetNormalizedHorizontal = Mathf.Clamp01(targetScrollPosX / scrollableWidth);
            _m_iSerialize = ALSerializeOpMgr.next();
            new ScrollerSmoothMoveTaskHorizontal(this, targetNormalizedHorizontal).deal();
        }
    }
}
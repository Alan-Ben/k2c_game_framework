using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoPageSettleSlotContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsBuildingInfoPageSettleSlotContainerItem, GGUIMonoMarsBuildingInfoPageSettleSlotContainer, GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem>
    {
        private MarsBuildingInfo _m_buildingInfo;


        public GGUISubWndMarsBuildingInfoPageSettleSlotContainer([NotNull] GGUIMonoMarsBuildingInfoPageSettleSlotContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem _createItemWnd(GGUIMonoMarsBuildingInfoPageSettleSlotContainerItem _itemMono)
        {
            return new GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem _itemWnd, int _index)
        {
            _itemWnd.refreshWnd(_m_buildingInfo, _index);
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo, bool _reset)
        {
            _m_buildingInfo = _buildingInfo;
            if (_m_buildingInfo == null || !_m_buildingInfo.settleSlotData.isValid())
            {
                refreshWnd(0);
                return;
            }

            int slotNum = _m_buildingInfo.settleSlotData.refObj.slot_num;
            if (_m_buildingInfo.settleSlotData.nextRefObj != null)
                slotNum = _m_buildingInfo.settleSlotData.nextRefObj.slot_num;
            
            refreshWnd(slotNum);
            if (_reset)
            {
                Canvas.ForceUpdateCanvases();
                int currentIndex = Mathf.CeilToInt((float)_m_buildingInfo.settleSlotData.peopleCount / GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count);
                moveIfCantSee(getItem(currentIndex - 1), false);
            }
        }
        public void moveIfCantSee(GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem _item, bool _needFade = true)
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
            if (_needFade)
                new ScrollerSmoothMoveTaskHorizontal(this, targetNormalizedHorizontal).deal();
            else
                wnd.scrollRect.horizontalNormalizedPosition = targetNormalizedHorizontal;
        }
    }
}
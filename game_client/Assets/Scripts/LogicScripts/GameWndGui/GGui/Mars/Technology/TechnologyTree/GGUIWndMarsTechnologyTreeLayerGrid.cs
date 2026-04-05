using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技树层级Grid容器
    /// </summary>
    public class GGUIWndMarsTechnologyTreeLayerGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsTechnologyTreeLayerItem, GGUIMonoMarsTechnologyTreeLayerGrid, GGUIWndMarsTechnologyTreeLayerItem>
    {
        private GRefdataCoreMgr.MarsTechnologyTypeLayerGroup _m_rTechnologyTypeLayerGroup;

        public GGUIWndMarsTechnologyTreeLayerGrid(GGUIMonoMarsTechnologyTreeLayerGrid _gridMono) : base(_gridMono)
        {
            initWnd();
        }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
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
            _m_rTechnologyTypeLayerGroup = null;
        }

        protected override void _onRefreshItemWnd(GGUIWndMarsTechnologyTreeLayerItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemWnd == null || _m_rTechnologyTypeLayerGroup == null)
                return;

            GRefdataCoreMgr.MarsTechnologyLayerRefObj layerData = _m_rTechnologyTypeLayerGroup.getTechnologyLayerRef(_itemIdx + GRefdataCoreMgr.MarsTechnologyLayerRefObj.RootLayer);
            if (layerData == null)
                return;

            _itemWnd.setData(_m_rTechnologyTypeLayerGroup.technologyType, layerData);
        }


        protected override GGUIWndMarsTechnologyTreeLayerItem _createItemWnd(GGUIMonoMarsTechnologyTreeLayerItem _itemMono)
        {
            if (_itemMono == null)
                return null;

            GGUIWndMarsTechnologyTreeLayerItem itemWnd = new GGUIWndMarsTechnologyTreeLayerItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 刷新Grid数据
        /// </summary>
        public void setData(GRefdataCoreMgr.MarsTechnologyTypeLayerGroup _technologyTypeLayerGroup)
        {
            _m_rTechnologyTypeLayerGroup = _technologyTypeLayerGroup;
            if (_m_rTechnologyTypeLayerGroup == null)
            {
                setItemCount(0);
                return;
            }

            // 因为层数是从GRefdataCoreMgr.MarsTechnologyLayerRefObj.RootLayer开始的，所以这里需要减去RootLayer再加1
            setItemCount(_m_rTechnologyTypeLayerGroup.maxLayer - GRefdataCoreMgr.MarsTechnologyLayerRefObj.RootLayer + 1);
        }
        
        /// <summary>
        /// moveToLayer
        /// </summary>
        /// <param name="_layer">指定层</param>
        public void moveToLayer(int _layer)
        {
            CommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if (wnd == null || wnd.scrollRect == null || wnd.scrollRect.content == null || wnd.scrollRect.viewport == null
                    || wnd.gridAreaMaskObj == null || !isShow)
                    return;

                if (_m_rTechnologyTypeLayerGroup == null)
                    return;

                int index = _layer - GRefdataCoreMgr.MarsTechnologyLayerRefObj.RootLayer;
            
                // 获取item的位置（相对于content）
                Vector2 itemPos = getItemPos(index);
            
                // 计算滚动位置，使item位于可视区域中心
                if (wnd.scrollRect.horizontal)
                {
                    // 水平滚动
                    float canMoveWidth = allHeight - wnd.gridAreaMaskObj.rect.width;
                    float tmpWidth = canMoveWidth + wnd.gridAreaMaskObj.rect.width * 0.5f - itemPos.y;
                    float horizontalRate = Mathf.Clamp(tmpWidth / canMoveWidth, 0f, 1f);
            
                    wnd.scrollRect.horizontalNormalizedPosition = horizontalRate;
                }
            
                if (wnd.scrollRect.vertical)
                {
                    // 垂直滚动
                    float canMoveHeight = allHeight - wnd.gridAreaMaskObj.rect.height;
                    float tmpHeight = -wnd.gridAreaMaskObj.rect.height * 0.5f - itemPos.y;
                    float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);
            
                    wnd.scrollRect.verticalNormalizedPosition = 1 - verticalRate;
                }
            });
        }
    }
}

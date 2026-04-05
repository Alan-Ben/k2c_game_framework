using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndTowerChapterItemGridPVP : _ATNPGGUIWndShowAnimGrid<GGUIMonoTowerChapterItem,GGUIMonoTowerChapterItemGrid,GGUIWndTowerChapterItem>
    {
        private TowerChapterRefObj _m_chapterRefObj;
        private List<TowerLevelInfo> _m_itemDataList = new List<TowerLevelInfo>();

        public GGUIWndTowerChapterItemGridPVP(GGUIMonoTowerChapterItemGrid gridMono) : base(gridMono)
        {
            initWnd();
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
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndTowerChapterItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemMono?.setInfo(_m_itemDataList[_itemIdx]);
        }

        protected override GGUIWndTowerChapterItem _createItemWnd(GGUIMonoTowerChapterItem _itemMono)
        {
            GGUIWndTowerChapterItem itemWnd = new GGUIWndTowerChapterItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(List<TowerLevelInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);

        }
        
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最下面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetBottom(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;
            
            // 计算content的坐标活动范围
            if (wnd.gridAreaUIObj != null && null != wnd.gridAreaMaskObj)
            {
                float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                moveToVerticalRate(1- wnd.gridAreaUIObj.rect.height / contentRange + (_index + 1) * (wnd.itemTemplate.height + wnd.spaceSize.y) / contentRange);
            }
        }
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最上面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetTop(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;
            // 计算content的坐标活动范围
            if (wnd.gridAreaUIObj != null && null != wnd.gridAreaMaskObj)
            {
                float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                moveToVerticalRate((_index * (wnd.itemTemplate.height + wnd.spaceSize.y)) / contentRange);
            }
        }
        
        
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最上面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetTopNextFrame(int _index)
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                moveToTargetTop(_index);
            });
        }
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最下面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetBottomNextFrame(int _index)
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                moveToTargetBottom(_index);
            });
        }
        
        //移动到顶部
        public void moveToTopNextFrame()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(moveToTop);
        }
    }
}

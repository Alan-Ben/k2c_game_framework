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
    public class GGUIWndTowerChapterItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoTowerChapterItem,GGUIMonoTowerChapterItemGrid,GGUIWndTowerChapterItem>
    {
        private TowerChapterRefObj _m_chapterRefObj;

        public GGUIWndTowerChapterItemGrid(GGUIMonoTowerChapterItemGrid gridMono) : base(gridMono)
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
            if (_itemIdx < 0)
                return;
            if(_m_chapterRefObj == null || null == _m_chapterRefObj.stage_list)
                return;
            // 反向排序
            // _itemIdx = (totalCount - 1 - _itemIdx);
          
            int level = _itemIdx  + 1;
            
            _itemMono?.setInfo(new TowerLevelInfo(_m_chapterRefObj, level));
        }

        protected override GGUIWndTowerChapterItem _createItemWnd(GGUIMonoTowerChapterItem _itemMono)
        {
            GGUIWndTowerChapterItem itemWnd = new GGUIWndTowerChapterItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(TowerChapterRefObj _chapterRefObj)
        {
            _m_chapterRefObj = _chapterRefObj;
            if (_m_chapterRefObj == null)
                return;
            setItemCount(_m_chapterRefObj.level_count);
        }
        
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最下面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetBottom(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 计算content的坐标活动范围
                if (wnd.gridAreaUIObj != null && null != wnd.gridAreaMaskObj)
                {
                    float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                    moveToVerticalRate(1- wnd.gridAreaUIObj.rect.height / contentRange + (_index + 1) * (wnd.itemTemplate.height + wnd.spaceSize.y) / contentRange);
                }
            });
        }
        
        /// <summary>
        /// 移动到目标item,_index从0开始，目标item和显示范围的最上面对齐
        /// </summary>
        /// <param name="_index"></param>
        public void moveToTargetTop(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 计算content的坐标活动范围
                if (wnd.gridAreaUIObj != null && null != wnd.gridAreaMaskObj)
                {
                    float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                    moveToVerticalRate((_index * (wnd.itemTemplate.height + wnd.spaceSize.y)) / contentRange);
                }
            });
        }
        //移动到顶部
        public void moveToTopNextFrame()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(moveToTop);
        }
    }
}

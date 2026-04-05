using System;
using System.Collections.Generic;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public class GGUIWndTileMatchTaskBlockItemContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoTileMatchTaskBlockItemContainer, GGUIWndTileMatchTaskBlockItem>
    {
        [NotNull] private List<GGUIWndTileMatchTaskBlockItem> _m_lSubWndList = new List<GGUIWndTileMatchTaskBlockItem>();
        
        public GGUIWndTileMatchTaskBlockItemContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_lSubWndList.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            dealAllShowWnd((_itemWnd) =>
            {
                _itemWnd?.hideWnd();
            });
        }

        protected override void _onReset()
        {
            dealAllShowWnd((_itemWnd) =>
            {
                _itemWnd?.resetWnd();
            });
        }

        protected override GGUIWndTileMatchTaskBlockItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            GGUIWndTileMatchTaskBlockItem blockItemWnd = new GGUIWndTileMatchTaskBlockItem(_itemMono);
            return blockItemWnd;
        }

        public void setData(List<TileMatchTaskBlockInfo> _blockInfoList)
        {
            if (_blockInfoList == null || _blockInfoList.Count <= 0)
            {
                dealAllShowWnd((_itemWnd) => _itemWnd?.hideWnd());
                return;
            }

            TileMatchTaskBlockInfo blockInfo = null;
            GGUIWndTileMatchTaskBlockItem blockItemWnd = null;
            int wndCount = 0;
            for (int i = 0, count = _blockInfoList.Count; i < count; i++)
            {
                blockInfo = _blockInfoList[i];
                if (blockInfo == null)
                    continue;

                if (wndCount >= _m_lSubWndList.Count)
                {
                    blockItemWnd = addItemWnd();
                    if(blockItemWnd != null)
                        _m_lSubWndList.Add(blockItemWnd);
                }
                else
                {
                    blockItemWnd = _m_lSubWndList[wndCount];
                }

                if (blockItemWnd != null)
                {
                    blockItemWnd.showWnd();
                    blockItemWnd.setData(blockInfo.blockId, blockInfo.doneNum, blockInfo.needTotalNum);

                    wndCount++;
                }
            }
            
            for(int i = wndCount, count = _m_lSubWndList.Count; i < count; i++)
            {
                blockItemWnd = _m_lSubWndList[i];
                if(blockItemWnd == null)
                    continue;

                blockItemWnd.hideWnd();
            }
        }
        
        public void dealAllShowWnd(Action<GGUIWndTileMatchTaskBlockItem> _action)
        {
            if(_action == null)
                return;

            GGUIWndTileMatchTaskBlockItem blockItemWnd = null;
            for(int i = 0, count = _m_lSubWndList.Count; i < count; i++)
            {
                blockItemWnd = _m_lSubWndList[i];
                if(blockItemWnd == null || !blockItemWnd.isShow)
                    continue;
                
                _action(blockItemWnd);
            }
        }

        /// <summary>
        /// 获取任务格子
        /// </summary>
        /// <param name="_blockId"></param>
        /// <returns></returns>
        public GGUIWndTileMatchTaskBlockItem getTaskItemWnd(long _blockId)
        {
            GGUIWndTileMatchTaskBlockItem blockItemWnd = null;
            for(int i = 0, count = _m_lSubWndList.Count; i < count; i++)
            {
                blockItemWnd = _m_lSubWndList[i];
                if(blockItemWnd == null || !blockItemWnd.isShow || blockItemWnd.blockId != _blockId)
                    continue;

                return blockItemWnd;
            }

            return null;
        }

        /// <summary>
        /// 是否所有任务收集完成
        /// </summary>
        /// <returns></returns>
        public bool isAllItemCollectedDone()
        {
            foreach (var itemWnd in _m_lSubWndList)
            {
                if(itemWnd != null && itemWnd.isShow && itemWnd.doneNum < itemWnd.needTotalNum)
                    return false;
            }

            return true;
        }
    }
}
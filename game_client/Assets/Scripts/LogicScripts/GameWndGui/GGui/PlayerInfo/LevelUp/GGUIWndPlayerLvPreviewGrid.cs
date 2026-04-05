using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.EventSystems;

namespace GOE
{
    // 等级预览列表容器
    public class GGUIWndPlayerLvPreviewGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoPlayerLvPreviewGridItem, GGUIMonoPlayerLvPreviewGrid, GGUIWndPlayerLvPreviewGridItem>
    {
        //等级数据
        private List<PlayerLvlRefObj> _m_lvRefObjList;

        private ScrollRectEx _m_scrollRectEx;

        //当前下标
        private int _m_curIdx;

        private Action<PlayerLvlRefObj> _m_chgLvRefAction;

        public GGUIWndPlayerLvPreviewGrid(GGUIMonoPlayerLvPreviewGrid _containerMono) : base(_containerMono)
        {
            _m_lvRefObjList = new List<PlayerLvlRefObj>();
            initWnd();
        }

        protected override void _onDiscard()
        {
            _m_lvRefObjList.Clear();
            _m_scrollRectEx.onEndDragDelegate -= _onEndDrag;
            _m_scrollRectEx = null;

            ALUGUICommon.uncombineBtnClick(wnd.lastBtn, _lastBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.nextBtn, _nextBtnDidClick);
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;

            _m_curIdx = (int)NPPlayer.instance.playerInfo.getCurrentLevel() - 1;//0级不展示这里需要减1

            _refresh();

            //第一次进来直接移动位置有问题
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _moveToIdx(_m_curIdx);
            }, 0.05f);
        }

        protected override void _onWndInitDone()
        {
            if(null != wnd.scrollRect)
            {
                _m_scrollRectEx = (ScrollRectEx)wnd.scrollRect;
                _m_scrollRectEx.onEndDragDelegate += _onEndDrag;
            }

            ALUGUICommon.combineBtnClick(wnd.lastBtn, _lastBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.nextBtn, _nextBtnDidClick);
        }
        protected override GGUIWndPlayerLvPreviewGridItem _createItemWnd(GGUIMonoPlayerLvPreviewGridItem _itemMono)
        {
            // 创建对象
            GGUIWndPlayerLvPreviewGridItem gridItem = new GGUIWndPlayerLvPreviewGridItem(_itemMono);
            return gridItem;
        }
        protected override void _refreshItemwnd(GGUIWndPlayerLvPreviewGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx >= _m_lvRefObjList.Count)
                return;

            //获取数据对象
            PlayerLvlRefObj refObj = _m_lvRefObjList[_itemIdx];
            if (null == refObj)
                return;

            _itemWnd.refreshItem(refObj);
        }

        public void setChgLvRefAction(Action<PlayerLvlRefObj> _action)
        {
            _m_chgLvRefAction = _action;
        }

        /// <summary>
        /// 从静态数据获取所有需要展示的数据，并进行展示
        /// </summary>
        /// <returns></returns>
        protected void _refresh()
        {
            _m_lvRefObjList.Clear();
            _m_lvRefObjList.AddRange(GRefdataCoreMgr.instance.playerLvlCore.refList);

            //移除等级0的数据不展示
            for (int i = 0; i < _m_lvRefObjList.Count; i++)
            {
                if (_m_lvRefObjList[i].lvl == 0)
                {
                    _m_lvRefObjList.RemoveAt(i);
                    break;
                }
            }
            
            setItemCount(_m_lvRefObjList.Count);
        }

        /// <summary>
        /// 停止拖拽
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onEndDrag(PointerEventData _pointData)
        {
            _moveToIdx(_getCurIdx());
        }
        
        /// <summary>
        /// 获取当前下标
        /// </summary>
        private int _getCurIdx()
        {
            //content 总长度
            float width = _m_scrollRectEx.content.rect.width;
            //当前滚动长度
            float scrollW = -_m_scrollRectEx.content.anchoredPosition.x;

            //滚动到最近的位子
            float itemW = wnd.itemW + wnd.spaceSize.x;
            if (itemW == 0)
                return 0;

            int idx = (int)(scrollW / itemW);
            if (scrollW % wnd.itemW > itemW / 2)
                idx++;

            return idx;
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        private void _moveToIdx(int _idx)
        {
            if (_idx < 0 || _idx > _m_lvRefObjList.Count - 1)
                return;

            _m_curIdx = _idx;
            moveToHorizontalRate(1 - _idx * 1.0f / _m_lvRefObjList.Count);
            if (null != _m_chgLvRefAction)
                _m_chgLvRefAction(_m_lvRefObjList[_idx]);

            ALUGUICommon.setGameObjEnable(wnd.lastBtn, _m_curIdx != 0);
            ALUGUICommon.setGameObjEnable(wnd.nextBtn, _m_curIdx != (_m_lvRefObjList.Count - 1));
        }

        private void _lastBtnDidClick(GameObject _go)
        {
            _moveToIdx(_m_curIdx-1);
        }

        private void _nextBtnDidClick(GameObject _go)
        {
            _moveToIdx(_m_curIdx + 1);

        }
    }
}

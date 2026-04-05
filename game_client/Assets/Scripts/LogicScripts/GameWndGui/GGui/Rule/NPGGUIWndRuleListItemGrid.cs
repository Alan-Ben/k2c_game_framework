using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则列表item容器
    /// </summary>
    public class NPGGUIWndRuleListItemGrid : _ANPGGUIBasicGridSubWnd<NPGGUIMonoRuleListItem,NPGGUIMonoRuleListItemGrid,NPGGUIWndRuleListItem>
    {
        //窗口容器
        private List<NPGGUIWndRuleListItem> _m_lItemList;
        private List<NPRuleRefObj> _m_itemList;
        private List<NPGGUIRuleItemBarController> _m_lBarList;
        //已选中展开的item下标列表
        private HashSet<int> _m_hExpandIndexList;


        public NPGGUIWndRuleListItemGrid(NPGGUIMonoRuleListItemGrid _gridMono) : base(_gridMono)
        {
            _m_lItemList = new List<NPGGUIWndRuleListItem>();
            _m_lBarList = new List<NPGGUIRuleItemBarController>();
            _m_hExpandIndexList = new HashSet<int>();
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
            if (_m_lItemList != null)
            {
                NPGGUIWndRuleListItem temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.discard();
                }
                _m_lItemList.Clear();
            }

            if (null != _m_lBarList)
            {
                NPGGUIRuleItemBarController bar = null;
                for (int i = 0; i < _m_lBarList.Count; ++i)
                {
                    bar = _m_lBarList[i];
                    if (bar == null)
                        continue;
                    removeBar(bar);
                    bar.discard();
                }
                _m_lBarList.Clear();
            }

            _m_hExpandIndexList?.Clear();
        }

        protected override void _onWndInitDone()
        {
            
        }

        protected override NPGGUIWndRuleListItem _createItemWnd(NPGGUIMonoRuleListItem _itemMono)
        {
            return new NPGGUIWndRuleListItem(_itemMono);
        }

        protected override void _refreshItemwnd(NPGGUIWndRuleListItem _itemMono, int _itemIdx)
        {
           
            if(_itemIdx<0 || _itemIdx >= _m_itemList.Count)
                return;

            NPRuleRefObj info = _m_itemList[_itemIdx];
            bool isSelect = _m_hExpandIndexList != null && _m_hExpandIndexList.Contains(_itemIdx);
            _itemMono.setInfo(info, _itemIdx, isSelect, _clickExpand);
        }

        public void showItemList(List<NPRuleRefObj> _itemList)
        {
            _m_itemList = _itemList;
            setItemCount(_m_itemList.Count);
            moveToTop();
            
        }
        
        /// <summary>
        /// 点击展开或者收起
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_isExpand"></param>
        /// <param name="_index"></param>
        private void _clickExpand(NPGGUIWndRuleListItem _itemWnd, bool _isExpand, int _index)
        {
            if (_isExpand)
            {
                if (_m_hExpandIndexList != null && !_m_hExpandIndexList.Contains(_index))
                    _m_hExpandIndexList.Add(_index);
                _addItemBar(_itemWnd, _index);
            }
            else
            {
                if (_m_hExpandIndexList != null && _m_hExpandIndexList.Contains(_index))
                    _m_hExpandIndexList.Remove(_index);
                _removeItemBar(_itemWnd);
            }
        }

        private void _addItemBar(NPGGUIWndRuleListItem _itemWnd, int _index)
        {
            NPGGUIRuleItemBarController bar = null;
            for (int i = 0; i < _m_lBarList.Count; ++i)
            {
                bar = _m_lBarList[i];
                if (bar == null)
                    continue;
                if (bar.ruleRefObj.id == _itemWnd.ruleRefObj.id)
                {
                    //已有bar
                    bar.setInsertIndex(_index);
                    
                    ALCommonActionMonoTask.addNextFrameTask(() =>
                    {
                        // _checkMoveToItemTop(_index);
                        // _checkMoveToBarBottom(_index, bar);
                        forceRefreshBar();
                    });
                    return;
                }
            }

            bar = new NPGGUIRuleItemBarController(wnd.gridAreaUIObj);
            bar.setInsertIndex(_index + 1);
            bar.setInfo(_itemWnd.ruleRefObj);
            _m_lBarList.Add(bar);
            addBar(bar);

            forceRefreshBar();
            
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                // _checkMoveToItemTop(_index);
                // _checkMoveToBarBottom(_index, bar);
                forceRefreshBar();
            });
        }

        private void _removeItemBar(NPGGUIWndRuleListItem _itemWnd)
        {
            NPGGUIRuleItemBarController bar = null;
            for (int i = 0; i < _m_lBarList.Count; ++i)
            {
                bar = _m_lBarList[i];
                if (bar == null)
                    continue;
                if (bar.ruleRefObj.id == _itemWnd.ruleRefObj.id)
                {
                    _m_lBarList.Remove(bar);
                    removeBar(bar);
                    bar.discard();
                    bar = null;
                    return;
                }
            }
        }
        /// <summary>
        /// 检查是否需要滑动到点击item的顶部
        /// </summary>
        /// <param name="_itemIdx"></param>
        private void _checkMoveToItemTop(int _itemIdx)
        {
            if (null == wnd || null == wnd.gridAreaUIObj || null == wnd.gridAreaMaskObj || null == wnd.itemTemplate)
                return;

            //获取滑动的范围
            float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;

            //根据选中下标获取item的顶部值
            int perLineItemCount = wnd.perLineItemCount;
            if (perLineItemCount == 0)
                perLineItemCount = 1;
            float selectItemY = wnd.paddingForSide.y + (_itemIdx / perLineItemCount) * (wnd.itemTemplate.height + wnd.spaceSize.y);

            //当前滑动的距离
            float curScrollY = (1 - wnd.scrollRect.verticalNormalizedPosition) * contentRange;

            //如果当前滑动距离大于item的顶部值
            float margin = curScrollY - selectItemY;

            //如果展开bar需要滑动
            if (margin > 0)
            {
                //需要滑动的距离
                float needScrollY = curScrollY - margin;
                float rate = needScrollY / contentRange;
                moveToVerticalRate(rate);
            }
        }
        
        /// <summary>
        /// 检查是否需要滑动到展开bar的底部
        /// </summary>
        private void _checkMoveToBarBottom(int _itemIdx, NPGGUIRuleItemBarController _itemBar)
        {
            if (null == wnd || null == wnd.gridAreaUIObj || null == wnd.gridAreaMaskObj || null == wnd.itemTemplate)
                return;

            //获取滑动的范围
            float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;

            //根据选中下标获取item的底部值
            float selectItemY = wnd.paddingForSide.y + (_itemIdx / wnd.perLineItemCount + 1) * (wnd.itemTemplate.height + wnd.spaceSize.y);

            //当前滑动的距离
            float curScrollY = (1 - wnd.scrollRect.verticalNormalizedPosition) * contentRange;

            //如果item的底部值加上bar的高度 大于总高度 需要往上移
            float margin = (selectItemY + _itemBar.barHeight) - curScrollY - wnd.gridAreaMaskObj.rect.height;

            //如果展开bar需要滑动
            if (margin > 0)
            {
                //需要滑动的距离
                float needScrollY = margin + curScrollY;
                float rate = needScrollY / contentRange;
                moveToVerticalRate(rate);
            }
        }
    }
}

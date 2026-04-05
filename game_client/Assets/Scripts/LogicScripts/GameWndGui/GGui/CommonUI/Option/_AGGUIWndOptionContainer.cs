using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public abstract class _AGGUIWndOptionContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATNPGGUIWndShowAnimContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AGGUIMonoOptionItem
        where _T_CONTAINER_MONO : _AGGUIMonoOptionContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _AGGUIWndOptionItem<_T_ITEM_MONO, _T_ITEM_WND>
    {
        private List<_T_ITEM_WND> _m_lSubWndList;
        
        protected _AGGUIWndOptionContainer(_T_CONTAINER_MONO _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        
        public event Action<_T_ITEM_WND> onOptionClick;

        protected sealed override void _onShowWnd() 
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _hideAllWnd();
            
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            // 因为父窗口在销毁时会销毁所有加载的子窗口, 所以这里只要清除列表就行
            if(_m_lSubWndList != null)
                _m_lSubWndList.Clear();
            
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            // 因为父窗口在销毁时会销毁所有加载的子窗口, 所以这里只要清除列表就行
            if(_m_lSubWndList != null)
                _m_lSubWndList.Clear();
            
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            _onWndInitDoneEx();
        }

        protected sealed override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            _itemWnd.onClick += _onClickItem;
            _onAddItemWndEx(_itemWnd);
        }

        protected sealed override void _discardItem(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            //基类没有_onRemove方法，与add对应的就是discard
            _itemWnd.onClick -= _onClickItem;
            _discardItemEx(_itemWnd);
        }

        /// <summary>
        /// 点击了item
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onClickItem(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            
            onOptionClick?.Invoke(_itemWnd);
        }

        protected void _setOptionCount(int _count)
        {
            if (_count <= 0)
            {
                _hideAllWnd();
                return;
            }
            
            if (_m_lSubWndList == null)
                _m_lSubWndList = new List<_T_ITEM_WND>();
            
            _T_ITEM_WND itemWnd = null;
            for (int i = 0; i < _count; i++)
            {
                if (i >= _m_lSubWndList.Count)
                {
                    itemWnd = addItemWnd();
                    if(itemWnd != null)
                        _m_lSubWndList.Add(itemWnd);
                }
                else
                {
                    itemWnd = _m_lSubWndList[i];
                    if (itemWnd == null)
                    {
                        itemWnd = addItemWnd();
                        _m_lSubWndList[i] = itemWnd;
                    }
                }

                if (itemWnd != null)
                {
                    itemWnd.showWnd();
                    _showItemWnd(i, itemWnd);
                }
            }
        }

        /// <summary>
        /// 显示每个item的方法
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_itemWnd"></param>
        protected abstract void _showItemWnd(int _index, _T_ITEM_WND _itemWnd);
        
        /// <summary>
        /// 隐藏所有窗口
        /// </summary>
        private void _hideAllWnd()
        {
            if (_m_lSubWndList == null)
                return;

            foreach (_T_ITEM_WND itemWnd in _m_lSubWndList)
            {
                if(itemWnd == null)
                    continue;
                
                itemWnd.hideWnd();
            }
        }

        /// <summary>
        /// 消耗所有窗口
        /// </summary>
        private void _discardAllWnd()
        {
            if (_m_lSubWndList == null)
                return;

            foreach (_T_ITEM_WND itemWnd in _m_lSubWndList)
            {
                if(itemWnd == null)
                    continue;
                
                itemWnd.discard();
            }
            
            _m_lSubWndList.Clear();
            _m_lSubWndList = null;
        }

        #region 子类窗体相关
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        protected virtual void _onAddItemWndEx(_T_ITEM_WND _itemWnd) { }
        protected virtual void _discardItemEx(_T_ITEM_WND _itemWnd) { }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _AGGUISubWndCommonContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATNPGGUIWndShowAnimContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>, _IScrollerSmoothMovable
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _ATNPGGUIMonoShowAnimContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        private int _m_itemNum;
        [NotNull][ItemNotNull]private readonly List<_T_ITEM_WND> _m_itemList;
        protected int _m_iSerialize;

        protected _AGGUISubWndCommonContainer(_T_CONTAINER_MONO _containerMono) : base(_containerMono)
        {
            _m_itemList = new List<_T_ITEM_WND>();
        }
        
        
        public int itemNum { get { return _m_itemNum; } }
        
        
        protected override void _onShowWnd()
        {            
            foreach (_T_ITEM_WND temp in _m_itemList)
            {
                temp.showWnd();
            }
        }

        protected override void _onHideWnd()
        {
            foreach (_T_ITEM_WND temp in _m_itemList)
            {
                temp.hideWnd();
            }

            _m_iSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            foreach (_T_ITEM_WND temp in _m_itemList)
            {
                temp.resetWnd();
            }
            
            _m_itemList.Clear();
        }

        protected override void _onDiscard()
        {
            foreach (_T_ITEM_WND temp in _m_itemList)
            {
                temp.discard();
            }
            
            _m_itemList.Clear();

            _onDiscardEx();
        }

        protected override void _onWndInitDone()
        {
            refreshWnd();
        }

        public void refreshWnd(int _itemNum)
        {
            _m_itemNum = _itemNum;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null)
                return;

            int count = 0;
            for (int i = 0; i < _m_itemNum; i++)
            {
                _T_ITEM_WND itemWnd;
                if (i >= _m_itemList.Count)
                {
                    itemWnd = _addItemWnd(i);
                    if (itemWnd == null)
                        continue;

                    _m_itemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_itemList[i];

                _refreshItemWnd(itemWnd, i);
                if (_m_bIsShow) itemWnd.showWnd();
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_itemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_itemList[j]);
                //从队列删除
                _m_itemList.RemoveAt(j);
            }

            _onRefreshWnd();
        }

        protected virtual _T_ITEM_WND _addItemWnd(int _wndIndex)
        {
            return addItemWnd();
        }
        
        /// <summary>
        /// 添加一个新窗口
        /// </summary>
        public _T_ITEM_WND addNewItem()
        {
            _T_ITEM_WND itemWnd = addItemWnd();
            if (itemWnd == null)
                return null;
            
            _m_itemList.Add(itemWnd);
            _refreshItemWnd(itemWnd, _m_itemList.Count - 1);

            return itemWnd;
        }

        public void refreshAllItem()
        {
            for (int i = 0; i < _m_itemList.Count; i++)
            {
                _T_ITEM_WND itemWnd = _m_itemList[i];
                _refreshItemWnd(itemWnd, i);
            }
        }

        public void refreshItem(int _index)
        {
            _T_ITEM_WND itemWnd = _m_itemList.SafeGet(_index);
            if(itemWnd != null)
                _refreshItemWnd(itemWnd, _index);
        }

        public void dealAllWnd(Action<_T_ITEM_WND> _action)
        {
            if (_action == null)
                return;

            _m_itemList.ForEach(_action);
        }

        public _T_ITEM_WND getItem(int _index)
        { 
            if (_index < 0 || _index >= _m_itemList.Count)
                return null;

            return _m_itemList[_index];
        }
        
        public _T_ITEM_WND getItem(Predicate<_T_ITEM_WND> _match)
        {
            if (_match == null)
                return null;
            
            return _m_itemList.Find(_match);
        }

        protected abstract void _refreshItemWnd([NotNull] _T_ITEM_WND _itemWnd, int _index);
        protected virtual void _onRefreshWnd() {}
        protected virtual void _onDiscardEx() {}
        
        ScrollRect _IScrollerSmoothMovable.scrollRect { get { return wnd == null ? null : wnd.scrollRect; } }
        long _IScrollerSmoothMovable.serialize { get { return _m_iSerialize; } }
    }
}
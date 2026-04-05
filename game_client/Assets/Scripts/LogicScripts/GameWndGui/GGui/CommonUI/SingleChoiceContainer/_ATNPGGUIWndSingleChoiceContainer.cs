using System;

namespace GOE
{
    /// <summary>
    /// 单选列表基类
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _ATNPGGUIWndSingleChoiceContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATNPGGUIWndShowAnimContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _ANPGGUIMonoSingleChoiceItem
        where _T_CONTAINER_MONO : _ATNPGGUIMonoSingleChoiceContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATNPGGUIWndSingleChoiceItem<_T_ITEM_MONO, _T_ITEM_WND>
    {
        protected _T_ITEM_WND _m_wCurSelectItemWnd;//当前选中的item

        protected _ATNPGGUIWndSingleChoiceContainer(_T_CONTAINER_MONO _mono) : base(_mono)
        {

        }

        /// <summary> 当前选中的item </summary>
        public _T_ITEM_WND curSelectItemWnd { get { return _m_wCurSelectItemWnd; } }
        /// <summary> 选中的item变更 </summary>
        public event Action<_T_ITEM_WND> onSelectItemChg;

        protected sealed override void _onShowWnd() 
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd() 
        {
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            resetSelect();
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            resetSelect();
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
        protected void _onClickItem(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            resetSelect();
            _m_wCurSelectItemWnd = _itemWnd;
            _m_wCurSelectItemWnd.setSelectShow(true);
            _onSelectItemChg(_itemWnd);
        }

        /// <summary>
        /// 选中的item发生了变化
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected virtual void _onSelectItemChg(_T_ITEM_WND _itemWnd)
        {
            onSelectItemChg?.Invoke(_itemWnd);
        }

        /// <summary>
        /// 重置当前选中
        /// </summary>
        public void resetSelect()
        {
            _m_wCurSelectItemWnd?.setSelectShow(false);
            _m_wCurSelectItemWnd = null;
        }

        /// <summary>
        /// 选中某个item
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void setSelectItem(_T_ITEM_WND _itemWnd)
        {
            _onClickItem(_itemWnd);
        }

        /// <summary>
        /// 选中某个item, 不进行回调调用
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void setSelectItemWithOutCallBack(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            resetSelect();
            _m_wCurSelectItemWnd = _itemWnd;
            _m_wCurSelectItemWnd.setSelectShow(true);
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

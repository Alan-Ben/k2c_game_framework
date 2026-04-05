using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***********************************
     * 列表显示对象的抽象基类
     **/
    public abstract class _AALGUIBaseListItem<T> : ALGUIBaseMouseStatWnd
    {
        /** 所在容器的对象 */
        private _AALGUIBaseListWnd<T> _m_wParentListWnd;

        /** 列序号 */
        private int _m_iRowIdx;
        /** 行序号 */
        private int _m_iLineIdx;

        public _AALGUIBaseListItem()
            : base(new Rect())
        {
            ALGUIWndPrintActionDelegate = OnPain;
        }

        public int dataIndex
        {
            get { return _parent._getShowItemIndex(_rowIdx, _lineIdx); }
        }

        protected internal int _rowIdx { get { return _m_iRowIdx; } set { _m_iRowIdx = value; } }
        protected internal int _lineIdx { get { return _m_iLineIdx; } set { _m_iLineIdx = value; } }

        protected internal _AALGUIBaseListWnd<T> _parent
        {
            get { return _m_wParentListWnd; }
            set { _m_wParentListWnd = value; }
        }

        /*****************
         * 获取本子控件对象的具体数据对象
         **/
        public T getData()
        {
            if (null == _parent)
                return default(T);

            return _parent._getShowData(dataIndex);
        }

        /****************
         * 重载绘制函数
         **/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            if (null == _parent)
            {
                return;
            }

            //获取数据对象
            T data = _parent._getShowData(dataIndex);

            //绘制控件部分
            _painData(data);
        }

        /****************
         * 刷新对象的数据以及显示
         **/
        public void initItem()
        {
            initItem(getData());
        }

        /*************************
         * 在父控件进行数据调整时调用的函数
         **/
        public abstract void initItem(T _data);

        /*************************
         * 根据带入的数据对象，绘制本列表的显示对象
         **/
        protected abstract void _painData(T _data);
    }
}

#endif

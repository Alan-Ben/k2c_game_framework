using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
   
    /// <summary>
    /// 主游历场景其它物件展示对象
    /// </summary>
    public abstract class _AGTravelMainCommonView<T> : _AGTravelItemView
    where T : MonoBehaviour
    {
        private T _m_itemMono;
        private readonly NPGGoIndex _m_goIndex;

        public _AGTravelMainCommonView(NPGGoIndex _goIndex, Transform _parent) : base(_parent)
        {
            _m_goIndex = _goIndex;
        }

        protected T itemMono { get => _m_itemMono; }

        protected override void _onLoaded(GameObject _go)
        {
            if (null == _go)
                return;
            _m_itemMono = _go.GetComponent<T>();
            _onLoadedEx(_go);
        }

        protected abstract void _onLoadedEx(GameObject _go);

        protected override NPGGoIndex _getLoadGoIndex()
        {
            return _m_goIndex;
        }

        protected override void _onDiscard()
        {
            _onDiscardEx();
        }

        protected abstract void _onDiscardEx();

        protected override void _onHide()
        {
            _onHideEx();
        }

        protected abstract void _onHideEx();

        protected override void _onShow()
        {
            _onShowEx();
        }

        protected abstract void _onShowEx();
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
namespace GOE
{
    public class NPNestedScrollRect : NPScrollRect
    {
        private ScrollRect _m_parentScrollRect;
        private bool _m_isRouteToParent;
 
 
        protected override void Awake()
        {
            base.Awake();
            _m_parentScrollRect = transform.parent?.GetComponentInParent<ScrollRect>();
        }
 
 
        private bool _needRouteToParent(Vector2 _delta)
        {
            if (_m_parentScrollRect == null)
                return false;
 
            if (horizontal && vertical)
                return false;
 
            if (horizontal)
                return Mathf.Abs(_delta.y) > Mathf.Abs(_delta.x);
 
            if (vertical)
                return Mathf.Abs(_delta.x) > Mathf.Abs(_delta.y);
 
            return true;
        }
 
 
        public override void OnPointerDown(PointerEventData _eventData)
        {
            _m_isRouteToParent = false;
            base.OnPointerDown(_eventData);
        }
 
        public sealed override void OnRealBeginDrag(PointerEventData _eventData)
        {
            _m_isRouteToParent = _needRouteToParent(_eventData.delta);
            if (_m_isRouteToParent)
            {
                _m_parentScrollRect.OnBeginDrag(_eventData);
                return;
            }
 
            base.OnRealBeginDrag(_eventData);
        }
 
        public override void OnDrag(PointerEventData _eventData)
        {
            if (_m_isRouteToParent)
            {
                _m_parentScrollRect.OnDrag(_eventData);
                return;
            }
 
            base.OnDrag(_eventData);
        }
 
        public sealed override void OnRealEndDrag(PointerEventData _eventData)
        {
            if (_m_isRouteToParent)
            {
                _m_parentScrollRect.OnEndDrag(_eventData);
                _m_isRouteToParent = false;
                return;
            }
 
            base.OnRealEndDrag(_eventData);
        }
 
        public override void OnPointerUp(PointerEventData _eventData)
        {
            if (_m_isRouteToParent)
                return;
 
            base.OnPointerUp(_eventData);
        }
    }
}
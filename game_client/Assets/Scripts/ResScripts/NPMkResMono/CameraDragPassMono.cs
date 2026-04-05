using System;
using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    public class CameraDragPassMono : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public bool blockOthersClick = true;
        private float _m_fPressStartTime = 0f;
        
#if NP_GAME
        private TouchInfo _m_touchInfo;
#endif
        
        private IPointerClickHandler[] _m_otherClickHandlers;
        private bool[] _m_otherClickHandlersEnabled;
        
        private void Awake()
        {
#if NP_GAME
            _m_touchInfo = new TouchInfo(EALGUIOpButtonType.OP_BTN);
            
            _m_otherClickHandlers = GetComponents<IPointerClickHandler>();
            _m_otherClickHandlersEnabled = new bool[_m_otherClickHandlers.Length];
#endif
        }

        public void OnPointerDown(PointerEventData _eventData)
        {
#if NP_GAME
            if (_eventData == null || _m_touchInfo == null || ALInputControl.instance == null || GameInputListener.instance == null) 
                return;
            
            _m_fPressStartTime = Time.unscaledTime;
            GameInputListener.instance.OnPress(gameObject, true, _m_touchInfo);
#endif
        }
        public void OnBeginDrag(PointerEventData _eventData)
        {
#if NP_GAME
            if (_eventData == null || _m_touchInfo == null || ALInputControl.instance == null || GameInputListener.instance == null) 
                return;
            
            if (blockOthersClick && _m_otherClickHandlers != null)
            {
                for (int i = 0; i < _m_otherClickHandlers.Length; i++)
                {
                    IPointerClickHandler handler = _m_otherClickHandlers[i];
                    MonoBehaviour mono = handler as MonoBehaviour;
                    if (mono == null || mono == this)
                        continue;

                    _m_otherClickHandlersEnabled[i] = mono.enabled;
                    mono.enabled = false;
                }
            }
            
            _m_touchInfo.setPress(ALInputControl.instance.getOpSerialize(EALGUIOpButtonType.OP_BTN), _eventData.position, gameObject, false);
            GameInputListener.instance.OnDragStart(gameObject, _m_touchInfo, Time.unscaledTime - _m_fPressStartTime);
            _m_touchInfo.setUnPress();
#endif
        }

        public void OnDrag(PointerEventData _eventData)
        {
#if NP_GAME
            if (_eventData == null || _m_touchInfo == null || ALInputControl.instance == null || GameInputListener.instance == null) 
                return;
            _m_touchInfo.setPress(ALInputControl.instance.getOpSerialize(EALGUIOpButtonType.OP_BTN), _eventData.position, gameObject, false);
            GameInputListener.instance.OnDrag(gameObject, _eventData.delta, _m_touchInfo);
            _m_touchInfo.setUnPress();
#endif
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
#if NP_GAME
            if (_eventData == null || _m_touchInfo == null || ALInputControl.instance == null || GameInputListener.instance == null) 
                return;
            
            if (blockOthersClick && _m_otherClickHandlers != null)
            {
                for (int i = 0; i < _m_otherClickHandlers.Length; i++)
                {
                    IPointerClickHandler handler = _m_otherClickHandlers[i];
                    MonoBehaviour mono = handler as MonoBehaviour;
                    if (mono == null || mono == this)
                        continue;

                    bool wasEnabled = _m_otherClickHandlersEnabled[i];
                    mono.enabled = wasEnabled;
                }
            }
            
            _m_touchInfo.setPress(ALInputControl.instance.getOpSerialize(EALGUIOpButtonType.OP_BTN), _eventData.position, gameObject, false);
            GameInputListener.instance.OnDragEnd(gameObject, _m_touchInfo);
            _m_touchInfo.setUnPress();
#endif
        }
    }
}
using System;
using UnityEngine;

namespace GOE
{
    public class GTDCommonPosClickDragMono : _AMonoOutCombatClickDrag_CommonTDAni
    {
        public event Action onHolding;
        public event Action onPress;
        public event Action onUnPress;

        public event Action onClick;
#if NP_GAME
        public event Action<GameObject, TouchInfo, float> onDragStart;
        public event Action<GameObject, Vector2, TouchInfo> onDrag; 
        public event Action<GameObject, TouchInfo> onDragEnd; 
#endif
        protected override void _onHolding()
        {
#if NP_GAME
            onHolding?.Invoke();
#endif
        }

        protected override void _onPressEx()
        {
#if NP_GAME
            onPress?.Invoke();
#endif
        }

        protected override void _onUnPressEx()
        {
#if NP_GAME
            onUnPress?.Invoke();   
#endif
        }

        protected override void _onClickEx()
        {
#if NP_GAME
            onClick?.Invoke();
#endif
        }

#if NP_GAME
        
        protected override void _OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            onDragStart?.Invoke(_go, _touchInfo, _pressToDragTime);
        }

        protected override void _OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            onDrag?.Invoke(_go, _delta, _touchInfo);
        }

        protected override void _OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            onDragEnd?.Invoke(_go, _touchInfo);
        }
#endif
    }
}
using UnityEngine;

namespace GOE
{
    public abstract class _AMonoOutCombatClickDrag_CommonTDAni : _AMonoOutCombatClick_CommonTDAni
    {
#if NP_GAME
        public void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            _OnDragStart(_go, _touchInfo, _pressToDragTime);
        }

        public void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            _OnDrag(_go, _delta, _touchInfo);
        }

        public void OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            _OnDragEnd(_go, _touchInfo);
        }
        
        protected abstract void _OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime);
	    
        //弹起按钮的时候的处理
        protected abstract void _OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo);
        
        //点击时候处理
        protected abstract void _OnDragEnd(GameObject _go, TouchInfo _touchInfo);
#endif
    }
}
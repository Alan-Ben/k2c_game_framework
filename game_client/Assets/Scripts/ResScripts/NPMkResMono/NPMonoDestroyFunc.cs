using System;
using UnityEngine;

namespace GOE
{
    public class NPMonoDestroyFunc : MonoBehaviour
    {
        private Action _m_destroyFunc;
        public void setDestroyFunc(Action _func)
        {
            _m_destroyFunc = _func;
        }
        
        private void OnDestroy()
        {
            _m_destroyFunc?.Invoke();
            _m_destroyFunc = null;
        }
    }
}
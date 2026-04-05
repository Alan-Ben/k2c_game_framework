using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    public abstract class _ANPGGUIBasicSubWnd<T> : _ATALBasicUISubWnd<T> where T : _AALBasicUIWndMono
    {
        public _ANPGGUIBasicSubWnd(T _wnd)
            : base(_wnd)
        {
        } 
    }
}

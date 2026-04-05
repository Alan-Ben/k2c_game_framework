using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 战斗天赋控件小窗口
    /// </summary>
    public abstract class _ANPGGUIBasicGridItemWnd<T> : _ATALUGUIBasicGridItemWnd<T> where T : _TALUGUIMonoGridItem
    {
        protected _ANPGGUIBasicGridItemWnd(T _wnd)
                : base(_wnd)
        {
        }

    }
}

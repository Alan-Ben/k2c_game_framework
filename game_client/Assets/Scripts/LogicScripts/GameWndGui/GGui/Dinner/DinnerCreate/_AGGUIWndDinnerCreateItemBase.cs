using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _AGGUIWndDinnerCreateItemBase : _ATALBasicLoadPrefabSubUIWnd<_AGGUIMonoDinnerCreateItemBase>
    {
        public _AGGUIWndDinnerCreateItemBase(Transform _parent) : base(_parent)
        {
        }

        public abstract void setInfo(DinnerCreateItemShowInfo _info);
        public abstract void setCreatDinner();
    }
}

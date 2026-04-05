using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    public abstract class _ANPGGUIBasicLoadPrefabSubWnd<T> : _ATALBasicLoadPrefabSubUIWnd<T> where T : _AALBasicUIWndMono
    {
        public _ANPGGUIBasicLoadPrefabSubWnd(Transform _parent)
            : base(_parent)
        {
        }
#if AL_PUERTS
        /// <summary>
        /// 增加对应puerts的处理
        /// </summary>
        protected override ALPuertsManager _puertsMgr
        {
            get { return NPPuertsMgr.instance; }
        }
#endif
    }
}

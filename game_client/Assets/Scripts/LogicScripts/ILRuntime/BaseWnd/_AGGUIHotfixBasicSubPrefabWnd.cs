using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用的热更工程subPrefabWnd基类
    /// </summary>
    public abstract class _AGGUIHotfixBasicSubPrefabWnd : _ANPGGUIBasicLoadPrefabSubWnd<GGUIHotfixCommonMono>
    {
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public _AGGUIHotfixBasicSubPrefabWnd(Transform _parent) : base(_parent)
        {
        }
    }
}
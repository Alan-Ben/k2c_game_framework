using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGUISubWndLoadPrefabItem<T> : _ATALBasicLoadPrefabSubUIWnd<T> where T : _AALBasicUIWndMono
    {
        private readonly string _m_assetPath;
        private readonly string _m_objName;

        public _AGUISubWndLoadPrefabItem(string _assetPath, string _objName ,Transform _parent) : base(_parent)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }

        protected override string _monoAssetPath { get => _m_assetPath; }
        protected override string _monoObjName { get => _m_objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

    }
}
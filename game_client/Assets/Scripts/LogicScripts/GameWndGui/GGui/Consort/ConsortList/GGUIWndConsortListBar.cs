using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子列表bar
    /// </summary>
    public class GGUIWndConsortListBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoConsortListBar>
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        
        public GGUIWndConsortListBar(Transform _parent, NPCommonAssetPathInfo _assetPathInfo) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }
    }
}
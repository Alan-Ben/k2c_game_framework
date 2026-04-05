using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子加护大臣prefabSubWnd
    /// </summary>
    public class GGUIPrefabSubWndConsortBlessHero : _ANPGGUIBasicLoadPrefabSubWnd<GGUIPrefabSubMonoConsortBlessHero>
    {
        private NPCommonAssetPathInfo _m_iCommonAssetPathInfo;//通用资源路径信息
        
        public GGUIPrefabSubWndConsortBlessHero(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;
        }
        
        protected override string _monoAssetPath { get { return _m_iCommonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iCommonAssetPathInfo?.obj_name; } }
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
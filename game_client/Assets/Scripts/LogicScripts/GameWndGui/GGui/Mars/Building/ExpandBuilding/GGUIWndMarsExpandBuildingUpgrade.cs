using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 拓展建筑升级窗口
    /// </summary>
    public class GGUIWndMarsExpandBuildingUpgrade : GGUIWndMarsBuildingUpgrade<GGUIMonoMarsExpandBuildingUpgrade>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        private bool _m_bNeedFocusBuilding;//是否需要聚焦建筑
        
        public GGUIWndMarsExpandBuildingUpgrade(string _sAssetPath, string _sObjName, bool _needFocusBuilding) : base()
        {
            _m_sAssetPath = _sAssetPath;
            _m_sObjName = _sObjName;

            _m_bNeedFocusBuilding = _needFocusBuilding;
        }

        protected override bool needFocusBuildingPos { get { return _m_bNeedFocusBuilding; } }

        protected override void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_UPGRADE);
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}
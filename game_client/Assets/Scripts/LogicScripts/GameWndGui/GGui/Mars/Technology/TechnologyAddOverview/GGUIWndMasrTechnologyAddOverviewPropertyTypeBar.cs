using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科技加成总览属性类型bar
    /// </summary>
    public class GGUIWndMasrTechnologyAddOverviewPropertyTypeBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMasrTechnologyAddOverviewPropertyTypeBar>
    {
        private NPCommonAssetPathInfo _m_assetPathInfo;
        
        public GGUIWndMasrTechnologyAddOverviewPropertyTypeBar(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_assetPathInfo = _assetPathInfo;
        }


        protected override string _monoAssetPath { get { return _m_assetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPathInfo?.obj_name; } }
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

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(MarsTechnologyTypeRefObj _technologyTypeRefObj)
        {
            if (wnd == null || _technologyTypeRefObj == null)
                return;

            if (wnd.txtTypeName != null)
                ALUGUICommon.setLabelTxt(wnd.txtTypeName, TextTranslate.instance.getLanguage(_technologyTypeRefObj.name));
        }
    }
}

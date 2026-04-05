using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品列表bar
    /// </summary>
    public class GGUIWndEquipMainListGridBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoEquipMainListGridBar>
    {
        public GGUIWndEquipMainListGridBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEquipMainListGridBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEquipMainListGridBar.objName; } }
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
        /// <param name="_quality"></param>
        public void setInfo(EQuality _quality)
        {
            if(wnd == null)
                return;

            NPQualityRefObj qualityRef = GRefdataCoreMgr.instance.getQualityRef(_quality);
            if (qualityRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(qualityRef.name));
        }
    }
}

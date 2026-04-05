using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴列表未拥有bar
    /// </summary>
    public class GGUIWndHeroListBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoHeroListBar>
    {
        public GGUIWndHeroListBar(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroListBar.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroListBar.objName; } }
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


using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndMarsExploreHUDRoot : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoMarsExploreHUDRoot>
    {
        [NotNull] public static GGUIWndMarsExploreHUDRoot instance { get { return _g_instance ??= new GGUIWndMarsExploreHUDRoot(); } }
        private static GGUIWndMarsExploreHUDRoot _g_instance;

        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreHUDRoot.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreHUDRoot.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}
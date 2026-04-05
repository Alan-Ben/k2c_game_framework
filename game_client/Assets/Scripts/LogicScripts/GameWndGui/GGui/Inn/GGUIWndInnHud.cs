using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndInnHud : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoInnHud>
    {
        [NotNull] public static GGUIWndInnHud instance { get { return _g_instance ??= new GGUIWndInnHud(); } }
        private static GGUIWndInnHud _g_instance;
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnHud.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnHud.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}
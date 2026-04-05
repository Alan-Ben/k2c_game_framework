
using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndBuildingFollow : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoBuildingFollow>
    {
        [NotNull] public static GGUIWndBuildingFollow instance { get { return _g_instance ??= new GGUIWndBuildingFollow(); } }
        private static GGUIWndBuildingFollow _g_instance;
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBuildingFollow.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingFollow.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}
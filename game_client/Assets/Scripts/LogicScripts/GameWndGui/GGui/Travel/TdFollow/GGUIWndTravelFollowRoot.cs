
using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 游历场景跟随窗口
    /// </summary>
    public class GGUIWndTravelFollowRoot : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoTravelFollowRoot>
    {
        [NotNull] public static GGUIWndTravelFollowRoot instance { get { return _g_instance ??= new GGUIWndTravelFollowRoot(); } }
        private static GGUIWndTravelFollowRoot _g_instance;


        protected override string _monoAssetPath { get { return GGUIMonoTravelFollowRoot.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelFollowRoot.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}

using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏游玩提示窗口
    /// </summary>
    public class GGUIWndTreasureHuntGamePlayGamingTip : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlayGamingTip>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlayGamingTip instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlayGamingTip(); } }
        private static GGUIWndTreasureHuntGamePlayGamingTip _g_instance;
        
        
        public GGUIWndTreasureHuntGamePlayGamingTip() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlayGamingTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlayGamingTip.objName; } }
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
            if (wnd == null)
                return;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }
    }
}
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntGamePlayEndEffect : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlayEndEffect>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlayEndEffect instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlayEndEffect(); } }
        private static GGUIWndTreasureHuntGamePlayEndEffect _g_instance;
        

        private bool _m_isWin;
        

        public GGUIWndTreasureHuntGamePlayEndEffect() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlayEndEffect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlayEndEffect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            wnd.setWin(_m_isWin);
            ALCommonTaskController.CommonActionAddMonoTask(() => 
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_END_EFFECT), wnd.autoCloseTime);
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
        

        public void refreshWnd(bool _isWin)
        {
            _m_isWin = _isWin;
            if (wnd != null && _m_bIsShow)
                wnd.setWin(_m_isWin);
        }
    }
}
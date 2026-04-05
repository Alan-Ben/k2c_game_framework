using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndTreasureHuntGamePlayResumeCountDown : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlayResumeCountDown>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlayResumeCountDown instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlayResumeCountDown(); } }
        private static GGUIWndTreasureHuntGamePlayResumeCountDown _g_instance;
        
        
        private ALCommonEnableTaskController _m_countdownTask;
        private int _m_countdownSeconds;


        public GGUIWndTreasureHuntGamePlayResumeCountDown()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlayResumeCountDown.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlayResumeCountDown.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;
            
            _m_countdownSeconds = wnd.countdownTime + 1;
            _m_countdownTask.setDisable();
            _m_countdownTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_checkCountdown, 1f);
        }
        protected override void _onHideWnd()
        {
            _m_countdownTask.setDisable();
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
        
        
        private void _checkCountdown()
        {
            if (wnd == null)
                return;

            _m_countdownSeconds -= 1;
            ALUGUICommon.setLabelTxt(wnd.txtCountdown, _m_countdownSeconds);
            if (_m_countdownSeconds <= 0)
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_RESUME_COUNT_DOWN);
        }
    }
}
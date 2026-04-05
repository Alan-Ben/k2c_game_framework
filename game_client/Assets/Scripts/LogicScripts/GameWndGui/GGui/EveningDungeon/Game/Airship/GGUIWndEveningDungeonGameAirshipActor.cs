using System;

namespace GOE
{
    public class GGUIWndEveningDungeonGameAirshipActor : _ANPGGUIBasicSubWnd<GGUIMonoEveningDungeonGameAirshipActor>
    {
        private EEveningDungeonGameAirshipState _m_eCurState;//当前状态
        
        public GGUIWndEveningDungeonGameAirshipActor(GGUIMonoEveningDungeonGameAirshipActor _wnd) : base(_wnd)
        {
            initWnd();
        }

        public EEveningDungeonGameAirshipState curState { get { return _m_eCurState; } }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }
        
        /// <summary>
        /// 设置状态
        /// </summary>
        public void setAirShipState(EEveningDungeonGameAirshipState _airshipState)
        {
            _m_eCurState = _airshipState;
            
            if(wnd != null && wnd.multiStateShow != null)
                wnd.multiStateShow.setShowData(_airshipState);
            
            // if (wnd == null || wnd.skeletonGraphic == null)
            // {
            //     _onSpineAniPlayDone?.Invoke();
            //     return;
            // }
            //
            // EveningDungeonGameAirshipStateShowConfig showConfig = wnd.getStateShowConfig(_airshipState);
            // if (showConfig == null || showConfig.animationConfig == null)
            // {
            //     _onSpineAniPlayDone?.Invoke();
            //     return;
            // }
            //
            // showConfig.animationConfig.playAnimation(wnd.skeletonGraphic, _onSpineAniPlayDone);
        }
    }
}
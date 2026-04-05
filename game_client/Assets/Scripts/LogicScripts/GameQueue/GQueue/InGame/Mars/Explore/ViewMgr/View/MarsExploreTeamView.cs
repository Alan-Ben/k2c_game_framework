using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MarsExploreTeamView : _AALBasicLoadObj
    {
        [NotNull] private readonly MarsExploreTeamInfo _m_teamInfo;
        private GGUICommonFollowTarget _m_followTarget;
        private GTDMonoMarsExploreTeamUnit _m_mono;
        private MarsExploreTeamPathLineView _m_pathLineView;


        public MarsExploreTeamView([NotNull] MarsExploreTeamInfo _teamInfo)
        {
            _m_teamInfo = _teamInfo;
        }


        public MarsExploreTeamInfo teamInfo { get { return _m_teamInfo; } }
        public GTDMonoMarsExploreTeamUnit mono { get { return _m_mono; } }
        public GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        public Vector3 position
        {
            get
            {
                if (_m_mono == null)
                    return Vector3.zero;
                return _m_mono.transform.position;
            }
        }


        protected override void _loadOp()
        {
            Vector3 spawnPosition = _calculateTargetPosition();
            NPGGoIndex goIndex = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_scene_go_index;

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_setLoadDone);

            MainAdditionMarsExploreTDScene.instance.createUnit<GTDMonoMarsExploreTeamUnit>(goIndex, spawnPosition, _mono =>
            {
                _m_mono = _mono;
                if (_m_mono != null)
                {
                    _onInitDone();

                    // Load path line view
                    _m_pathLineView = new MarsExploreTeamPathLineView(this);
                    _m_pathLineView.load(stepCounter.addDoneStepCount);
                }
                else
                {
                    stepCounter.addDoneStepCount();
                }

                stepCounter.addDoneStepCount();
            });
        }
        protected override void _discard()
        {
            _onDiscard();

            _m_pathLineView?.discard();
            _m_pathLineView = null;

            if (_m_mono != null)
            {
                NPGGoIndex goIndex = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_scene_go_index;
                MainAdditionMarsExploreTDScene.instance.discardUnit(goIndex, _m_mono);
                _m_mono = null;
            }
        }


        public void tick()
        {
            _updatePosition();
            // _m_pathLineView?.updateLine();
        }


        private void _updatePosition()
        {
            if (_m_mono == null)
                return;

            Vector3 targetPos = _calculateTargetPosition();
            _m_mono.transform.position = targetPos;
        }
        private Vector3 _calculateTargetPosition()
        {
            GTDMonoMarsExploreHomeBase homeBase = MainAdditionMarsExploreTDScene.instance.getHomeBase();
            if (homeBase == null)
                return Vector3.zero;

            Vector3 homePos = homeBase.transform.position;

            // Get event position if available
            long eventPosId = _m_teamInfo.statePosId;
            GTDMonoMarsExploreEventPos eventPos = MainAdditionMarsExploreTDScene.instance.getEventPos(eventPosId);
            if (eventPos == null)
                return homePos;

            Vector3 targetPos = eventPos.transform.position;

            // Calculate progress based on state and time
            float progress = _calculateProgress();

            // Interpolate between home and target based on state
            switch (_m_teamInfo.state)
            {
                case Common.MarsEnum.EMarsExploreTeamState.MARCH:
                    // Moving from home to event
                    return Vector3.Lerp(homePos, targetPos, progress);
                case Common.MarsEnum.EMarsExploreTeamState.BACK:
                    // Moving from event back to home
                    return Vector3.Lerp(targetPos, homePos, progress);
                default:
                    return homePos;
            }
        }
        private float _calculateProgress()
        {
            long currentTime = FpsAndPingMgr.instance.serverTimeTag;
            long startTime = _m_teamInfo.stateStartTime;
            long endTime = _m_teamInfo.stateEndTime;
            
            if (endTime <= startTime)
                return 0f;

            long totalDuration = endTime - startTime;
            long elapsed = currentTime - startTime;

            float progress = (float)elapsed / totalDuration;
            return Mathf.Clamp01(progress);
        }


        private void _onInitDone()
        {
            if (_m_mono == null)
                return;

            if (_m_mono.followTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.followTarget, Vector3.zero);
                GGUIWndMarsExploreHUDRoot.instance.regInstance(_m_followTarget);
            }
        }
        private void _onDiscard()
        {
            if (_m_followTarget != null)
            {
                GGUIWndMarsExploreHUDRoot.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }
        }
    }
}

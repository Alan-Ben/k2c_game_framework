using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MarsExploreTeamPathLineView : _AALBasicLoadObj
    {
        [NotNull] private readonly MarsExploreTeamView _m_teamView;
        private GTDMonoMarsExploreTeamPathLine _m_mono;


        public MarsExploreTeamPathLineView([NotNull] MarsExploreTeamView _teamView)
        {
            _m_teamView = _teamView;
        }


        public GTDMonoMarsExploreTeamPathLine mono { get { return _m_mono; } }
        public LineRenderer lineRenderer { get { return _m_mono?.lineRenderer; } }


        protected override void _loadOp()
        {
            Vector3 spawnPosition = Vector3.zero; // LineRenderer uses world positions, not local
            NPGGoIndex goIndex = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_path_line_scene_go_index;
            MainAdditionMarsExploreTDScene.instance.createUnit<GTDMonoMarsExploreTeamPathLine>(goIndex, spawnPosition, _mono =>
            {
                _m_mono = _mono;
                if (_m_mono != null)
                    _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _onDiscard();

            if (_m_mono != null)
            {
                NPGGoIndex goIndex = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_path_line_scene_go_index;
                MainAdditionMarsExploreTDScene.instance.discardUnit(goIndex, _m_mono);
                _m_mono = null;
            }
        }


        public void updateLine()
        {
            if (_m_mono == null || _m_mono.lineRenderer == null)
                return;

            _getPathPositions(out Vector3 startPos, out Vector3 endPos);
            if (_m_teamView.teamInfo.state == EMarsExploreTeamState.BACK)
                (startPos, endPos) = (endPos, startPos); // Swap positions for BACK state
            
            _m_mono.lineRenderer.positionCount = 2;
            _m_mono.lineRenderer.SetPosition(0, startPos);
            _m_mono.lineRenderer.SetPosition(1, endPos);
        }


        private void _onInitDone()
        {
            if (_m_mono == null || _m_mono.lineRenderer == null)
                return;

            // TODO: Configure line renderer material, width, color, etc.
            updateLine();
        }
        private void _onDiscard()
        {
        }


        private void _getPathPositions(out Vector3 homePos, out Vector3 eventPos)
        {
            GTDMonoMarsExploreHomeBase homeBase = MainAdditionMarsExploreTDScene.instance.getHomeBase();
            if (homeBase == null)
            {
                homePos = Vector3.zero;
                eventPos = Vector3.zero;
                return;
            }

            homePos = homeBase.transform.position;

            long eventPosId = _m_teamView.teamInfo.statePosId;
            GTDMonoMarsExploreEventPos eventPosObj = MainAdditionMarsExploreTDScene.instance.getEventPos(eventPosId);
            if (eventPosObj == null)
            {
                eventPos = homePos;
                return;
            }

            eventPos = eventPosObj.transform.position;
        }
    }
}


using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class StageGoalBuildingView : _AALBasicLoadObj
    {
        [NotNull] private readonly StageGoalBigStepRefObj _m_bigStepRefObj;
        private readonly Vector3 _m_position;
        private readonly NPGGoIndex _m_resIndex;
        private readonly long _m_buildSfxId;
        private readonly float _m_buildSfxDelay;

        private GTDMonoStageGoalBuilding _m_mono;
        private GGUICommonFollowTarget _m_nameFollowTarget;
        private GGUIWndStageGoalBuildingEntranceFollowItemController _m_nameFollower;
        
        public StageGoalBuildingView([NotNull] StageGoalBigStepRefObj _bigStepRef, Vector3 _position)
        {
            _m_bigStepRefObj = _bigStepRef;
            _m_position = _position;
            _m_resIndex = _bigStepRef.building_index;
            _m_buildSfxId = _bigStepRef.build_sfx_id;
            _m_buildSfxDelay = _bigStepRef.build_sfx_delay;
        }
        

        public Vector3 position { get { return _m_position; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        
       
        protected override void _loadOp()
        {
            void createBuilding(Action _complete)
            {
                MainAdditionBuildingTDScene.instance.createBuilding<GTDMonoStageGoalBuilding>(_m_resIndex, _m_position,
                    _mono =>
                    {
                        if (_mono == null)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        _m_mono = _mono;

                        if (_m_mono.nameHudTarget != null)
                        {
                            _m_nameFollowTarget = new GGUICommonFollowTarget(_m_mono.nameHudTarget, Vector3.zero);
                            GGUIWndBuildingFollow.instance.regInstance(_m_nameFollowTarget);
                        }

                        if (_m_mono.clickMono != null)
                            _m_mono.clickMono.onClick += _onBuildingClick;

                        if (_m_nameFollowTarget != null)
                        {
                            _m_nameFollower = new GGUIWndStageGoalBuildingEntranceFollowItemController();
                            _m_nameFollower.setStepRef(_m_bigStepRefObj);
                            _m_nameFollowTarget.addController(_m_nameFollower);
                        }

                        _complete?.Invoke();
                    });
            }

            bool needShowBuildSfx = NPPlayer.instance.stageGoalComp.needShowBigStepBuildingBuild();
            if (!needShowBuildSfx || _m_buildSfxId <= 0)
                createBuilding(_setLoadDone);
            else
            {
                GCommon.setBuildingIsUnderConstruction(true, _m_buildSfxId);
                MainAdditionBuildingTDScene.instance.focusToTarget(_m_position, 0.5f);
                CommonTDSfxObj sfxObj = PlaySfxMgr.instance.playSfxByPos(_m_buildSfxId, _m_position);
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    GCommon.setBuildingIsUnderConstruction(false, _m_buildSfxId);
                    createBuilding(() =>
                    {
                        sfxObj?.forceDiscard();
                        _setLoadDone();
                    });
                }, _m_buildSfxDelay);
            }
        }
        protected override void _discard()
        {
            if (_m_mono == null)
                return;
            
            _m_nameFollowTarget?.discard();
            _m_nameFollowTarget = null;
            _m_nameFollower = null;
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onBuildingClick;
            
            MainAdditionBuildingTDScene.instance.discardBuilding<GTDMonoStageGoalBuilding>(_m_resIndex, _m_mono);
            _m_mono = null;
        }
        
        
        private void _onBuildingClick()
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndStageGoal.instance, UINodeTagConst.C_STAGE_GOAL, null, GGUIWndStageGoal.instance.setDefaultTab, 0);
        }
    }
}
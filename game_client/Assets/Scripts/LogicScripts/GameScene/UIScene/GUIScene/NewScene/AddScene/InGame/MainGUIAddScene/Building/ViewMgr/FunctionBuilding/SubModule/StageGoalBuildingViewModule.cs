using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class StageGoalBuildingViewModule : _AFunctionBuildingViewMgrSubModule
    {
        [NotNull] private readonly Dictionary<GTDMonoStageGoalFunction, StageGoalBuildingView> _m_buildingViewDict;
        private bool _m_isInit;
        private int _m_initSerialize;
        
        public StageGoalBuildingViewModule()
        {
            _m_buildingViewDict = new Dictionary<GTDMonoStageGoalFunction, StageGoalBuildingView>();
        }


        public override void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            _complete?.Invoke();
        }
        public override void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;

            foreach (StageGoalBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.discard();
            }
            _m_buildingViewDict.Clear();
            
            _m_initSerialize = ALSerializeOpMgr.next();
        }
        public override void addBuilding(_AGTDMonoBuildingFunction _function, Action _complete)
        {
            if (!_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            
            if (_function is not GTDMonoStageGoalFunction farmingFunction || farmingFunction.parentTrans == null)
            {
                _complete?.Invoke();
                return;
            }
            
            StageGoalBigStepRefObj bigStepRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            if (bigStepRefObj == null)
            {
                _complete?.Invoke();
                return;
            }
            
            StageGoalBuildingView buildingView = new StageGoalBuildingView(bigStepRefObj, farmingFunction.parentTrans.position);
            _m_buildingViewDict.Add(farmingFunction, buildingView);
            buildingView.load(_complete);
        }
        public override void removeBuilding(_AGTDMonoBuildingFunction _function)
        {
            if (!_m_isInit)
                return;
            
            if (_function is not GTDMonoStageGoalFunction farmingFunction)
                return;

            if (!_m_buildingViewDict.TryGetValue(farmingFunction, out StageGoalBuildingView buildingView))
                return;
            
            buildingView.discard();
            _m_buildingViewDict.Remove(farmingFunction);
        }
    }
}
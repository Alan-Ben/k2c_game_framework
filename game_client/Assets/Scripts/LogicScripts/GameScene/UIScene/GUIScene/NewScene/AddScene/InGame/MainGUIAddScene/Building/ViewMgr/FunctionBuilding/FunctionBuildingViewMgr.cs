using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class FunctionBuildingViewMgr
    {
        [ItemNotNull, NotNull] private readonly List<_AFunctionBuildingViewMgrSubModule> _m_moduleList;
        
        
        public FunctionBuildingViewMgr()
        {
            _m_moduleList = new List<_AFunctionBuildingViewMgrSubModule>()
            {
                new BusinessBuildingViewModule(),
                new FarmingBuildingViewModule(),
                new StageGoalBuildingViewModule(),
                new RoomBuildingViewModule(),
            };
        }
        
        
        public void init(Action _complete)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_complete);
            stepCounter.chgTotalStepCount(_m_moduleList.Count);
            foreach (_AFunctionBuildingViewMgrSubModule module in _m_moduleList)
            {
                module.init(stepCounter.addDoneStepCount);
            }
        }
        public void discard()
        {
            foreach (_AFunctionBuildingViewMgrSubModule module in _m_moduleList)
            {
                module.discard();
            }
        }
        

        public void addFunctionBuilding(_AGTDMonoBuildingFunction _buildingFunction, Action _complete)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_complete);
            stepCounter.chgTotalStepCount(_m_moduleList.Count);
            foreach (_AFunctionBuildingViewMgrSubModule module in _m_moduleList)
            {
                module.addBuilding(_buildingFunction, stepCounter.addDoneStepCount);
            }
        }
        public void removeFunctionBuilding(_AGTDMonoBuildingFunction _buildingFunction)
        {
            foreach (_AFunctionBuildingViewMgrSubModule module in _m_moduleList)
            {
                module.removeBuilding(_buildingFunction);
            }
        }
    }
}
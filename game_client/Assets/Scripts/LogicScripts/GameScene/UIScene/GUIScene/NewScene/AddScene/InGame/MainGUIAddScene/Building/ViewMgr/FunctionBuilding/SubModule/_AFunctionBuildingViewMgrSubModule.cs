using System;

namespace GOE
{
    public abstract class _AFunctionBuildingViewMgrSubModule
    {
        public abstract void init(Action _complete);
        public abstract void discard();
        public abstract void addBuilding(_AGTDMonoBuildingFunction _function, Action _complete);
        public abstract void removeBuilding(_AGTDMonoBuildingFunction _function);
    }
}
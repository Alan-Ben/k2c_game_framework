using Common.ConditionEnum;

namespace GOE
{
    public class BuildingConditionFalse : _ABasicBuildingCondition
    {
        public override EBuildingConditionType conditionType { get { return EBuildingConditionType.NONE; } }
        public override bool isEnable(BusinessBuildingRefObj _data, NPVarInfo _varVariableInfo)
        {
            return false;
        }
    }
}
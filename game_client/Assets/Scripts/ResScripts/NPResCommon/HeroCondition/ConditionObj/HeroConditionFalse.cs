using Common.ConditionEnum;

namespace GOE
{
    public class HeroConditionFalse : _ABasicHeroCondition
    {
        public override EHeroConditionType conditionType { get { return EHeroConditionType.NONE; } }
        public override bool isEnable(HeroRefObj _data, HeroConditionVarInfo _varVariableInfo)
        {
            return false;
        }
    }
}
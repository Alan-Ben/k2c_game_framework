using Common.ConditionEnum;

namespace GOE
{
    /// <summary>
    /// 大臣条件附带数值参数信息
    /// </summary>
    public class HeroConditionVarInfo : _ATVarInfo<EHeroVariableVarType, HeroConditionVarObj>
    {
        protected override HeroConditionVarObj _getNewVarObj()
        {
            return WCGSingleton<HeroConditionVarObjCache>.instance.popItem();
        }

        protected override void _resetVarObj(HeroConditionVarObj _varObj)
        {
            WCGSingleton<HeroConditionVarObjCache>.instance.pushBackCacheItem(_varObj);
        }
    }
}
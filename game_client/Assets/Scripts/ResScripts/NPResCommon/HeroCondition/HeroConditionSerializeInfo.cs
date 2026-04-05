using System;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class HeroConditionSerializeInfo : _ATBasicConditionSerializeInfo<HeroConditionGroupObj, HeroConditionVarInfo
#if NP_GAME
        , HeroRefObj
#endif
    >
    {
        protected override HeroConditionGroupObj getConditionGroupObj(string s_condition, string _err)
        {
            return HeroConditionGroupObj.readConditionGroupList(s_condition, string.Empty);
        }

        public override bool IsEnable(
#if NP_GAME
            HeroRefObj _heroRefObj,
#endif
            HeroConditionVarInfo _varVariableInfo)
        {
            HeroConditionGroupObj conditionGroup = condition;
            if (null == conditionGroup)
                return true;

            return HeroConditionGroupObj.IsEnable(condition
#if NP_GAME
                , _heroRefObj
#else
                ,null
#endif
                    , _varVariableInfo);
        }
        
        public static HeroConditionSerializeInfo ReadFromString(string _str)
        {
            HeroConditionSerializeInfo info = new HeroConditionSerializeInfo();
            info.ParseFromString(_str, string.Empty);

            return info;
        }
        public static HeroConditionSerializeInfo ReadFromString(string _str, string _fieldName)
        {
            HeroConditionSerializeInfo info = new HeroConditionSerializeInfo();
            info.ParseFromString(_str, _fieldName);

            return info;
        }
    }
}
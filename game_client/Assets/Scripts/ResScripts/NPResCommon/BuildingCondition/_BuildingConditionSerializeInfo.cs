using System;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class _BuildingConditionSerializeInfo : _ATBasicConditionSerializeInfo<BuildingConditionGroupObj, NPVarInfo
#if NP_GAME
        , BusinessBuildingRefObj
#endif
    >
    {
        protected override BuildingConditionGroupObj getConditionGroupObj(string s_condition, string _err)
        {
            return BuildingConditionGroupObj.readConditionGroupList(s_condition, string.Empty);
        }

        public override bool IsEnable(
#if NP_GAME
            BusinessBuildingRefObj _businessBuildingRefObj,
#endif
            NPVarInfo _varVariableInfo)
        {
            BuildingConditionGroupObj conditionGroup = condition;
            if (null == conditionGroup)
                return true;

            return BuildingConditionGroupObj.IsEnable(condition
#if NP_GAME
                , _businessBuildingRefObj
#else
                ,null
#endif
                    , _varVariableInfo);
        }
        
        public static _BuildingConditionSerializeInfo ReadFromString(string _str)
        {
            _BuildingConditionSerializeInfo info = new _BuildingConditionSerializeInfo();
            info.ParseFromString(_str, string.Empty);

            return info;
        }
        public static _BuildingConditionSerializeInfo ReadFromString(string _str, string _fieldName)
        {
            _BuildingConditionSerializeInfo info = new _BuildingConditionSerializeInfo();
            info.ParseFromString(_str, _fieldName);

            return info;
        }
    }
}
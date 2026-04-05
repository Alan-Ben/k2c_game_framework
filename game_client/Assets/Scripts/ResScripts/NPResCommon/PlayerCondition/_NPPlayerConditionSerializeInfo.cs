using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class _NPPlayerConditionSerializeInfo : _ATBasicConditionSerializeInfo<NPPlayerConditionGroupObj, NPVarInfo
#if NP_GAME
        , NPPlayer
#endif
    >
    {
        protected override NPPlayerConditionGroupObj getConditionGroupObj(string s_condition, string _err)
        {
            return NPPlayerConditionGroupObj.readConditionGroupList(s_condition, string.Empty);
        }

        
        /// <summary>
        /// 判断本对象的条件是否匹配
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public override bool IsEnable(
#if NP_GAME
            NPPlayer _conditionDealerData, 
#endif
            NPVarInfo _varVariableInfo)
        {
            return IsEnable(_varVariableInfo);
        }
        
#if NP_GAME
        /// <summary>
        /// 判断本对象的条件是否匹配
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool IsEnable(NPVarInfo _varVariableInfo)
        {
            NPPlayerConditionGroupObj conditionGroup = condition;
            if (null == conditionGroup)
                return true;

            return NPPlayerConditionGroupObj.IsEnable(conditionGroup, _varVariableInfo);
        }
        
        /// <summary>
        /// 是否没有条件 或 条件成立(若对这个方法结果取非, 就代表 是否存在条件 且 条件不满足)
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool isNoConditionOrEnable(NPVarInfo _varVariableInfo)
        {
            return isEmpty || IsEnable(_varVariableInfo);
        }
        
#endif
        
        public static _NPPlayerConditionSerializeInfo ReadFromString(string _str)
        {
            _NPPlayerConditionSerializeInfo info = new _NPPlayerConditionSerializeInfo();
            info.ParseFromString(_str, string.Empty);

            return info;
        }
        public static _NPPlayerConditionSerializeInfo ReadFromString(string _str, string _fieldName)
        {
            _NPPlayerConditionSerializeInfo info = new _NPPlayerConditionSerializeInfo();
            info.ParseFromString(_str, _fieldName);

            return info;
        }
    }
}

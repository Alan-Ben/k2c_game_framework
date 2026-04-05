using ALPackage;
using Common.ConditionEnum;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 判断建筑是否是指定偏向属性 CS_SPEC_ATTR_TYPE:ESpecAttrType
    /// </summary>
    public class BuildingCondition_CS_SPEC_ATTR_TYPE : _ABasicBuildingCondition
    {
        private ESpecAttrType _m_eBasicAttrType;//特长枚举
        
        public override EBuildingConditionType conditionType { get { return EBuildingConditionType.CS_SPEC_ATTR_TYPE; } }
        
        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static BuildingCondition_CS_SPEC_ATTR_TYPE readStr(ALStringReader _reader)
        {
            if (_reader == null)
            {
                UnityEngine.Debug.LogError("Can not read str for EBuildingConditionType.CS_SPEC_ATTR_TYPE _reader == null");
                return null;
            }
            
            string attrTypeStr = _reader.readItem(':');
            if (string.IsNullOrEmpty(attrTypeStr))
            {
                UnityEngine.Debug.LogError("Can not read str for EBuildingConditionType.CS_SPEC_ATTR_TYPE[" + _reader.srcString + "]");
                return null;
            }

            BuildingCondition_CS_SPEC_ATTR_TYPE cond = new BuildingCondition_CS_SPEC_ATTR_TYPE();

            ALCommon.TryEnumParse(typeof(ESpecAttrType), attrTypeStr, out cond._m_eBasicAttrType);
            return cond;
        }
        
        public override bool isEnable(BusinessBuildingRefObj _data, NPVarInfo _varVariableInfo)
        {
            if (_data == null && _varVariableInfo == null)
                return false;
#if NP_GAME
            if(_data != null)
                return _data.attr_type == _m_eBasicAttrType;

            if(_varVariableInfo != null)
                return _varVariableInfo.getValue(ENPPlayerVariableVarType.SPEC_ATTR_TYPE) == (long)_m_eBasicAttrType;
#endif
            return false;
        }
    }
}
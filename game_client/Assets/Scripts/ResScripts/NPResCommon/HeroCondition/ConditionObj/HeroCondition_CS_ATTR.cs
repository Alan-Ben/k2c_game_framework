using ALPackage;
using Common.ConditionEnum;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 判断大臣是否拥有该特长 CS_ATTR:EBasicAttrType
    /// </summary>
    public class HeroCondition_CS_ATTR : _ABasicHeroCondition
    {
        private ESpecAttrType _m_eBasicAttrType;//特长枚举
        
        public override EHeroConditionType conditionType { get { return EHeroConditionType.CS_ATTR; } }
        
        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static HeroCondition_CS_ATTR readStr(ALStringReader _reader)
        {
            if (_reader == null)
            {
                UnityEngine.Debug.LogError("Can not read str for EHeroConditionType.CS_ATTR _reader == null");
                return null;
            }
            
            string attrTypeStr = _reader.readItem(':');
            if (string.IsNullOrEmpty(attrTypeStr))
            {
                UnityEngine.Debug.LogError("Can not read str for EHeroConditionType.CS_ATTR[" + _reader.srcString + "]");
                return null;
            }

            HeroCondition_CS_ATTR cond = new HeroCondition_CS_ATTR();

            ALCommon.TryEnumParse(typeof(ESpecAttrType), attrTypeStr, out cond._m_eBasicAttrType);
            return cond;
        }
        
        public override bool isEnable(HeroRefObj _data, HeroConditionVarInfo _varVariableInfo)
        {
            if (_data == null)
                return false;

#if NP_GAME
            return _data.spec_attr_type == _m_eBasicAttrType;
#endif
            return false;
        }
    }
}
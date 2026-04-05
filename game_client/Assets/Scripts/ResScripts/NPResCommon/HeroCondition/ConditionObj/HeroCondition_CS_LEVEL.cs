using ALPackage;
using Common.ConditionEnum;

namespace GOE
{
    /// <summary>
    /// 判断大臣等级是否在范围内 CS_LEVEL:min:max
    /// </summary>
    public class HeroCondition_CS_LEVEL : _ABasicHeroCondition
    {
        private long _m_lMinValue;
        private long _m_lMaxValue;
        
        public override EHeroConditionType conditionType { get { return EHeroConditionType.CS_LEVEL; } }
        
        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static HeroCondition_CS_LEVEL readStr(ALStringReader _reader)
        {
            if (_reader == null)
            {
                UnityEngine.Debug.LogError("Can not read str for EHeroConditionType.CS_LEVEL _reader == null");
                return null;
            }
            
            string minValueStr = _reader.readItem(':');
            string maxValueStr = _reader.readItem(':');
        
            if (string.IsNullOrEmpty(minValueStr) || string.IsNullOrEmpty(maxValueStr))
            {
                UnityEngine.Debug.LogError("Can not read str for EHeroConditionType.CS_LEVEL[" + _reader.srcString + "]");
                return null;
            }

            HeroCondition_CS_LEVEL cond = new HeroCondition_CS_LEVEL();
        
            cond._m_lMinValue = long.Parse(minValueStr);
            cond._m_lMaxValue = long.Parse(maxValueStr);
            return cond;
        }
        
        public override bool isEnable(HeroRefObj _data, HeroConditionVarInfo _varVariableInfo)
        {
            if (_data == null)
                return false;

#if NP_GAME
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_data.id);
            long heroLevel = heroInfo?.level ?? 0;

            return WCGLongRange.inRange(heroLevel, _m_lMinValue, _m_lMaxValue);      
#endif
            return false;
        }
    }
}
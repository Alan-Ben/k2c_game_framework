using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 已经领取七天登录奖励数量C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT
    /// </summary>
    public class NPPlayerCondition_C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT : _ANPBasicPlayerCondition
    {
        private int _m_lMinCount = -1;//最小值
        private int _m_lMaxCount = -1;//最大值
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT cond = new NPPlayerCondition_C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT();

            string minCount = _reader.readItem(':');
            string maxCount = _reader.readItem(':');

            cond._m_lMinCount = int.Parse(minCount);
            if (minCount == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.C_SEVEN_DAY_LOGIN_HAD_DRAW_ALL[" + _reader.srcString + "]");
                return null;
            }
            if(null != maxCount)
                cond._m_lMaxCount = int.Parse(maxCount);

         
            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return isRange(NPPlayer.instance.sevenDayLoginComp.hadDrawRewardCount, _m_lMinCount, _m_lMaxCount);
#else
	        return false;
#endif
        }
    }
}
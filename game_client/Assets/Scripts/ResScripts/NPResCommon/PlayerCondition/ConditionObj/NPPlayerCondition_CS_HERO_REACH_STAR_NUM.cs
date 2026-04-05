using System;
using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 达到指定星级大臣数量 CS_HERO_REACH_STAR_NUM:星级:min（:max）
    /// </summary>
	public class NPPlayerCondition_CS_HERO_REACH_STAR_NUM : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HERO_REACH_STAR_NUM; } }

        private long _m_lStar;
        private long _m_lMinValue = -1;//-1代表无下限
        private long _m_lMaxValue = -1;//-1代表无上限

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_HERO_REACH_STAR_NUM readStr(ALStringReader _reader)
	    {
            NPPlayerCondition_CS_HERO_REACH_STAR_NUM cond = new NPPlayerCondition_CS_HERO_REACH_STAR_NUM();

            string starStr = _reader.readItem();
            string rawMinValue = _reader.readItem();
            if (string.IsNullOrEmpty(starStr) || string.IsNullOrEmpty(rawMinValue))
            {
                Debug.LogError($"读取CS_HERO_REACH_STAR_NUM失败，str:{_reader.srcString}");
                return null;
            }

            try
            {
                cond._m_lStar = long.Parse(starStr);
                cond._m_lMinValue = long.Parse(rawMinValue);

                string rawMaxValue = _reader.readItem();
                if (!string.IsNullOrEmpty(rawMaxValue))
                    cond._m_lMaxValue = long.Parse(rawMaxValue);
                else
                    cond._m_lMaxValue = -1;
            }
            catch (Exception e)
            {
                Debug.LogError($"读取CS_HERO_REACH_STAR_NUM失败，str:{_reader.srcString},{e}");
            }
            

            return cond;
        }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            return isRange(NPPlayer.instance.heroComponent.getFitStarHeroCount(_m_lStar), _m_lMinValue, _m_lMaxValue);
#else
        return false;
#endif
        }
    }
}
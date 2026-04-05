using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星空闲人口数量判断 CS_MARS_IDLE_PEOPLE_NUM:min_value（:max_value）
    /// </summary>
    public class NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_MARS_IDLE_PEOPLE_NUM; } }

        private WCGLongRange _m_lRange;

        public static NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM readStr(ALStringReader _reader)
        {
            NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM cond = new NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM();

            string minVS = _reader.readItem(':');
            if (null != minVS)
            {
                string maxVS = _reader.readItem(':');
                if (null != maxVS)
                    cond._m_lRange = new WCGLongRange(minVS, maxVS);
                else
                    cond._m_lRange = new WCGLongRange(minVS, "-1");
            }
            else
            {
                cond._m_lRange = new WCGLongRange(1, -1);
            }

            return cond;
        }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return isRange(NPPlayer.instance.marsComp.peopleSubComponent.idlePeopleNum, _m_lRange.min, _m_lRange.max);
#else
            return false;
#endif
        }
    }
}
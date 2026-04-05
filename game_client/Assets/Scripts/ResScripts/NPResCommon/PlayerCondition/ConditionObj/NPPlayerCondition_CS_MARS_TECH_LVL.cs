using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星科技等级 CS_MARS_TECH_LVL:tech_id(:minLevel:maxLevel)
    /// </summary>
    public class NPPlayerCondition_CS_MARS_TECH_LVL : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_MARS_TECH_LVL; } }

        private long _m_lTechId;
        private WCGLongRange _m_lRange;

        public static NPPlayerCondition_CS_MARS_TECH_LVL readStr(ALStringReader _reader)
        {
            string techId = _reader.readItem(':');
            if (null == techId)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_MARS_TECH_LVL[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_MARS_TECH_LVL cond = new NPPlayerCondition_CS_MARS_TECH_LVL();

            cond._m_lTechId = ALCommon.ParseLong(techId);

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
            if (NPPlayer.instance == null || NPPlayer.instance.marsComp == null)
                return false;

            MarsTechnologyInfo techInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_m_lTechId);
            if (techInfo == null)
                return false;

            long level = techInfo.lvl;
            return isRange(level, _m_lRange.min, _m_lRange.max);
#else
            return false;
#endif
        }
    }
}

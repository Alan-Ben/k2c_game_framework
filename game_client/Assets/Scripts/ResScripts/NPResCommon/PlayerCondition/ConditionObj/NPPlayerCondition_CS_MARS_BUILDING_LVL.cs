using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星建筑等级 CS_MARS_BUILDING_LVL:building_id(:minLevel:maxLevel)
    /// </summary>
    public class NPPlayerCondition_CS_MARS_BUILDING_LVL : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_MARS_BUILDING_LVL; } }

        private long _m_lBuildingId;
        private WCGLongRange _m_lRange;

        public static NPPlayerCondition_CS_MARS_BUILDING_LVL readStr(ALStringReader _reader)
        {
            string buildingId = _reader.readItem(':');
            if (null == buildingId)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_MARS_BUILDING_LVL[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_MARS_BUILDING_LVL cond = new NPPlayerCondition_CS_MARS_BUILDING_LVL();

            cond._m_lBuildingId = ALCommon.ParseLong(buildingId);

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
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_m_lBuildingId);
            if (buildingInfo == null || buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt || buildingInfo.state == MarsBuildingInfo.StateType.Constructing)
                return false;

            long level = buildingInfo.level;
            return isRange(level, _m_lRange.min, _m_lRange.max);
#else
            return false;
#endif
        }
    }
}
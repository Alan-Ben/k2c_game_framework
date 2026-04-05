using NPEnum;
using ALPackage;
using Common.BuildingEnum;

namespace GOE
{
    /// <summary>
    /// 玩家已放置建筑功能等级 CS_BUILDING_FUNC_LEVEL:building_id:EBuildingFuncEnum（:minLevel:maxLevel）
    /// </summary>
    public class NPPlayerCondition_CS_BUILDING_FUNC_LEVEL : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL; } }

	    private long _m_lBuildingId;//建筑id
	    private EBuildingFuncEnum _m_eBuildFuncType;//建筑功能类型
	    private WCGLongRange _m_lRange;//范围值

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_BUILDING_FUNC_LEVEL readStr(ALStringReader _reader)
	    {
	        string buildingId = _reader.readItem(':');
	        string buildingFuncType = _reader.readItem(':');
	        if(null == buildingId || null == buildingFuncType)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_BUILDING_FUNC_LEVEL cond = new NPPlayerCondition_CS_BUILDING_FUNC_LEVEL();

            cond._m_lBuildingId = ALCommon.ParseLong(buildingId);
            cond._m_eBuildFuncType = (EBuildingFuncEnum)ALPackage.ALCommon.EnumParse(typeof(EBuildingFuncEnum), buildingFuncType, true);

	        string minVS = _reader.readItem(':');
	        if(null != minVS)
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
            long level = NPPlayer.instance.buildingComp.getBuildingLevel(_m_eBuildFuncType, _m_lBuildingId);
            return isRange(level, _m_lRange.min, _m_lRange.max);
#else
	        return false;
#endif
        }
	}
}
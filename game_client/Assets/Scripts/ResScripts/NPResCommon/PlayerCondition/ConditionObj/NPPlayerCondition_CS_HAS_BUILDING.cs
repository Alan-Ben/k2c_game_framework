using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
	/// 玩家已放置建筑 CS_HAS_BUILDING:building_id
	/// </summary>
    public class NPPlayerCondition_CS_HAS_BUILDING : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAS_BUILDING; } }

	    private long _m_lBuildingId;//建筑id

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_HAS_BUILDING readStr(ALStringReader _reader)
	    {
	        string buildingId = _reader.readItem(':');
	        if(null == buildingId)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_HAS_BUILDING[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_HAS_BUILDING cond = new NPPlayerCondition_CS_HAS_BUILDING();

	        cond._m_lBuildingId = ALCommon.ParseLong(buildingId);
	        return cond;
	    }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            return NPPlayer.instance.buildingComp.hasBuilding(_m_lBuildingId);
#else
	        return false;
#endif
        }
	}
}
using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
	/// 是否解锁建筑产品 CS_HAD_UNLOCK_BUILDING_PRODUCT:建筑id:产品id
	/// </summary>
    public class NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT : _ANPBasicPlayerCondition
    {
        private long _m_lBuildingId;//建筑id
        private long _m_lProductId;//产品id

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAD_UNLOCK_BUILDING_PRODUCT; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT readStr(ALStringReader _reader)
	    {
	        string buildingId = _reader.readItem(':');
	        string productId = _reader.readItem(':');
	        if(string.IsNullOrEmpty(buildingId) || string.IsNullOrEmpty(productId))
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_HAD_UNLOCK_BUILDING_PRODUCT[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT cond = new NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT();

	        cond._m_lBuildingId = ALCommon.ParseLong(buildingId);
	        cond._m_lProductId = ALCommon.ParseLong(productId);
	        return cond;
	    }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
			BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(_m_lBuildingId);
			if(buildingInfo == null)
				return false;

            return buildingInfo.isProductUnlocked(_m_lProductId);
#else
	        return false;
#endif
        }
	}
}
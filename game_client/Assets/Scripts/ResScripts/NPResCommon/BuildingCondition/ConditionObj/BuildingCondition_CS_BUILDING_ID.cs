using ALPackage;
using Common.ConditionEnum;

namespace GOE
{
    /// <summary>
    /// 判断是否是指定建筑 CS_BUILDING_ID:BuildingId
    /// </summary>
    public class BuildingCondition_CS_BUILDING_ID : _ABasicBuildingCondition
    {
        private long _m_lBuildingId;//建筑id
        
        public override EBuildingConditionType conditionType { get { return EBuildingConditionType.CS_BUILDING_ID; } }
        
        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static BuildingCondition_CS_BUILDING_ID readStr(ALStringReader _reader)
        {
            if (_reader == null)
            {
                UnityEngine.Debug.LogError("Can not read str for EBuildingConditionType.CS_BUILDING_ID _reader == null");
                return null;
            }
            
            string idString = _reader.readItem(':');
            if (string.IsNullOrEmpty(idString))
            {
                UnityEngine.Debug.LogError("Can not read str for EBuildingConditionType.CS_BUILDING_ID[" + _reader.srcString + "]");
                return null;
            }

            BuildingCondition_CS_BUILDING_ID cond = new BuildingCondition_CS_BUILDING_ID();
            cond._m_lBuildingId = ALCommon.ParseLong(idString);
            return cond;
        }
        
        public override bool isEnable(BusinessBuildingRefObj _data, NPVarInfo _varVariableInfo)
        {
            if (_data == null)
                return false;
#if NP_GAME
            return _data.building_id == _m_lBuildingId;
#endif
            return false;
        }
    }
}
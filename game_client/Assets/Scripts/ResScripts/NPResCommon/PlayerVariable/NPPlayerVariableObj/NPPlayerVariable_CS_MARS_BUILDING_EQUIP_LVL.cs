using ALPackage;
using NPEnum;
using System;


namespace GOE
{
    /// <summary>
    /// 指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID
    /// </summary>
    public class NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL : _ANPBasicPlayerVariableObj
    {
        private long _m_lBuildingId;//火星建筑ID
        private long _m_lEquipId;//部件ID

        protected NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_MARS_BUILDING_EQUIP_LVL; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
            long level = 0;
#if NP_GAME
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_m_lBuildingId);
            if (buildingInfo != null && buildingInfo.equipmentData.equipmentList != null && 
                buildingInfo.state != MarsBuildingInfo.StateType.Unbuilt && buildingInfo.state != MarsBuildingInfo.StateType.Constructing)
            {
                for (int i = 0; i < buildingInfo.equipmentData.equipmentList.Count; i++)
                {
                    MarsBuildingEquipmentInfo equipInfo = buildingInfo.equipmentData.equipmentList[i];
                    if (equipInfo != null && equipInfo.refObj.id == _m_lEquipId && equipInfo.isUnlock)
                    {
                        level = equipInfo.level;
                        break;
                    }

                }
            }
#endif
            return level;
        }

        public static NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL readVariable(ALStringReader _reader)
        {
            NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL variableObj = new NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL();
            string buildingIdStr = _reader.readItem('@');
            string equipIdStr = _reader.readItem('@');

            if (string.IsNullOrEmpty(buildingIdStr))
            {
                UnityEngine.Debug.LogError("高级公式——指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID Error Str: " + _reader.srcString);
                return null;
            }

            if (string.IsNullOrEmpty(equipIdStr))
            {
                UnityEngine.Debug.LogError("高级公式——指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID Error Str: " + _reader.srcString);
                return null;
            }

            try
            {
                variableObj._m_lBuildingId = long.Parse(buildingIdStr);
                variableObj._m_lEquipId = long.Parse(equipIdStr);
                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("高级公式——指定火星建筑指定部件等级 CS_MARS_BUILDING_EQUIP_LVL@火星建筑ID@部件ID Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}

using ALPackage;
using NPEnum;
using System;


namespace GOE
{
    /// <summary>
    /// 指定火星建筑等级 CS_MARS_BUILDING_LVL@火星建筑ID
    /// </summary>
    public class NPPlayerVariable_CS_MARS_BUILDING_LVL : _ANPBasicPlayerVariableObj
    {
        private long _m_lBuildingId;//火星建筑ID

        protected NPPlayerVariable_CS_MARS_BUILDING_LVL()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_MARS_BUILDING_LVL; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
            long level = 0;
#if NP_GAME
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_m_lBuildingId);
            if (buildingInfo != null && buildingInfo.state != MarsBuildingInfo.StateType.Unbuilt && buildingInfo.state != MarsBuildingInfo.StateType.Constructing)
                level = buildingInfo.level;
#endif
            return level;
        }

        public static NPPlayerVariable_CS_MARS_BUILDING_LVL readVariable(ALStringReader _reader)
        {
            NPPlayerVariable_CS_MARS_BUILDING_LVL variableObj = new NPPlayerVariable_CS_MARS_BUILDING_LVL();
            string buildingIdStr = _reader.readItem('@');

            if (string.IsNullOrEmpty(buildingIdStr))
            {
                UnityEngine.Debug.LogError("高级公式——指定火星建筑等级 CS_MARS_BUILDING_LVL@火星建筑ID Error Str: " + _reader.srcString);
                return null;
            }

            try
            {
                variableObj._m_lBuildingId = long.Parse(buildingIdStr);
                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("高级公式——指定火星建筑等级 CS_MARS_BUILDING_LVL@火星建筑ID Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}

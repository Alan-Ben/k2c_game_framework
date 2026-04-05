using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;


namespace GOE
{
    /// <summary>
    /// 指定火星建筑派遣居民数量 CS_MARS_BUILDING_DISPATCH_NUM@火星建筑ID
    /// </summary>
    public class NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM : _ANPBasicPlayerVariableObj
    {
        private long _m_lBuildingId;//火星建筑ID

        protected NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_MARS_BUILDING_DISPATCH_NUM; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
            long peopleCount = 0;
#if NP_GAME
            if (_m_lBuildingId <= 0)
            {
                List<MarsBuildingInfo> buildingInfoList = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoList();
                foreach (MarsBuildingInfo marsBuildingInfo in buildingInfoList)
                {
                    if (marsBuildingInfo != null && marsBuildingInfo.state != MarsBuildingInfo.StateType.Unbuilt && marsBuildingInfo.state != MarsBuildingInfo.StateType.Constructing)
                        peopleCount += marsBuildingInfo.settleSlotData.peopleCount;
                }
            }
            else
            {
                MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_m_lBuildingId);
                if (buildingInfo != null && buildingInfo.state != MarsBuildingInfo.StateType.Unbuilt && buildingInfo.state != MarsBuildingInfo.StateType.Constructing)
                    peopleCount = buildingInfo.settleSlotData.peopleCount;
            }
#endif
            return peopleCount;
        }

        public static NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM readVariable(ALStringReader _reader)
        {
            NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM variableObj = new NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM();
            string buildingIdStr = _reader.readItem('@');

            if (string.IsNullOrEmpty(buildingIdStr))
            {
                variableObj._m_lBuildingId = 0;
                return variableObj;
            }
            else
            {
                try
                {
                    variableObj._m_lBuildingId = long.Parse(buildingIdStr);
                    return variableObj;
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogError("高级公式——指定火星建筑派遣居民数量 CS_MARS_BUILDING_DISPATCH_NUM@火星建筑ID Error Str: " + _reader.srcString);
                    return null;
                }
            }

        }
    }
}

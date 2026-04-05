using System;
using ALPackage;
using NPEnum;
using Common.BuildingEnum;

namespace GOE
{
    /// <summary>
    /// 建筑等级 CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有）
    /// </summary>
    public class NPPlayerVariable_CS_BUILDING_LEVEL : _ANPBasicPlayerVariableObj
	{
	    private EBuildingFuncEnum _m_eBuildingFuncType;//建筑功能类型
        private long _m_lBuildingId;//建筑id

	    public EBuildingFuncEnum ValueType { get { return _m_eBuildingFuncType; } }

	    protected NPPlayerVariable_CS_BUILDING_LEVEL()
	    {
	    }

        /// <summary>
		/// 获取条件类型
		/// </summary>
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_BUILDING_LEVEL; } }


	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
            return NPPlayer.instance.buildingComp.getBuildingLevel(_m_eBuildingFuncType, _m_lBuildingId);
#else
	        return 0L;
#endif
        }


	public static NPPlayerVariable_CS_BUILDING_LEVEL readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string typeS = _reader.readItem('@');
	        string buildingIdS = _reader.readItem('@');
	        //逐个判断
	        if (string.IsNullOrEmpty(typeS))
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - typeS example: CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有） Error Str: " + _reader.srcString);
	            return null;
	        }
	        if (string.IsNullOrEmpty(buildingIdS))
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - buildingIdS example: CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有） Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariable_CS_BUILDING_LEVEL variableObj = new NPPlayerVariable_CS_BUILDING_LEVEL();

	        try {
	            variableObj._m_eBuildingFuncType = (EBuildingFuncEnum)ALCommon.EnumParse(typeof(EBuildingFuncEnum), typeS, true);
                variableObj._m_lBuildingId = ALCommon.GetLong(buildingIdS);

	            return variableObj;
	        }
	        catch (Exception) 
            {
	            UnityEngine.Debug.LogError("高级公式配置错误 - example: CS_BUILDING_LEVEL@建筑功能类型EBuildingFuncEnum@建筑id（如果传0为该类型下所有） Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
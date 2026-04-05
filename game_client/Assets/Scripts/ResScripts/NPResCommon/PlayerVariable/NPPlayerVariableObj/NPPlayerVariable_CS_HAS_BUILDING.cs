using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 是否拥有建筑 CS_HAS_BUILDING@建筑id 1=有 0=无
    /// </summary>
    public class NPPlayerVariable_CS_HAS_BUILDING : _ANPBasicPlayerVariableObj
	{
	    private long _m_lBuildingId;


	    protected NPPlayerVariable_CS_HAS_BUILDING()
	    {
	    }

        /// <summary>
        /// 获取条件类型
        /// </summary>
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_HAS_BUILDING; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
            return NPPlayer.instance.buildingComp.hasBuilding(_m_lBuildingId) ? 1 : 0;
#else
	        return 0L;
#endif
        }

	    public static NPPlayerVariable_CS_HAS_BUILDING readVariable(ALStringReader _reader)
	    {
	        NPPlayerVariable_CS_HAS_BUILDING variableObj = new NPPlayerVariable_CS_HAS_BUILDING();

	        //解析字符串
	        string buildingIdS = _reader.readItem('@');
	        //逐个判断
	        if (string.IsNullOrEmpty(buildingIdS))
	        {
	            UnityEngine.Debug.LogError("高级公式——是否拥有建筑 example: CS_HAS_BUILDING@建筑id 1=有 0=无 Error Str: " + _reader.srcString);
	            return null;
	        }

	        try
	        {
	            variableObj._m_lBuildingId = ALCommon.GetLong(buildingIdS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——是否拥有建筑 example: CS_HAS_BUILDING@建筑id 1=有 0=无 Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
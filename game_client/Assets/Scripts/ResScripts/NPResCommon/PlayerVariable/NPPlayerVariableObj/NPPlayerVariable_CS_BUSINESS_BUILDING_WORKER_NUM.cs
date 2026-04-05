using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 指定经营建筑员工数量 CS_BUSINESS_BUILDING_WORKER_NUM@建筑id）
    /// </summary>
    public class NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM : _ANPBasicPlayerVariableObj
	{
	    private long _m_lBuildingId;

	    protected NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM()
	    {
	    }

        /// <summary>
        /// 获取条件类型
        /// </summary>
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_BUSINESS_BUILDING_WORKER_NUM; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
            return NPPlayer.instance.buildingComp.getEmployeeNum(_m_lBuildingId);
#else
	        return 0L;
#endif
        }

	    public static NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM readVariable(ALStringReader _reader)
	    {
	        NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM variableObj = new NPPlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM();

	        //解析字符串
	        string buildingIdS = _reader.readItem('@');

	        try
            {
                if (string.IsNullOrEmpty(buildingIdS))
                    variableObj._m_lBuildingId = 0;
				else
	                variableObj._m_lBuildingId = ALCommon.GetLong(buildingIdS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——指定经营建筑员工数量 example: CS_BUSINESS_BUILDING_WORKER_NUM@建筑id） Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
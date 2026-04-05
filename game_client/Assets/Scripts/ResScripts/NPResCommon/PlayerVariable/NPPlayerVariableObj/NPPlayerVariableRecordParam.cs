using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using Common.PlayerEnum;
using NPEnum;

using GOE;


namespace GOE
{
	/// <summary>
	/// 高级公式变量-玩家记录 CS_RECORD_PARAM@ENPPlayerRecordParam
	/// </summary>
	public class NPPlayerVariableRecordParam : _ANPBasicPlayerVariableObj
	{
	    /** 属性类型枚举 */
	    private ENPPlayerRecordParam _m_eventRecordType;

	    private long _m_subId;

	    protected NPPlayerVariableRecordParam()
	    {
	    }

	      /******************
	     * 获取条件类型
	     */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_RECORD_PARAM; } }


	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        //获取对应的值
	        return NPPlayer.instance.recordComp.getValue(_m_eventRecordType);
#else
	        return 0L;
#endif
	    }


	public static NPPlayerVariableRecordParam readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string paramS = _reader.readItem('@');
	        string subIdS = _reader.readItem('@');

	        if (null == subIdS)
	            subIdS = "0";
	        //逐个判断
	        if (null == paramS)
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: CS_RECORD_PARAM@ENPPlayerRecordParam Error Str: " + _reader.srcString);
	            return null;
	        }

            NPPlayerVariableRecordParam variableObj = new NPPlayerVariableRecordParam();

	        try {
	            variableObj._m_eventRecordType = (ENPPlayerRecordParam)ALCommon.EnumParse(typeof(ENPPlayerRecordParam), paramS, true);
	            variableObj._m_subId = long.Parse(subIdS);

	            return variableObj;
	        }
	        catch (Exception) {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: CS_RECORD_PARAM@ENPPlayerRecordParam Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
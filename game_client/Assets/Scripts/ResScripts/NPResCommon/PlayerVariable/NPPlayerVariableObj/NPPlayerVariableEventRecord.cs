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
	/// 高级公式变量-玩家事件记录计数 CS_EVENT_RECORD@ENPPlayerEventRecordType@sub_id
	/// </summary>
	public class NPPlayerVariableEventRecord : _ANPBasicPlayerVariableObj
	{
	    /** 属性类型枚举 */
	    private EPlayerEventRecordType _m_eventRecordType;

	    private long _m_subId;

	    protected NPPlayerVariableEventRecord()
	    {
	    }

	      /******************
	     * 获取条件类型
	     */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_EVENT_RECORD; } }


	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        //获取对应的值
	        return NPPlayer.instance.eventRecordComp.getValue(_m_eventRecordType,_m_subId);
#else
	        return 0L;
#endif
	    }


	public static NPPlayerVariableEventRecord readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string paramS = _reader.readItem('@');
	        string subIdS = _reader.readItem('@');

	        if (null == subIdS)
	            subIdS = "0";
	        //逐个判断
	        if (null == paramS)
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type:subid Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariableEventRecord variableObj = new NPPlayerVariableEventRecord();

	        try {
	            variableObj._m_eventRecordType = (EPlayerEventRecordType)ALCommon.EnumParse(typeof(EPlayerEventRecordType), paramS, true);
	            variableObj._m_subId = long.Parse(subIdS);

	            return variableObj;
	        }
	        catch (Exception) {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type:subid Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
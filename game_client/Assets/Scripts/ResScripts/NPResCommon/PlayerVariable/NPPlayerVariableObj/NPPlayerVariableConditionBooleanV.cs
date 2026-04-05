using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using NPEnum;

using GOE;

namespace GOE
{
	/// <summary>
	/// 返回条件的布尔值 CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略
	/// </summary>
	public class NPPlayerVariableConditionBooleanV : _ANPBasicPlayerVariableObj
	{
	    private _NPPlayerConditionSerializeInfo _m_condition;
	    protected NPPlayerVariableConditionBooleanV()
	    {
	    }

	    /******************
	   * 获取条件类型
	   */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_CONDITION_BOOLEAN_V; } }
	    public _NPPlayerConditionSerializeInfo condition { get => _m_condition; }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        return _m_condition != null && _m_condition.IsEnable(_variableInfo) ? 1 : 0;
#else
	        return 0L;
#endif
	    }

	    public static NPPlayerVariableConditionBooleanV readVariable(ALStringReader _reader)
	    {
	        try
	        {
	            NPPlayerVariableConditionBooleanV variableObj = new NPPlayerVariableConditionBooleanV();
	            string conditionS = _reader.readItem('@');
	            variableObj._m_condition = _NPPlayerConditionSerializeInfo.ReadFromString(conditionS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——返回条件的布尔值 - CONDITION_BOOLEAN_V example: CS_CONDITION_BOOLEAN_V@（条件） Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
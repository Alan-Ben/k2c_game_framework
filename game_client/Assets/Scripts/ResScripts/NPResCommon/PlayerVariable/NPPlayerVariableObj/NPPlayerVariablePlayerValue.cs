using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	/// <summary>
	/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
	/// </summary>
	public class NPPlayerVariablePlayerValue : _ANPBasicPlayerVariableObj
	{
	    /** 属性类型枚举 */
	    private ENPPlayerValueType _m_eValueType;

	    public ENPPlayerValueType ValueType { get { return _m_eValueType; } }

	    protected NPPlayerVariablePlayerValue()
	    {
	    }

	      /******************
	     * 获取条件类型
	     */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_VALUE; } }


	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        //获取对应玩家的值
	        return NPPlayer.instance.getValue(_m_eValueType);
#else
	        return 0L;
#endif
	    }


	public static NPPlayerVariablePlayerValue readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string valuetTypeS = _reader.readItem('@');
	        //逐个判断
	        if (null == valuetTypeS)
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariablePlayerValue variableObj = new NPPlayerVariablePlayerValue();

	        try {
	            variableObj._m_eValueType = (ENPPlayerValueType)ALCommon.EnumParse(typeof(ENPPlayerValueType), valuetTypeS, true);

	            return variableObj;
	        }
	        catch (Exception) {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
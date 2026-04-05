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
	public class NPPlayerVariablePlayerParam : _ANPBasicPlayerVariableObj
	{
	    /** 属性类型枚举 */
	    private ENPPlayerParam _m_eParamType;

	    public ENPPlayerParam paramType { get { return _m_eParamType; } }

	    protected NPPlayerVariablePlayerParam()
	    {
	    }

	      /******************
	     * 获取条件类型
	     */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_PARAM; } }


	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        //获取对应玩家的值
	        return NPPlayer.instance.playerInfo?.getValue(_m_eParamType) ?? 0;
#else
	        return 0L;
#endif
	    }


	public static NPPlayerVariablePlayerParam readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string paramS = _reader.readItem('@');
	        //逐个判断
	        if (null == paramS)
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariablePlayerParam variableObj = new NPPlayerVariablePlayerParam();

	        try {
	            variableObj._m_eParamType = (ENPPlayerParam)ALCommon.EnumParse(typeof(ENPPlayerParam), paramS, true);

	            return variableObj;
	        }
	        catch (Exception) {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Value example: enum:value_type Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
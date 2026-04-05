using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	/// <summary>
	/// 高级公式变量-TEAM_VALUE - 队伍值    格式如下：枚举@队伍属性类型
	/// </summary>
	public class NPPlayerVariablePlayerPro : _ANPBasicPlayerVariableObj
	{
	    /** 属性类型枚举 */
	    private ENPPlayerPropertyType _m_ePropertyType;
    
	    public ENPPlayerPropertyType PropertyType { get { return _m_ePropertyType; } }

	    protected NPPlayerVariablePlayerPro() {
	        _m_ePropertyType = ENPPlayerPropertyType.NONE;
	    }

	    /******************
	     * 获取条件类型
	     */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_PROPERTY; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        return NPPlayer.instance.playerPropertyMgr.getValue(_m_ePropertyType);
#else
	        return 0L;
#endif
	    }

	    public static NPPlayerVariablePlayerPro readVariable (ALStringReader _reader)
	    {
	        //解析字符串
	        string propertyS = _reader.readItem('@');
	        //逐个判断
	        if (null == propertyS)
	        {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Pro example: enum:property Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariablePlayerPro variableObj = new NPPlayerVariablePlayerPro();
	        try {
	            variableObj._m_ePropertyType = (ENPPlayerPropertyType)ALCommon.EnumParse(typeof(ENPPlayerPropertyType), propertyS, true);

	            return variableObj;
	        }
	        catch (Exception) {
	            UnityEngine.Debug.LogError("高级公式配置错误 - Pro example: enum:property Error Str: " + _reader.srcString);
	            return null;
	        }
	    }

	}
}
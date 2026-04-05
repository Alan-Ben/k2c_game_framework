using UnityEngine;
using System.Collections;
using System;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	public class NPPlayerVariableBufL : _ANPBasicPlayerVariableObj
	{
	    private long _m_lBufId;//buffId

	    public long bufId { get { return _m_lBufId; } }

	    protected NPPlayerVariableBufL()
	    {
	    }

	    /******************
	   * 获取条件类型
	   */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_BUF_L; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        NPPlayerBuffInfo info = NPPlayer.instance.playerBuffComp.lookup(_m_lBufId);
	        if (null == info)
	            return 0L;

	        return info.layer;
#else
	        return 0L;
#endif
	    }


	    public static NPPlayerVariableBufL readVariable(ALStringReader _reader)
	    {
	        string bufIdS = _reader.readItem('@');
	        if (null == bufIdS)
	        {
	            UnityEngine.Debug.LogError("高级公式——卡牌状态——配置错误 - BUF_L   example: enum:bufId Error Str: " + _reader.srcString);
	            return null;
	        }

	        NPPlayerVariableBufL variableObj = new NPPlayerVariableBufL();

	        try
	        {
	            variableObj._m_lBufId = long.Parse(bufIdS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——卡牌状态——配置错误 - BUF_L   example: enum:bufId Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
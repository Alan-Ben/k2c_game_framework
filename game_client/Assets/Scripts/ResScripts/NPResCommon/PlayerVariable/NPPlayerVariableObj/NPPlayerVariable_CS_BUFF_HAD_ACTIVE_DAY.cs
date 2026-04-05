using System;
using NPEnum;
using ALPackage;


namespace GOE
{
	public class NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY : _ANPBasicPlayerVariableObj
	{
	    private long _m_lBufId;

	    public long buffId { get { return _m_lBufId; } }

	    protected NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY()
	    {
	    }

	    /******************
	   * 获取条件类型
	   */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_BUFF_HAD_ACTIVE_DAY; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        NPPlayerBuffInfo info = NPPlayer.instance.playerBuffComp.lookup(_m_lBufId);
	        if (null == info)
	            return 0L;

	        return info.hadActiveDay;
#else
	        return 0L;
#endif
	    }


	    public static NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY readVariable(ALStringReader _reader)
	    {
	        string bufIdS = _reader.readItem('@');
	        if (null == bufIdS)
	        {
	            UnityEngine.Debug.LogError("高级公式——卡牌状态——配置错误 - BUF_T   example: enum:bufId Error Str: " + _reader.srcString);
	            return null;
	        }

            NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY variableObj = new NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY();

	        try
	        {
	            variableObj._m_lBufId = long.Parse(bufIdS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——卡牌状态——配置错误 - BUF_T   example: enum:bufId Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}
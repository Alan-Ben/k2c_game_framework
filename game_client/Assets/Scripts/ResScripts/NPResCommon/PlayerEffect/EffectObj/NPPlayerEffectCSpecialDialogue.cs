using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectCSpecialDialogue : _ANPPlayerEffectInfo
	{
	    private ENPClientSpecialDoalogueDealType _m_eSpecialDealType;       //特殊处理效果枚举

	    public NPPlayerEffectCSpecialDialogue()
	    {
	        _m_eSpecialDealType = ENPClientSpecialDoalogueDealType.NONE;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SPECIAL_DIALOGUE; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
		    NPGGUIWndDialogue.instance.callDealType(_m_eSpecialDealType);
#endif
	    }

	    public static NPPlayerEffectCSpecialDialogue readEffect(string _str)
	    {
		    NPPlayerEffectCSpecialDialogue effectObj = new NPPlayerEffectCSpecialDialogue();

	        try
	        {
	            effectObj._m_eSpecialDealType = (ENPClientSpecialDoalogueDealType)ALCommon.EnumParse(typeof(ENPClientSpecialDoalogueDealType), _str, true);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——特殊处理效果——配置错误 - C_SPECIAL   example: enum:specialDealType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
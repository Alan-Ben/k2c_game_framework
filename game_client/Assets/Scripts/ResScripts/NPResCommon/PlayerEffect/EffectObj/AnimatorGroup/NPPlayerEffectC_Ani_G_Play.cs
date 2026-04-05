using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectC_Ani_G_Play : _ANPPlayerEffectInfo
	{
	    private string _m_sAnimtorName;     //处理的动作对象标记
	    private string _m_sPlayAni;         //播放的动作名称

	    public NPPlayerEffectC_Ani_G_Play()
	    {
	        _m_sAnimtorName = string.Empty;
	        _m_sPlayAni = string.Empty;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_ANI_G_PLAY; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        AnimatorControllerGroupMgr.instance.playAnimator(_m_sAnimtorName, _m_sPlayAni);
#endif
	    }

	    public static NPPlayerEffectC_Ani_G_Play readEffect(string _str)
	    {
	        string[] strs = _str.Split(':');
	        if(strs.Length < 2)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_ANI_G_PLAY   example: enum:animatorObjName:animatorStateName Error Str: " + _str);
	            return null;
	        }

	        NPPlayerEffectC_Ani_G_Play effectObj = new NPPlayerEffectC_Ani_G_Play();

	        try
	        {
	            effectObj._m_sAnimtorName = strs[0];
	            effectObj._m_sPlayAni = strs[1];

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_ANI_G_PLAY   example: enum:animatorObjName:animatorStateName Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
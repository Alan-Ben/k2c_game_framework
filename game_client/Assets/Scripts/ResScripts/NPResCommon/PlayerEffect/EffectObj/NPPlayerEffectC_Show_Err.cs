using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectC_Show_Err : _ANPPlayerEffectInfo
	{
	    private string _m_sKey;
	    private List<string> _m_lArgs;

	    public NPPlayerEffectC_Show_Err()
	    {
	        _m_sKey = string.Empty;
	        _m_lArgs = new List<string>();
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOW_ERR; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //弹出翻译结果
	        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_sKey, _m_lArgs));
         
#endif
	    }

	    public static NPPlayerEffectC_Show_Err readEffect(string _str)
	    {
	        string[] strs = _str.Split(':');
	        if(strs.Length < 1)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_SHOW_ERR   example: enum:key:args Error Str: " + _str);
	            return null;
	        }

	        NPPlayerEffectC_Show_Err effectObj = new NPPlayerEffectC_Show_Err();

	        try
	        {
	            effectObj._m_sKey = strs[0];
	            for(int i = 1; i < strs.Length; i++)
	            {
	                effectObj._m_lArgs.Add(strs[i]);
	            }

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_SHOW_ERR   example: enum:key:args Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
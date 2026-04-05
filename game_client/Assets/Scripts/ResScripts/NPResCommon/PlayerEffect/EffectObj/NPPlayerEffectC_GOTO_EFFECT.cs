using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;
using NPCommon;


namespace GOE
{
	public class NPPlayerEffectC_GOTO_EFFECT : _ANPPlayerEffectInfo
	{

		private List<NPSOEffectGotoRefObj> _m_gotoEffectList;

	    public NPPlayerEffectC_GOTO_EFFECT()
	    {
		    _m_gotoEffectList = new List<NPSOEffectGotoRefObj>();
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_GOTO_EFFECT; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
		    NPSOEffectGotoRefObj refObj = null;
		    for (int i = 0; i < _m_gotoEffectList.Count; i++)
		    {
			    refObj = _m_gotoEffectList[i];
			    if (null == refObj)
				    continue;
			    if (refObj.deal_cond != null && !refObj.deal_cond.IsEnable(null))
				    continue; 
			    //第一个通过cond判断的执行效果，后续就跳过
			    refObj.effect.dealEffect();
			    break;
		    }
#endif
	    }

	    public static NPPlayerEffectC_GOTO_EFFECT readEffect(string _str)
	    {
		    
		    String[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if(strs.Length == 0)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_GOTO_EFFECT   至少配置一个effect_goto的id" + _str);
	            return null;
	        }

	        NPPlayerEffectC_GOTO_EFFECT effectObj = new NPPlayerEffectC_GOTO_EFFECT();

	        try
	        {
#if NP_GAME
		        NPSOEffectGotoRefObj refObj = null;
		        long parseId = 0;
		        for (int i = 0; i < strs.Length; i++)
		        {
			        if (long.TryParse(strs[i], out parseId))
			        {
				        refObj = GRefdataCoreMgr.instance.effectGoToRefCore.getRef(parseId);
				        if (null != refObj)
				        {
					        effectObj._m_gotoEffectList.Add(refObj);
				        }
			        }   
		        }
#endif
		        return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_DEAL_ID   example: enum:type:id Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
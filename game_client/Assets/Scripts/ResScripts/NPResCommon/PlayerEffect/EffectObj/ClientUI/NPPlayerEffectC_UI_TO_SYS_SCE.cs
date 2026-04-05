using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectC_UI_TO_SYS_SCE : _ANPPlayerEffectInfo
	{
	    private ESysSceneType _m_eSysSceneType;       //系统UI视图枚举
	    private List<string> _m_sArgs;//参数字符串,有些跳转需要具体的参数

	    public NPPlayerEffectC_UI_TO_SYS_SCE()
	    {
	        _m_eSysSceneType = ESysSceneType.NONE;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_UI_TO_SYS_SCE; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        GCommon.enterUIMainNodeShow(_m_eSysSceneType, _m_sArgs);
#endif
	    }

	    public static NPPlayerEffectC_UI_TO_SYS_SCE readEffect(string _str)
	    {
	        String[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if (strs.Length < 1)
	        {
	            UnityEngine.Debug.LogError("Can not read str for NPPlayerEffectC_UI_TO_SYS_SCE[" + _str + "]");
	            return null;
	        }

	        NPPlayerEffectC_UI_TO_SYS_SCE effectObj = new NPPlayerEffectC_UI_TO_SYS_SCE();
	        effectObj._m_eSysSceneType = ESysSceneType.NONE;
	        effectObj._m_sArgs = new List<string>();

	        try
	        {
	            effectObj._m_eSysSceneType = (ESysSceneType)ALCommon.EnumParse(typeof(ESysSceneType), strs[0], true);
	            //参数列表
	            for (int i = 1; i < strs.Length; ++i)
	            {
	                effectObj._m_sArgs.Add(strs[i]);
	            }
	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——跳转系统——配置错误 - C_UI_TO_SYS_SCE   example: enum:systemType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectCUseBagItem : _ANPPlayerEffectInfo
	{
	    private long _m_lItemId;

	    public NPPlayerEffectCUseBagItem()
	    {
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_USE_BAG_ITEM; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        GCommon.showBagUseItemsMono(ENPItemType.BAG_ITEM, _m_lItemId);
#endif
	    }

	    public static NPPlayerEffectCUseBagItem readEffect(string _str)
	    {
	        NPPlayerEffectCUseBagItem effectObj = new NPPlayerEffectCUseBagItem();

	        //拆分字符串后进行读取
	        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if(strs.Length < 1)
	        {
	            UnityEngine.Debug.LogWarning("NPPlayerEffectCUseBagItem 配置错误! C_USE_BAG_ITEM  example: enum:itemId  error str:  " + _str);
	            return null;
	        }

	        try
	        {
	            effectObj._m_lItemId = long.Parse(strs[0]);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogWarning("NPPlayerEffectCUseBagItem 配置错误! C_USE_BAG_ITEM  example: enum:itemId  error str:  " + _str);
	            return null;
	        }
	    }
	}
}
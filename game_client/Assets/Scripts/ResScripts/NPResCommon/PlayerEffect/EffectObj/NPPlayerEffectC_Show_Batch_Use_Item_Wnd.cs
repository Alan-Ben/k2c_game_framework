using System;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 打开批量使用道具窗口 C_SHOW_BATCH_USE_ITEM_WND:itemId
	/// </summary>
	public class NPPlayerEffectC_Show_Batch_Use_Item_Wnd : _ANPPlayerEffectInfo
	{
	    private long _m_lItemId;

	    public NPPlayerEffectC_Show_Batch_Use_Item_Wnd()
	    {
	    }

		/// <summary>
		/// 效果类型
		/// </summary>
		public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOW_BATCH_USE_ITEM_WND; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        GCommon.showBagBatchUse(ENPItemType.BAG_ITEM, _m_lItemId);
#endif
	    }

	    public static NPPlayerEffectC_Show_Batch_Use_Item_Wnd readEffect(string _str)
	    {
	        NPPlayerEffectC_Show_Batch_Use_Item_Wnd effectObj = new NPPlayerEffectC_Show_Batch_Use_Item_Wnd();

	        //拆分字符串后进行读取
	        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if(strs.Length < 1)
	        {
	            UnityEngine.Debug.LogWarning("NPPlayerEffectC_Show_Batch_Use_Item_Wnd 配置错误! C_SHOW_BATCH_USE_ITEM_WND  example: C_SHOW_BATCH_USE_ITEM_WND:itemId  error str:  " + _str);
	            return null;
	        }

	        try
	        {
	            effectObj._m_lItemId = long.Parse(strs[0]);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogWarning("NPPlayerEffectC_Show_Batch_Use_Item_Wnd 配置错误! C_SHOW_BATCH_USE_ITEM_WND  example: C_SHOW_BATCH_USE_ITEM_WND:itemId  error str:  " + _str);
	            return null;
	        }
	    }
	}
}
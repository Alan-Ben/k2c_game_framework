using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerEffectC_UI_SHOW_CUST_WND : _ANPPlayerEffectInfo
	{
	    private NPCommonAssetPathInfo _m_piAssetPathInfo;
	    private EALUIWndLayer _m_eWndLayer;


	    public NPPlayerEffectC_UI_SHOW_CUST_WND()
	    {
	        _m_piAssetPathInfo = new NPCommonAssetPathInfo();
	        _m_eWndLayer = EALUIWndLayer.NORMAL;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_UI_SHOW_CUST_WND; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        QueueMgr.instance.AddNode(new NPGAddNodeCustomWnd(_m_piAssetPathInfo.asset_path, _m_piAssetPathInfo.obj_name, _m_eWndLayer));
#endif
	    }

	    public static NPPlayerEffectC_UI_SHOW_CUST_WND readEffect(string _str)
	    {
	        NPPlayerEffectC_UI_SHOW_CUST_WND effectObj = new NPPlayerEffectC_UI_SHOW_CUST_WND();

	        try
	        {
	            // 拆分字符串：assetPath:objName 或 assetPath:objName:EALUIWndLayer
	            string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	            if (strs.Length < 2)
	            {
	                UnityEngine.Debug.LogError("配置错误 - C_UI_SHOW_CUST_WND 参数不足 example: assetPath:objName 或 assetPath:objName:EALUIWndLayer Error Str: " + _str);
	                return null;
	            }

	            // 解析资源路径信息（前两个参数）
	            effectObj._m_piAssetPathInfo = new NPCommonAssetPathInfo(strs[0], strs[1]);

	            // 解析窗口层级（可选的第三个参数）
	            if (strs.Length >= 3 && !string.IsNullOrEmpty(strs[2]))
	            {
	                effectObj._m_eWndLayer = (EALUIWndLayer)ALCommon.EnumParse(typeof(EALUIWndLayer), strs[2], true);
	            }

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("配置错误 - C_UI_SHOW_CUST_WND   example: assetPath:objName 或 assetPath:objName:EALUIWndLayer Error Str: " + _str);
	            return null;
	        }
	    }
	}
}
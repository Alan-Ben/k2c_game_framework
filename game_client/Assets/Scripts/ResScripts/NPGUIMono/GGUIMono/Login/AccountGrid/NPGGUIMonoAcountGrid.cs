using UnityEngine;
using System.Collections;
using  ALPackage;
using GOE;

/// <summary>
/// 账号选择下拉窗口的的窗口脚本
/// </summary>
public class NPGGUIMonoAcountGrid :  _TALUGUIMonoGridWnd<NPGGUIMonoAcountGridItem>{
    

	[ALHeader("关闭遮罩/按钮")]
	public GameObject btnMask;

	/************
     * 资源加载路径
    **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2307); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2307); } }
}

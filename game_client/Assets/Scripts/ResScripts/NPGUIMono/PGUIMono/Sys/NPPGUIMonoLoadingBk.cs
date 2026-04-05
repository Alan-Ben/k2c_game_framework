using UnityEngine.UI;
using ALPackage;
using UnityEngine;

/**********************
 * 游戏加载背景窗口对象
 **/
public class NPPGUIMonoLoadingBk : _AALBasicUIWndMono
{
    [ALHeader("加载提示")]
    public Text txtLoadingTxt;//加载提示
    [ALHeader("加载进度条")]
    public RectTransform sldProgress;//加载进度条
    [ALHeader("加载进度提示")]
    public Text txtProgress;//加载进度提示
    [ALHeader("加载进度条")]
    public Slider sldProgress2;//加载进度条
    
    [ALHeader("加载进度")]
    public Text txtLoadingProgress;
    [ALHeader("加载进度key(两个参数, 第一个参数是小数点前部分, 第二个参数是小数点后第一位)")]
    public string txtLoadingProgressKey;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "win_loading"; } }
}

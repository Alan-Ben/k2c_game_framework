using System;
using UnityEngine.UI;
using ALPackage;
using UnityEngine;

public class NPPGUIMonoTransparentBk : _AALBasicUIWndMono {
    //背景点击对象
    public GameObject bkClickGo;
    public RawImage imgRT;
    [Range(0,1)]
    [ALHeader("值越小越黑")]
    public float alpha = 0.753f;

    //获取RT的默认颜色
    public Color getRTDefaultColor()
    {
        Color color = Color.white * alpha;
        color.a = 1;
        return color;
    }
    
    /************
  * 资源加载路径
  **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "win_transparent_bk"; } }
}

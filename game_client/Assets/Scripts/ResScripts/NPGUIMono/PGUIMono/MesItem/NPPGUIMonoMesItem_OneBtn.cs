using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;

/**************************
 * 注册界面
 **/
public class NPPGUIMonoMesItem_OneBtn : _AALBasicUIWndMono
{
    /** 标题显示部分 */
    public Text textTitle;
    /** 文字显示部分 */
    public Text textLabel;

    /** 单个按钮对象 */
    public GameObject btn;
    public Text btnTxt;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "mes_one_btn"; } }
}

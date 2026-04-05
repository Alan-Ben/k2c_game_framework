using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;

/**************************
 * 注册界面
 **/
public class NPPGUIMonoMesItem_TwoBtn : _AALBasicUIWndMono
{
    /** 标题显示部分 */
    public Text textTitle;
    /** 文字显示部分 */
    public Text textLabel;

    /** 左侧按钮对象 */
    public GameObject leftBtn;
    public Text leftBtnTxt;

    /** 右侧按钮对象 */
    public GameObject rightBtn;
    public Text rightBtnTxt;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "mes_two_btn"; } }
}

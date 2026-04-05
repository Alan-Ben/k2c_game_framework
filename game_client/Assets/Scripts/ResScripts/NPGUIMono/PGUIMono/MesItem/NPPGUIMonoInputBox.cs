using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPPGUIMonoInputBox : _AALBasicUIWndMono
{
    /** 文字显示部分 */
    public UnityEngine.UI.Text textLabel;

    /** 单个按钮对象 */
    public GameObject btn;
    public Text btnTxt;
    public InputField inputField;
    public Text inputTxt;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "input_box"; } }
}

using System;
using System.Reflection;
using System.Text.RegularExpressions;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//继承PropertyAttribute类，
//便能用一个自定义的PropertyDrawer的类去控制继承PropertyAttribute类的变量在Inspector面板中的显示

//PropertyDrawer用于属性的自定义显示，DecoratorDrawer用于从其对应的PropertyAttribute中获取的数据来绘制装饰元素                                                   
//          具体PropertyDrawer参考：https://docs.unity3d.com/cn/2020.2/ScriptReference/PropertyDrawer.html
//          具体DecoratorDrawer参考：https://docs.unity3d.com/cn/2020.2/ScriptReference/DecoratorDrawer.html

//AttributeUsage,属性描述使用范围限定
//AttributeTargets，可用ALHeader标记描述的限定范围，若未使用AttributeUsage，则默认AttributeTargets.All
//                  可以使用或（|）操作符组合属性目标枚举中列出的项
//AllowMultiple=true允许属性多次附加到指定的类型，false则只能附加一次
//          例：  [ALHeader("描述11111")]
//                [ALHeader("描述22222")]
//                public int _m_field;
//          此时，  _m_field描述有2个，描述11111  和  描述22222，后加的描述在属性面板中显示在上面一行
//Inherited，继承标志，用于指定属性是否能被继承，默认false，若继承标志被设置为true,
//          它的含义将依赖于AllowMultiple标志的值。若继承标志被设置为true，
//          并且AllowMultiple标志是false,则改属性将忽略继承属性。
//          若继承标志和AllowMultiple标志都被设置为true，
//          则改属性的成员将与派生属性的成员合并



[AttributeUsage(AttributeTargets.Field,AllowMultiple =true,Inherited =true)]
public class ALHeader : PropertyAttribute
{
    /// <summary>
    /// 描述文本
    /// </summary>
    private string _m_sName = "";
    /// <summary>
    /// 文本颜色
    /// </summary>
    private string _m_sHtmlColor = "#ffffff";
    /// <summary>
    /// 属性描述
    /// </summary>
    /// <param name="_name">描述文字</param>
    public ALHeader(string _name)
    {
        this._m_sName = _name;
    }
    /// <summary>
    /// 属性描述
    /// </summary>
    /// <param name="_name">描述文字</param>
    /// <param name="_htmlColor">格式：#00fff0</param>
    public ALHeader(string _name, string _htmlColor)
    {
        this._m_sName = _name;
        this._m_sHtmlColor = _htmlColor;
    }
    //public NPHeader(string _name, string _htmlColor,bool )
    //{
    //    this._m_sName = _name;
    //    this._m_sHtmlColor = _htmlColor;
    //}

    public string SName { get => _m_sName;}
    public string SHtmlColor { get => _m_sHtmlColor;}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ALHeader))]
#endif
public class ALHeaderDrawer
#if UNITY_EDITOR
    : DecoratorDrawer
#endif
{
#if UNITY_EDITOR
    private GUIStyle _m_style = new GUIStyle();
    public override void OnGUI(Rect position)
    {
        ALHeader _npHeader = attribute as ALHeader;
           if(_npHeader == null)
        {
            return;
        }
        _m_style.normal.textColor = htmlToColor(_npHeader.SHtmlColor);
        position = EditorGUI.IndentedRect(position);
        GUI.Label(position, _npHeader.SName, _m_style);
    }
    /// <summary> Html颜色转换为Color </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color htmlToColor(string hex)
    {
        // 编辑器默认颜色
        if (string.IsNullOrEmpty(hex)) return new Color(1f, 1f, 1f);
        
        // 转换颜色
        hex = hex.ToLower();
        if (hex.IndexOf("#") == 0 && hex.Length == 7)
        {
            int r = Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        return new Color(1f, 1f, 1f);
    }
#endif
}
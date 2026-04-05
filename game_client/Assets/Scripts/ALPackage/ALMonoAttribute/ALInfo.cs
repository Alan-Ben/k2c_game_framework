
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 用于在inspector上显示信息注释的attribute
/// </summary>
/// <remarks>
/// 使用方法
///     [ALInfo("aaa")]
///     public int aaa;
/// 将会在inspector上显示一个MsgBox
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple =true)]
public class ALInfo : PropertyAttribute
{
    // 注释内容
    private string _m_sInfoContent;
#if UNITY_EDITOR
    // 注释类型
    private MessageType _m_eInfoType;
#endif

    public ALInfo(string _content)
    {
        _m_sInfoContent = _content;
#if UNITY_EDITOR
        _m_eInfoType = MessageType.Info;
#endif
    }

#if UNITY_EDITOR
    public ALInfo(string _content, MessageType _type)
    {
        _m_sInfoContent = _content;
        _m_eInfoType = _type;
    }
#endif

    /// <summary>
    /// 注释内容 
    /// </summary>
    public string content { get { return _m_sInfoContent; } }
#if UNITY_EDITOR
    /// <summary>
    /// 注释类型
    /// </summary>
    public MessageType type { get { return _m_eInfoType; } }
#endif
}

/// <summary>
/// ALInfo属性的绘画器
/// </summary>
/// <remarks>
/// 为ALInfo在Inspector上的绘制逻辑
/// </remarks>
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ALInfo))]
#endif
public class ALInfoDrawer
#if UNITY_EDITOR
    : DecoratorDrawer
#endif
{
#if UNITY_EDITOR
    // 写死常量
    private const int _m_iIconWidth = 55;
    private const int _m_iFontSize = 12;
    private const int _m_iMinHeight = 40;
    
    // 记录下box的高度
    private float _m_fBoxHeight;

    public override void OnGUI(Rect position)
    {
        ALInfo target = (ALInfo)attribute;

        // 设置helpBox的文字大小
        int originSize = EditorStyles.helpBox.fontSize;
        EditorStyles.helpBox.fontSize = _m_iFontSize;
        
        // 计算helpBox所在的位置
        Rect helpPosition = position;
        _m_fBoxHeight = _getHeight(target.content);
        helpPosition.height = _m_fBoxHeight;

        // 绘制helpBox
        EditorGUI.HelpBox(helpPosition, target.content, target.type);
        
        // 还原系统之前的helpBox的文字大小
        EditorStyles.helpBox.fontSize = originSize;
    }

    public override float GetHeight()
    {
        // 如果没有计算过，就在这里先计算一次
        if (_m_fBoxHeight == 0)
        {
            ALInfo target = (ALInfo)attribute;
            _m_fBoxHeight = _getHeight(target.content);
        }

        // 返回box的高度，+2让其有一些间隙
        return _m_fBoxHeight + 2;
    }

    // 计算HelpBox的高度
    private float _getHeight(string message)
    {
        // 获取当前helpBox的样式
        GUIStyle style = new GUIStyle(EditorStyles.helpBox);
        style.richText = true;
        style.fontSize = _m_iFontSize;

        // 计算高度，要减掉提示Icon所占用的宽度，宽度值是直接试出来的
        float newHeight = style.CalcHeight(new GUIContent(message), EditorGUIUtility.currentViewWidth - _m_iIconWidth);
        // minHeight也是icon的高度，高度也是直接试出来的
        return Mathf.Max(_m_iMinHeight, newHeight);
    }
#endif
}
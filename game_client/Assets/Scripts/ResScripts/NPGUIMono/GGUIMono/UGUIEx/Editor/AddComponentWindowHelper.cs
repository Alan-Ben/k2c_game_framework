using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// 这个脚本会在Unity启动时，修改编辑器UI中一些元素的布局
[InitializeOnLoad]
public static class AddComponentWindowHelper
{
    static AddComponentWindowHelper()
    {
        EditorApplication.update += ModifyAddComponentButtonStyle;
    }

    private static int s_BtnStyleInitCount; 

    private static GUIStyle s_AddComponentButtonStyle;

    
    static void ModifyAddComponentButtonStyle()
    {
        // 延迟获取样式
        s_BtnStyleInitCount++;
        
        if (s_BtnStyleInitCount < 3)
            return ;

        var style = GetAddComponentButtonStyle();
        if (style != null)
        {
            style.fixedWidth = 500;
        }
        EditorApplication.update -= ModifyAddComponentButtonStyle;
    }
    
    /// <summary>
    /// 获取 PropertyEditor.Styles.addComponentButtonStyle
    /// </summary>
    public static GUIStyle GetAddComponentButtonStyle()
    {
        if (s_AddComponentButtonStyle != null)
            return s_AddComponentButtonStyle;
        
        try
        {
            // 1. 获取 PropertyEditor 类型
            Assembly editorAssembly = typeof(Editor).Assembly;
            Type propertyEditorType = editorAssembly.GetType("UnityEditor.PropertyEditor");
            
            if (propertyEditorType == null)
            {
                Debug.LogError("无法找到 PropertyEditor 类型");
                return null;
            }
            
            // 2. 获取嵌套的 Styles 类型
            Type stylesType = propertyEditorType.GetNestedType("Styles", BindingFlags.NonPublic | BindingFlags.Public);
            
            if (stylesType == null)
            {
                Debug.LogError("无法找到 PropertyEditor.Styles 类型");
                return null;
            }
            
            // 3. 获取静态字段 addComponentButtonStyle
            FieldInfo styleField = stylesType.GetField("addComponentButtonStyle", 
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            
            if (styleField == null)
            {
                Debug.LogError("无法找到 addComponentButtonStyle 字段");
                return null;
            }
            
            // 4. 获取字段值
            s_AddComponentButtonStyle = styleField.GetValue(null) as GUIStyle;
            
            return s_AddComponentButtonStyle;
        }
        catch (TypeInitializationException ex)
        {
            // Styles 静态构造函数抛出异常时的处理
            Debug.LogWarning($"[AddComponentWindowHelper] PropertyEditor.Styles 初始化失败: {ex.InnerException?.Message ?? ex.Message}");
            s_AddComponentButtonStyle = null;

            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"反射获取 addComponentButtonStyle 失败: {ex.Message}");
            s_AddComponentButtonStyle = null;
            return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

[CustomEditor(typeof(TextEmoji))]
public class TextEmojiEditor : UnityEditor.UI.TextEditor
{
    private SerializedProperty m_emojiAtlas;
    protected override void OnEnable()
    {
        base.OnEnable();
        m_emojiAtlas = serializedObject.FindProperty("m_emojiAtlas");
        _initFontType();
    }

    public override void OnInspectorGUI()
    {
        _DrawFontTypeAndUseLanguage();
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(m_emojiAtlas, new GUIContent("EmojiAtlas"));
        if (GUI.changed)
            EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
        _CheckFontTypeFontChange();
    }
    
    
    #region FontType

    protected SerializedProperty m_FontTypeProperty;
    protected Font _m_initFont;//初始字体
    
    private void _initFontType()
    {
        m_FontTypeProperty = serializedObject.FindProperty("m_fontType");

        var textEx = (Text)target;
        _m_initFont = textEx == null ? null : textEx.font;

        if (!Application.isPlaying)
        {
            _chgFont(target);//在游戏没有运行时enable时, 刷新一次使用的字体
        }
    }

    private void _DrawFontTypeAndUseLanguage()
    {
        if (!Application.isPlaying)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_FontTypeProperty);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                _chgFont(target);
            }
                
            ENPLanguage textUseLanguage = (ENPLanguage)EditorPrefs.GetInt("textUseLanguage", 0);
            EditorGUI.BeginChangeCheck();
            textUseLanguage = (ENPLanguage)EditorGUILayout.EnumPopup("使用语言", textUseLanguage);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("textUseLanguage", (int)textUseLanguage);
                _chgFont(target);
            }
        }
        else
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(m_FontTypeProperty);
            EditorGUI.EndDisabledGroup();
        }
    }
    /// <summary>
    ///  检查字体是否被修改
    /// </summary>
    private void _CheckFontTypeFontChange()
    {
        if (!Application.isPlaying)
        {
            var text = target as Text;
            if (text != null && text.font != _m_initFont)
            {
                Debug.LogError_EditorOnly("TextEmoji 不允许手动修改Font, 请通过变换FontType或使用语言修改需要使用的Font", text);
                text.font = _m_initFont;
                EditorUtility.SetDirty(target);
            }
        }
    }

    private void _chgFont(UnityEngine.Object target)
    {
        if(Application.isPlaying)//游戏运行时不刷新字体
            return;
        
        Text text = target as Text;
        if(text == null || m_FontTypeProperty == null)
            return;
        EFontType fontType = (EFontType) m_FontTypeProperty.enumValueIndex;
        Font font = FontTypeUtil.getLanguageFont(target, fontType);
        _m_initFont = font;
        text.font = font;
        EditorUtility.SetDirty(target);
        Resources.UnloadUnusedAssets();
    }
    #endregion
}
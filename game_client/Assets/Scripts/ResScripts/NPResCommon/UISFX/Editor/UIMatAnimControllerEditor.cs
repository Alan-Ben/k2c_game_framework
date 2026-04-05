using System;
using MG;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIMatAnimController))]
public class UIMatAnimControllerEditor : Editor
{
    private GUIStyle waringStyle;
    private void OnEnable()
    {
        if(Application.isPlaying)
            return;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UIMatAnimController mono = target as UIMatAnimController;;
        if(mono == null)
        {
            return;
        }

        if (waringStyle == null)
        {
            waringStyle = new GUIStyle(GUI.skin.label) {normal = {textColor = Color.red}, alignment = TextAnchor.MiddleCenter};
        }
        if(!Application.isPlaying && !mono._m_openDebugInEditor)
        {
            return;
        }
        GUILayout.Label(mono._m_needAnim ? "" : "未指定正确的Property，请检查", waringStyle, GUILayout.ExpandWidth(true));
    }
}
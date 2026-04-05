
using System.Collections.Generic;
using System.Reflection;
using GOE;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GGUICustomMonoCloseNodeBtn))]
public class GGUICustomMonoCloseNodeBtnInspector : Editor
{
    public class NodeTagInfo
    {
        [NotNull]public string fieldName;
        [NotNull]public string value;
    }
    
    private string _m_filter = "";
    [NotNull]private List<NodeTagInfo> _m_nodeTags = new List<NodeTagInfo>();
    [NotNull]private List<NodeTagInfo> _m_filteredNodeTags = new List<NodeTagInfo>();
    
    private int _m_curSelect = 0;
    private bool _m_toggleShowSelect = true;
    Vector2 _m_scrollPos = Vector2.zero;
    
    public void OnEnable()
    {
#if NP_GAME
        BindingFlags flags =  BindingFlags.Public | BindingFlags.Static;
        FieldInfo[] fields = typeof(UINodeTagConst).GetFields(flags);
        _m_nodeTags.Clear();
        foreach (var field in fields)
        {
            if (field == null)
                continue;
            string fieldName = field.Name;
            string fieldValue = field.GetValue(null).ToString();
            var nodeTagInfo = new NodeTagInfo
            {
                fieldName = fieldName,
                value = fieldValue.ToString()
            };
            _m_nodeTags.Add(nodeTagInfo);
        }
        doFilter();
        GGUICustomMonoCloseNodeBtn mono = target as GGUICustomMonoCloseNodeBtn;
        if (mono == null)
            return;
        for (var i = 0; i < _m_filteredNodeTags.Count; i++)
        {
            NodeTagInfo nodeTag = _m_filteredNodeTags[i];
            if (nodeTag != null && mono.nodeTag == nodeTag.value)
            {
                _m_scrollPos = new Vector2(0, i * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) ;
                break;
            }
        }
#endif
    }
    
    public override void OnInspectorGUI()
    {
        GGUICustomMonoCloseNodeBtn mono = target as GGUICustomMonoCloseNodeBtn;
        if (mono == null)
            return;
        NodeTagInfo curNodeTage = getNodeTag(mono.nodeTag);
        EditorGUILayout.LabelField("要关闭的节点标签",  curNodeTage == null ? "NONE" : curNodeTage.fieldName);
        _m_toggleShowSelect = EditorGUILayout.BeginFoldoutHeaderGroup(_m_toggleShowSelect, "节点标签选择器");
        if (_m_toggleShowSelect)
        {
            string filter = EditorGUILayout.TextField("筛选条件", _m_filter);
            EditorGUI.indentLevel++;
            if (filter != _m_filter)
            {
                _m_filter = filter;
                doFilter();
            }

            _m_scrollPos = EditorGUILayout.BeginScrollView(_m_scrollPos, GUILayout.Height(200));
            foreach (var nodeTag in _m_filteredNodeTags)
            {
                if(nodeTag == null)
                    continue;
                bool isToggle = EditorGUILayout.Toggle(nodeTag.fieldName, nodeTag.value == mono.nodeTag);
                if (isToggle)
                {
                    if (mono.nodeTag != nodeTag.value)
                    {
                        mono.nodeTag = nodeTag.value;
                        EditorUtility.SetDirty(mono);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    
    

    private void doFilter()
    {
        _m_filteredNodeTags.Clear();
        foreach (var nodeTag in _m_nodeTags)
        {
            if(nodeTag == null)
                continue;
            if (string.IsNullOrEmpty(_m_filter) || nodeTag.fieldName.ToLower().Contains(_m_filter.ToLower()))
            {
                _m_filteredNodeTags.Add(nodeTag);
            }
        }
    }

    private NodeTagInfo getNodeTag(string _nodeTagValue)
    {
        foreach (NodeTagInfo nodeTag in _m_filteredNodeTags)
        {
            if(nodeTag != null && _nodeTagValue == nodeTag.value)
            {
                return nodeTag;
            }
        }
        return null;
    }
}

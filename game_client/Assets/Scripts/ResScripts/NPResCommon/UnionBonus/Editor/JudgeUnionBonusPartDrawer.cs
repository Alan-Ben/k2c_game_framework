using System;
using CommonEnum;
using GOE;
using NPEnum;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(JudgeUnionBonusPart))]
public class JudgeUnionBonusPartDrawer : PropertyDrawer
{
    private float totalHeight;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        totalHeight = 0;
        float singleLineHeight = EditorGUIUtility.singleLineHeight + 2;
        
        // 获取序列化属性
        SerializedProperty filterTypeProp = property.FindPropertyRelative("filterType");
        SerializedProperty idProp = property.FindPropertyRelative("id");

        if (filterTypeProp == null || idProp == null)
        {
            Debug.LogError($"[JudgeUnionBonusPartDrawer] FindPropertyRelative error");
            return;
        }

        // 显示枚举选择框
        Rect typeRect = new Rect(position.x, position.y + totalHeight, position.width, singleLineHeight);
        EditorGUI.PropertyField(typeRect, filterTypeProp, new GUIContent("Bonus过滤类型"));
        totalHeight += singleLineHeight;

        EBonusFilterType filterType = (EBonusFilterType)filterTypeProp.enumValueIndex;
        Rect idRect = new Rect(position.x, position.y + totalHeight, position.width, singleLineHeight);
        totalHeight += singleLineHeight;
        if (filterType != EBonusFilterType.NONE)
        {
            // 动态显示 id 字段（根据 filterType 切换）
            switch (filterType)
            {
                case EBonusFilterType.HERO_ATTR:
                    _drawEnum<ESpecAttrType>(idProp, idRect, "特长类型");
                    break;

                case EBonusFilterType.BUILDING_ATTR:
                    _drawEnum<ESpecAttrType>(idProp, idRect, "特长类型");
                    break;

                case EBonusFilterType.BUILDING_ID:
                    // 直接输入建筑ID
                    idProp.longValue = EditorGUI.LongField(idRect, "建筑ID", idProp.longValue);
                    break;
                
                case EBonusFilterType.STUDENT_SEX:
                    _drawEnum<EChildSexType>(idProp, idRect, "性别类型");
                    break;
                
                case EBonusFilterType.STUDENT_ATTR:
                    _drawEnum<ESpecAttrType>(idProp, idRect, "特长类型");
                    break;
                
                case EBonusFilterType.CONSORT_ID:
                    // 指定家人id
                    idProp.longValue = EditorGUI.LongField(idRect, "家人ID", idProp.longValue);
                    break;
                
                case EBonusFilterType.HERO_ID:
                    // 指定家人id
                    idProp.longValue = EditorGUI.LongField(idRect, "大臣ID", idProp.longValue);
                    break;
                
                case EBonusFilterType.QUALITY:
                    // 显示 EQuality 枚举选择
                    _drawEnum<EQuality>(idProp, idRect, "品质");
                    break;
                
                case EBonusFilterType.TREASURE_HUNT_TREASURE_ID:
                    // 指定奇物id
                    idProp.longValue = EditorGUI.LongField(idRect, "奇物ID", idProp.longValue);
                    break;
                
                default:
                    idProp.longValue = EditorGUI.LongField(idRect, "过滤项目子ID", idProp.longValue);
                    break;
            }
        }
        else
        {
            //最后增加空行
            EditorGUI.LabelField(idRect, "");
        }
    }

    private void _drawEnum<T>(SerializedProperty _property, Rect _rect, string label) 
        where T : Enum
    {
        if(_property == null)
            return;
        
        // 将 _property.longValue 转换为泛型 T 的枚举值
        T enumValue = (T)Enum.ToObject(typeof(T), _property.longValue);
        // 使用 EditorGUI 显示枚举选择框
        enumValue = (T)EditorGUI.EnumPopup(_rect, label, enumValue);
        // 将选择后的枚举值转换回 long 并赋值给 _property.longValue
        _property.longValue = Convert.ToInt64(enumValue);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return totalHeight;
    }
}
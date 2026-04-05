using System.Collections.Generic;
using GOE;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomPropertyDrawer(typeof(NextStepTrigger))]
    public class NextStepTriggerDrawer : PropertyDrawer
    {
        private float _m_perHeight = 18;
        private float _m_allHeight = 0;
        private Dictionary<string, float> _m_propertyHeight = new Dictionary<string, float>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            _m_allHeight = 0;
            _m_perHeight = EditorGUI.GetPropertyHeight(property, false);
            
            SerializedProperty triggerType = property.FindPropertyRelative("triggerType");
            SerializedProperty btnTrigger = property.FindPropertyRelative("btnTrigger");
            SerializedProperty triggerMsgType = property.FindPropertyRelative("triggerMsgType");
            SerializedProperty autoTriggerDelayTime = property.FindPropertyRelative("autoTriggerDelayTime");
            SerializedProperty triggerFunc = property.FindPropertyRelative("triggerFunc");

            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y ,150,_m_perHeight),property.isExpanded, label, true);
            _m_allHeight += _m_perHeight;
            
            if (property.isExpanded)
            {
                _getPropertyField(position, triggerType);

                int triggerTypeValue = triggerType.enumValueFlag;
                switch (triggerTypeValue)
                {
                    case (int)ENextStepTriggerType.CLICK:
                        _getPropertyField(position, btnTrigger);
                        break;
                    
                    case (int)ENextStepTriggerType.MSG:
                        _getPropertyField(position, triggerMsgType);
                        break;
                    
                    case (int)ENextStepTriggerType.AUTO:
                        _getPropertyField(position, autoTriggerDelayTime);
                        break;
                }
                
                _getPropertyField(position, triggerFunc);
            }
            // _m_allHeight += _m_perHeight;
            if (!_m_propertyHeight.ContainsKey(property.propertyPath))
            {
                _m_propertyHeight.Add(property.propertyPath, _m_allHeight);
            }
            else
            {
                _m_propertyHeight[property.propertyPath] = _m_allHeight;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            float heightfalse = EditorGUI.GetPropertyHeight(property, label, false);
            if (property.isExpanded)
            {
                float allHeight = _m_propertyHeight.ContainsKey(property.propertyPath) ? _m_propertyHeight[property.propertyPath] : 0;
                return heightfalse + allHeight;
            }
            return heightfalse;
        }
        
        private void _getPropertyField(Rect _sourcePosition,SerializedProperty _property)
        {
            EditorGUI.PropertyField(new Rect(_sourcePosition.x,_sourcePosition.y + _m_allHeight ,_sourcePosition.width,_m_perHeight), _property, true);
            _m_allHeight += EditorGUI.GetPropertyHeight(_property, true);
        }
        
        
        private void _getTextField(Rect _sourcePosition,SerializedProperty _property)
        {
            GUIStyle style = EditorStyles.textField;
            style.stretchHeight = true;
            style.wordWrap = true;
            _property.stringValue = EditorGUI.TextField(new Rect(_sourcePosition.x,_sourcePosition.y + _m_allHeight ,_sourcePosition.width,EditorGUI.GetPropertyHeight(_property, true)), 
                _property.name,_property.stringValue);
            _m_allHeight += EditorGUI.GetPropertyHeight(_property, true);
            
            style.stretchHeight = false;
            style.wordWrap = false;
        }
        


        /// <summary>
        /// 查找数据为空的对象
        /// </summary>
        /// <param name="_property"></param>
        /// <returns></returns>
        private bool propertyIsEmpty(SerializedProperty _property)
        {
            switch (_property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    if (_property.intValue == 0)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Boolean:
                    if (_property.boolValue == false)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Float:
                    if (_property.floatValue == 0f)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.String:
                    if (string.IsNullOrEmpty(_property.stringValue))
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.ObjectReference:
                    if (_property.objectReferenceValue == null)
                    {
                        if (_property.objectReferenceInstanceIDValue != 0) //引用Id不为0，说明是丢失了引用
                            return false;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Vector2:
                    if (_property.vector2Value == Vector2.zero)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Vector3:
                    if (_property.vector3Value == Vector3.zero)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Vector4:
                    if (_property.vector4Value == Vector4.zero)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.ArraySize:
                    if (_property.arraySize == 0)
                        return true;
                    else
                    {
                        return false;
                    }

                case SerializedPropertyType.Enum:
                    if (_property.enumValueIndex == 0)
                        return true;
                    else
                    {
                        return false;
                    }
            }

            if (_property.isArray && _property.arraySize == 0)
            {
                return true;
            }

            return false;
        }
    }
}
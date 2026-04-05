using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomPropertyDrawer(typeof(MiniQTEGameStepObj))]
    public class MiniQTEGameStepObjDrawer : PropertyDrawer
    {
        private float _m_perHeight = 18;
        private float _m_allHeight = 0;
        private Dictionary<string, float> _m_propertyHeight = new Dictionary<string, float>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            float heightfalse = EditorGUI.GetPropertyHeight(property, label, false);
            if (property.isExpanded)
            {
                SerializedProperty showAllFieldsProperty = property.FindPropertyRelative("_showAllFields");
                if (null != showAllFieldsProperty && showAllFieldsProperty.boolValue)
                    return height;

                float allHeight = _m_propertyHeight.ContainsKey(property.propertyPath) ? _m_propertyHeight[property.propertyPath] : 0;
                return heightfalse + allHeight;
            }
            return heightfalse;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            _m_allHeight = 0;
            _m_perHeight = EditorGUI.GetPropertyHeight(property, false);
            
            SerializedProperty selfGo = property.FindPropertyRelative("selfGo");
            SerializedProperty stepName = property.FindPropertyRelative("stepName");
            
            SerializedProperty beforeStartStepEffect = property.FindPropertyRelative("beforeStartStepEffect");
            
            SerializedProperty enableGo = property.FindPropertyRelative("enableGo");
            SerializedProperty disableGo = property.FindPropertyRelative("disableGo");
            
            SerializedProperty stepVoiceIndex = property.FindPropertyRelative("stepVoiceIndex");
            
            SerializedProperty nextStepTriggerList = property.FindPropertyRelative("nextStepTriggerList");
            
            SerializedProperty stepDoneEffect = property.FindPropertyRelative("stepDoneEffect");
            SerializedProperty toNextStepDelayTime = property.FindPropertyRelative("toNextStepDelayTime");
            
            SerializedProperty exitEnableGo = property.FindPropertyRelative("exitEnableGo");
            SerializedProperty exitDisableGo = property.FindPropertyRelative("exitDisableGo");
            
            
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y ,150,_m_perHeight),property.isExpanded, label, true);
            _m_allHeight += _m_perHeight;
            
            // GUILayout.EndHorizontal();
            
            if (property.isExpanded)
            {
                //是否提示警告
                bool isWaringTip = false;
                string tipMessage = string.Empty;
                string parentName = string.Empty;
                if (selfGo != null && selfGo.propertyType == SerializedPropertyType.ObjectReference && selfGo.objectReferenceValue != null)
                {
                    GameObject selfParentGo = selfGo.objectReferenceValue as GameObject;
                    if (null != selfParentGo && null != selfParentGo.transform)
                    {
                        parentName = selfParentGo.name;
                        isWaringTip = checkWaring(selfParentGo.transform, out tipMessage);
                    }
                }

                if (isWaringTip)
                {
                    GUIStyle waringStyle = new GUIStyle();
                    waringStyle.fontSize = 15;
                    waringStyle.normal.textColor = Color.red;
                    EditorGUI.LabelField(new Rect(position.x,position.y + _m_allHeight ,position.width,_m_perHeight), $"{tipMessage}有mono拖错拉，嗷呜~", waringStyle);
                    _m_allHeight += _m_perHeight;
                }
                
                GUIStyle titleStyle2 = new GUIStyle();
                titleStyle2.fontSize = 15;
                titleStyle2.normal.textColor = Color.yellow;
                EditorGUI.LabelField(new Rect(position.x,position.y + _m_allHeight ,position.width,_m_perHeight),$"{parentName}*******************", titleStyle2);
                _m_allHeight += _m_perHeight;

                //校验提示函数
                bool checkWaring(Transform _parent, out string _valueTip)
                {
                    _valueTip = string.Empty;

                    if (nextStepTriggerList == null)
                    {
                        _valueTip = "nextStepTriggerList";
                    }
                    
                    return false;
                }
                
                SerializedProperty showAllFieldsProperty = property.FindPropertyRelative("_showAllFields");
                EditorGUI.PropertyField(new Rect(position.x,position.y + _m_allHeight ,position.width,_m_perHeight), showAllFieldsProperty, new GUIContent("******是否打开所有选项******"));
                _m_allHeight += _m_perHeight;
                int showCount = 0;
                
                if (showAllFieldsProperty.boolValue)
                {
                    _getPropertyField(position, selfGo);
                    _getPropertyField(position, stepName);
                    _getPropertyField(position, beforeStartStepEffect);
                    _getPropertyField(position, enableGo);
                    _getPropertyField(position, disableGo);
                    _getPropertyField(position, stepVoiceIndex);
                    _getPropertyField(position, nextStepTriggerList);
                    _getPropertyField(position, stepDoneEffect);
                    _getPropertyField(position, toNextStepDelayTime);
                    _getPropertyField(position, exitEnableGo);
                    _getPropertyField(position, exitDisableGo);
                    
                    showCount += 11;
                }
                else
                {
                    if (!propertyIsEmpty(selfGo))
                    {
                        _getPropertyField(position, selfGo);
                        showCount++;
                    }
                    
                    if (!propertyIsEmpty(stepName))
                    {
                        _getPropertyField(position, stepName);
                        showCount++;
                    }

                    if (!propertyIsEmpty(beforeStartStepEffect))
                    {
                        _getPropertyField(position, beforeStartStepEffect);
                        showCount++;
                    }

                    if (!propertyIsEmpty(enableGo))
                    {
                        _getPropertyField(position, enableGo);
                        showCount++;
                    }

                    if (!propertyIsEmpty(disableGo))
                    {
                        _getPropertyField(position, disableGo);
                        showCount++;
                    }

                    if (null != stepVoiceIndex)
                    {
                        SerializedProperty mainId = stepVoiceIndex.FindPropertyRelative("mainId");
                        SerializedProperty subId = stepVoiceIndex.FindPropertyRelative("subId");
                        if (!propertyIsEmpty(mainId) || !propertyIsEmpty(subId))
                        {
                            _getPropertyField(position, stepVoiceIndex);
                            showCount++;
                        }
                    }

                    // 触发器不能为空, 一定要显示
                    _getPropertyField(position, nextStepTriggerList);
                    showCount++;

                    if (!propertyIsEmpty(stepDoneEffect))
                    {
                        _getPropertyField(position, stepDoneEffect);
                        showCount++;
                    }

                    if (!propertyIsEmpty(toNextStepDelayTime))
                    {
                        _getPropertyField(position, toNextStepDelayTime);
                        showCount++;
                    }

                    if (!propertyIsEmpty(exitEnableGo))
                    {
                        _getPropertyField(position, exitEnableGo);
                        showCount++;
                    }
                    
                    if (!propertyIsEmpty(exitDisableGo))
                    {
                        _getPropertyField(position, exitDisableGo);
                        showCount++;
                    }
                }
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
            // if (property.propertyPath == "stepList.Array.data[0]")
            // {
            //     
            //     float height = EditorGUI.GetPropertyHeight(property, label, true);
            //     float heightfalse = EditorGUI.GetPropertyHeight(property, label, false);
            //     Debug.LogError($"------------property:{property.propertyPath},height:{height},heightfalse:{heightfalse},_m_allHeight:{_m_allHeight}");
            // }
            
            

            // GUILayout.BeginHorizontal();

            #region 对列表操作，最好在所有绘制完后再处理，特别是删除，会报错

            if (GUI.Button(new Rect(position.x + position.width - 200, position.y ,95,_m_perHeight),"复制自己"))
            {
                SerializedObject parentSerializedObject = property.serializedObject;
                SerializedProperty stepObjList = parentSerializedObject.FindProperty("stepObjList");
                for (int i = 0; i < stepObjList.arraySize; i++)
                {
                    if (stepObjList.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                    {
                        stepObjList.InsertArrayElementAtIndex(i);
                        break;
                    }
                }
            }
            
            if(GUI.Button(new Rect(position.x + position.width - 100,position.y ,95,_m_perHeight),"删除自己"))
            {
                SerializedObject parentSerializedObject = property.serializedObject;
                SerializedProperty stepObjList = parentSerializedObject.FindProperty("stepObjList");
                for (int i = 0; i < stepObjList.arraySize; i++)
                {
                    if (stepObjList.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                    {
                        stepObjList.DeleteArrayElementAtIndex(i);
                        break;
                    }
                }
            };
            #endregion
            
            
            EditorGUI.EndProperty();
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
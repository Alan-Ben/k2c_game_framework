using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MG.MGEditor
{
    [CustomPropertyDrawer(typeof(NPGGUIMonoTutorialWndStepObj))]
    public class NPGGUIMonoTutorialWndStepObjDrawer : PropertyDrawer
    {
        //用来记录每一列显示个数
        private Dictionary<int, int> _m_showCount = new Dictionary<int, int>();
        private float _m_perHeight = 18;
        private float _m_allHeight = 0;
        private Dictionary<string, float> _m_propertyHeight = new Dictionary<string, float>();
        private List<SerializedProperty> _m_serializedProperties = new List<SerializedProperty>();

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
            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty moveMaskRectStr = property.FindPropertyRelative("moveMaskRectStr");
            SerializedProperty MoveMaskRect = property.FindPropertyRelative("MoveMaskRect");
            SerializedProperty isMoveMaskEnable = property.FindPropertyRelative("isMoveMaskEnable");

            SerializedProperty highlightGoRectTransform = property.FindPropertyRelative("highlightGoRectTransform");
            SerializedProperty highlightGoLocationStr = property.FindPropertyRelative("highlightGoLocationStr");
            
            SerializedProperty nextStepBtn = property.FindPropertyRelative("nextStepBtn");
            SerializedProperty stepDealFunc = property.FindPropertyRelative("stepDealFunc");
            
            SerializedProperty clickPreDealFunc = property.FindPropertyRelative("clickPreDealFunc");
            SerializedProperty clickDealFunc = property.FindPropertyRelative("clickDealFunc");
            
            SerializedProperty triggerType = property.FindPropertyRelative("triggerType");
            SerializedProperty triggerTypeArgs = property.FindPropertyRelative("triggerTypeArgs");
            
            SerializedProperty autoNextStepCondition = property.FindPropertyRelative("autoNextStepCondition");
            
            SerializedProperty dragInfo = property.FindPropertyRelative("dragInfo");
            SerializedProperty enableGo = property.FindPropertyRelative("enableGo");
            SerializedProperty disableGo = property.FindPropertyRelative("disableGo");
            
            SerializedProperty exitEnableGo = property.FindPropertyRelative("exitEnableGo");
            SerializedProperty exitDisableGo = property.FindPropertyRelative("exitDisableGo");
            SerializedProperty gudieVoiceIndex = property.FindPropertyRelative("gudieVoiceIndex");
            
            SerializedProperty delayClickResponseTime = property.FindPropertyRelative("delayClickResponseTime");
            SerializedProperty delayInitMoveMaskTime = property.FindPropertyRelative("delayInitMoveMaskTime");
            SerializedProperty autoDelayToNextStep = property.FindPropertyRelative("autoDelayToNextStep");
            SerializedProperty traceId = property.FindPropertyRelative("traceId");

            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y ,150,_m_perHeight),property.isExpanded, label, true);
            _m_allHeight += _m_perHeight;
            
            // GUILayout.EndHorizontal();
            //当前是第几项
            int curIndex = 0;
            SerializedProperty stepIndexList = property.serializedObject.FindProperty("stepList");
            for (int i = 0; i < stepIndexList.arraySize; i++)
            {
                if (stepIndexList.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                {
                    curIndex = i;
                    break;
                }
            }
            
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
                    
                    RectTransform MoveMaskRectValue = MoveMaskRect.objectReferenceValue as RectTransform;
                    if (null != MoveMaskRectValue && !MoveMaskRectValue.IsChildOf(_parent))
                    {
                        _valueTip = "MoveMaskRect";
                        return true;
                    }
                    
                    RectTransform nextStepBtnValue = nextStepBtn.objectReferenceValue as RectTransform;
                    if (null != nextStepBtnValue && !nextStepBtnValue.IsChildOf(_parent))
                    {
                        _valueTip = "nextStepBtn";
                        return true;
                    }

                    if (dragInfo != null)
                    {
                        SerializedProperty dragEndRect = dragInfo.FindPropertyRelative("dragEndRect");
                        SerializedProperty dragGo = dragInfo.FindPropertyRelative("dragGo");

                        if (dragEndRect != null)
                        {
                            RectTransform dragEndRectValue = dragEndRect.objectReferenceValue as RectTransform;
                            if (null != dragEndRectValue && !dragEndRectValue.IsChildOf(_parent))
                            {
                                _valueTip = "dragInfo";
                                return true;
                            }
                        }
                        
                        if (dragGo != null)
                        {
                            GameObject dragGoValue = dragGo.objectReferenceValue as GameObject;
                            if (null != dragGoValue && null != dragGoValue.transform && !dragGoValue.transform.IsChildOf(_parent))
                            {
                                _valueTip = "dragInfo";
                                return true;
                            }
                        }
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
                    _getPropertyField(position, name);
                    _getPropertyField(position, moveMaskRectStr);
                    _getPropertyField(position, MoveMaskRect);
                    _getPropertyField(position, isMoveMaskEnable);
                    _getPropertyField(position, highlightGoRectTransform);
                    _getPropertyField(position, highlightGoLocationStr);
                    _getTextField(position, stepDealFunc);
                    _getPropertyField(position, nextStepBtn);
                    _getPropertyField(position, clickPreDealFunc);
                    _getTextField(position, clickDealFunc);
                    _getPropertyField(position, triggerType);
                    _getPropertyField(position, triggerTypeArgs);
                    _getPropertyField(position, autoNextStepCondition);
                    _getPropertyField(position, dragInfo);
                    _getPropertyField(position, enableGo);
                    _getPropertyField(position, disableGo);
                    _getPropertyField(position, exitEnableGo);
                    _getPropertyField(position, exitDisableGo);
                    _getPropertyField(position, gudieVoiceIndex);
                    _getPropertyField(position, delayClickResponseTime);
                    _getPropertyField(position, delayInitMoveMaskTime);
                    _getPropertyField(position, autoDelayToNextStep);
                    _getPropertyField(position, traceId);
                    showCount += 22;
                }
                else
                {
                    if (!propertyIsEmpty(selfGo))
                    {
                        _getPropertyField(position, selfGo);
                        showCount++;
                    }
                    
                    if (!propertyIsEmpty(name))
                    {
                        _getPropertyField(position, name);
                        showCount++;
                    }

                    if (!propertyIsEmpty(moveMaskRectStr))
                    {
                        _getPropertyField(position, moveMaskRectStr);
                        showCount++;
                    }

                    if (!propertyIsEmpty(MoveMaskRect))
                    {
                        _getPropertyField(position, MoveMaskRect);
                        showCount++;
                    }

                    if (!propertyIsEmpty(isMoveMaskEnable))
                    {
                        _getPropertyField(position, isMoveMaskEnable);
                        showCount++;
                    }
                    
                    if(!propertyIsEmpty(highlightGoRectTransform))
                    {
                        _getPropertyField(position, highlightGoRectTransform);
                        showCount++;
                        
                        _getPropertyField(position, highlightGoLocationStr);
                        showCount++;
                    }

                    if (!propertyIsEmpty(stepDealFunc))
                    {
                        _getTextField(position, stepDealFunc);
                        showCount++;
                    }

                    if (!propertyIsEmpty(nextStepBtn))
                    {
                        _getPropertyField(position, nextStepBtn);
                        showCount++;
                    }

                    if (!propertyIsEmpty(clickPreDealFunc))
                    {
                        _getPropertyField(position, clickPreDealFunc);
                        showCount++;
                    }

                    if (!propertyIsEmpty(clickDealFunc))
                    {
                        _getTextField(position,clickDealFunc);
                        showCount++;
                    }

                    if (!propertyIsEmpty(triggerType))
                    {
                        _getPropertyField(position, triggerType);
                        showCount++;
                    }

                    if (!propertyIsEmpty(triggerTypeArgs))
                    {
                        _getPropertyField(position, triggerTypeArgs);
                        showCount++;
                    }
                    
                    if (!propertyIsEmpty(autoNextStepCondition))
                    {
                        _getPropertyField(position, autoNextStepCondition);
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

                    if (null != gudieVoiceIndex)
                    {
                        SerializedProperty mainId = gudieVoiceIndex.FindPropertyRelative("mainId");
                        SerializedProperty subId = gudieVoiceIndex.FindPropertyRelative("subId");
                        if (!propertyIsEmpty(mainId) || !propertyIsEmpty(subId))
                        {
                            _getPropertyField(position, gudieVoiceIndex);
                            showCount++;
                        }
                    }

                    if (!propertyIsEmpty(delayClickResponseTime))
                    {
                        _getPropertyField(position, delayClickResponseTime);
                        showCount++;
                    }

                    if (!propertyIsEmpty(delayInitMoveMaskTime))
                    {
                        _getPropertyField(position, delayInitMoveMaskTime);
                        showCount++;
                    }

                    //autoDelayToNextStep特殊判断有需求小于0就隐藏
                    if (autoDelayToNextStep != null && autoDelayToNextStep.floatValue > 0f)
                    {
                        _getPropertyField(position, autoDelayToNextStep);
                        showCount++;
                    }

                    if(!propertyIsEmpty(traceId))
                    {
                        _getPropertyField(position, traceId);
                        showCount++;
                    }

                    //这个只能特殊处理
                    if (dragInfo != null)
                    {
                        SerializedProperty dragEndRect = dragInfo.FindPropertyRelative("dragEndRect");
                        SerializedProperty dragGo = dragInfo.FindPropertyRelative("dragGo");
                        SerializedProperty dragEndFunc = dragInfo.FindPropertyRelative("dragEndFunc");
                        SerializedProperty dragBeginFunc = dragInfo.FindPropertyRelative("dragBeginFunc");
                        SerializedProperty dragFunc = dragInfo.FindPropertyRelative("dragFunc");
                        SerializedProperty dragFailureFunc = dragInfo.FindPropertyRelative("dragFailureFunc");
                        if (!propertyIsEmpty(dragEndRect) || !propertyIsEmpty(dragGo) || !propertyIsEmpty(dragEndFunc)
                            || !propertyIsEmpty(dragBeginFunc) || !propertyIsEmpty(dragFunc) ||
                            !propertyIsEmpty(dragFailureFunc))
                        {
                            _getPropertyField(position, dragInfo);
                            showCount++;
                        }
                    }
                }
                
                

                if (_m_showCount.ContainsKey(curIndex))
                {
                    if (_m_showCount[curIndex] != showCount)
                    {
                        EditorGUI.FocusTextInControl(string.Empty);
                        _m_allHeight += _m_perHeight;
                    }

                    _m_showCount[curIndex] = showCount;
                }
                else
                {
                    _m_showCount.Add(curIndex, showCount);
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
                SerializedProperty stepList = parentSerializedObject.FindProperty("stepList");
                for (int i = 0; i < stepList.arraySize; i++)
                {
                    if (stepList.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                    {
                        stepList.InsertArrayElementAtIndex(i);
                        break;
                    }
                }
            }
            
            if(GUI.Button(new Rect(position.x + position.width - 100,position.y ,95,_m_perHeight),"删除自己"))
            {
                SerializedObject parentSerializedObject = property.serializedObject;
                SerializedProperty stepList = parentSerializedObject.FindProperty("stepList");
                for (int i = 0; i < stepList.arraySize; i++)
                {
                    if (stepList.GetArrayElementAtIndex(i).propertyPath == property.propertyPath)
                    {
                        stepList.DeleteArrayElementAtIndex(i);
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
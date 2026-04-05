using UnityEngine;
using UnityEditor;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using ALPackage;
using Microsoft.CSharp;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace GOE
{
	/// <summary>
	/// 用于editor下自定义读取Ilruntime的dll
	/// 配合MonoSkin使用
	/// </summary>
	[CustomEditor(typeof(MonoSkin))]
	public class MonoSkinEditor : UnityEditor.Editor
	{
#if NP_GAME
		//热更工程的统一名称空间
		protected const string _m_ILRuntimeNamespace = "Hotfix";
		//热更工程dll的位置
		protected const string _m_ILRuntimeDllPatch = "Assets/hotfix~/Hotfix/bin/Editor/Hotfix.dll";
		
		protected SerializedProperty _m_exportClassName;//导出类名
		protected SerializedProperty _m_propList;//属性列表
		//列表折叠状态(key为SerializedProperty的propertyPath)
		protected Dictionary<string, bool> _m_listFoldoutMap = new Dictionary<string, bool>();

		void OnEnable()
		{
			//初始化自定义数据
			this._m_exportClassName = this.serializedObject.FindProperty(MonoSkinConst.monoSkin_exportClassName);
			this._m_propList = this.serializedObject.FindProperty(MonoSkinConst.monoSkin_propList);

			//如果类名不为空，每次打开时候刷一下
			if (!string.IsNullOrEmpty(this._m_exportClassName.stringValue))
			{
				_refreshFiledValue();
			}
		}
		
		
		// 创建自定义检视面板
		public override void OnInspectorGUI()
		{  
			base.OnInspectorGUI();
			this.serializedObject.Update();

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUILayout.Space();

			//绘制导出类名
			_m_exportClassName.stringValue = EditorGUILayout.TextField("导出类名", _m_exportClassName.stringValue);

			//绘制加载按钮，包括按钮逻辑
			if (GUILayout.Button("刷新编译mono"))
			{
				_refreshFiledValue();
			}

			//绘制mono列表
			_drawPropertyAreas();

			//应用改动属性
			this.serializedObject.ApplyModifiedProperties();
		}

		//复制每个字段值
		private void _refreshFiledValue()
		{
			ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
			domain.LoadAssemblyFile(_m_ILRuntimeDllPatch);
			Type type = domain.getDllType($"{_m_ILRuntimeNamespace}.{this._m_exportClassName.stringValue}");

			if(null == type)
				return;

			FieldInfo[] fieldInfos = type.GetFields();

			if(fieldInfos.Length == 0)
				return;
            
			//第几个属性，确认一次+1
			int index = 0;
			for (int i = 0; i < fieldInfos.Length; i++)
			{
				FieldInfo fieldInfo = fieldInfos[i];
				
				if(null == fieldInfo)
		 			continue;
				
				//Ilruntime自定义的Header，可以用于注释和判断是否需要序列化
				HotfixMonoAttribute customAttribute = fieldInfo.GetCustomAttribute<HotfixMonoAttribute>();
				if(null == customAttribute)
		 			continue;
				
				//不序列化不生成
				if(!customAttribute.isSerialize)
		 			continue;
                
				SerializedProperty curPropRef = null;
				int curPropIndex = -1;
				
				//根据名字取一下旧值，看下有没有
				for (int j = 0; j < _m_propList.arraySize; j++)
				{
					SerializedProperty itemRefValue = this._m_propList.GetArrayElementAtIndex(j);
					if (itemRefValue.FindPropertyRelative(MonoSkinConst.singleProperty_propName).stringValue == fieldInfo.Name)
					{
						curPropRef = itemRefValue;
						curPropIndex = j;
					}
				}
				
				//如果有同字段名数据，把这个数据挪到第一个
				if (null != curPropRef && curPropIndex != -1)
				{
					_m_propList.MoveArrayElement(curPropIndex, index);
					curPropIndex = index;
				}
				//如果没有，说明是新增的字段，或者改名，这种情况的引用值不管了
				else
				{
					//生成新的对应数据
					this._m_propList.InsertArrayElementAtIndex(index);
					curPropRef = this._m_propList.GetArrayElementAtIndex(index);
					curPropIndex = index;
				}
                
				index++;
                
				SerializedProperty typeRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propType);
				SerializedProperty nameRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propName);
				SerializedProperty valRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propVal);
				SerializedProperty descRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propDesc);
				SerializedProperty propTypeNameRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propTypeName);
				SerializedProperty propTypeAssemblyRef = curPropRef.FindPropertyRelative(MonoSkinConst.singleProperty_propTypeAssembly);
                
				//如果改名字段，新增字段，或者字段类型改变需要清除引用信息，策划重新拖
				if (nameRef.stringValue != fieldInfo.Name
					|| typeRef.stringValue != fieldInfo.FieldType.Name
				    || propTypeNameRef.stringValue != fieldInfo.FieldType.FullName
				    || propTypeAssemblyRef.stringValue != fieldInfo.FieldType.Assembly.GetName().Name)
				{
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objValue).objectReferenceValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objListValue).ClearArray();
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_intValue).intValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_longValue).longValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_floatValue).floatValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_boolValue).boolValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_strValue).stringValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_colorValue).colorValue = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_vector3Value).vector3Value = default;
					valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_serializedClassFieldNames).ClearArray();
				}
				
				//赋值每个字段必须要的属性，包括类型，名字等
				nameRef.stringValue = fieldInfo.Name;
				descRef.stringValue = customAttribute.notes;
				typeRef.stringValue = fieldInfo.FieldType.Name;
				propTypeNameRef.stringValue = fieldInfo.FieldType.FullName;
				propTypeAssemblyRef.stringValue = fieldInfo.FieldType.Assembly.GetName().Name;
			}

			//删除多余的
			for (int i = _m_propList.arraySize - 1; i >= index; i--)
			{
				_m_propList.DeleteArrayElementAtIndex(i);
			}
			
			//应用改动属性
			this.serializedObject.ApplyModifiedProperties();
		}
        
		//绘制属性列表
		protected void _drawPropertyAreas()
		{
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("------------------------------------------------------------------------------------------------------------------------------");

			EditorGUILayout.Space();


			//Display our list to the inspector window

			for (int i = 0; i < this._m_propList.arraySize; ++i)
			{
				SerializedProperty propRef = this._m_propList.GetArrayElementAtIndex(i);

				SerializedProperty typeRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propType);
				SerializedProperty nameRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propName);
				SerializedProperty valRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propVal);
				SerializedProperty descRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propDesc);
				SerializedProperty propTypeNameRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propTypeName);
				SerializedProperty propTypeAssemblyRef = propRef.FindPropertyRelative(MonoSkinConst.singleProperty_propTypeAssembly);
				
				//注释
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(descRef.stringValue, EditorStyles.whiteLargeLabel);
				EditorGUILayout.EndHorizontal();
                
				//变量名
				EditorGUILayout.BeginHorizontal();

				//数据类型
				Type curType = _getTypeFromAssembly(propTypeAssemblyRef.stringValue, propTypeNameRef.stringValue);

				//列表类型在字段名处显示Foldout折叠控件和元素数量
				bool isList = null != curType && curType.IsGenericType && curType.GetGenericTypeDefinition() == typeof(List<>);
				if (isList)
				{
					SerializedProperty listValRef = valRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objListValue);
					int listCount = null != listValRef ? listValRef.arraySize : 0;
					string foldoutKey = valRef.propertyPath;
					if (!_m_listFoldoutMap.ContainsKey(foldoutKey))
						_m_listFoldoutMap[foldoutKey] = true;
					_m_listFoldoutMap[foldoutKey] = EditorGUILayout.Foldout(_m_listFoldoutMap[foldoutKey], $"{nameRef.stringValue} ({listCount})", true);
				}
				else
				{
					EditorGUILayout.LabelField(nameRef.stringValue);
				}

				//绘制item
				_drawFieldItem(curType, valRef);
				
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
			}

			EditorGUILayout.Space();
		}

		//绘制具体字段item
		protected void _drawFieldItem(Type _curType, SerializedProperty _valueRef)
		{
			if(null == _curType)
				return;
			
			//int类型
			if (typeof(int) == _curType)
			{
				SerializedProperty valueRef_IntValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_intValue);
				valueRef_IntValue.intValue = EditorGUILayout.IntField("", valueRef_IntValue.intValue);
			}
			//byte类型(用int存储)
			else if (typeof(byte) == _curType)
			{
				SerializedProperty valueRef_IntValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_intValue);
				valueRef_IntValue.intValue = (byte)Mathf.Clamp(EditorGUILayout.IntField("", valueRef_IntValue.intValue), 0, 255);
			}
			//long类型
			else if (typeof(long) == _curType)
			{
				SerializedProperty valueRef_LongValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_longValue);
				valueRef_LongValue.longValue = EditorGUILayout.LongField("", valueRef_LongValue.longValue);
			}
			//float类型
			else if (typeof(float) == _curType)
			{
				SerializedProperty valueRef_FloatValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_floatValue);
				valueRef_FloatValue.floatValue = EditorGUILayout.FloatField("", valueRef_FloatValue.floatValue);
			}
			//bool类型
			else if (typeof(bool) == _curType)
			{
				SerializedProperty valueRef_BoolValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_boolValue);
				valueRef_BoolValue.boolValue = GUILayout.Toggle(valueRef_BoolValue.boolValue, "");
			}
			//string类型
			else if (typeof(string) == _curType)
			{
				SerializedProperty valueRef_StrValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_strValue);
				valueRef_StrValue.stringValue = EditorGUILayout.TextField("", valueRef_StrValue.stringValue);
			}
			//color类型
			else if (typeof(Color) == _curType)
			{
				SerializedProperty valueRef_ColorValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_colorValue);
				valueRef_ColorValue.colorValue = EditorGUILayout.ColorField("", valueRef_ColorValue.colorValue);
			}
			//Vector3类型
			else if (typeof(Vector3) == _curType)
			{
				SerializedProperty valueRef_Vector3Value = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_vector3Value);
				valueRef_Vector3Value.vector3Value = EditorGUILayout.Vector3Field("", valueRef_Vector3Value.vector3Value);
			}
			//列表类型特殊处理
			else if (_curType.IsGenericType && _curType.GetGenericTypeDefinition() == typeof(List<>))
			{
				SerializedProperty valueRef_objListValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objListValue);

				//绘制增添元素和删减元素按钮
				if (GUILayout.Button("+", GUILayout.MaxWidth(123), GUILayout.MaxHeight(18)))
				{
					valueRef_objListValue.InsertArrayElementAtIndex(valueRef_objListValue.arraySize);
				}
				if (GUILayout.Button("-", GUILayout.MaxWidth(123), GUILayout.MaxHeight(18)))
				{
					valueRef_objListValue.DeleteArrayElementAtIndex(valueRef_objListValue.arraySize - 1);
				}
				EditorGUILayout.EndHorizontal();

				//列表折叠状态检查
				string foldoutKey = _valueRef.propertyPath;
				if (!_m_listFoldoutMap.ContainsKey(foldoutKey))
					_m_listFoldoutMap[foldoutKey] = true;

				if (_m_listFoldoutMap[foldoutKey])
				{
					//约束的泛型参数
					Type genericArgumentsType =  _curType.GetGenericArguments()[0];
					//判断列表元素是否为可序列化类，需要额外分隔
					bool isSerializableItem = MonoSkinHelper.isSerializableClass(genericArgumentsType);
					EditorGUI.indentLevel++;
					for (int a = 0; a < valueRef_objListValue.arraySize; a++)
					{
						SerializedProperty itemSerializedProperty = valueRef_objListValue.GetArrayElementAtIndex(a);
						
						//可序列化类item之间绘制分隔线和索引标签
						if (isSerializableItem)
						{
							EditorGUILayout.LabelField($"  [{a}]", EditorStyles.miniLabel);
						}

						EditorGUILayout.BeginHorizontal();
						//绘制list里面每一个item
						_drawFieldItem(genericArgumentsType, itemSerializedProperty);
						EditorGUILayout.EndHorizontal();

						//可序列化类item末尾绘制分隔线
						if (isSerializableItem && a < valueRef_objListValue.arraySize - 1)
						{
							EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
						}
					}
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.BeginHorizontal();
			}
			//可序列化类类型处理(带有[System.Serializable]且不是UnityEngine.Object的类)
			else if (MonoSkinHelper.isSerializableClass(_curType))
			{
				_drawSerializableClassField(_curType, _valueRef);
			}
			//找不到就当作默认引用类型
			else
			{
				SerializedProperty valueRef_objValue = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objValue);
				valueRef_objValue.objectReferenceValue = EditorGUILayout.ObjectField("", valueRef_objValue.objectReferenceValue, _curType, true);
			}
		}

		/// <summary>
		/// 根据程序集名称获取类型
		/// </summary>
		/// <param name="assemblyName">程序集名称</param>
		/// <param name="typeName">类型名称</param>
		/// <returns></returns>
		private Type _getTypeFromAssembly(string assemblyName, string typeName)
		{
			if (string.IsNullOrEmpty(assemblyName))
				return _getType(typeName);

			Assembly assembly = Assembly.Load(assemblyName);
			if (assembly != null)
			{
				return assembly.GetType(typeName);
			}
			//找不到全局遍历unity进程appdomain下的所有Assembly，比较耗时
			return _getType(typeName);
		}
		
		/// <summary>
		/// 根据名称获取Type
		/// 全局遍历unity进程appdomain下的所有Assembly的方式寻找
		/// </summary>
		/// <param name="typeName">类型名称</param>
		/// <returns></returns>
		private Type _getType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
				return typeof(GameObject);

			Type type = null;
			Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
			int assemblyArrayLength = assemblyArray.Length;
			for (int i = 0; i < assemblyArrayLength; ++i)
			{
				type = assemblyArray[i].GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}

			for (int i = 0; (i < assemblyArrayLength); ++i)
			{
				Type[] typeArray = assemblyArray[i].GetTypes();
				int typeArrayLength = typeArray.Length;
				for (int j = 0; j < typeArrayLength; ++j)
				{
					if (typeArray[j].Name.Equals(typeName))
					{
						return typeArray[j];
					}
				}
			}

			return type == null ? typeof(GameObject) : type;
		}

		/// <summary>
		/// 绘制可序列化类的子字段
		/// </summary>
		private void _drawSerializableClassField(Type _curType, SerializedProperty _valueRef)
		{
			FieldInfo[] serFields = MonoSkinHelper.getSerializableFields(_curType);
			if (null == serFields || serFields.Length == 0)
				return;

			SerializedProperty fieldNamesRef = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_serializedClassFieldNames);
			SerializedProperty subValuesRef = _valueRef.FindPropertyRelative(MonoSkinConst.skinPropertyValue_objListValue);

			//确保数组大小与字段数量一致
			while (fieldNamesRef.arraySize < serFields.Length)
				fieldNamesRef.InsertArrayElementAtIndex(fieldNamesRef.arraySize);
			while (fieldNamesRef.arraySize > serFields.Length)
				fieldNamesRef.DeleteArrayElementAtIndex(fieldNamesRef.arraySize - 1);
			while (subValuesRef.arraySize < serFields.Length)
				subValuesRef.InsertArrayElementAtIndex(subValuesRef.arraySize);
			while (subValuesRef.arraySize > serFields.Length)
				subValuesRef.DeleteArrayElementAtIndex(subValuesRef.arraySize - 1);

			//结束当前行，准备绘制子字段
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;
			for (int i = 0; i < serFields.Length; i++)
			{
				FieldInfo subField = serFields[i];
				fieldNamesRef.GetArrayElementAtIndex(i).stringValue = subField.Name;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("  " + subField.Name, GUILayout.MaxWidth(150));

				SerializedProperty subValueRef = subValuesRef.GetArrayElementAtIndex(i);
				_drawFieldItem(subField.FieldType, subValueRef);

				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;

			//重新开始一行（和调用者的EndHorizontal配对）
			EditorGUILayout.BeginHorizontal();
		}
#endif
	}
}
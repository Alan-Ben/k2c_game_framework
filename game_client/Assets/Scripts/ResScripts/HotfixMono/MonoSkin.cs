using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace GOE
{
    //属性名字常量
    public class MonoSkinConst
    {
        //--------------------monoskin数据字段------------------------
        public const string monoSkin_exportClassName = "exportClassName";//导出的代码类名
        public const string monoSkin_propList = "propList";//保存的属性列表
        
        //--------------------monoskin存储的每个字段的信息------------------------
        public const string singleProperty_propType = "propType";//字段类型
        public const string singleProperty_propName = "propName";//字段名
        public const string singleProperty_propVal = "propVal";//字段的值
        public const string singleProperty_propDesc = "propDesc";//字段的注释
        public const string singleProperty_propTypeName = "propTypeName";//字段类型完整
        public const string singleProperty_propTypeAssembly = "propTypeAssembly";//字段类型所属的程序集

        //--------------------monoskin存储的每个字段里面的值的信息------------------------
        public const string skinPropertyValue_objValue = "objValue";
        public const string skinPropertyValue_objListValue = "objListValue";
        public const string skinPropertyValue_intValue = "intValue";
        public const string skinPropertyValue_longValue = "longValue";
        public const string skinPropertyValue_floatValue = "floatValue";
        public const string skinPropertyValue_boolValue = "boolValue";
        public const string skinPropertyValue_strValue = "strValue";
        public const string skinPropertyValue_colorValue = "colorValue";
        public const string skinPropertyValue_vector3Value = "vector3Value";
        public const string skinPropertyValue_serializedClassFieldNames = "serializedClassFieldNames";
    }

    /// <summary>
    /// MonoSkin共用工具方法，Editor和Runtime均可使用
    /// 修改可序列化类的判断逻辑或字段获取逻辑时，只需修改此处
    /// </summary>
    public static class MonoSkinHelper
    {
        /// <summary>
        /// 判断是否为可序列化类(非UnityEngine.Object、非基础类型)
        /// </summary>
        public static bool isSerializableClass(Type _type)
        {
            if (null == _type)
                return false;
            if (!Attribute.IsDefined(_type, typeof(System.SerializableAttribute), true))
                return false;
            if (typeof(Object).IsAssignableFrom(_type))
                return false;
            if (_type.IsPrimitive || _type == typeof(string))
                return false;
            if (_type == typeof(Color) || _type == typeof(Vector3))
                return false;
            if (_type.IsEnum)
                return false;
            return true;
        }

        /// <summary>
        /// 获取类型的所有可序列化字段(包括父类中的public和[SerializeField]标记的字段)
        /// 从基类到子类顺序返回
        /// </summary>
        public static FieldInfo[] getSerializableFields(Type _type)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            //收集类型继承链
            List<Type> hierarchy = new List<Type>();
            Type currentType = _type;
            while (null != currentType && currentType != typeof(object))
            {
                hierarchy.Add(currentType);
                currentType = currentType.BaseType;
            }
            //从基类到子类遍历，保证基类字段在前
            for (int i = hierarchy.Count - 1; i >= 0; i--)
            {
                FieldInfo[] fields = hierarchy[i].GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                foreach (FieldInfo field in fields)
                {
                    //包含public字段和带[SerializeField]的私有字段
                    if (field.IsPublic || field.IsDefined(typeof(SerializeField), false))
                    {
                        result.Add(field);
                    }
                }
            }
            return result.ToArray();
        }
    }

    /// <summary>
    /// monoSKin的mono
    /// </summary>
    public class MonoSkin : MonoBehaviour
    {
        [HideInInspector]
        public string exportClassName;//导出的代码类名
        [HideInInspector]
        public List<SingleProperty> propList;//保存的属性列表


        /// <summary>
        /// 根据字段名获取单个属性
        /// </summary>
        public T getProperty<T>(string propName)
        {
            if (null == propList)
                return default;
            
            foreach (SingleProperty singleProperty in propList)
            {
                if(null != singleProperty && singleProperty.propName == propName)
                {
                    return _getSingleSkinPropertyValue<T>(singleProperty.propVal);
                }
            }

            Debug.LogError_EditorOnly($"{gameObject.name} UI没有配置 {propName}", gameObject);
            return default;
        }
        
        /// <summary>
        /// 根据字段名获取单个属性
        /// </summary>
        public object getProperty(Type _type, string propName)
        {
            if (null == propList)
                return null;
            
            foreach (SingleProperty singleProperty in propList)
            {
                if(null != singleProperty && singleProperty.propName == propName)
                {
                    return _getSingleSkinPropertyValue(_type, singleProperty.propVal);
                }
            }

            Debug.LogError_EditorOnly($"{gameObject.name} UI没有配置 {propName}", gameObject);
            return default;
        }
        
        /// <summary>
        /// 根据泛型约束跟属性值返回对应具体值
        /// </summary>
        protected T _getSingleSkinPropertyValue<T>(SkinPropertyValue _skinPropertyValue)
        {
            if (null == _skinPropertyValue)
                return default;
            Type t = typeof(T);
            return (T)_getSingleSkinPropertyValue(t, _skinPropertyValue);
        }

        /// <summary>
        /// 根据type跟属性值返回对应具体值
        /// </summary>
        protected object _getSingleSkinPropertyValue(Type _type, SkinPropertyValue _skinPropertyValue)
        {
            if (null == _type || null == _skinPropertyValue)
                return null;
            
            //每个类型对应解析
            if (_type == typeof(int))
            {
                return Convert.ChangeType(_skinPropertyValue.intValue, _type);
            }
            else if (_type == typeof(byte))
            {
                return (byte)_skinPropertyValue.intValue;
            }
            else if (_type == typeof(long))
            {
                return Convert.ChangeType(_skinPropertyValue.longValue, _type);
            }
            else if (_type == typeof(float))
            {
                return Convert.ChangeType(_skinPropertyValue.floatValue, _type);
            }
            else if (_type == typeof(bool))
            {
                return Convert.ChangeType(_skinPropertyValue.boolValue, _type);
            }
            else if (_type == typeof(string))
            {
                return Convert.ChangeType(_skinPropertyValue.strValue, _type);
            }
            else if (_type == typeof(Color))
            {
                return Convert.ChangeType(_skinPropertyValue.colorValue, _type);
            }
            else if (_type == typeof(Vector3))
            {
                return Convert.ChangeType(_skinPropertyValue.vector3Value, _type);
            }
            //列表类型特殊处理
            else if (_type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(List<>))
            {
                if(null == _skinPropertyValue.objListValue || _skinPropertyValue.objListValue.Length == 0)
                    return null;
                
                //约束的泛型参数
                Type genericArgumentsType = _type.GetGenericArguments()[0];
                //反射构造对应list，后续这边如果性能有问题，可以改根据genericArgumentsType类型写死约束的List
                IList resultList = (IList)Activator.CreateInstance(_type);
                if (null == resultList)
                    return null;
                
                //循环解析每个item，放入对应List
                foreach (SkinPropertyValue itemPropertyValue in _skinPropertyValue.objListValue)
                {
                    object listObj = _getSingleSkinPropertyValue(genericArgumentsType, itemPropertyValue);
                    resultList.Add(listObj);
                }
                return resultList;
            }
            //可序列化类处理(带有[System.Serializable]且不是UnityEngine.Object的类)
            else if (MonoSkinHelper.isSerializableClass(_type))
            {
                return _deserializeSerializableClass(_type, _skinPropertyValue);
            }
            //其他都当作通用引用类型
            else
            {
                if (null == _skinPropertyValue.objValue)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return Convert.ChangeType(_skinPropertyValue.objValue, _type);
                    }
                    catch (Exception e)
                    {
#if UNITY_EDITOR
                        //TODO 这边有一种情况，mono写的是基类，资源上是子类，比如资源是TextEx 但是mono写的是Text，用上面的api是会报错的，但是不影响使用
                        //TODO 也不能直接返回，不然泛型获取方式有问题，可能可以再找下可以兼容的API
                        Debug.LogWarning($"类型转换可能错误{_skinPropertyValue.objValue} ===》 {_type}", this);
#endif
                        return _skinPropertyValue.objValue;
                    }
                }
            }
        }

        #region 可序列化类支持

        /// <summary>
        /// 反序列化可序列化类
        /// </summary>
        private object _deserializeSerializableClass(Type _type, SkinPropertyValue _skinPropertyValue)
        {
            if (null == _skinPropertyValue.serializedClassFieldNames || null == _skinPropertyValue.objListValue)
                return null;
            if (_skinPropertyValue.serializedClassFieldNames.Length == 0 || _skinPropertyValue.objListValue.Length == 0)
                return null;

            object instance = null;
            try
            {
                instance = Activator.CreateInstance(_type);
            }
            catch (Exception e)
            {
                Debug.LogError_EditorOnly($"无法创建可序列化类实例: {_type.Name}, {e.Message}");
                return null;
            }

            if (null == instance)
                return null;

            int count = Mathf.Min(_skinPropertyValue.serializedClassFieldNames.Length, _skinPropertyValue.objListValue.Length);
            for (int i = 0; i < count; i++)
            {
                string fieldName = _skinPropertyValue.serializedClassFieldNames[i];
                if (string.IsNullOrEmpty(fieldName))
                    continue;

                FieldInfo fieldInfo = _findFieldInHierarchy(_type, fieldName);
                if (null == fieldInfo)
                    continue;

                object fieldValue = _getSingleSkinPropertyValue(fieldInfo.FieldType, _skinPropertyValue.objListValue[i]);
                if (null != fieldValue)
                {
                    fieldInfo.SetValue(instance, fieldValue);
                }
            }

            return instance;
        }

        /// <summary>
        /// 在类型继承链中查找字段(支持private和public)
        /// </summary>
        private static FieldInfo _findFieldInHierarchy(Type _type, string _fieldName)
        {
            Type current = _type;
            while (null != current && current != typeof(object))
            {
                FieldInfo field = current.GetField(_fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                if (null != field)
                    return field;
                current = current.BaseType;
            }
            return null;
        }

        #endregion

        #region 内部属性类

        /// <summary>
        /// 每个属性保存的内容
        /// </summary>
        [System.Serializable]
        public class SingleProperty
        {
            //字段类型
            public string propType;
            //字段名
            public string propName;
            //字段的值
            public SkinPropertyValue propVal;
            //字段的注释
            public string propDesc;
            //字段类型完整
            public string propTypeName;
            //字段类型所属的程序集
            public string propTypeAssembly;
        }

        
        /// <summary>
        /// 每个属性值保存的内容
        /// 有新增要改monoSkin跟monoSkinEditor两个地方的解析函数
        /// </summary>
        [System.Serializable]
        public class SkinPropertyValue
        {
            //引用类型的值
            public Object objValue;
            //List类型的值
            public SkinPropertyValue[] objListValue;
            //int类型的值
            public int intValue;
            //long类型的值
            public long longValue;
            //float类型的值
            public float floatValue;
            //bool类型的值
            public bool boolValue;
            //str类型的值
            public string strValue;
            //color类型的值
            public Color colorValue;
            //Vector3类型的值
            public Vector3 vector3Value;
            //可序列化类的子字段名列表(用于映射objListValue中的值到对应字段)
            public string[] serializedClassFieldNames;
        }

        #endregion
    }


}
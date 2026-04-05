using System;
using System.Collections.Generic;
using System.Globalization;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 热更配表的统一解析函数，数据不可信赖
    /// 对比GOK做法为了避免每个类自己存一份Dictionary，减少内存
    /// </summary>
    public class HotfixRefCommonParseDealer : HotfixCacheClass<HotfixRefCommonParseDealer>
    {

        //内容标记，表面当前解析的是什么
        private string _m_contextTag;
        //数据键值对
        [NotNull]private Dictionary<string, string> _m_keyValueDict = new Dictionary<string, string>();

        //设置内容标记，解析报错时候可以用，没实际意义
        public void setTag(string _tag)
        {
            _m_contextTag = _tag;
        }

        /// <summary>
        /// 被 cache 回收时调用
        /// </summary>
        public override void reset()
        {
            _m_contextTag = string.Empty;
            
            _m_keyValueDict.Clear();
        }

        /// <summary>
        /// 被 cache 销毁时调用
        /// </summary>
        public override void discard()
        {
            _m_contextTag = null;
            
            _m_keyValueDict.Clear();
        }

        //增加一个数据
        public void addKeyValue(string _key, string _value)
        {
            _m_keyValueDict.Add(_key, _value);
        }
        
        
#region getXXX
        public string getString(string _name, bool _canNull = true)
        {
            if (!_m_keyValueDict.TryGetValue(_name, out string result))
            {
                if(_canNull == false)
                {
                    Debug.LogError($"{_m_contextTag}\t字段不存在{_name}");
                }
            }
            return result;
        }


        public string getString(string _name, out bool _isExist, bool _canNull = true)
        {
            if (!_m_keyValueDict.TryGetValue(_name, out string result))
            {
                if(_canNull == false)
                {
                    Debug.LogError($"{_m_contextTag}\t字段不存在{_name}");
                }
                _isExist = false;
            }
            else
            {
                _isExist = true;
            }
            return result;
        }

        public int getInt(string _name, bool _canNull = true)
        {
            bool isExistField;
            string tempValue = getString(_name, out isExistField, _canNull);
            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canNull && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{_name}为空");
                }
                return 0;
            }
            if(!int.TryParse(tempValue, out int result))
            {
                Debug.LogError($"表\"{GetType().FullName}\"：{_m_contextTag}\t数据填写错误: {_name},填的{tempValue}不是int");
            }
            return result;
        }

        public long getLong(string _name, bool _canNull = true)
        {
            bool isExistField;
            string tempValue = getString(_name, out isExistField, _canNull);
            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canNull && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{_name}为空");
                }
                return 0;
            }
            if(!long.TryParse(tempValue, out long result))
            {
                Debug.LogError($"表\"{GetType().FullName}\"：{_m_contextTag}\t数据填写错误: {_name},填的{tempValue}不是long");
            }
            return result;
        }

        public bool getBool(string _name, bool _canNull = true)
        {
            bool isExistField;
            string tempValue = getString(_name, out isExistField, _canNull);
            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canNull && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{_name}为空");
                }
                return false;
            }
            if(!bool.TryParse(tempValue, out bool result))
            {
                Debug.LogError($"表\"{GetType().FullName}\"：{_m_contextTag}\t数据填写错误: {_name},填的{tempValue}不是bool");
            }
            return result;
        }

        public float getFloat(string _name, bool _canNull = true)
        {
            bool isExistField;
            string tempValue = getString(_name, out isExistField, _canNull);
            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canNull && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{_name}为空");
                }
                return 0;
            }
            if(!float.TryParse(tempValue, NumberStyles.Number, CultureInfo.InvariantCulture, out float result))
            {
                Debug.LogError($"表\"{GetType().FullName}\"：{_m_contextTag}\t数据填写错误: {_name},填的{tempValue}不是float");
            }
            return result;
        }

        public List<T> getList<T>(string _name, bool _canNull = true)
        {
            List<T> list = new List<T>();
            bool isExistField;
            string tempValue = getString(_name, out isExistField, _canNull);
            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canNull && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{_name}为空");
                }
                return list;
            }
            string[] strs = tempValue.Split(new char[] { ';', ':' });
            for (var i = 0; i < strs.Length; i++)
            {
                string tempStr = strs[i];
                object value = ParseValue(tempStr, typeof(T));
                if (value == null)
                {
                    Debug.LogError($"表\"{GetType().FullName}\"：{_name}的{_m_contextTag}\t数据填写错误: {tempStr}");
                }
                else
                {
                    list.Add((T) value);
                }
            }


            return list;
        }

        
        public TEnum getEnum<TEnum> (string key, bool _canEmpty = true)
        {
            string strValue = "";
            if (!_m_keyValueDict.TryGetValue(key.ToLowerInvariant(), out strValue)) {
                Debug.LogError(string.Format("表\"{0}\"：{1}列数据填写错误: {3}", GetType().FullName, key, strValue));
                return (TEnum) Activator.CreateInstance(typeof(TEnum));
            }

            if (strValue == string.Empty || strValue == "")
            {
                if (!_canEmpty)
                    Debug.LogError(string.Format("表\"{0}\"：{1}列数据填写错误: {3}", GetType().FullName, key, strValue));
                return (TEnum) Activator.CreateInstance(typeof(TEnum));
            }

            try
            {
                TEnum result = (TEnum) Enum.Parse(typeof(TEnum), strValue.Trim());
                if (result.ToString() == "-1")
                {
                    Debug.LogError($"表{GetType().FullName}：{key}列数据: {strValue}填写错误, 无法解析出正确的{typeof(TEnum)}枚举");
                    return (TEnum) Activator.CreateInstance(typeof(TEnum));
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                Debug.LogError(string.Format("表\"{0}\"：{1}列数据填写错误: {3}", GetType().FullName, key, strValue));
                return (TEnum) Activator.CreateInstance(typeof(TEnum));
            }
        }
        
        public Vector2 getVector2(string _name, bool _canNull = true)
        {
            Vector2 v2 = new Vector2();
            string tempValue = getString(_name);
            if (string.IsNullOrEmpty(tempValue))
            {
                if (!_canNull)
                    Debug.LogError($"{_m_contextTag}的字段{_name}为空");
                return v2;
            }

            string[] strs = tempValue.Split(new char[] { ';', ':' });
            if (strs.Length < 2)
            {
                Debug.LogError($"表\"{_m_contextTag}\"\t数据填写错误: {_name},填的{tempValue}不是Vector2");
                return v2;
            }
            v2.Set(ALCommon.ParseFloat(strs[0]), ALCommon.ParseFloat(strs[1]));
            return v2;
        }
        
        public Vector2Int getVector2Int(string _name, bool _canNull = true)
        {
            Vector2Int v2 = new Vector2Int();
            string tempValue = getString(_name);
            if (string.IsNullOrEmpty(tempValue))
            {
                if (!_canNull)
                    Debug.LogError($"{_m_contextTag}的字段{_name}为空");
                return v2;
            }

            string[] strs = tempValue.Split(new char[] { ';', ':' });
            if (strs.Length < 2)
            {
                Debug.LogError($"表\"{_m_contextTag}\"\t数据填写错误: {_name},填的{tempValue}不是Vector2");
                return v2;
            }
            v2.Set(ALCommon.ParseInt(strs[0]), ALCommon.ParseInt(strs[1]));
            return v2;
        }
        
        public Vector3 getVector3(string key, bool _canEmpty = true)
        {
            Vector3 v3 = new Vector3();
            bool isExistField;
            string tempValue = getString(key, out isExistField, _canEmpty);

            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canEmpty && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{key}为空");
                }
                return v3;
            }
            
            if (!tempValue.Trim().Equals(""))
            {
                string[] strs = tempValue.Split(new char[] { ';', ':' });
                if (strs.Length < 3)
                {
                    Debug.LogError(string.Format("表\"{0}\"：第{1}行第{2}列数据填写错误，格式 [x;y:z] : {3}", GetType().FullName, key, tempValue));
                    return v3;
                }

                v3.Set(ALCommon.ParseFloat(strs[0]), ALCommon.ParseFloat(strs[1]), ALCommon.ParseFloat(strs[2]));
            }
            else
            {
                if (!_canEmpty)
                    Debug.LogError(string.Format("表\"{0}\"：第{1}行第{2}列数据填写错误: {3},  不可为空！", GetType().FullName, key, tempValue));
            }
            return v3;
        }
        
        public Vector3Int getVector3Int(string key, bool _canEmpty = true)
        {
            Vector3Int v3 = new Vector3Int();
            bool isExistField;
            string tempValue = getString(key, out isExistField, _canEmpty);

            if(string.IsNullOrEmpty(tempValue))
            {
                if(!_canEmpty && isExistField)
                {
                    Debug.LogError($"{_m_contextTag}\t字段{key}为空");
                }
                return v3;
            }
            
            if (!tempValue.Trim().Equals(""))
            {
                string[] strs = tempValue.Split(new char[] { ';', ':' });
                if (strs.Length < 3)
                {
                    Debug.LogError(string.Format("表\"{0}\"：第{1}行第{2}列数据填写错误，格式 [x;y:z] : {3}", GetType().FullName, key, tempValue));
                    return v3;
                }

                v3.Set(ALCommon.ParseInt(strs[0]), ALCommon.ParseInt(strs[1]), ALCommon.ParseInt(strs[2]));
            }
            else
            {
                if (!_canEmpty)
                    Debug.LogError(string.Format("表\"{0}\"：第{1}行第{2}列数据填写错误: {3},  不可为空！", GetType().FullName, key, tempValue));
            }
            return v3;
        }
        
        public Rect getRect(string _name, bool _canNull = true)
        {
            Rect rect = new Rect();
            string tempValue = getString(_name);
            if (string.IsNullOrEmpty(tempValue))
            {
                if (!_canNull)
                    Debug.LogError($"{_m_contextTag}的字段{_name}为空");
                return rect;
            }

            string[] strs = tempValue.Split(new char[] { ';', ':' });
            if (strs.Length < 4)
            {
                Debug.LogError($"表\"{_m_contextTag}\"\t数据填写错误: {_name},填的{tempValue}不是rect");
                return rect;
            }

            rect.Set(ALCommon.ParseFloat(strs[0]), ALCommon.ParseFloat(strs[1]), ALCommon.ParseFloat(strs[2]), ALCommon.ParseFloat(strs[3]));
            return rect;
        }

        public Color getColor(string _name, bool _canEmpty = true)
        {
            string tempValue = getString(_name);
            if (string.IsNullOrEmpty(tempValue))
            {
                if (!_canEmpty)
                    Debug.LogError($"{_m_contextTag}的字段{_name}为空");
                return Color.white;
            }
            
            byte br = byte.Parse(tempValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(tempValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(tempValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte cc = byte.Parse(tempValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            
            return new Color(r, g, b, a);
        }
        
        // 解析字段值
        public static object ParseValue(string _value, Type _type)
        {
            try
            {
                if(_value.Equals(string.Empty))
                {
                    if(_type == typeof(string))
                    {
                        return "";
                    }
                    return Activator.CreateInstance(_type, true);
                }
                else
                {
                    _value = _value.Trim();

                    // 枚举 暂不支持
                    if(_type.IsEnum)
                    {
                        Debug.LogError("热更工程里不能直接解析枚举");
                        return null;
                    }

                    // 字符串
                    else if(_type == typeof(string))
                    {
                        return _value;
                    }

                    // 浮点型
                    else if(_type == typeof(float))
                    {
                        if(_value == "0" || _value == "" || _value == string.Empty)
                            return 0f;

                        return float.Parse(_value, CultureInfo.InvariantCulture);
                    }

                    // 整形
                    else if(_type == typeof(int))
                    {
                        if(_value == "")
                            return 0;

                        return int.Parse(_value);
                    }

                    else if(_type == typeof(bool))
                    {
                        return bool.Parse(_value);
                    }

                    else if(_type == typeof(long))
                    {
                        return long.Parse(_value);
                    }
                    else if (_type == typeof(NPGGoIndex))
                    {
                        NPGGoIndex goIndex = new NPGGoIndex();
                        goIndex.readIndex(_value);
                        return goIndex;
                    }
                }
            }
            catch(System.Exception ex)
            {
                Debug.LogError($"ParseValue type:{_type.ToString()}, value:{_value}, failed: {ex.ToString_ILRuntime()}");
            }
            return null;
        }

#endregion
    }
}
/****************************************************************************
 * Copyright (c) 2017 ~ 2018.8 liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Hotfix
{
    public static class ObjectExtension
    {
        public static string ToStringByReflection(this object _obj)
        {
            if(_obj == null)
            {
                return "null";
            }
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = _obj.GetType().GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                System.Reflection.PropertyInfo property = properties[i];
                sb.Append(property.Name);
                sb.Append(": ");
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(_obj, null));
                }

                sb.Append(";");
            }
            return sb.ToString();
        }
    }
    public static class ExtensionExtension
    {
        public static string ToString_ILRuntime(this Exception _exception)
        {
            return $"{_exception.GetType()}:{_exception.Message}\n{_exception.Data["StackTrace"]}";
        }
    }

    public static class DictionaryExtension
    {
        public static V hotfixTryGetValue<K, V>(this IDictionary<K, V> _dict, K _key)
        {
            V value = default(V);
            if (_dict == null)
                return value;

            if (_key == null)
                return value;

            _dict.TryGetValue(_key, out value);
            return value;
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// 拼接翻译key
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_activityId"></param>
        /// <returns></returns>
        public static string replaceActivity(this string _str, long _activityId)
        {
            if (string.IsNullOrEmpty(_str))
                return _str;

            return string.Format(_str, _activityId);
        }

        /// <summary>
        /// 拼接红点id
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_activity"></param>
        /// <returns></returns>
        public static long replaceActivity(this long _num, long _activity)
        {
            return (1 * (long)Math.Pow(10, _activity.ToString().Length) + _activity) * 100 + _num;
        }
    }
}
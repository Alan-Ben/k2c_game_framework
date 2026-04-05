using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 枚举工具库
    /// </summary>
    public class MJSDK_EnumUtil
    {
        /// <summary>
        /// 拓展方法: 获取枚举利用[EnumBindString("")]对应所绑定的值
        /// </summary>
        /// <returns>返回所绑定的值（未绑定则返回对应的枚举名称）</returns>
        public static string GetBindString(Enum en)
        {
            try
            {
                var attr = System.Attribute.GetCustomAttribute(en.GetType().GetField(en.ToString()), typeof(EnumBindString));
                if (attr is EnumBindString)
                {
                    var strValue = attr as EnumBindString;
                    return strValue.GetStringValue();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("获取枚举绑定值发生错误->" + ex.Message);
            }
            return en.ToString();
        }
    }

    /// <summary>
    /// 自定义属性，用于给枚举值设置自定义字符串
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumBindString : System.Attribute
    {
        readonly string name;
        public EnumBindString(string value)
        {
            this.name = value;
        }

        public string GetStringValue()
        {
            return name;
        }
    }
}

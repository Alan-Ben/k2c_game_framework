using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 梦加工具库(针对梦加业务相关通用工具)
    /// </summary>
    public class MJSDK_MJUtil 
    {
        /// <summary>
        /// 转换成梦加通用语言类型
        /// </summary>
        /// <param name="_lan"></param>
        /// <returns></returns>
        public static string convertMJLanType(string _lan)
        {
            //如果为空的话直接返回空值，并报错
            if (string.IsNullOrEmpty(_lan))
            {
                MJSDK_Log.mjsdkLog(" convertMJLanType err,reason:_lan is null", E_MJSDK_BusType.Error);
                return "";
            }

            string newLan = _lan;

            //MJSDK zh_CN/zh-Hans转换成标准语言zh-CN（简体中文）
            if (_lan.Equals("zh_CN") || _lan.Equals("zh-Hans"))
            {
                newLan = "zh-CN";
            }
            //MJSDK zh_TW/zh_HK/zh-Hant转换成标准语言zh-TW（繁体中文）
            if (_lan.Equals("zh_TW") || _lan.Equals("zh_HK") || _lan.Equals("zh-Hant"))
            {
                newLan = "zh-TW";
            }
            return newLan;
        }
    }
}

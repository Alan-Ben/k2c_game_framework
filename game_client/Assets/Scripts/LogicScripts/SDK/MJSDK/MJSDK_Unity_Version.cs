using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    //MJSDK_Unity大版本号
    public class MJSDK_Unity_Version
    {
        /// <summary>
        /// 打印主版本号信息
        /// </summary>
        public static string printMJSDKUnityVersion()
        {
            string version_str = "0.7.3.0_2025090201";
            Debug.Log("【MJSDK_Unity_Version】MJSDK_Unity_Version：" + version_str);
            return version_str;
        }
    }
}

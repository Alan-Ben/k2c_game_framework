using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 自动导出的时候根据属性判断对应值是否可为空
    /// </summary>
    public class ALVersion
    {
        public static int majorVersion = 1;
        public static int minorVersion = 4;
        public static int buildVersion = 3;
        public static int fixVersion = 0;

        //输出版本号
        public static void printVersion()
        {
            
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("<color=green>【提示信息】</color>AL Package Version: " + majorVersion + "." + minorVersion + "." + buildVersion + "." + fixVersion);
#endif
            
        }
    }
}

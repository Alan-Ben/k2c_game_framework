using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{

    public enum ALLogLevel
    {
        VERBOSE = 10,
        DEBUG = 20,
        SYS_OUT = 30,
        WARNING = 40,
        ERROR = 50,
        CRUSH = 60,
    }

    public class ALLog
    {
        private static ALLog g_instance = new ALLog();

        public static ALLog Instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = new ALLog();

                return g_instance;
            }
        }

        private void showLog(ALLogLevel _logLvl, string _str)
        {
            if (_logLvl >= ALSOGlobalSetting.Instance.logLevel)
            {
                if (_logLvl == ALLogLevel.CRUSH || _logLvl == ALLogLevel.ERROR)
                    //输出到异常信息输出窗口
                    UnityEngine.Debug.LogError("ALLog - [ " + _logLvl.ToString() + " ] " + _str);
                else if (_logLvl == ALLogLevel.WARNING)
                    UnityEngine.Debug.LogWarning("ALLog - [ " + _logLvl.ToString() + " ] " + _str);
                else
                    UnityEngine.Debug.Log("ALLog - [ " + _logLvl.ToString() + " ] " + _str);
            }
        }

        public static void Verbose(string _str)
        {
            Instance.showLog(ALLogLevel.VERBOSE, _str);
        }
        
        public static void Debug(string _str)
        {
            Instance.showLog(ALLogLevel.DEBUG, _str);
        }

        public static void Warning(string _str)
        {
            Instance.showLog(ALLogLevel.WARNING, _str);
        }

        public static void Error(string _str)
        {
            Instance.showLog(ALLogLevel.ERROR, _str);
        }

        public static void Crush(string _str)
        {
            Instance.showLog(ALLogLevel.CRUSH, _str);
        }

        public static void Sys(string _str)
        {
            Instance.showLog(ALLogLevel.SYS_OUT, _str);
        }
    }
}

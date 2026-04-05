using System;
using ALPackage;
using GOE;

namespace GOESDK
{
    /// <summary>
    /// SDK工具类
    /// </summary>
    public static class SDKUtil
    {
        //打印日志
        public static void showSDKDebugLog(params object[] _message)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                string str = "";
                if (_message != null && _message.Length > 0)
                {
                    for (int i = 0; i < _message.Length; i++)
                    {
                        str += _message[i];
                    }
                }
                UnityEngine.Debug.Log($"【MJSDK】======>{str}");
            }
        }

        //打印错误日志
        public static void showSDKLogError(string _str)
        {
            UnityEngine.Debug.LogError($"【MJSDK】======>{_str}");
        }

        //打印错误日志
        public static void showSDKLogError(string _tag, int _code, string _msg)
        {
            UnityEngine.Debug.LogError($"【MJSDK】======>[{_tag}] 发生错误 code:{_code},msg:{_msg}");
        }

        /// <summary>
        /// 检查是否使用SDK，是否初始化SDK
        /// </summary>
        /// <returns></returns>
        public static bool checkSDK(string _tag, bool _isInitLib, Action<int, string> _failDelegate)
        {
            if (!SDKMgr.instance.isUseSDK)
            {
                showSDKDebugLog("[", _tag, "]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"{_tag} 未使用SDK");
                return false;
            }

            if (!SDKMgr.instance.isInit)
            {
                showSDKLogError($"[{_tag}] SDK未初始化");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"{_tag} SDK未初始化");
                return false;
            }

            if (!_isInitLib)
            {
                showSDKDebugLog("[", _tag, "]", " 未添加对应组件或组件未初始化成功");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"{_tag} 未添加对应组件或组件未初始化成功");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否使用SDK，是否初始化SDK
        /// </summary>
        /// <returns></returns>
        public static bool checkSDK(string _tag, Action<int, string> _failDelegate)
        {
            if (!SDKMgr.instance.isUseSDK)
            {
                showSDKDebugLog("[", _tag, "]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"{_tag} 未使用SDK");
                return false;
            }

            if (!SDKMgr.instance.isInit)
            {
                showSDKLogError($"[{_tag}] SDK未初始化");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"{_tag} SDK未初始化");
                return false;
            }

            return true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 方法相关工具库
    /// </summary>
    public class MJSDK_MethodUtil
    {
        /// <summary>
        /// 反射调用MJSDK接口
        /// </summary>
        /// <param name="_className">类名</param>
        /// <param name="_methodName">方法名</param>
        /// <param name="_sucAction">成功回执</param>
        /// <param name="_failAction">失败回执</param>
        /// <param name="_arg">参数</param>
        public static void Invoke_MJSDKMethod(
            string _className,
            string _methodName,
            Action<object> _sucAction,
            Action<int, string> _failAction,
            object _arg = null)
        {
            try
            {
                //通过string类型的strClass获得同名类“t”
                Type class_type = Type.GetType(MJSDK_Mgr_Mark.MJSDK_Namespace + _className);

                if (class_type == null)
                {
                    _failAction(MJSDK_Error.C_Unity_Exception_Err, "Invoke_MJSDKMethod调用失败。 库未找到:" + _className + "组件");
                    return;
                }

                //通过string类型的strMethod获得同名的方法“method”
                MethodInfo pro_funtion = class_type.GetMethod(_methodName);
                if (pro_funtion == null)
                {
                    _failAction(MJSDK_Error.C_Unity_Exception_Err, "Invoke_MJSDKMethod调用失败。 在" + _className + "组件库，未找到" + _methodName + "协议");
                    return;
                }
                //方法参数
                object[] objParams;

                //是否有参数
                if (_arg == null)
                {
                    objParams = new object[2];
                    //成功回执
                    objParams[0] = _sucAction;
                    //失败回执
                    objParams[1] = _failAction;
                }
                else
                {
                    objParams = new object[3];
                    //默认传入原参数
                    objParams[0] = _arg;
                    //成功回执
                    objParams[1] = _sucAction;
                    //失败回执
                    objParams[2] = _failAction;
                }

                //方法执行
                pro_funtion.Invoke(class_type, objParams);
            }
            catch (Exception ex)
            {
                if (_failAction != null)
                {
                    _failAction(MJSDK_Error.C_Unity_Exception_Err, "Invoke_MJSDKMethod调用异常。调用【className】:" + _className + "【methodName】:" + _methodName + " 【error】:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 反射调用MJSDK接口
        /// </summary>
        /// <param name="_className">包体空间.类名（namespace.class）</param>
        /// <param name="_methodName">方法名（namespace.class.fun）</param>
        /// <param name="objParams">参数（参数）</param>
        /// <param name="_failAction">失败</param>
        public static void Invoke_MJSDKMethod(
            string _className,
            string _methodName,
            object[] objParams,
            Action<string> _failAction = null)
        {
            try
            {
                //通过string类型的strClass获得同名类“t”
                Type class_type = Type.GetType(MJSDK_Mgr_Mark.MJSDK_Namespace + _className);
                if (class_type == null)
                {
                    if (_failAction != null)
                    {
                        _failAction("Invoke_MJSDKMethod调用失败。 未找到:" + _className);
                    }
                    return;
                }

                //通过string类型的strMethod获得同名的方法“method”
                MethodInfo pro_funtion = class_type.GetMethod(_methodName);
                if (pro_funtion == null)
                {
                    if (_failAction != null)
                    {
                        _failAction("Invoke_MJSDKMethod调用失败。 在" + _className + "未找到" + _methodName + "方法");
                    }
                    return;
                }
                //方法执行
                pro_funtion.Invoke(class_type, objParams);
            }
            catch (Exception ex)
            {
                if (_failAction != null)
                {
                    _failAction("Invoke_MJSDKMethod调用异常。调用【className】:" + _className + "【methodName】:" + _methodName + " 【error】:" + ex.Message);
                }
            }
        }


        //埋点key序列号
        private static long _g_KeySerialize = 1000;
        /// <summary>
        /// 获取新的回调序列号并返回
        /// </summary>
        /// <returns></returns>
        public static long getNewSerialize() { return _g_KeySerialize++; }
    }
}

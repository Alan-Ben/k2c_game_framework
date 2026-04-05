using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// mjsdk 库类工具
    /// </summary>
    public class MJSDK_Tool_Lib
    {
        //平台组件信息
        public static MJSDK_Basic_2Engine_mj_componentInfo g_mj_componentInfo;
        /// <summary>
        /// 根据项目集成组件信息 进行自动初始化Unity脚本库集
        /// </summary>
        /// <param name="_obj"></param>
        public static void initUnityLib(Action<bool, string> _execteResultCB)
        {
            //数据清空
            g_mj_componentInfo = null;

            MJSDK_BasicLib.mj_componentInfo((MJSDK_Basic_2Engine_mj_componentInfo _componentInfo) =>
            {
                //初始化库开始    
                MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitUnityLib, "Start Init Unity Libs" + JsonUtility.ToJson(_componentInfo));
                //平台组件数据赋值
                g_mj_componentInfo = _componentInfo;
                //是否初始化成功
                bool isInitSuc = true;
                //初始化过程信息
                string initMsg = "";

                //循环各个组件
                foreach (var item in _componentInfo.component)
                {
                    if (string.IsNullOrEmpty(item.tag))
                    {
                        //有一个未初始化成功，就算整体初始化失败
                        isInitSuc = false;
                        //错误信息收集
                        initMsg = initMsg + "\n" + "组件库初始化，未收到平台返回的标识符。请联系MJ平台进行排查";
                        continue;
                    }

                    //如果组件标识符是下列三种，就无需初始化对应的库
                    if (item.tag.Equals("MJSDK_Unity")
                    || item.tag.Equals("MJSDK_Common")
                    || item.tag.Contains("MJSDK_Basic")
                    || item.tag.Equals("MJSDK_Resource"))
                    {
                        MJSDK_Log.mjsdkLog("组件库：" + item.tag + "无需初始化UnitySDK中的脚本库");
                        continue;
                    }

                    try
                    {
                        //组件库名称
                        string component_lib_name = "MJSDK_Package." + item.tag + "Lib";
                        //通过string类型的strClass获得同名类“t”
                        Type class_type = Type.GetType(component_lib_name);

                        if (class_type == null)
                        {
                            isInitSuc = false;
                            initMsg = initMsg + "\n" + "组件库：" + item.tag + "初始化失败.. error:" + "未能找到" + component_lib_name;
                        }
                        else
                        {
                            //库模块初始化接口
                            MethodInfo init_funtion = class_type.GetMethod("init");
                            if (init_funtion != null)
                            {
                                object[] objParams = new object[1];
                                //参数
                                objParams[0] = item;
                                //先执行一遍初始化库
                                init_funtion.Invoke(class_type, objParams);
                            }
                            else
                            {
                                isInitSuc = false;
                                initMsg = initMsg + "\n" + "组件库：" + item.tag + "初始化失败.. error:" + "在" + component_lib_name + "组件库，未找到init协议";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //有一个未初始化成功，就算整体初始化失败
                        isInitSuc = false;
                        //错误信息收集
                        initMsg = initMsg + "\n" + "组件库：" + item.tag + "初始化失败.. error:" + ex.Message;
                    }
                }

                //执行结果回执
                if (_execteResultCB != null)
                {

                    if (isInitSuc)
                    {
                        initMsg = "初始化Unity脚本库成功";
                    }
                    else
                    {
                        //打印错误信息
                        MJSDK_Log.mjsdkLog(initMsg, E_MJSDK_BusType.Error_SDKInitErr);
                    }

                    _execteResultCB(isInitSuc, initMsg);
                };

            }, (int errorCode, string errorMsg) =>
            {
                //执行结果回执
                if (_execteResultCB != null) { _execteResultCB(false, "errorCode:" + errorCode + " errorMsg:" + errorMsg); };
            });
        }

        /// <summary>
        /// 检查初始化状态
        /// </summary>
        /// <returns></returns>
        public static bool checkInitState(string _component_tag, bool _unityLib_isInit, Action<int, string> _failDelegate)
        {
#if UNITY_IOS || UNITY_ANDROID
            if (!_unityLib_isInit)
            {
                string errorMsg = _component_tag + " init error(初始化异常）。可能原因：未集成对应平台组件/平台组件相关依赖失败";
                //获取组件初始化错误信息
                string componentInitErrorMsg = MJSDK_Tool_Lib.getComponentErrorMsg(_component_tag);
                //不为空替换原先默认的错误信息，提示具体错误信息
                if (!string.IsNullOrEmpty(componentInitErrorMsg)) { errorMsg = componentInitErrorMsg; }
                MJSDK_Log.mjsdkLog(errorMsg, E_MJSDK_BusType.Error);
                if (_failDelegate == null)
                {
                    //如果回调值为空的话，向全局MJSDK逻辑层发送错误信息 
                    if (MJSDK.isInit) { MJSDK.logicInterface.onMJSDKError(MJSDK_Error.C_Unity_ComponentVersionErr, errorMsg); }
                    else { MJSDK_Log.mjsdkLog("checkInitState: _failDelegate is null。reason:" + errorMsg, E_MJSDK_BusType.Error); };
                }
                else
                {

                    _failDelegate(MJSDK_Error.C_Unity_ComponentVersionErr, errorMsg);
                }
                return false;
            }
#endif
            return true;
        }



        #region 组件初始化错误信息
        private static Dictionary<string, string> componentInitError_dic = new Dictionary<string, string>();

        /// <summary>
        /// 存储 组件库初始化失败错误
        /// </summary>
        /// <param name="_component_tag"></param>
        /// <param name="_errMsg"></param>
        public static void setComponentErrorMsg(string _component_tag, string _errMsg)
        {
            //判断是否初始化过
            if (componentInitError_dic == null)
            {
                componentInitError_dic = new Dictionary<string, string>();
            }
            //错误信息
            string errMsg = _errMsg;
            if (componentInitError_dic.ContainsKey(_component_tag))
            {
                //取出原先数据
                errMsg = componentInitError_dic[_component_tag];
                //拼接信息的
                errMsg += "\n" + _errMsg;
                //修改组件初始化错误信息
                componentInitError_dic[_component_tag] = errMsg;
            }
            else
            {
                //添加组件初始化错误信息
                componentInitError_dic.Add(_component_tag, errMsg);
            }
        }

        /// <summary>
        /// 获取 组件库初始化失败错误
        /// </summary>
        /// <param name="_component_tag"></param>
        /// <returns></returns>
        public static string getComponentErrorMsg(string _component_tag)
        {
            //错误信息
            string errMsg = "";
            if (componentInitError_dic.ContainsKey(_component_tag))
            {
                //取出原先数据
                errMsg = componentInitError_dic[_component_tag];

            }
            return errMsg;
        }

        #endregion
    }
}

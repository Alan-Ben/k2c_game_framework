using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK的主管理对象，将记录相关的全局对象和信息
    /// 并提供相关的调用方法
    /// </summary>
    public class MJSDK
    {
        //SDK是否已经初始化的标记
        private static bool _g_bIsInit = false;
        //SDK交互的Go对象
        private static GameObject _g_goSDKInterfaceGo = null;
        //交互的Unity脚本对象
        private static MJSDK_MainMono _g_mmMainMono = null;
        //逻辑的实际处理对象接口
        private static _IMJSDK_DealLogicInterface _g_liLogicInterface = null;
        //MJSDK_Common版本号
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version("MJSDK_Common", 0, 1, 20, 0);

        //安卓类对象，通过类对象获取实际的activity对象用于通讯
        private static AndroidJavaClass _g_jcJavaClass = null;
        private static AndroidJavaObject _g_joActivityObj = null;

        //是否已经初始化
        public static bool isInit { get { return _g_bIsInit; } }
        //获取逻辑处理对象
        public static _IMJSDK_DealLogicInterface logicInterface { get { return _g_liLogicInterface; } }

        /// <summary>
        /// SDK的初始化函数，需要在App最开始调用本初始化函数
        /// <param name="_dealLogicInterface">mjsdk消息回执接口</param>
        /// <param name="_initResultDelegate">初始化结果回执</param>
        /// <param name="_openAgreement">是否启动“用户协议逻辑”，使用场景：国内启动应用，用户交互的第一步是需要用户同意隐私协议相关，方可继续使用应用。。</param>
        /// </summary>
        public static void init(_IMJSDK_DealLogicInterface _dealLogicInterface, Action<bool, string> _initResultDelegate, bool _openAgreement = false)
        {
            try
            {
                //打印MJSDK_Unity大版本号信息
                MJSDK_Unity_Version.printMJSDKUnityVersion();

                //判断是否已经初始化，是则报错退出
                if (_g_bIsInit)
                {
                    string logMsg = "MJSDK is Already Inited!";
                    MJSDK_Log.mjsdkLog(logMsg, E_MJSDK_BusType.SDKInitSuc);
                    if (_initResultDelegate != null) { _initResultDelegate(_g_bIsInit, logMsg); }
                    ;
                    return;
                }
                //开始初始化
                MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_StartInit, "Start InitSDK V:" + MJSDK_Unity_Version.printMJSDKUnityVersion() + " Agreement:" + _openAgreement);
                //-----------------------------初始化消息回执对象-----------------------------
                //带入的处理对象不可为空
                if (null == _dealLogicInterface)
                {
                    string errMsg = "MJSDK init with _IMJSDK_DealLogicInterface is null!";
                    MJSDK_Log.mjsdkLog(errMsg, E_MJSDK_BusType.Error_SDKInitErr);
                    MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitFail, errMsg);
                    if (_initResultDelegate != null) { _initResultDelegate(_g_bIsInit, errMsg); }
                    ;
                    return;
                }

                //必须声明初始化结果回执
                if (null == _initResultDelegate)
                {
                    string errMsg = "MJSDK init with _initResultDelegate is null!";
                    MJSDK_Log.mjsdkLog(errMsg, E_MJSDK_BusType.Error_SDKInitErr);
                    MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitFail, errMsg);
                    if (_initResultDelegate != null) { _initResultDelegate(_g_bIsInit, errMsg); }
                    ;
                    return;
                }

                //版本号输出
                g_Version.printVersion();

                //设置逻辑处理对象
                _g_liLogicInterface = _dealLogicInterface;

                //-----------------------------初始化MJSDK对象-----------------------------
                //创建全局最高层的Go对象
                _g_goSDKInterfaceGo = new GameObject("MJSDK");
                //添加脚本
                _g_mmMainMono = _g_goSDKInterfaceGo.AddComponent<MJSDK_MainMono>();
                //设置对象不随Scene销毁
                GameObject.DontDestroyOnLoad(_g_goSDKInterfaceGo);

                //初始化Android对象
                if (!checkAndroidJavaClassStatus())
                {
                    string errMsg = "MJSDK Android JavaClass Err";
                    MJSDK_Log.mjsdkLog(errMsg, E_MJSDK_BusType.Error_SDKInitErr);
                    if (_initResultDelegate != null) { _initResultDelegate(_g_bIsInit, errMsg); }
                    ;
                    return;
                }

                //平台依赖版本判定
                judgePlatformDependenceVersion();
                //-----------------------------初始化所有Unity库-----------------------------
                initUnityLib_PlatfromLib((bool isSuc, string msg) =>
                {
                    //最终状态赋值
                    _g_bIsInit = isSuc;
                    _initResultDelegate(isSuc, msg);
                    //进行埋点
                    if (isSuc)
                    {
                        MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitSuc, "初始化UnitySDK成功");
                    }
                    else
                    {
                        MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitFail, msg);
                    }
                }, _openAgreement);
            }
            catch (Exception ex)
            {
                string errMsg = "初始化失败,error:" + ex.Message;
                MJSDK_Log.mjsdkLog(errMsg, E_MJSDK_BusType.Error_SDKInitErr);
                //进行埋点
                MJSDK_BasicLib.mj_sdkTrace(MJSDK_Event.C_Unity_Event_InitFail, errMsg);
                if (_initResultDelegate != null) { _initResultDelegate(false, errMsg); }
                ;
            }
        }


        /// <summary>
        /// 平台依赖版本判定
        /// </summary>
        public static void judgePlatformDependenceVersion()
        {
            //主要用来判断依赖手机平台MJSDK_Unity
            MJSDK_Component_Version mjsdk_unity_Version = new MJSDK_Component_Version("MJSDK_Unity", 0, 0, 0, 0);
#if UNITY_IOS
            //判断与iOS平台MJSDK_Unity是否匹配
            mjsdk_unity_Version.judgePlatformDependenceVersionIsEnoughBysys_version(0, 1, 3, 4);
#elif UNITY_ANDROID
            //判断与Android平台MJSDK_Unity是否匹配
            mjsdk_unity_Version.judgePlatformDependenceVersionIsEnoughBysys_version(0,1,2,0);
#endif
        }


        /// <summary>
        /// 检查Android对象情况
        /// </summary>
        private static bool checkAndroidJavaClassStatus()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                //android对象不能为空，为空的话进行初始化
                if (_g_jcJavaClass != null && _g_joActivityObj != null)
                {
                    return true;
                }

                //获取对应的安卓环境对象
                _g_jcJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                if (null == _g_jcJavaClass)
                {
                    MJSDK_Log.mjsdkLog(" MJSDK get Android Java Class fail!", E_MJSDK_BusType.Error_SDKInitErr);
                    return false;
                }

                //通过currentActivity函数获取实际的activity对象
                _g_joActivityObj = _g_jcJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                if (null == _g_joActivityObj)
                {
                    MJSDK_Log.mjsdkLog(" MJSDK get Android Unity Activity fail!", E_MJSDK_BusType.Error_SDKInitErr);
                    return false;
                }

                //初始化-代理回执（Android）
                MJSDK_Unity_CallBack_Proxy.getInstance().initData();
            }
            return true;
        }

        //异步初始化Unity脚本库与平台库
        private static void initUnityLib_PlatfromLib(Action<bool, string> _initResultDelegate, bool _openAgreement)
        {
            try
            {
                //await Task.Run(() => {
                //根据项目集成组件信息 进行自动初始化Unity脚本库集
                MJSDK_Tool_Lib.initUnityLib((bool _isSuc, string _msg) =>
                {

                    if (_isSuc)
                    {
                        //开启
                        if (_openAgreement)
                        {
                            _initResultDelegate(true, _msg);
                            return;
                        }
                        //国际包MJSDK初始化--国内包后续进行兼容即初始化由客服端等待用户同意隐私协议后
                        MJSDK_BasicLib.mj_initThirdSDK((string _reaultStr) =>
                        {

                            _initResultDelegate(true, _reaultStr);
                        }, (int errorCode, string errorMsg) =>
                        {

                            _initResultDelegate(false, "errorCode:" + errorCode + " errorMsg:" + errorMsg);
                        });
                    }
                    else
                    {
                        _initResultDelegate(false, _msg);
                    }
                });
                //});
            }
            catch (Exception ex)
            {
                string errMsg = "initUnityLib_PlatfromLib error:" + ex.Message;
                _initResultDelegate(false, errMsg);
                MJSDK_Log.mjsdkLog(errMsg, E_MJSDK_BusType.Error_SDKInitErr);
            }
        }


        #region 平台入口
#if UNITY_IOS
        //iOS主接口
        [DllImport("__Internal")]
        private static extern void unityCallFunc(string paramstr);
#endif

        /// <summary>
        /// 调用手机平台功能主入口
        /// Ios函数名： CallPlatformMethod
        /// 安卓部分函数名： unityCallFunc
        /// 统一带入一个String参数，json格式。
        /// </summary>
        /// <param name="_msg">参数</param>
        public static void CallPhonePlatformFuntion(string _msg)
        {
            try
            {
                MJSDK_Log.mjsdkLog("Call Phone Msg:" + _msg, E_MJSDK_BusType.SendMsgToSDK);

                if (Application.isEditor)
                {
                    MJSDK_Log.mjsdkLog("CallPhonePlatformFuntion in Editor Mode, skip call function.", E_MJSDK_BusType.SendMsgToSDK);
                    return; // 跳过编辑器模式
                }

                // 运行时判断平台
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
#if UNITY_IOS
                    unityCallFunc(_msg);
#else
                    object[] objParams = new object[1];
                    objParams[0] = _msg;
                    //MJSDK_Dll不支持iOS平台,,结合MJSDK_IOSBridge.cs进行反射调用
                    MJSDK_MethodUtil.Invoke_MJSDKMethod("MJSDK_IOSBridge", "unityCallIOSFunc", objParams,
                        (string errorMsg) =>
                        {
                            MJSDK_Log.mjsdkLog("CallPhonePlatformFuntion error:" + errorMsg, E_MJSDK_BusType.Error);
                        });
#endif
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    //检查Android对象情况
                    if (checkAndroidJavaClassStatus())
                        _g_joActivityObj.Call("unityCallFunc", _msg);
                    else
                        MJSDK_Log.mjsdkLog(" Call SDK func MJSDK Android Unity Activity is null!", E_MJSDK_BusType.Error);
                }
            }
            catch (Exception ex)
            {
                MJSDK_Log.mjsdkLog("CallPhonePlatformFuntion error:" + ex.Message, E_MJSDK_BusType.Error);
            }
        }

        /// <summary>
        /// 发送消息到SDK端进行处理
        /// 可以带上回调处理对象，有回调处理对象的，在SDK处理完毕之后会将处理结果通过回调调用回来
        /// </summary>
        /// <param name="_mainOrder"></param>
        /// <param name="_subOrder"></param>
        /// <param name="_msg"></param>
        public static void sendMsgToPhonePlatform(string _mainOrder, string _subOrder, string _msg)
        {
            sendMsgToPhonePlatform(_mainOrder, _subOrder, _msg, null);
        }

        public static void sendMsgToPhonePlatform(string _mainOrder, string _subOrder, string _msg, _IMJSDK_CallbackDealer _dealer)
        {
            //注册回调对象
            long callbackSerialize = 0;
            if (null != _dealer)
            {
                callbackSerialize = MJSDK_CallbackMgr.instance.regCallback(_dealer);
            }

            MJSDK_SendSDKMsg msg = new MJSDK_SendSDKMsg();
            msg.mainOrder = _mainOrder;
            msg.subOrder = _subOrder;
            msg.callbackId = callbackSerialize;
            msg.args = _msg;

            string json_str = JsonUtility.ToJson(msg);

            //调用SDK部分的函数
            CallPhonePlatformFuntion(json_str);
        }

        /************************
         * 注册不同的消息处理对象
         * @param _mainOrder
         * @param _subOrder
         * @param _dealer
         */
        public static void regMsgDealer(string _mainOrder, string _subOrder, _AMJSDK_MsgSubDealer _dealer)
        {
            MJSDK_MsgDispather.instance.regSubDealer(_mainOrder, _subOrder, _dealer);
        }
        #endregion
    }
}

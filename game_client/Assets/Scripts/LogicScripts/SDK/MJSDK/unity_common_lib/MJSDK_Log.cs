using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum E_MJSDK_BusType
    {
        [EnumBindString("【MJSDK_Unity】")]
        Def = 0,            //MJSDK_Unity log信息标识
        [EnumBindString("【MJSDK_Unity_SendMsgToSDK】")]
        SendMsgToSDK = 1,   //MJSDK_Unity 向手机平台发送消息
        [EnumBindString("【MJSDK_Unity_ReceiveMsg】")]
        ReceiveMsg = 2,     //MJSDK_Unity 接收手机平台回执消息
        [EnumBindString("【MJSDK_Unity_SDKInitSuc】")]
        SDKInitSuc = 3,     //MJSDK_Unity 初始化成功标识
        [EnumBindString("【MJSDK_Unity_GetSDKApi】")]
        GetSDKApi = 4,      //MJSDK_Unity 接收MJSDK消息协议

        [EnumBindString("MJSDK_Unity_Other】")]
        Other = 100,        //MJSDK_Unity 其他

        //------------------错误相关信息-------------------
        [EnumBindString("【MJSDK_Unity_Error】")]
        Error = 200,               //MJSDK_Unity 错误log信息标识
        [EnumBindString("【MJSDK_Unity_Error_SDKInitErr】")]
        Error_SDKInitErr = 201,    //MJSDK_Unity 初始化错误表示
        [EnumBindString("【MJSDK_Unity_Error_GetSDKErr】")]
        Error_GetSDKErr = 202,     //MJSDK_Unity 接收MJSDK错误信息
        [EnumBindString("【MJSDK_Unity_Error_DealSDKMsgErr】")]
        Error_DealSDKMsgErr = 203, //MJSDK_Unity 处理SDK返回值事件异常
        [EnumBindString("【MJSDK_Unity_Error_BusiPayErr】")]
        Error_BusiPayErr = 204,    //MJSDK_Unity 支付业务
        [EnumBindString("【MJSDK_Unity_Error_BusiLoginErr】")]
        Error_BusiLoginErr = 205,  //MJSDK_Unity 登录业务
    }
    /// <summary>
    /// 日志输出层
    /// </summary>
    public class MJSDK_Log
    {
        //错误业务头部编号
        public const int C_Unity_Err_Head = 200;
        //msg：日志信息
        //e_MJSDK_BusType:业务信息
        public static void mjsdkLog(string msg, E_MJSDK_BusType e_MJSDK_BusType = E_MJSDK_BusType.Def) {

            try
            {
                //日志标识
                string logMark = MJSDK_EnumUtil.GetBindString(e_MJSDK_BusType);
                //业务编号
                int busCode = (int)e_MJSDK_BusType;
                //大于错误头部编号情况下，进行错误表示打印
                if (busCode >= C_Unity_Err_Head)
                {
                    Debug.LogError(logMark + msg);
                }
                else {
#if MJSDK_UNITY_DEBUG
                    Debug.Log(logMark + msg);
#endif 
                }
            }
            catch (System.Exception ex)
            { 
                Debug.LogError("call mjsdkLog,error:" + ex.Message);
            }
        }
    }
}
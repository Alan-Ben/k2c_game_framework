using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
     /// <summary>
    /// 服务端公共接口支持组件的错误定义常量类
    /// </summary>
    public class MJSDK_PhpApiCommonError : MJSDK_Error
    {
        public const int C_Unity_PhpApi_Error_NetError = 30001;        //网络异常
        public const int C_Unity_PhpApi_Reponse_Parse_Fail = 30002;    //报文数据解析失败
        //SDK服务端（登录、支付）事务处理失败  服务端错误码详情：http://apidoc.mjggpt.com/web/#/31/713
        public const int C_Unity_PhpApi_LoginOrPay_Msg_DealFail = 30301;
        //SDK服务端（事件统计）事务处理失败    服务端错误码详情：http://apidoc.mjggpt.com/web/#/33/1012
        public const int C_Unity_PhpApi_Trace_Msg_DealFail = 30302;
        //SDK服务端  (工具）事务处理失败    服务端错误码详情：http://public-api.dreamplusgames.com/web/#/39/1392
        public const int C_Unity_PhpApi_Tool_Msg_DealFail = 30303;
        //获取手机验证码失败（图片、sms）    服务端错误码详情：http://apidoc.mjggpt.com/web/#/31/713
        public const int C_Unity_PhpApi_Mobile_GetCodeFail = 30304;         
    }
}

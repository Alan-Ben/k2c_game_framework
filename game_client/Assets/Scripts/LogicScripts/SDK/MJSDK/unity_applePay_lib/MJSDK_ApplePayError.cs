using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 苹果支付错误码
    /// </summary>
    public class MJSDK_ApplePayError : MJSDK_Error
    {
        public const int C_Unity_ApplePay_Device_NoSupport_Pay = 50101; //iOS系统低，不支持苹果应用内支付
        public const int C_Unity_ApplePay_Search_Product_Error = 50102;   //苹果后台未获取商品信息
        public const int C_Unity_ApplePay_Pay_Fail = 50103;    //支付失败
        public const int C_Unity_ApplePay_Deferred = 50104;  //等待确认 儿童模式
        public const int C_Unity_ApplePay_Local_Price_Fail = 50105;  //档位价格本地化失败
    }
}

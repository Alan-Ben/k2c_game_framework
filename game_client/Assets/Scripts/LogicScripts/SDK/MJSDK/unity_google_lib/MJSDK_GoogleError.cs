using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Google组件脚本库  错误码
    /// </summary>
    public class MJSDK_GoogleError:MJSDK_Error
    {
        public const int C_Unity_Google_Get_UserInfo_Fial = 50201;    //获取用户信息失败
        public const int C_Unity_Google_Search_Product_Error = 50202;    //查询商品失败
        public const int C_Unity_Google_Pay_Fail = 50203;    //支付失败
        public const int C_Unity_Google_Close_Pay_Transaction_fail = 50204;    //关闭订单事务失败
    }
}

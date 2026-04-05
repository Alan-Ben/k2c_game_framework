using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Xiaomi组件脚本库  错误码
    /// </summary>
    public class MJSDK_XiaomiError : MJSDK_Error
    {
        //查询商品失败
        public const int C_Xiaomi_querySkuDetailsAsync_Fail = 50401;
        //支付失败
        public const int C_Xiaomi_Pay_Fail = 50402;
    }
}
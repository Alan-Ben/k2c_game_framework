using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Samsung组件脚本库  错误码
    /// </summary>
    public class MJSDK_SamsungError : MJSDK_Error
    {
        //查询商品失败
        public const int C_Samsung_querySkuDetailsAsync_Fail = 50501;
        //支付失败
        public const int C_Samsung_Pay_Fail = 50502;
    }
}

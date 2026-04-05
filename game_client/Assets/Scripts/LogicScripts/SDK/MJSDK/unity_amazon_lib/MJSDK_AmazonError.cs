using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Amazon组件脚本库  错误码
    /// </summary>
    public class MJSDK_AmazonError : MJSDK_Error
    {
        //查询商品失败
        public const int C_Amazon_querySkuDetailsAsync_Fail = 50701;
        //支付失败
        public const int C_Amazon_Pay_Fail = 50702;
        //支付取消
        public const int C_Amazon_Pay_Cancel = 50703;
    }
}
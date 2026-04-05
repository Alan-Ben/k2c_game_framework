using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK_RuStore组件脚本库  错误码
    /// </summary>
    public class MJSDK_RuStoreError : MJSDK_Error
    {
        //查询商品失败
        public const int C_OneStore_querySkuDetailsAsync_Fail = 50601;
        //支付失败
        public const int C_OneStore_Pay_Fail = 50602;
        //支付取消
        public const int C_OneStore_Pay_Cancel = 50603;
    }
}

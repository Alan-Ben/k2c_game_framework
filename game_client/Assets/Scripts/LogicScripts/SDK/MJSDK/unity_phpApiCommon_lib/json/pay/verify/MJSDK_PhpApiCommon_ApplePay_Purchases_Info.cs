using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// 苹果应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_ApplePay_Purchases_Info
    {
        [Preserve]
        //支付票据
        public string receiptData;
        [Preserve]
        //订单ID
        public string order_id;
        [Preserve]
        //实付金额
        public string payment;
        [Preserve]
        //实付货币类型
        public string payment_code;
    }
}

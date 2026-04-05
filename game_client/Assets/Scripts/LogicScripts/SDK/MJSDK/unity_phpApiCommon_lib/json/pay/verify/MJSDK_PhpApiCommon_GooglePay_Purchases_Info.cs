using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// Google应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_GooglePay_Purchases_Info
    {
        [Preserve]
        //充值成功后返回的json
        public string json_data;
        [Preserve]
        //充值成功后返回的签名
        public string signature;
        [Preserve]
        //实付金额
        public string payment;
        [Preserve]
        //实付货币类型
        public string payment_code;
        [Preserve]
        //订单号
        public string order_id;
        [Preserve]
        //商品id
        public string sku_id;
    }
}
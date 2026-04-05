using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// 小米应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_XiaomiPay_Purchases_Info
    {
        [Preserve]
        //小米返回的购买token
        public string purchaseToken;
        [Preserve]
        //订单号
        public string order_id;
        [Preserve]
        //第三方产品ID（必须是配置在小米后台商品id）
        public string sku_id;
        [Preserve]
        //实付金额
        public string payment;
        [Preserve]
        //实付货币类型
        public string payment_code;
    }
}
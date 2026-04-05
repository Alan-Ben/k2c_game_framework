using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// OneStore应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_OneStorePay_Purchases_Info
    {
        [Preserve]
        //订单号
        public string order_id;
        [Preserve]
        //第三方产品ID（必须是配置在OneStore后台商品id）
        public string sku_id;
        [Preserve]
        //实付金额
        public string payment;
        [Preserve]
        //实付货币类型
        public string payment_code;
    }
}
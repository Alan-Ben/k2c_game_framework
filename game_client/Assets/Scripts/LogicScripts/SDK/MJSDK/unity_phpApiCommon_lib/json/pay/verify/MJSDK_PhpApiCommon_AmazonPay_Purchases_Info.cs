using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// Amazon应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_AmazonPay_Purchases_Info
    {
        [Preserve]
        //订单号
        public string order_id;
        [Preserve]
        //第三方产品ID（必须是配置在Amazon后台商品id）
        public string sku_id;
        [Preserve]
        //亚马逊返回的用户唯一ID
        public string user_id;
        [Preserve]
        //亚马逊返回的购买ID
        public string receipt_id;
    }
}
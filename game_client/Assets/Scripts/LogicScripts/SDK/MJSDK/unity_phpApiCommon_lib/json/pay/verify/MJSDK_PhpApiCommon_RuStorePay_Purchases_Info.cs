using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// ruStore应用内商品购买信息 - 消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_RuStorePay_Purchases_Info
    {
        [Preserve]
        //RuStore invoiceId
        public string invoiceId;
        [Preserve]
        //RuStore purchaseToken
        public string purchaseToken;
        [Preserve]
        //服务端订单ID
        public string order_id;
    }
}
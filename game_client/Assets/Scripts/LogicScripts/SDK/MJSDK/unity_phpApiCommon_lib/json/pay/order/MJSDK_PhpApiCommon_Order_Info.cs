using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// 订单成功创建信息-消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Order_Info
    {
        //第三方充值签名
        public string pay_sign;
        //订单ID
        public string order_id;
        //第三方产品ID
        public string sku_id;
        //充值回调的扩展参数
        public string extension;
        //商品类型  普通商品:inapp，订阅商品:subs
        public string product_type = "inapp";
    }
}
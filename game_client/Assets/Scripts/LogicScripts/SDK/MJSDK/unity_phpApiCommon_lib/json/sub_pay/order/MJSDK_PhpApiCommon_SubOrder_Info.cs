using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// 订阅订单成功创建信息-消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_SubOrder_Info
    {

        //订阅订单ID
        [Preserve]
        public string sub_id;
        //第三方产品ID
        [Preserve]
        public string pay_product_id;
        //第三方产品分组
        [Preserve]
        public string pay_product_group;
        //充值回调的扩展参数
        [Preserve]
        public string extension;
        //商品类型  普通商品:inapp，订阅商品:subs
        [Preserve]
        public string product_type = "subs";
    }
}
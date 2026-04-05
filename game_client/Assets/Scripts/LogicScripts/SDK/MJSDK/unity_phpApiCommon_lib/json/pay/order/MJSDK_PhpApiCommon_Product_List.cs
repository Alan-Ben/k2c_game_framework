using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// 档位商品信息（标配 -- 一般以美元为主） 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Product_List
    {
        public List<MJSDK_PhpApiCommon_product> product;
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_product
    {
        //应用产品ID
        [Preserve]
        public string app_product_id;
        //第三方支付id
        [Preserve]
        public string pay_product_id;
        [Preserve]
        //三方支付分组--订阅商品使用
        public string pay_product_group;
    }
}
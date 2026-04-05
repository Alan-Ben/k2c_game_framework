using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// 第三方渠道-localProduct：第三方渠道-档位信息转换为本地商品信息 消息结构体
    /// </summary>
    public class MJSDK_2Engine_ThirdParty_localProduct_list
    {
        public List<MJSDK_ThirdParty_local_product> product;
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    [Serializable]
    public class MJSDK_ThirdParty_local_product
    {
        //第三方平台（苹果、google、华为）支付id
        [Preserve]
        public string pay_product_id;
        //商品id（本地商品id）
        [Preserve]
        public string app_product_id;
        //第三方平台（苹果、google、华为）订阅群组id
        [Preserve]
        public string pay_product_gruop;
        //当地价格（如：6.00）
        [Preserve]
        public string price;
        //货币类型（如：CNY）
        [Preserve]
        public string currency;
        //货币符号（如：¥）
        [Preserve]
        public string symbol;
        //商品标题
        [Preserve]
        public string title;
        //说明
        [Preserve]
        public string dec;
        //格式化价格（如：¥6.00）
        [Preserve]
        public string formattedPrice;
        //国家码（如：CN）
        [Preserve]
        public string countryCode;
    }
}
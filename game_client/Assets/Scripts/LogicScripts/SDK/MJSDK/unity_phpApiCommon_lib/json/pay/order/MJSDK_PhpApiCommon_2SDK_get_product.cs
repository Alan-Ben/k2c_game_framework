using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(order-getProduct：支付-获取充值商品)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_get_product : MJSDK_2SDK_Base
    {
        //用户登录令牌
        public string token;
        // 支付类型, 
        // 微信：wechat , 支付宝：alipay , 苹果：apple , 
        // 谷歌：google ,华为：huaweipay ,小米：mipay ,
        // 三星：samsung, rustore：rustore, onestore：onestore, amazon：amazon
        public string pay_type;
    }
}
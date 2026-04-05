using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(order-createOrder：支付-创建普通订单)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_create_subOrder : MJSDK_2SDK_Base
    {
        //用户登录成功token令牌（user_token)
        [Preserve]
        public string token;
        //服务器标识 ID
        [Preserve]
        public string server_id;
        //角色 ID
        [Preserve]
        public string role_id;
        //扩展参数,原样通知到通知地址
        [Preserve]
        public string extension;
        // 支付类型, 
        // 微信：wechat , 支付宝：alipay , 苹果：apple , 
        // 谷歌：google ,华为：huaweipay ,小米：mipay ,
        // 三星：samsung, rustore：rustore, onestore：onestore, amazon：amazon
        [Preserve]
        public string pay_type;
        //充值产品ID（项目组自己的）
        [Preserve]
        public string product_id;
        //游戏订单号（项目组自己的）
        [Preserve]
        public string app_order_id;
        //回调地址ID
        [Preserve]
        public string callback_id;
    }
}

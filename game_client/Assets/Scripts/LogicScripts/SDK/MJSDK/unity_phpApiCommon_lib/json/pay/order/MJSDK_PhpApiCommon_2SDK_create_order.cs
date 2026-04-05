using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(order-createOrder：支付-创建普通订单)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_create_order : MJSDK_2SDK_Base
    {
        //用户登录成功token令牌（user_token)
        public string token;
        //平台区域ID,(平台ID×100+区域ID)，公式：platformId×100+areaID
        //如：平台ID：1，区域ID：2 值为:（ 1 × 100+2=102）
        //-----------------
        //https://apidoc.mjggpt.com/web/#/9/147   取platformId
        //https://apidoc.mjggpt.com/web/#/9/191  取areaID
        public string platfrom_region;
        //服务器标识 ID
        public string server_id;
        //角色 ID
        public string role_id;
        //扩展参数,原样通知到通知地址
        public string extension;
        // 支付类型, 
        // 微信：wechat , 支付宝：alipay , 苹果：apple , 
        // 谷歌：google ,华为：huaweipay ,小米：mipay ,
        // 三星：samsung, rustore：rustore, onestore：onestore, amazon：amazon
        public string pay_type;
        //充值产品ID（项目组自己的）
        public string product_id;
        //游戏订单号（项目组自己的）
        public string app_order_id;
        //产品名称（项目组自己的）
        public string product_name;

        //----------------sdk（服务端）档位id（如果没有配置支付产品不用填写。比如支付宝、微信等）----------------
        //SDK组服务端提供（项目组需提供配置在商店google/苹果后台的档位信息）
        public string sdk_pay_id;


        //----------------callback_id，callback_url必须传一个，callback_id优先级较高----------------
        //回调地址ID
        public string callback_id;
        //付款成功后通知地址,如果Callback_id 有填入,callback_url可不用填。
        public string callback_url;


        //----------------下列字段在微信和支付宝支付方式情况是必传的。其他支付方式不用传----------------
        //价格（微信、支付宝必传，其他支付方式可为空 金额单位：元）
        public string amount;
        //货币类型（微信、支付宝必传，其他支付方式可为空）
        public string amount_type;
    }
}

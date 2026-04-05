using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
namespace MJSDK_Package
{
    /// <summary>
    /// 第三方渠道 支付--票据编号列表 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Pay_TransactionId_List
    {
        [Preserve]
        public List<MJSDK_PhpApiCommon_Pay_TransactionId> sub_list;
        [Preserve]
        // 支付类型, 
        // 微信：wechat , 支付宝：alipay , 苹果：apple , 
        // 谷歌：google ,华为：huaweipay ,小米：mipay ,
        // 三星：samsung, rustore：rustore, onestore：onestore, amazon：amazon
        public string pay_type;
    }

    /// <summary>
    /// 票据信息
    /// </summary>
    [Serializable]
    public class MJSDK_PhpApiCommon_Pay_TransactionId
    {
        [Preserve]
        //平台商品子id
        public string sku_id;
        [Preserve]
        //平台商品组id
        public string sku_group_id;
        [Preserve]
        //支付票据原生订单id,只有恢复购买时候有值
        public string original_transaction_id;
        [Preserve]
        //支付票据流程订单id
        public string transaction_id;
        //状态 0:初始状态 1:可恢复 2：重试恢复中 3：重推游戏中
        [Preserve]
        public string status = "0";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// 订单验证成功回执 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_OrderVerify 
    {
        //返回 1：成功 、0：失败
        public string verify_result;
        //订单ID
        public string order_id;
    }
}
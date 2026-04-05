using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(第三方平台埋点-purchaseEvent：购买事件上报)消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_2SDK_ThirdParty_purchaseEvent_base : MJSDK_2SDK_Base
    {
        //MJSDK游戏订单id
        public string mj_order_id;
        //游戏订单号
        public string app_order_id;
        //实付金额
        public string revenue;
        //实付货币类型
        public string currency;
        //商品档位id
        public string goods_id;
        //服务器id
        public string server_id;
        //扩展参数json(string)
        public string ext;

        /// <summary>
        /// 参数校验
        /// </summary>
        public static bool checkParam(MJSDK_2SDK_ThirdParty_purchaseEvent_base param)
        {
            if (param == null
                || string.IsNullOrEmpty(param.mj_order_id)
                || string.IsNullOrEmpty(param.app_order_id)
                || string.IsNullOrEmpty(param.goods_id)
                || string.IsNullOrEmpty(param.server_id)
                || string.IsNullOrEmpty(param.revenue)
                || string.IsNullOrEmpty(param.currency))
            {
                return false;
            }
            return true;
        }
    }
}

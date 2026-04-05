using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(google-queryPurchases：google检查漏单（项目组如果在start那边（也就是应用启动时）调用，请延迟一秒）)消息结构体
    /// </summary>
    public class MJSDK_Google_2Engine_google_queryPurchases : MJSDK_2Engine_Base
    {
        //获取所有漏单
        public List<MJSDK_PhpApiCommon_GooglePay_Purchases_Info> purchases;
    }
}

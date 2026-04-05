using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(apple-leakOrder：苹果-获取漏单信息)消息结构体 
    /// </summary>
    public class MJSDK_ApplePay_2Engine_applePay_leakOrder : MJSDK_2Engine_Base
    {
        //漏单列表
        public List<MJSDK_PhpApiCommon_ApplePay_Purchases_Info> leakOrder;
    }
}
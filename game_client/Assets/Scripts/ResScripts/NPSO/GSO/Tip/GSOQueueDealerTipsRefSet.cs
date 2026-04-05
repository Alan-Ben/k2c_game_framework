using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

using GOE;

[System.Serializable]
public class QueueDealerTipsRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)queue_dealer_tip_type; } }
    public ETipQueueType queue_dealer_tip_type;//队列处理提示类型
    public int priority_id;//优先级id，越小越优先
    public float space_time = 1f;//这个tip到发送下一个tip的间隔
}


public class GSOQueueDealerTipsRefSet : _TALSOBasicRefSet<QueueDealerTipsRefObj>
{

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "queue_dealer_tips"; } }
}

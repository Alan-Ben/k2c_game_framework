using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(noti-local：本地推送)消息结构体   
    /// </summary>
    public class MJSDK_MobPush_2Engine_noti_local : MJSDK_2Engine_Base
    {
        //本地推送标签（添加本地推送成功后会返回对应标签）
        public string push_id;
    }
}

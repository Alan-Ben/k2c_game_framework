using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;

/*******************
 * 自定义信息存储对象，由策划自定义使用，避免修改NPMainCameraMono脚本
 **/
#if NP_GAME

[System.Serializable]
public class NPGSOGameCustomInfo : ScriptableObject
{
    
    [ALHeader("自定义数据是否生效")]
    public bool isEnable;
    
    //所有可连接平台的信息列表
    [ALHeader("自定义可连接平台的信息列表")]
    public List<WCGPlatLoginInfo> platInfoList;
    
    public static string objName { get { return "game_custom_info"; } }
}

#endif

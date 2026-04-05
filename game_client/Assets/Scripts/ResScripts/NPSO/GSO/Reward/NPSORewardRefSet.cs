using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;





/**************
 * 奖励表结构
 **/
[System.Serializable]
public class NPSORewardRefObj : _IALBasicRefObj
{
    public long _refId { get { return reward_id; } }

    public long reward_id;                           //奖励唯一ID

    public List<NPCommonCostItem> show_item_list;    //奖励预览信息，客户端用来做UI上的奖励内容预览，实际给什么由服务器根据配置的掉落概率计算得最终结果下发

    public List<int> show_pro_list;                  //展示概率列表

    public bool show_item_list_expand;//是否展开展示奖励列表(不展开则读图标)
}

public class NPSORewardRefSet : _TALSOBasicRefSet<NPSORewardRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "reward"; } }
}

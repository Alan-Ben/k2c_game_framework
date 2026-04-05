using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 头像框表结构
    /// </summary>
    [System.Serializable]
    public class PlayerIconBgkRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // 头像框ID

        public long asset_path_id;//资源路径
        public NPEnum.ENPTimeAddType add_type;//有效期叠加类型

        public _NPPlayerConditionSerializeInfo show_condition;//展示条件
        public bool disable_cannot_see = false;//无效时是否可视
        public long sort_id; // 头像框排序id
    }

    /// <summary>
    /// 头像框数据集
    /// </summary>
    public class GSOPlayerIconBgkRefSet : _TALSOBasicRefSet<PlayerIconBgkRefObj>
    {
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "icon_bgk"; } }
    }
}


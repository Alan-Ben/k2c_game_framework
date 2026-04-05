using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 不同类型配置
    /// </summary>
    [Serializable]
    public class NPGGUIGetShowOffItemMono
    {
        [ALHeader("物品类型类型")]
        public ENPItemType type;

        [ALHeader("需要加载的respathId")]
        public long resPathid;
    }

    /// <summary>
    /// 获得装扮类道具界面（头像、头像框、气泡框、称号）
    /// </summary>
    public class NPGGUIMonoGetShowOffItem : _AALBasicUIWndMono
    {
        [ALHeader("子窗口父节点")]
        public Transform itemSubParent;
        [ALHeader("不同类型显示配置")]
        public List<NPGGUIGetShowOffItemMono> specialItemMonoList;
        [ALHeader("点击关闭")]
        public GameObject closeBtn;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("类型")]
        public Text txtType;
        [ALHeader("有效期")]
        public Text txtLeftTime;
        [ALHeader("停留动画")]
        public CommonAnimationSingleInfo idleAni;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_GET_SPECIAL_ITEM); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_GET_SPECIAL_ITEM); } }
    }

}

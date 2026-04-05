using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 提升途径-使用道具item
    /// </summary>
    public class GGUIMonoImproveWayUseItem : _AALBasicUIWndMono
    {
        [ALHeader("跳转按钮")]
        public GameObject btnGoTo;
        [ALHeader("使用道具途径描述")]
        public Text txtDesc;
        [ALHeader("没有道具时显示的GO列表")]
        public List<GameObject> goNoItemShowList;
        [ALHeader("没有道具时隐藏的GO列表")]
        public List<GameObject> goNoItemHideList;
    }
}

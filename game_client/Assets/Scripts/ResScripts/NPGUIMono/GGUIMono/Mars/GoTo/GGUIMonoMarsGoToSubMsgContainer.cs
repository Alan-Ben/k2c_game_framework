using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 前往火星留言容器
    /// </summary>
    public class GGUIMonoMarsGoToSubMsgContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoMarsGoToSubMsgContainerItem>
    {
        [ALHeader("移动一个item距离的时间（秒）")]
        public float moveItemDuration = 1.5f;
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("列表为空时隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}

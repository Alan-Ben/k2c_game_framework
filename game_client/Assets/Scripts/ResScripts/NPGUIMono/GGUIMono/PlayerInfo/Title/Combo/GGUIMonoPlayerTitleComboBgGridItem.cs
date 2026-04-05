using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 组合称号底框
    /// </summary>
    public class GGUIMonoPlayerTitleComboBgGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("新标识红点")]
        public GameObject goNewRedTip;
        [ALHeader("底框加载父节点")]
        public Transform transParent;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        [ALHeader("是当前佩戴时需要显示的GO列表")]
        public List<GameObject> goCurrentShowList;
        [ALHeader("是当前佩戴时需要隐藏的GO列表")]
        public List<GameObject> goCurrentHideList;
    }
}

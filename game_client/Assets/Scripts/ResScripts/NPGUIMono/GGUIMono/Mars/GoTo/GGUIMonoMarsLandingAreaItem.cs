using UnityEngine;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 到达火星选择登录地点item
    /// </summary>
    public class GGUIMonoMarsLandingAreaItem : _AALBasicUIWndMono
    {
        [ALHeader("选中按钮")]
        public GameObject btnSelect;
        [ALHeader("选中时需要展示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        [ALHeader("对应对话id")]
        public long dialogueId;
    }
}
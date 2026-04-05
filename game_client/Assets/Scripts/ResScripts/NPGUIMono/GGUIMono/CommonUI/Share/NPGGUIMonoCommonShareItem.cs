using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享item
    /// </summary>
    public class NPGGUIMonoCommonShareItem : _AALBasicUIWndMono
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("分享按钮")]
        public GameObject btnShare;
        [ALHeader("分享CD时 显示的物体")]
        public List<GameObject> goListShowOnShareCd;
        [ALHeader("分享CD时 隐藏的物体")]
        public List<GameObject> goListHideOnShareCd;
        [ALHeader("离线时 显示的物体")]
        public List<GameObject> goListShowOnOffline;
        [ALHeader("离线时 隐藏的物体")] 
        public List<GameObject> goListHideOnOffline;
        [ALHeader("额外显示的文本")] 
        public TextEx exText;
    }
}

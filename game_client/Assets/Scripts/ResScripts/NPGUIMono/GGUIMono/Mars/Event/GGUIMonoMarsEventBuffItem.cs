using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 事件BuffItem
    /// </summary>
    public class GGUIMonoMarsEventBuffItem : _AALBasicUIWndMono
    {
        [ALHeader("buff item")]
        public GGUIMonoPlayerBuffItem buffItem;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}
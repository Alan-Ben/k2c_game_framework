using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoChapterEventDispatchCondStateItem : _AALBasicUIWndMono
    {
        [ALHeader("达成条件时显示")]
        public List<GameObject> hasInfoShow;
        [ALHeader("没达成时显示")]
        public List<GameObject> hasInfoHide;
    }
}

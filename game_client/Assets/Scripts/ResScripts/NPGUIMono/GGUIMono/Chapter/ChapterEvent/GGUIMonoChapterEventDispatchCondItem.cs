using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoChapterEventDispatchCondItem : _AALBasicUIWndMono
    {
        [ALHeader("条件描述")]
        public Text txtDesc;
        [ALHeader("条件达成文字颜色")]
        public Color txtColorConditionMatch;
        [ALHeader("条件未达成文字颜色")]
        public Color txtColorConditionNotMatch;
        [ALHeader("条件达成显示GO")]
        public List<GameObject> goConditionMatchShow;
        [ALHeader("条件未达成显示GO")]
        public List<GameObject> goConditionNotMatchShow;
    }
}

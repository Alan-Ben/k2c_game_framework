using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoChapterEventChoiceItem : _AALBasicUIWndMono
    {
        [ALHeader("选项名")]
        public TextEx txtName;
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
    }
}

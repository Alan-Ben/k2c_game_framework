using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 家人带半身像item
    /// </summary>
    public class GGUIMonoConsortSpIconItem : _AALBasicUIWndMono
    {
        [ALHeader("家人名字")]
        public TextEx txtName;
        [ALHeader("家人半身像")]
        public RawImage imgIcon;
        [ALHeader("家人特殊图片背景")]
        public Image imgSpIconBg;
    }
}
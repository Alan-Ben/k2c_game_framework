using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴带半身像item
    /// </summary>
    public class GGUIMonoHeroSpIconItem : _AALBasicUIWndMono
    {
        [ALHeader("伙伴名字")]
        public TextEx txtName;
        [ALHeader("伙伴半身像")]
        public RawImage imgIcon;
        [ALHeader("伙伴特殊图片背景")]
        public Image imgSpIconBg;
        [ALHeader("实力")]
        public Text txtPower;
    }
}
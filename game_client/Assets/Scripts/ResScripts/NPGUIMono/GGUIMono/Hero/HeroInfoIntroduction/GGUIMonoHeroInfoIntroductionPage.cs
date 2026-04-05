using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴信息简介页签
    /// </summary>
    public class GGUIMonoHeroInfoIntroductionPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("称号")]
        public Text txtTitle;
        [ALHeader("出生地")]
        public Text txtBirthplace;
        [ALHeader("职业")]
        public Text txtOccupation;
        [ALHeader("简介")]
        public Text txtIntroduction;
    }
}


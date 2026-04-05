using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 招聘简历附加窗口
    /// </summary>
    public class GGUIMonoHireResumeItem : _AALBasicUIWndMono
    {
        [ALHeader("形象图片")]
        public RawImage imgCharacter;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("年龄")]
        public Text txtAge;
        [ALHeader("介绍")]
        public Text txtIntroduce;
        [ALHeader("标签")]
        public Text txtTag;
        [ALHeader("同意时需要显示的GO")]
        public GameObject goAgree;
        [ALHeader("拒绝时需要显示的GO")]
        public GameObject goRefuse;
    }
}
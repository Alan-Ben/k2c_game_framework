using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChildGraduationContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("子嗣的收益")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("毕业礼物")]
        public NPGGUIMonoCommonItem monoPresentItem;
        [ALHeader("查看详情按钮")]
        public GameObject btnDetail;
    }
}
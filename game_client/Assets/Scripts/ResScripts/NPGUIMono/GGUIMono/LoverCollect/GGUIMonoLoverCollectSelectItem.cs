using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoLoverCollectSelectItem : _AALBasicUIWndMono
    {
        [ALHeader("情人名字")]
        public Text txtLoverName;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
    }
}

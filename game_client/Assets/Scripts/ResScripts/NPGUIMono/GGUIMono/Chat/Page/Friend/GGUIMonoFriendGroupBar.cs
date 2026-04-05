using ALPackage;
using UnityEngine;

namespace GOE
{
    // 好友分组bar
    public class GGUIMonoFriendGroupBar : _AALBasicUIWndMono
    {
        [ALHeader("编辑按钮")]
        public GameObject btnEdi;
        [ALHeader("筛选按钮")]
        public NPGGUIMonoCommonTab tabSelected;
        [ALHeader("筛选内容显示文本")]
        public TextEx textTypeName;

    }
}
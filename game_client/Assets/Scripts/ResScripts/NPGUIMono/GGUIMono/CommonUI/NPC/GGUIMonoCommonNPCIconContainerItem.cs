using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoCommonNPCIconContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("名字")]
        public Text txtName;
    }
}
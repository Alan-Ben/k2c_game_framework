using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 权益卡描述列表item
    /// </summary>
    public class GGUIMonoPrivilegeCardDescContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("权益描述标题")]
        public Text txtTitle;
        [ALHeader("权益描述内容")]
        public Text txtContent;
    }
}

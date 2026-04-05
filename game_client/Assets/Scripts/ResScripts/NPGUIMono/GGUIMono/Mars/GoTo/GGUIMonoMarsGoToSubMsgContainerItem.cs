using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 前往火星留言容器item
    /// </summary>
    public class GGUIMonoMarsGoToSubMsgContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("内容")]
        public Text txtContent;
        [ALHeader("时间描述")]
        public Text txtTimeDesc;
    }
}

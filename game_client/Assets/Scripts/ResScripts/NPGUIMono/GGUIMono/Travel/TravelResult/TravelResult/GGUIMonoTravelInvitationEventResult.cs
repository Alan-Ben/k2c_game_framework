using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 指定邀约事件结果窗口
    /// </summary>
    public class GGUIMonoTravelInvitationEventResult : _ATravelResultMono
    {
        [ALHeader("妃子iconItem")]
        public GGUIMonoConsortIconItem monoConsortIcon;

        public static long uiResPathId { get { return 3605; } }
    }
}
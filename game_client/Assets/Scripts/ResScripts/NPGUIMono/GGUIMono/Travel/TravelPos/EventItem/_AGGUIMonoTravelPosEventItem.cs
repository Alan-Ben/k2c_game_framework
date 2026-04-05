using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item基础Mono
    /// </summary>
    public class _AGGUIMonoTravelPosEventItem : _AALBasicUIWndMono
    {
        [ALHeader("事件名称文本")]
        public TextEx txtEventName;
        [ALHeader("事件描述文本")]
        public TextEx txtEventDesc;

        [ALHeader("事件banner")]
        public RawImage imgEventBanner;

        [ALHeader("事件角色半身像")]
        public RawImage imgEventRoleMid;
        [ALHeader("事件角色名")]
        public TextEx txtEventRoleName;
    }
}

using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 运营公告页签列表
    /// </summary>
    public class GGUIMonoAnnouncementTabContainer : _ATNPGGUIMonoSingleChoiceContainer<GGUIMonoAnnouncementTabContainerItem>
    {
        [ALHeader("遮罩部分")]
        public RectTransform areaMaskObj;
        [ALHeader("拖动列表引用")]
        public NPScrollRect commonScrollRect;
    }
}
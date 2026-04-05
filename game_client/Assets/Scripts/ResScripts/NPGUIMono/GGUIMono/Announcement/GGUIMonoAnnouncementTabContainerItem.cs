using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 运营公告页签列表item
    /// </summary>
    public class GGUIMonoAnnouncementTabContainerItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("页签文本")]
        public Text txtContent;
        [ALHeader("公告页签图标")]
        public RawImage imgIcon;
        [ALHeader("未读红点")]
        public GameObject goRedTip;
    }
}
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星航行日志列表item
    /// </summary>
    public class GGUIMonoMarsGoToLogGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("日志标题")]
        public Text txtTitle;
        [ALHeader("日志内容")]
        public Text txtContent;
        [ALHeader("日志触发时间")]
        public Text txtTime;
    }
}
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟日志列表bar
    /// </summary>
    public class GGUIMonoGuildLogListBar : _AALBasicUIWndMono
    {
        [ALHeader("日期")]
        public Text txtDate;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4931); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4931); } }
    }
}

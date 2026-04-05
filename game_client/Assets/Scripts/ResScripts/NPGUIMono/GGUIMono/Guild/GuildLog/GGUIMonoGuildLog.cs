using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟日志界面
    /// </summary>
    public class GGUIMonoGuildLog : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("日志列表")]
        public GGUIMonoGuildLogGrid monoGrid;
        [ALHeader("日志保留数量描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4930); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4930); } }
    }
}

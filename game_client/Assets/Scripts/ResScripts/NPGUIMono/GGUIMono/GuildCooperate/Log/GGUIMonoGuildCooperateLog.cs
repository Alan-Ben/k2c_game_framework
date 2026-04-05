using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作日志弹窗
    /// </summary>
    public class GGUIMonoGuildCooperateLog : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("日志列表")]
        public GGUIMonoGuildCooperateLogGrid monoLogGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4937); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4937); } }
    }
}
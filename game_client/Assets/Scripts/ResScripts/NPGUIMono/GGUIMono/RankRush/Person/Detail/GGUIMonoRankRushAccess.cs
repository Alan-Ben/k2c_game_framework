using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜获取途径界面
    /// </summary>
    public class GGUIMonoRankRushAccess : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("获取途径列表")]
        public GGUIMonoRankRushAccessContainer monoAccseeContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3905); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3905); } }
    }
}
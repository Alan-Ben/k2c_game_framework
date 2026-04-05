using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 小游戏暂停弹窗
    /// </summary>
    public class NPGGUIMonoMiniGamePause : _AALBasicUIWndMono
    {
        [ALHeader("继续按钮")]
        public GameObject btnResume;
        [ALHeader("退出按钮")]
        public GameObject btnQuit;
        

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10001); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10001); } }
    }
}

using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 学徒系统主界面
    /// </summary>
    public class GGUIMonoSchool : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1232); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1232); } }
    }
}
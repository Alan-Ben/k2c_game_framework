using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择实验室窗口
    /// </summary>
    public class GGUIMonoTreasureHuntLabSelect : _AALBasicUIWndMono
    {
        [ALHeader("实验室选择容器")]
        public GGUIMonoTreasureHuntLabSelectContainer monoLabContainer;

        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6804); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6804); } }
    }
}
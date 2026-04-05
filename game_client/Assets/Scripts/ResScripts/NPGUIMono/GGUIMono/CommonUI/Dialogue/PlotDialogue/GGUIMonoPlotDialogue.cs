using ALPackage;

namespace GOE
{
    /// <summary>
    /// 剧情对话事件弹窗
    /// </summary>
    public class GGUIMonoPlotDialogue : _AALBasicUIWndMono
    {
        [ALHeader("剧情对话附加窗口")] 
        public GGUIMonoSubPlotDialogue monoSubPlotDialogue;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3401); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3401); } }
    }
}


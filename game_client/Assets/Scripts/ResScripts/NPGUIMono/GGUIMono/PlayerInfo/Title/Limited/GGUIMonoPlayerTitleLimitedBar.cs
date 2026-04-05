using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 限时称号列表bar
    /// </summary>
    public class GGUIMonoPlayerTitleLimitedBar : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1721); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1721); } }
    }
}

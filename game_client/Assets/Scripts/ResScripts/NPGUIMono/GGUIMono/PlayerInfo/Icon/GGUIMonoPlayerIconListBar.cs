using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 头像列表bar
    /// </summary>
    public class GGUIMonoPlayerIconListBar : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1726); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1726); } }
    }
}

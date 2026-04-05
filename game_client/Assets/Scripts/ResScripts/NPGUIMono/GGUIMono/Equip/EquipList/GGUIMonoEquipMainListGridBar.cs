using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品列表bar
    /// </summary>
    public class GGUIMonoEquipMainListGridBar : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1809); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1809); } }
    }
}

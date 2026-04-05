using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴列表已拥有bar
    /// </summary>
    public class GGUIMonoHeroListOwnBar : _AALBasicUIWndMono
    {
        [ALHeader("伙伴已获得数量文本")]
        public Text txtNum;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1030); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1030); } }
    }
}

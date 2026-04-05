using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟建设界面
    /// </summary>
    public class GGUIMonoGuildConstruct : _AALBasicUIWndMono
    {
        [ALHeader("联盟基础信息")]
        public GGUIMonoGuildSubBaseInfo monoBaseInfo;
        [ALHeader("金币建设横栏描述")]
        public Text txtGoldBarDesc;
        [ALHeader("道具建设横栏描述")]
        public Text txtItemBarDesc;
        [ALHeader("金币建设列表")]
        public GGUIMonoGuildConstructContainer monoGoldConstructContainer;
        [ALHeader("道具建设列表")]
        public GGUIMonoGuildConstructContainer monoItemConstructContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4915); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4915); } }
    }
}

using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟等级预览界面
    /// </summary>
    public class GGUIMonoGuildLevelPreview : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("联盟旗帜")]
        public RawImage imgFlag;
        [ALHeader("上个等级信息")]
        public GameObject btnPrevious;
        [ALHeader("下个等级信息")]
        public GameObject btnNext;
        [ALHeader("等级联盟描述")]
        public Text txtLevel;
        [ALHeader("升级效果列表")]
        public GGUIMonoGuildLevelPreviewContainer monoLevelDescContainer;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4904); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4904); } }
    }
}

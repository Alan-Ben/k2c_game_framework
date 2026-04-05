using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场指定谈判结束伙伴获得实力弹窗
    /// </summary>
    public class GGUIMonoArenaBattleFinishHeroAddPower : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("伙伴头像")]
        public RawImage imgHeroIcon;
        [ALHeader("伙伴头像品质背景")]
        public Image imgHeroIconBg;
        [ALHeader("增加的实力")]
        public Text txtAddPower;
        [ALHeader("实力提升描述")]
        public Text txtDesc;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5217); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5217); } }
    }
}
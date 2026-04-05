using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴经营技能自动解锁弹窗
    /// </summary>
    public class GGUIMonoHeroBusinessSkillUnlock : _AALBasicUIWndMono
    {
        [ALHeader("伙伴头像")]
        public RawImage imgHeroIcon;
        [ALHeader("伙伴头像品质底图")]
        public Image imgIconBg;
        [ALHeader("伙伴名称")]
        public Text txtHeroName;
        [ALHeader("解锁描述")]
        public Text txtUnlockDesc;
        [ALHeader("技能描述")]
        public Text txtSkillDesc;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("查看按钮")]
        public GameObject btnGoTo;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1007); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1007); } }
    }
}
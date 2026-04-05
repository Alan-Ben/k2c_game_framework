using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴获得弹窗
    /// </summary>
    public class GGUIMonoHeroGet : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase showCaseWnd;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("相性名称")]
        public Text txtSpecAttrName;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("称号")]
        public Text txtTitle;
        [ALHeader("出生地")]
        public Text txtBirthplace;
        [ALHeader("职业")]
        public Text txtOccupation;
        [ALHeader("简介")]
        public Text txtIntroduction;
        [ALHeader("showcase背景在舞台的下标")]
        public int showcaseBgIndex = 2;



        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1001); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1001); } }
    }
}

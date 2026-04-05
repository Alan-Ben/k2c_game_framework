using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴信息简弹窗
    /// </summary>
    public class GGUIMonoHeroInfoIntroduction : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("称号")]
        public Text txtTitle;
        [ALHeader("职业")]
        public Text txtOccupation;
        [ALHeader("简介")]
        public Text txtIntroduction;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1014); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1014); } }
    }
}
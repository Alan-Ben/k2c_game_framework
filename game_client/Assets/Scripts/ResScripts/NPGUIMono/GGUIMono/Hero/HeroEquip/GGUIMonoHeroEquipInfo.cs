using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴当前佩戴藏品展示界面
    /// </summary>
    public class GGUIMonoHeroEquipInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("替换按钮")] 
        public GameObject btnChange;
        [ALHeader("查看详情按钮")] 
        public GameObject btnDetail;
        [ALHeader("卸下藏品按钮")]
        public GameObject btnRemove;
        [ALHeader("藏品图标")]
        public RawImage imgEquipIcon;
        [ALHeader("额外等级图标")]
        public RawImage imgAdditionLevelIcon;
        [ALHeader("增加实力值")]
        public Text txtPower;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("藏品增加实力百分比")]
        public Text txtEquipAddPer;
        [ALHeader("藏品资质")]
        public Text txtEquipTalent;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("需要根据品质改变颜色的图片")]
        public MaskableGraphic setColorByQualityImage;
        [ALHeader("有更高品质藏品可替换红点提示GO")]
        public GameObject goRedTip;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1016); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1016); } }
    }
}
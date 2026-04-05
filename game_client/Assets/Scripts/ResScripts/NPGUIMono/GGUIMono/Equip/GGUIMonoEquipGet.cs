using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 获得藏品弹窗
    /// </summary>
    public class GGUIMonoEquipGet : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("品质GO父节点")]
        public Transform goQualityParent;
        [ALHeader("等级附加图标")]
        public RawImage imgAdditionQualityIcon;
        [ALHeader("需要根据品质改变颜色的图片")]
        public MaskableGraphic setColorByQualityImage;

        /************
        * 资源加载路径
        */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1808); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1808);} }
    }
}
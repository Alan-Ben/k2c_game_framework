using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴家人使用道具完成界面
    /// </summary>
    public class GGUIMonoBagItemHeroConsortUseResult : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("属性图标")]
        public RawImage imgAttrIcon;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("结果列表")]
        public GGUIMonoBagItemHeroConsortUseResultContainer monoResultContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1917); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1917); } }
    }
}
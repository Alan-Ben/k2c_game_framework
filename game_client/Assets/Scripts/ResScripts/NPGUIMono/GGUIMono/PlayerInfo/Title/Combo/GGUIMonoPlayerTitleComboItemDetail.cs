using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家组合称号详情弹窗
    /// </summary>
    public class GGUIMonoPlayerTitleComboItemDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("称号名称")]
        public Text txtName;
        [ALHeader("称号描述")]
        public Text txtDesc;
        [ALHeader("称号来源")]
        public Text txtSource;
        [ALHeader("称号类型")]
        public Text txtType;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1722); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1722); } }
    }
}
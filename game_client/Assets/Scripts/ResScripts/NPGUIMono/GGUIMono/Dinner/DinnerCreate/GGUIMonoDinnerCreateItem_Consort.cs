using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoDinnerCreateItem_Consort : _AGGUIMonoDinnerCreateItemBase
    {
        [ALHeader("宴会名字")]
        public TextEx txtName;
        [ALHeader("宴会描述")]
        public TextEx txtDesc;
        [ALHeader("凭证倒计时")]
        public TextEx txtLifeTime;
        [ALHeader("妃子卡片形象")]
        public GGUIMonoConsortIconItem consortCardItem;
        /// <summary>
        /// 资源加载路径
        /// </summary>
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2920); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2920);} }
    }
}
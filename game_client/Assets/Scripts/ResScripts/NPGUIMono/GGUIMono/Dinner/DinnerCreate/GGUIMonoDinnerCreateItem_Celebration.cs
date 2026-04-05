using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoDinnerCreateItem_Celebration : _AGGUIMonoDinnerCreateItemBase
    {
        [ALHeader("宴会名字")]
        public TextEx txtName;
        [ALHeader("宴会描述")]
        public TextEx txtDesc;
        [ALHeader("宴会凭证图标")]
        public RawImage texPermitIcon;
        [ALHeader("凭证倒计时")]
        public TextEx txtLifeTime;
        /// <summary>
        /// 资源加载路径
        /// </summary>
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2921); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2921);} }
    }
}
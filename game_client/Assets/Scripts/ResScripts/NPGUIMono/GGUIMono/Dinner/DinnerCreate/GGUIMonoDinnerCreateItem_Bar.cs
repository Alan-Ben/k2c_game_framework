using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoDinnerCreateItem_Bar : _AGGUIMonoDinnerCreateItemBase
    {
        [ALHeader("Bar名字")]
        public TextEx txtName;
        /// <summary>
        /// 资源加载路径
        /// </summary>
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2921); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2921);} }
    }
}
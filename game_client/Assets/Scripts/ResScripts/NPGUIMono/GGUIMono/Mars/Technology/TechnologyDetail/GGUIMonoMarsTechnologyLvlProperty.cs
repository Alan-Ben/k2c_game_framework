using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsTechnologyLvlProperty : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("科技名称文本")]
        public TextEx txtTechnologyName;
        
        [ALHeader("属性列表Grid")]
        public GGUIMonoMarsLvlPropertyShowItemGrid monoPropertyListGrid;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7308); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7308); } }
    }
}

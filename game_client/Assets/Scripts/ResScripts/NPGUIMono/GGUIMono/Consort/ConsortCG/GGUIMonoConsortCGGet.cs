using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 获取CG弹窗
    /// </summary>
    public class GGUIMonoConsortCGGet : _AALBasicUIWndMono
    {
        [ALHeader("cg图片")]
        public RawImage cgImage;

        [ALHeader("CG名")]
        public TextEx txtCGName;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1429); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1429);} }
    }
}
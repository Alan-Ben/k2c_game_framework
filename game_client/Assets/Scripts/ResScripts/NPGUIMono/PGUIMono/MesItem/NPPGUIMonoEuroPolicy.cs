using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// GDPR隐私协议弹窗
    /// </summary>
    public class NPPGUIMonoEuroPolicy : _AALBasicUIWndMono
    {
        [ALHeader("Logo的父节点")]
        public Transform logoParent;
        [ALHeader("按钮")]
        public GameObject btn;

        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
        public static string objName { get { return "win_login_serve_notice"; } }
    }
}

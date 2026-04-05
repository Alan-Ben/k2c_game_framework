
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAddPack : _AALBasicUIWndMono
    {
        [ALHeader("下载器的列表")]
        public GGUIMonoAddPackInstallerContainer installerContainer;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("停止所有下载并关闭按钮")]
        public GameObject btnAbortAll;
        
        /************
       * 资源加载路径
       */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_ADD_PACK); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_ADD_PACK); } }
    }
}
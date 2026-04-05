using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 招募主页面
    /// </summary>
    public class GGUIMonoRecruitMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2700); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2700);} }
    }
}
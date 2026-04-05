using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技加成总览
    /// </summary>
    public class GGUIMonoMasrTechnologyAddOverview : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("属性列表")]
        public GGUIMonoMasrTechnologyAddOverviewPropertyGrid monoPropertyGrid;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7306); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7306);} }
    }
}
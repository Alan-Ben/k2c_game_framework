using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意
    /// </summary>
    public class GGUIMonoMarsPopularWill : _AALBasicUIWndMono
    {
        [ALHeader("满意度进度条")]
        public NPGGUIMonoProgress satisfactionDegreeProgressBar;

        [ALHeader("满意度描述")]
        public TextEx satisfactionDegreeDesc;

        [ALHeader("tab列表")]
        public GGUIMonoMarsPopularWillTabList tabList;
        
        [ALHeader("返回按钮")]
        public List<GameObject> btnReturnList;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7205); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7205);} }
    }
}
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 成就主窗口
    /// </summary>
    public class GGUIMonoAchieveStep : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("成就点进度")]
        public GGUIMonoAchievePointProgress monoAchievePointProgress;
        [ALHeader("步骤列表")]
        public GGUIMonoAchieveStepGrid monoStepGrid;


        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3002); } }
    }
}

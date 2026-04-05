using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 周卡委派形象选择
    /// </summary>
    public class GGUIMonoWeekCardAssignTDShow : _AALBasicUIWndMono
    {
        [ALHeader("执政官形象")]
        public GGUIMonoCommonShowCase assignShowcase;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确定更换按钮")]
        public GameObject btnConfirm;
        [ALHeader("知己页签")]
        public NPGGUIMonoCommonTab tabConsort;
        [ALHeader("骑士页签")]
        public NPGGUIMonoCommonTab tabHero;
        [ALHeader("选择图标列表")]
        public GGUIMonoWeekCardAssignTDShowItemContainer iconItemContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4506); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4506); } }
    }
}
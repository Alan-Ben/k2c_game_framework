using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物放入
    /// </summary>
    public class GGUIMonoTreasureHuntLabTreasurePutIn : _AALBasicUIWndMono
    {
        [ALHeader("描述文本")]
        public TextEx txtDesc;
        [ALHeader("描述文本key(两个参数, 1.奇物名, 2.实验室名)")]
        public string txtDescKey;
        
        [ALHeader("奇物信息子窗口")]
        public GGUIMonoTreasureHuntTreasureInfo monoTreasureInfo;
        
        [ALHeader("解锁技能信息子窗口")]
        public GGUIMonoTreasureHuntSkillInfo monoSkillInfo;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        [ALHeader("前往查看按钮卡")]
        public GameObject btnGoto;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6809); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6809); } }
    }
}
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取奇物窗口
    /// </summary>
    public class GGUIMonoTreasureHuntCaptureTreasureResult : _AALBasicUIWndMono
    {
        [ALHeader("奇物信息")]
        public GGUIMonoTreasureHuntTreasureInfo monoTreasureInfo;

        [ALHeader("可激活技能信息")]
        public GGUIMonoTreasureHuntSkillInfo monoTreasureSkillInfo;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6813); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6813); } }
    }
}
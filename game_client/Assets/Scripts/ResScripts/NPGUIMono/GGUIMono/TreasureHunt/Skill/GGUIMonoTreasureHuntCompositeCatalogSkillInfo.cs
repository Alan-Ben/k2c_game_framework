using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴技能信息
    /// </summary>
    public class GGUIMonoTreasureHuntCompositeCatalogSkillInfo : GGUIMonoTreasureHuntSkillInfo
    {
        [ALHeader("解锁进度描述")]
        public TextEx txtUnlockProcessDesc;
        [ALHeader("解锁进度描述key")]
        public string txtUnlockProcessDescKey;
        
        [ALHeader("解锁进度达到时文本颜色")]
        public Color unlockProcessReachColor = Color.green;
        [ALHeader("解锁进度未达到时文本颜色")]
        public Color unlockProcessNotReachColor = Color.red;
    }
}
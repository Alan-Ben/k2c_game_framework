using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子经营技能阶段
    /// </summary>
    public class GGUIMonoConsortBusinessSkillStage : _AALBasicUIWndMono
    {
        [ALHeader("技能item加载的父节点")]
        public Transform itemParent;
        
        [ALHeader("进度条")]
        public Slider slider;
    }
}
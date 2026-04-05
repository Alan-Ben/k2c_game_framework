using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 经营技能进度条
    /// </summary>
    public class GGUIMonoConsortBusinessSkillSlider : _AALBasicUIWndMono
    {
        [ALHeader("进度条")]
        public Slider slider;
        
        [ALHeader("滑动区域")]
        public ScrollRect scrollRect;

        [ALHeader("item加载的父节点")]
        public Transform itemPrefab;
        [ALHeader("item间隔")]
        public float itemInterval = 100f;
    }
}
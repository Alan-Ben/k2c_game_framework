
using System;
using UnityEngine;
using UnityEngine.UI;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用pk进度条窗口
    /// </summary>
    public class NPGGUIMonoCommonPKSlider : MonoBehaviour
    {
        [ALHeader("滚动条")]
        public Slider sld;
        [ALHeader("左方数值")]
        public Text txtLeftNum;
        [ALHeader("右方数值")]
        public Text txtRightNum;
        
        [ALHeader("左方数值变化适合播放的动画")]
        public Animation leftNumAni;
        [ALHeader("左方数值变化适合播放的动画名")]
        public String leftNumAniName;
        [ALHeader("右方数值变化适合播放的动画")]
        public Animation rightNumAni;
        [ALHeader("右方数值变化适合播放的动画名")]
        public String rightNumAniName;
        
        [ALHeader("sld变化时间（秒）")]
        public float sldChangeTimeS;
        [ALHeader("text值变化时间延迟（秒）")]
        public float textChangeTimeS;
    }
}
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 文本打字机效果子窗口
    /// </summary>
    public class GGUIMonoTextTypewriter : _AALBasicUIWndMono
    {
        [ALHeader("文本txt")]
        public TextEx txt;
        
        [ALHeader("每个字符显示间隔时间(毫秒)")]
        public float perCharShowIntervalMs;

        [ALHeader("点击显示全部文本内容按钮")]
        public GameObject btnShowAllText;
    }
}
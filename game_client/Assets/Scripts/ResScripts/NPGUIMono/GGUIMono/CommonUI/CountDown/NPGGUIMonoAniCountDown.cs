using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用带特殊动画倒计时
    /// </summary>
    public class NPGGUIMonoAniCountDown : _AALBasicUIWndMono
    {
        [ALHeader("需要开始特殊展示的秒数n")]
        public long specialShowMiniSec = -1;
        [ALHeader("倒计时翻译key")]
        public string secTransKey;
        [ALHeader("正常倒计时文本")]
        public Text txtNormalCD;
        [ALHeader("特殊倒计时文本")]
        public Text txtSpecialCD;

        [ALHeader("小于等于n秒时需要展示的GO列表")]
        public List<GameObject> goSpecShowList;
        [ALHeader("小于等于n秒时需要隐藏的GO列表")]
        public List<GameObject> goSpecHideList;

        [ALHeader("小于等于n秒时每秒播放动画")]
        public Animation specialAnimation;
        [ALHeader("小于等于n秒时每秒播放动画名称")]
        public string specialAnimationName;
    }
}

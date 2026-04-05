using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 简单的倒计时
    /// </summary>

    public class NPGGUIMonoCommonCountDown : _AALBasicUIWndMono
    {

        [ALHeader("倒计时文本")]
        public Text countDownTxt;

        [ALHeader("倒计时文本")]
        public TextMeshProUGUIEx countDownTxtMeshPro;

        [ALHeader("当倒计时小于多少秒时文本颜色变化")]
        public long minTimeS;

        [ALHeader("变化之前的颜色")]
        public Color normalColor = Color.yellow;

        [ALHeader("变化之后的颜色")]
        public Color chgColor = Color.red;

        [ALHeader("倒计时为0需要显示的GoList")]
        public List<GameObject> stopTickShowGoList;

        [ALHeader("倒计时为0需要隐藏的GoList")]
        public List<GameObject> stopTickHideGoList;
    }
}

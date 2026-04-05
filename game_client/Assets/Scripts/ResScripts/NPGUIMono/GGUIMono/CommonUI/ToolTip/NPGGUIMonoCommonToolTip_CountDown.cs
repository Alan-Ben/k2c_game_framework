using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 倒计时跟随的子窗口基类
    /// </summary>
    public class NPGGUIMonoCommonToolTip_CountDown : NPGGUIMonoCommonToolTip
    {
        [ALHeader("第一个倒计时")]
        public NPGGUIMonoCommonCountDown countDownOne;

        [ALHeader("第二个倒计时")]
        public NPGGUIMonoCommonCountDown countDownTwo;
    }
}
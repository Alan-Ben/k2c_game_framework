using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum ENPCommonCdBtnStatus
    {
        NORMAL,//正常
        CD,//CD中
    }

    /// <summary>
    /// 显示冷却时间的按钮
    /// </summary>
    public class NPGGUIMonoCommonCdBtn : _ATNPGGUIMonoStateBtn<ENPCommonCdBtnStatus>
    {
        [ALHeader("刷新间隔")]
        [Min(0.1f)]
        public float refreshInterval = 1f;
        [ALHeader("剩余冷却时间文本")]
        public Text txtCdLeftTime;
        [ALHeader("剩余冷却时间文本key")]
        public string leftTimeKey = TransKeyConst.common_value;
    }
}

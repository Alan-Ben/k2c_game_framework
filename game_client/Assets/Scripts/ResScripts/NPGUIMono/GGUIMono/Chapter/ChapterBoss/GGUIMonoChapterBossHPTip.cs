using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 血条调字tip
    /// </summary>
    public class GGUIMonoChapterBossHPTip : _AALBasicUIWndMono
    {
        [ALHeader("出现动画")]
        public Animation showAni;
        [ALHeader("血量文本")]
        public Text txtHp;
    }
}

using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带奖励列表的进度条
    /// </summary>
    public class GGUIMonoCommonRewardSliderPro : _AALBasicUIWndMono
    {
        [ALHeader("当前进度文本")]
        public TextEx txtCurProcess;
        [ALHeader("进度条")] 
        public Slider sldProcess;

        [ALHeader("每个奖励位置是否平分进度条")]
        public bool isDivideEqually;
        
        [ALHeader("进度条满时特效id")]
        public long fullSfxId;
        [ALHeader("进度条满时特效父节点")]
        public Transform fullSfxParent;
    }
}
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 充值返利主页面
    /// </summary>
    public class GGUIMonoRechargeRebatePage : _AALBasicUIWndMono
    {
        [ALHeader("顶部横幅图片")]
        public RawImage imgBanner;
        [ALHeader("标题父节点")]
        public Transform goTitleParent;
        [ALHeader("倒计时文本")]
        public Text txtLeftTime;
        [ALHeader("计数描述")]
        public Text txtCountDesc;
        [ALHeader("描述文本")]
        public Text txtDesc;
        [ALHeader("页签列表")]
        public GGUIMonoRechargeRebatePageTabContainer monoTabContainer;
        [ALHeader("组步骤列表")]
        public GGUIMonoRechargeRebatePageStepContainer monoStepContainer;
    }
}


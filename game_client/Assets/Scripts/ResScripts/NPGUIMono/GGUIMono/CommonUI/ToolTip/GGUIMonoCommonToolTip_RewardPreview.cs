using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoCommonToolTip_RewardPreview : NPGGUIMonoCommonToolTip
    {
        [ALHeader("标题文本")]
        public Text titleTxt;

        [ALHeader("内容文本")]
        public TextEx contentTxt;

        [ALHeader("奖励容器")]
        public NPGGUIMonoCommonItemContainer itemContainerMono;
    }
}

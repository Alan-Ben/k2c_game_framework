using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历博彩事件结果窗口 Mono
    /// </summary>
    public class GGUIMonoTravelGambleEventResult : _ATravelResultMono
    {
        [ALHeader("成功时显示的对象列表")]
        public List<GameObject> winShowList;

        [ALHeader("失败时显示的对象列表")]
        public List<GameObject> loseShowList;

        [ALHeader("JACKPOT大奖时显示的对象列表")]
        public List<GameObject> jackpotShowList;

        [ALHeader("押注变化量文本（Gain:+90 / Loss:-90）")]
        public TextEx txtDiamondChange;

        [ALHeader("押注前钻石通用组件")]
        public GGUIMonoCommonSimpleItem monoBeforeItem;

        [ALHeader("押注后钻石通用组件")]
        public GGUIMonoCommonSimpleItem monoAfterItem;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static long uiResPathId { get { return 3635; } }
    }
}

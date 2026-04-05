using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排行礼包入口item
    /// </summary>
    public class GGUIMonoRankGiftPackPointItem : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnCLick;
        [ALHeader("倒计时")]
        public Text txtTime;
        [ALHeader("倒计时2")]
        public Text txtTime2;  
        
        [ALHeader("已售罄显示的go列表")]
        public List<GameObject> limitShowGoList;
        [ALHeader("未购买显示的go列表")]
        public List<GameObject> normalShowGoList;
    }
}
using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民补充Hub
    /// </summary>
    public class GGUIMonoMarsResidentReplenishFollower : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("点击按钮")] 
        public GameObject btnClick;

        [ALHeader("倒计时文本")]
        public TextEx txtCountDown;
        public TMP_Text tmpCountDown;
        
        [ALHeader("居民补充状态显示列表")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>> replenishStateShowList;
    }
}
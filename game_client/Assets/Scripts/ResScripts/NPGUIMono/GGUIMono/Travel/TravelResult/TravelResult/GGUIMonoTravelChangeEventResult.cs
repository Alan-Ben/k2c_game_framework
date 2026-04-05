using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 交换事件结果窗口
    /// </summary>
    public class GGUIMonoTravelChangeEventResult : _ATravelResultMono
    {
        [ALHeader("有增加体力值时显示")]
        public List<GameObject> hasAddLazyCdShow;
        [ALHeader("有增加体力值时隐藏")]
        public List<GameObject> hasAddLazyCdHide;
        
        [ALHeader("获取的游历体力值")]
        public TextEx gainTravelCostLazyCd;
        [ALHeader("获取的游历体力值key(两个参数, 1.体力值名称, 2.体力值数量)")]
        public string gainTravelCostLazyCdKey;
        
        public static long uiResPathId { get { return 3607; } }
    }
}
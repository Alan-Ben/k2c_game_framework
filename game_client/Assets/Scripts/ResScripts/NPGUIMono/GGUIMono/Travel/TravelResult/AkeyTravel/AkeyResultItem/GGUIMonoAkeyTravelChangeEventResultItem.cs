using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历 - 交换事件结果item
    /// </summary>
    public class GGUIMonoAkeyTravelChangeEventResultItem : _AGGUIMonoAkeyTravelResultItem
    {
        [ALHeader("有增加体力值时显示")]
        public List<GameObject> hasAddLazyCdShow;
        [ALHeader("有增加体力值时隐藏")]
        public List<GameObject> hasAddLazyCdHide;

        [ALHeader("获取的游历体力值")]
        public TextEx gainTravelCostLazyCd;
        [ALHeader("获取的游历体力值key(两个参数, 1.体力值名称, 2.体力值数量)")]
        public string gainTravelCostLazyCdKey;
    }
}

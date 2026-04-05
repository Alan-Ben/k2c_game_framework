using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公告召唤记录Grid
    /// </summary>
    public class GGUIMonoSummonPublicRollRecordGrid : _ATNPGGUIMonoRefreshGrid<GGUIMonoSummonPublicRollRecordGridItem>
    {
        [ALHeader("没有数据时显示")]
        public List<GameObject> noItemShow;
        
        [ALHeader("显示召唤记录数量")]
        public int showItemCount = 10;
        
        [ALHeader("请求新数据的间隔时间")]
        public float reqNewDataInterval = 10f;
    }
}
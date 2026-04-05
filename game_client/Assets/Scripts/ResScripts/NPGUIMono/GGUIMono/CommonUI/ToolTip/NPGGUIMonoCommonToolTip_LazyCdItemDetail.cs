using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 物品详情
    /// </summary>
    public class NPGGUIMonoCommonToolTip_LazyCdItemDetail : NPGGUIMonoCommonToolTip_ItemDetail
    {
        [ALHeader("cd计数满时显示物体列表")]
        public List<GameObject> cdCountFullShowList;
        [ALHeader("cd计数空时显示物体列表")]
        public List<GameObject> cdCountEmptyShowList;
        [ALHeader("cd计数不满不空时显示物体列表")]
        public List<GameObject> cdCountNeitherFullNorEmptyShowList;
        
        [ALHeader("下次恢复时间倒计时")]
        public Text txtTimeNext;
        [ALHeader("下次恢复时间倒计时key, 一个参数:时间倒计时")]
        public string txtTimeNextKey;

        [ALHeader("恢复满的倒计时")]
        public Text txtTimeMax;
        [ALHeader("恢复满的倒计时Key, 一个参数:时间倒计时")]
        public string txtTimeMaxKey;

        [ALHeader("每次恢复需要的时间")]
        public Text txtAddPerNeedTime;
        [ALHeader("每次恢复需要的时间Key, 一个参数:恢复时间")]
        public string txtAddPerNeedTimeKey;

        [ALHeader("在显示数量时是否显示能获取的最多数量")]
        public bool needShowMaxCount = true;
        [ALHeader("显示数量时使用的key, 一个参数:数量(或 数量/最大数量 具体参数显示哪种由needShowMaxCount决定)")]
        public string txtNumKey;
    }
}
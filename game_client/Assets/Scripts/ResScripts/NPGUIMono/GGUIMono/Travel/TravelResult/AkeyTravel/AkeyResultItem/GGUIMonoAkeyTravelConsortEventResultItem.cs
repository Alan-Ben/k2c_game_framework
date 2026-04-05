using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历 - 妃子相关事件结果item
    /// </summary>
    public class GGUIMonoAkeyTravelConsortEventResultItem : _AGGUIMonoAkeyTravelResultItem
    {
        [ALHeader("有亲密度增加显示物体列表")]
        public List<GameObject> hasAddIntimacyShowGoList;
        [ALHeader("增加的亲密度")]
        public TextEx txtAddIntimacy;
        [ALHeader("增加的亲密度key")]
        public string txtAddIntimacyKey;

        [ALHeader("有好感度增加显示物体列表")]
        public List<GameObject> hasAddLikeShowGoList;
        [ALHeader("好感度进度条")]
        public NPGGUIMonoProgress monoLikeSlider;
        [ALHeader("增加的好感度")]
        public TextEx txtAddLike;
        [ALHeader("增加的好感度key")]
        public string txtAddLikeKey;
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTravelConsortEventResult : _ATravelResultMono
    {
        [ALHeader("妃子半身像")]
        public RawImage consortMidIcon;
        
        [ALHeader("玩家半身像")]
        public RawImage playerMidIcon;

        [ALHeader("有增加好感度时显示")]
        public List<GameObject> hasAddLikeShow;
        [ALHeader("好感度进度条")]
        public NPGGUIMonoProgress monoLikeSlider;
        [ALHeader("增加的好感度")]
        public TextEx txtAddLike;
        [ALHeader("增加的好感度key(一个参数 增加的好感度值)")]
        public string txtAddLikeKey;
        
        [ALHeader("有增加亲密度时显示")]
        public List<GameObject> hasAddIntimacyShow;
        [ALHeader("增加的亲密度")]
        public TextEx txtAddIntimacy;
        [ALHeader("增加的亲密度key(一个参数 增加的亲密度值)")]
        public string txtAddIntimacyKey;
        
        public static long uiResPathId { get { return 3612; } }
    }
}
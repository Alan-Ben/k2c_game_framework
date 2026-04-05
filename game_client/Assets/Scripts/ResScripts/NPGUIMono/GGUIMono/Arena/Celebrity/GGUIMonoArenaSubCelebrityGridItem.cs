using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜列表item
    /// </summary>
    public class GGUIMonoArenaSubCelebrityGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("谈判对象名称")]
        public Text txtFightName;
        [ALHeader("击败伙伴数量")]
        public Text txtDefeatCount;
        [ALHeader("谈判类型文本")]
        public Text txtFightType;
        [ALHeader("谈判按钮")]
        public GameObject btnAttack;
        [ALHeader("时间")]
        public Text txtTime;
        [ALHeader("是自己时显示的GO列表")]
        public List<GameObject> goSelfShowList;
        [ALHeader("是自己时隐藏的GO列表")]
        public List<GameObject> goSelfHideList;
    }
}
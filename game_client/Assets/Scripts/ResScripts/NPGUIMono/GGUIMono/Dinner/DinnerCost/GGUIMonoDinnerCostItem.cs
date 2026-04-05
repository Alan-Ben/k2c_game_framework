using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会赴宴消耗item
    /// </summary>
    public class GGUIMonoDinnerCostItem : _AALBasicUIWndMono
    {
        [ALHeader("加入按钮")]
        public GameObject btnJoin;
        [ALHeader("保存按钮")]
        public GameObject btnSave;
        [ALHeader("加成详情按钮")]
        public GameObject btnDetail;
        [ALHeader("名称")]
        public TextEx txtName;
        [ALHeader("可用次数")]
        public TextEx txtFixedCd;
        [ALHeader("赴宴宴会币")]
        public TextEx txtJoinCoin;
        [ALHeader("赴宴积分")]
        public TextEx txtJoinScore;
        [ALHeader("当前拥有的消耗物品数量")]
        public TextEx txtCostCount;
        [ALHeader("消耗")]
        public NPGGUIMonoCommonItem costItem;
        [ALHeader("赴宴方式不可用或次数用完的时候置灰对象")]
        public List<MaskableGraphic> goListGray;
        [ALHeader("赴宴方式不可用的时候显示")]
        public List<GameObject> emptyListShow;
        [ALHeader("赴宴方式不可用的时候隐藏")]
        public List<GameObject> emptyListHide;

        [ALHeader("赴宴次数没有限制的时候隐藏，有限制的时候显示")]
        public List<GameObject> goListHideCountNoLimit;
        
        [ALHeader("快速赴宴需要显示的对象")]
        public List<GameObject> quickJoinShow;
        [ALHeader("快速赴宴需要隐藏的对象")]
        public List<GameObject> quickJoinHide;
        [ALHeader("赴宴加成详情Tip加载节点")]
        public RectTransform toolTipsRoot;
    }
}

using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoCommonBonusShower : _AALBasicUIWndMono
    {
        [ALHeader("要展示哪个属性")]
        public EBonusPropertyType bonusType;
        [ALHeader("文本")]
        public Text txtBonus;
        [ALHeader("文本 key ")]
        public string txtBonusKey;
        [ALHeader("是否是百分比")]
        public bool isPercentage;
        [ALHeader("加成为0时需要显示的GO列表")]
        public List<GameObject> goZeroShowList;
        [ALHeader("加成为0时需要隐藏的GO列表")]
        public List<GameObject> goZeroHideList;
    }
}
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 钻石商店列表item
    /// </summary>
    public class GGUIMonoCashGiftPackGemContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("获得钻石数量")]
        public Text txtGainNum;
        [ALHeader("额外获得数量")]
        public Text txtAddNum;
        [ALHeader("获得VIP经验")]
        public Text txtVIPExp;
        [ALHeader("价格")]
        public Text txtPrice;
        [ALHeader("首充时需要展示的GO列表")]
        public List<GameObject> goFirstShowList;
        [ALHeader("首充时需要隐藏的GO列表")]
        public List<GameObject> goFirstHideList;
        [ALHeader("没有额外获得数量时需要显示的GO列表")]
        public List<GameObject> goNoAddNumShowList;
        [ALHeader("没有额外获得数量时需要隐藏的GO列表")]
        public List<GameObject> goNoAddNumHideList;
    }
}

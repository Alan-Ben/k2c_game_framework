using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    /// 宴会举办历史item
    /// </summary>
    public class GGUIMonoDinnerStartItem : _TALUGUIMonoGridItem
    {
        [ALHeader("赴宴玩家信息按钮")]
        public GameObject btnClick;
        [ALHeader("举办时间")]
        public TextEx txtTime;
        [ALHeader("宴会参加人数")]
        public TextEx txtJoinCount;
        [ALHeader("宴会积分")]
        public TextEx txtScore;
        [ALHeader("宴会名字")]
        public TextEx txtName;
        [ALHeader("宴会图片")]
        public RawImage texBanner;
        [ALHeader("妃子宴会的描述")]
        public TextEx txtConsortDesc;
        [ALHeader("妃子头像")]
        public GGUIMonoConsortIconItem consortIconItem;
        [ALHeader("妃子宴会需要显示的go")]
        public List<GameObject> consortDinnerShowGos;
        [ALHeader("妃子宴会需要隐藏的go")]
        public List<GameObject> consortDinnerHideGos;
        [ALHeader("宴会类型显隐")]
        public List<DinnerTypeShow> dinnerTypeShowList;
    }
}

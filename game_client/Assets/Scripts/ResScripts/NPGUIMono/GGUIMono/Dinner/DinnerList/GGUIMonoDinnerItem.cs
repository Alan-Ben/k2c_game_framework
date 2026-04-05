using System;
using System.Collections.Generic;
using ALPackage;
using ClientEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会加入的状态
    /// </summary>
    public enum EDinnerJoinStat
    {
        NO_JOINED,//未加入的其他人宴会
        JOINED,//已加入的宴会
    }

    [Serializable]
    public class DinnerTypeShow
    {
        [ALHeader("状态类型")] public EDinnerShowType dinnerType;
        [ALHeader("当前状态显示的go列表")] public List<GameObject> goListShow;
        [ALHeader("当前状态隐藏的go列表")] public List<GameObject> goListHide;

        public static void SetDinnerType(List<DinnerTypeShow> _list, EDinnerShowType _type)
        {
            if (_list != null)
                foreach (DinnerTypeShow dinnerTypeShow in _list)
                {
                    if (dinnerTypeShow != null && dinnerTypeShow.dinnerType == _type)
                    {
                        ALUGUICommon.setGameObjEnable(dinnerTypeShow.goListShow,true);
                        ALUGUICommon.setGameObjEnable(dinnerTypeShow.goListHide, false);
                    }
                }
        }
    }

    /// <summary>
    /// 宴会item
    /// </summary>
    public class GGUIMonoDinnerItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("已参加按钮")]
        public GameObject btnEntered;
        [ALHeader("宴会名称")]
        public TextEx txtDinnerName;
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon playerIcon;
        [ALHeader("妃子头像")]
        public GGUIMonoConsortIconItem consortIconItem;
        [ALHeader("子嗣头像")]
        public GGUIMonoChildInfo childCardItem;
        [ALHeader("宴会人数")]
        public TextEx txtCount;
        [ALHeader("宴会人气")]
        public TextEx txtScore;
        [ALHeader("宴会倒计时")]
        public TextEx txtCD;
        [ALHeader("宴会图片")]
        public RawImage texBanner;
        [ALInfo("NO_JOINED,//未加入的其他人宴会\n" +
                "JOINED,//已加入的宴会")]
        [ALHeader("加入的状态显示")]
        public List<NPCommonEnumStatInfo<EDinnerJoinStat>> statInfos;
        [ALHeader("宴会类型显隐")]
        public List<DinnerTypeShow> dinnerTypeShowList;
    }
}

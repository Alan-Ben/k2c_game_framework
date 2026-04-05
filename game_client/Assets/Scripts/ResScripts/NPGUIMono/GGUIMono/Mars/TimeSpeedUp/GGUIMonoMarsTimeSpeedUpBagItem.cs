using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsTimeSpeedUpBagItem : _AALBasicUIWndMono
    {
        [ALHeader("物品")]
        public NPGGUIMonoCommonItem item;
        [ALHeader("减少时间文本")]
        public TextEx txtReduceTime;
        [ALHeader("选中时显示的对象")]
        public List<GameObject> selectShowGoList;
        [ALHeader("点击对象")]
        public GameObject clickGo;
        [ALHeader("物品数量为空时显示")]
        public List<GameObject> zeroNumShow;
    }
}
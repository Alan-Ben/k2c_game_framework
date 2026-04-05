using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsTimeSpeedUpBagItemContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoMarsTimeSpeedUpBagItem>
    {
        [ALHeader("没有物品时显示的物体列表")]
        public List<GameObject> noItemShowGoList;
        [ALHeader("没有物品时隐藏的物体列表")]
        public List<GameObject> noItemHideGoList;
    }
}
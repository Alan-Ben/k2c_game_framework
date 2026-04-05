using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣卡片
    /// </summary>
    public class GGUIMonoGuildDispatchHeroCard : _AALBasicUIWndMono
    {
        [ALHeader("大臣卡片item")]
        public GGUIMonoHeroCommonCardItem monoHeroCardItem;

        [ALHeader("选中时显示物体列表")]
        public List<GameObject> selectedShowGoList;
        
        [ALHeader("加成百分比")]
        public TextEx txtAddPro;
        [ALHeader("加成百分比key")]
        public string txtAddProKey;
    }
}
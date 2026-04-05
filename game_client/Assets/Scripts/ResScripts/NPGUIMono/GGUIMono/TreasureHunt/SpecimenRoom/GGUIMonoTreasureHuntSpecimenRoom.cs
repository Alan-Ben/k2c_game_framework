using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 标本间
    /// </summary>
    public class GGUIMonoTreasureHuntSpecimenRoom : _AALBasicUIWndMono
    {
        [ALHeader("矿石列表")]
        public GGUIMonoTreasureHuntOreItemGrid monoOreItemGrid;

        [ALHeader("没有矿石待处理时显示")]
        public List<GameObject> noOrePendingShow;
        [ALHeader("有矿石待处理时显示")]
        public List<GameObject> hasOrePendingShow;
        
        [ALHeader("处理按钮")]
        public GameObject btnDeal;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}
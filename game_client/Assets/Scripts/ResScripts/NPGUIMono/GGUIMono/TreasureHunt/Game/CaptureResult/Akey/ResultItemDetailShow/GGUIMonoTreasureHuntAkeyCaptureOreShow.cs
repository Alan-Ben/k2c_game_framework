using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键捕获矿石展示
    /// </summary>
    public class GGUIMonoTreasureHuntAkeyCaptureOreShow : _AALBasicUIWndMono
    {
        [ALHeader("矿石信息")]
        public GGUIMonoTreasureHuntOreInfo monoOreInfo;
        
        [ALHeader("首次获得显示列表")]
        public List<GameObject> firstGotShowList;
        [ALHeader("首次获取隐藏列表")]
        public List<GameObject> firstGotHideList;

        [ALHeader("本服矿石质量记录")]
        public TextEx txtServerOreMassRecord;
        [ALHeader("本服矿石质量记录所有者名称")]
        public TextEx txtServerOreMassRecordOwnerName;
    }
}
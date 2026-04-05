using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物选择Item
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureSelectItem : _AALBasicUIWndMono
    {
        [ALHeader("奇物信息子窗口")]
        public GGUIMonoTreasureHuntTreasureInfo treasureInfoMono;

        [ALHeader("选中时显示的物体")]
        public List<GameObject> goSelectShowList;

        [ALHeader("选中时隐藏的物体")]
        public List<GameObject> goSelectHideList;
    }
}
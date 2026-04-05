using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoTreasureHuntLab : MonoBehaviour
    {
        // [ALHeader("奇物列表")]
        // public List<GTDMonoTreasureHuntLabTreasure> treasureList;

        [ALHeader("实验室未解锁时显示")]
        public List<GameObject> labLockShow;
        [ALHeader("实验室解锁时显示")]
        public List<GameObject> labUnlockShow;
        
        [ALHeader("奇物加载父节点")]
        public Transform treasureLoadParent;

        [ALHeader("有选中奇物时显示")]
        public List<GameObject> hasSelectedTreasureShow;
        [ALHeader("没有选中奇物时显示")]
        public List<GameObject> noSelectedTreasureShow;
    }
}
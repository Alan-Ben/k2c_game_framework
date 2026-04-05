using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class GGUISubMonoTileMatchGuestShow : _AHotfixBaseMono
    {
        [HotfixMono("客人预制加载父节点")]
        public Transform guestLoadParent;

        [HotfixMono("客人位置列表")]
        public List<Transform> guestPosTransList;

        [HotfixMono("客人起始位置")]
        public Transform guestOriTrans;
        
        [HotfixMono("首次进入时, 每一位客人开始移动的延迟时间")]
        public float firstEnterPerGuestStartMoveDelayTime = 0.2f;
        
        [HotfixMono("客人预制GoIndex列表")]
        public List<NPGGoIndex> guestPrefabGoIndexList;
        
        [HotfixMono("客人进场移动时间")]
        public float guestEnterMoveTime = 1f;
        [HotfixMono("客人离场移动时间")]
        public float guestExitMoveTime = 0.5f;
        [HotfixMono("客人前进移动时间")]
        public float guestAdvanceMoveTime = 0.5f;

        [HotfixMono("服务员位置")]
        public Transform waiterTrans;
        
        [HotfixMono("服务员预制GoIndex列表")]
        public List<NPGGoIndex> waiterPrefabGoIndexList;
    }
}
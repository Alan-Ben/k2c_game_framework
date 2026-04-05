using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{

    /// <summary>
    /// 大学座位mono
    /// </summary>
    public class GTDCollegePosItemMono : MonoBehaviour
    {
        [ALHeader("大学座位下标   从1开始")]
        public int posIdx;

        [ALHeader("UI跟随的节点")]
        public Transform followParent;

        [ALHeader("功能入口点的点击脚本")]
        public GTDCommonPosClickMono clickMono;
        
        [ALHeader("不同状态显示的GoList")]
        public List<NPCommonEnumStatInfo<ECollegePosStatus>> statusList;

        [ALHeader("缩放的值")]
        public float scaleValue = 1;
    }

}
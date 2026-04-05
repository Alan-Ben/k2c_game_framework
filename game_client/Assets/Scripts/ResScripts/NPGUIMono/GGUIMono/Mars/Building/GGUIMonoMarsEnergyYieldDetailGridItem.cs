using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsEnergyYieldDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("序号")]
        public Text txtNum;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("产速")]
        public Text txtYieldSpeed;
        [ALHeader("产量进度")]
        public Text txtYieldProgress;
        public Slider sldYieldProgress;
        [ALHeader("产量进度颜色列表")] 
        public Image imgYieldProgressFill;
        public List<GGUIMarsEnergyYieldDetailGridItemProgressColor> listProgressColor;
        [ALHeader("跳转到建筑按钮")]
        public GameObject btnJump;
        [ALHeader("跳转动画时间")]
        public float jumpTime = 0.5f;
        
        
        public void setProgressColor(float _normalize)
        {
            if (imgYieldProgressFill == null || listProgressColor == null || listProgressColor.Count == 0)
                return;

            foreach (GGUIMarsEnergyYieldDetailGridItemProgressColor item in listProgressColor)
            {
                if (item is { normalizeRange: not null } && item.normalizeRange.inRange(_normalize))
                {
                    imgYieldProgressFill.color = item.color;
                    return;
                }
            }
        }
    }


    [Serializable]
    public class GGUIMarsEnergyYieldDetailGridItemProgressColor
    {
        [ALHeader("进度范围")
        ,ALInfo("0~1 之间")]
        public WCGFloatRange normalizeRange;
        [ALHeader("进度颜色")]
        public Color color;
    }
}
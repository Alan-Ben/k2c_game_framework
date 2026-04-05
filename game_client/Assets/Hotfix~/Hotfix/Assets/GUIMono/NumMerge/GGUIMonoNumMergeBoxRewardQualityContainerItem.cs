
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBoxRewardQualityContainerItem : _AHotfixBaseMono
    {
        [HotfixMono("品质显示GO")]
        public GGUISubMonoQualityShowGo monoQualityShowGo;
        [HotfixMono("概率文本")]
        public Text txtProbability;
        [HotfixMono("需要作用颜色的对象列表")]
        public List<Graphic> listColorTargets;
        [HotfixMono("对比上一级有提升时的颜色")]
        public Color colorUpgrade;
        [HotfixMono("正常的颜色")]
        public Color colorNormal;
        
        
        public void setIsUpgrade(bool _isUpgrade)
        {
            Color targetColor = _isUpgrade ? colorUpgrade : colorNormal;
            ALUGUICommon.setUIObjColor(listColorTargets, targetColor);
        }
    }
}
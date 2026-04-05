using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠item
    /// </summary>
    public class GGUIMonoGuildDonateItem : _AALBasicUIWndMono
    {
        [ALHeader("名称")]
        public TextEx txtName;

        [ALHeader("图标")]
        public RawImage icon;
        
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        [ALHeader("fixedCd Mono")]
        public GGUIMonoCommonFixedCd monoFixedCd;

        [ALHeader("有免费捐赠条件时显示物体")]
        public List<GameObject> hasFreeDonateConditionShow;
        [ALHeader("显示免费捐赠描述ToolTip按钮")]
        public GameObject btnShowFreeDonateDescToolTip;
        [ALHeader("免费捐赠描述ToolTip资源ID")]
        public long freeDonateDescToolTipResId;
        [ALHeader("免费捐赠描述弹窗位置偏移")]
        public Vector2 freeDonateDescToolTipOffset;
        
        [ALHeader("消耗道具列表")]
        public NPGGUIMonoCommonItem monoCostItem;

        [ALHeader("免费捐赠显示")]
        public List<GameObject> freeDonateShowGoList;
        [ALHeader("免费捐赠隐藏")]
        public List<GameObject> freeDonateHideGoList;
        
        [ALHeader("捐赠按钮")]
        public GameObject btnDonate;
        
        
    }
}
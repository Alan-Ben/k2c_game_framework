using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴家人通用展示头像列表item
    /// </summary>
    public class GGUIMonoHeroConsortSimpleIconContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("头像品质背景")]
        public Image imgIconBg;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("解锁状态配置")] 
        public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> lockStatInfos;
        [ALHeader("未解锁时名称颜色")]
        public Color lockNameColor = Color.gray;
        [ALHeader("已解锁时名称颜色")]
        public Color unlockNameColor = Color.gray;
        [ALHeader("详情tip的x偏移量")]
        public float toolTipIntervalX;
        [ALHeader("详情tip的y偏移量")]
        public float toolTipIntervalY;
    }
}

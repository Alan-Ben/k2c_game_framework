using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{

    /// <summary>
    /// 士兵入口跟随tip
    /// </summary>
    public class GGUIMonoLevySoldierPointItem : _AGGUIMonoEntryPointFollowItemBase
    {
        [ALHeader("征收按钮")]
        public GameObject levyClickGo;

        [ALHeader("征收动画")]
        public Animation levyAnimation;

        [ALHeader("征收动画名称")]
        public string levyAnimationStr;

        [ALHeader("征收消耗不足提示key")]
        public string canNotLevyNoCostShowTipKey;

        [ALHeader("当前可点击次数")]
        public Text canClickCountTxt;

        [ALHeader("有点击次数需要显示的GoList 没有次数隐藏")]
        public List<GameObject> hasCountShowGoList;

        [ALHeader("长按连续间隔（秒）")]
        [Min(0.1f)]
        public float longPressInterval = 0.1f;

        [ALHeader("弹出tip位置")]
        public RectTransform tipStartPos;
        [ALHeader("弹出例子位置")]
        public RectTransform particleStartPos;

        [ALHeader("上浮提示显示参数")]
        [ALHeader("最高速率")]
        public float tipAccMaxRate = 1.0f;

        [ALHeader("待显示tip达到这个数开始加速")]
        public int tipAccMinNum;

        [ALHeader("待显示tip达到这个数达到最高速")]
        public int tipAccMaxNum;

        [ALHeader("暴击动画")]
        public Animation bigCritAni;
        [ALHeader("暴击动画名称")]
        public string bigCritAniStr;
        [ALHeader("暴击文本")]
        public TextMeshProUGUIEx bigCritTxt;

        [ALHeader("暴击特效弹出位置")]
        public Transform critSfxParPos;
        [ALHeader("暴击特效缩放")]
        public float critSfxScale;
        [ALHeader("暴击特效飞行时间(秒)")]
        public float critSfxFlyTime;
        
        [ALHeader("显示大数字特效缩放")]
        public float bigTxtSfxScale;

        [ALHeader("点完最后一次过多久点击才弹窗 单位秒")] 
        public float lastCilckShowTimeS = 2.0f;

        [ALHeader("冷却时间提示key")]
        public string lastCilckShowKey;
    }
}
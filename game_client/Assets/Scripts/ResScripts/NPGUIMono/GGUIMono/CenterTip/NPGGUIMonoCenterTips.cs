using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GOE
{
    /// <summary>
    /// 队列tip显示参数
    /// </summary>
    [Serializable]
    public class NPGGUITipShowParam
    {
        [ALHeader("tip显示类型")]
        public ENPTipShowType showType;

        [ALHeader("tip数量超过这个值，则加速播放")]
        public int tipsSpeedLimitNum = 10;

        [ALHeader("tip数量达到这个值，以最高速率播放")]
        public int tipsSpeedMaxNum = 20;

        [ALHeader("加速播放倍率上限")]
        public float tipsSpeedLimitRate = 2f;

        [ALHeader("独立的父节点，为空则使用统一的父节点")]
        public Transform parentGo;
    }

    /// <summary>
    /// 上浮提示
    /// </summary>
    public class NPGGUIMonoCenterTips : _AALBasicUIWndMono
    {
        [ALHeader("tip父节点")]
        public Transform parentGo;

        [ALHeader("队列tip显示参数")]
        public List<NPGGUITipShowParam> tipShowParamList;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_CENTER_TIPS); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_CENTER_TIPS); } }
    }
}

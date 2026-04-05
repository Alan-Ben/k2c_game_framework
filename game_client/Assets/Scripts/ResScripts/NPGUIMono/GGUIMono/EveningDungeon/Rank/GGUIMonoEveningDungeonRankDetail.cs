using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排行详情窗口
    /// </summary>
    public class GGUIMonoEveningDungeonRankDetail : _AALBasicUIWndMono
    {
        [ALHeader("排行榜列表")]
        public GGUIMonoEveningDungeonRankDetailGrid monoRankGrid;
        
        [ALHeader("我的排名")]
        public Text txtMyRank;
        [ALHeader("我的分数")]
        public Text txtMyScore;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5502); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5502); } }
    }
}

using ALPackage;
using ChatPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子分享弹窗
    /// </summary>
    public class GGUIMonoShareConsort : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;
        [ALHeader("分享按钮")]
        public GameObject shareBtn;
        [ALHeader("选中的妃子显示")]
        public GGUIMonoConsortCardItem monoConsortCard;
        [ALHeader("妃子羁绊信息")]
        public GGUISubMonoConsortFetterInfo monoConsortFetterInfo;
        
        [ALHeader("妃子列表")]
        public GGUIMonoShareIconItemGrid shareIconGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1321); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1321);} }
    }
}
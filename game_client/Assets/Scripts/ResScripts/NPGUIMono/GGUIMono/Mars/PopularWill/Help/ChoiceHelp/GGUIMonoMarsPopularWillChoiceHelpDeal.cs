﻿using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意选择求助 - 处理窗口
    /// </summary>
    public class GGUIMonoMarsPopularWillChoiceHelpDeal : _AALBasicUIWndMono
    {
        [ALHeader("求助描述文本")]
        public TextEx txtDesc;

        [ALHeader("NPC图片")]
        public RawImage npcImg;
        [ALHeader("NPC名")]
        public TextEx txtNpcName;
        
        [ALHeader("不同状态显示列表")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>> stateShowList;
        
        [ALHeader("处理前选项容器")]
        public GGUIMonoMarsPopularWillChoiceHelpOptionItemContainer monoBeforeDealChoiceHelpOptionItemContainer;
        
        [ALHeader("处理后选项容器")]
        public GGUIMonoMarsPopularWillChoiceHelpOptionItemContainer monoAfterDealChoiceHelpOptionItemContainer;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7206); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7206); } }
    }
}
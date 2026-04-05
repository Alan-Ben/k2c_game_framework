using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意信件奖励帮助 - 处理窗口
    /// </summary>
    public class GGUIMonoMarsPopularWillRewardHelpDeal : _AALBasicUIWndMono
    {
        [ALHeader("描述")] 
        public TextEx txtDesc;

        [ALHeader("NPC图片")]
        public RawImage npcImg;
        
        [ALHeader("NPC名")]
        public TextEx txtNpcName;

        [ALHeader("满意度不变显示列表")]
        public List<GameObject> satisfactionDegreeNoChgShowList;
        [ALHeader("满意度增加显示列表")]
        public List<GameObject> satisfactionDegreeAddShowList;
        [ALHeader("满意度降低显示列表")]
        public List<GameObject> satisfactionDegreeReduceShowList;
        
        [ALHeader("满意度变化文本")]
        public TextEx txtSatisfactionDegreeChange;
        [ALHeader("满意度增加文本key, 一个参数:满意度变化值")]
        public string txtSatisfactionDegreeAddKey;
        [ALHeader("满意度降低文本key, 一个参数:满意度变化值")]
        public string txtSatisfactionDegreeReduceKey;

        [ALHeader("奖励物品容器")]
        public NPGGUIMonoCommonItemContainer rewardItemContainer;
        
        [ALHeader("不同状态显示列表")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>> stateShowList;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7208); } }

        public static string objName { get { return UIResPathAssistant.getObjName(7208); } }
    }
}
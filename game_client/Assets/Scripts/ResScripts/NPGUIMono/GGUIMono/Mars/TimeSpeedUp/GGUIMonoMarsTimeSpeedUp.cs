using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星时间加速窗口
    /// </summary>
    public class GGUIMonoMarsTimeSpeedUp : _AALBasicUIWndMono
    {
        [ALHeader("标题")]
        public TextEx txtTitle;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("剩余时间")]
        public TextEx txtTime;
        public Slider sldTimeProgress;
        
        [ALHeader("可用物品列表")]
        public GGUIMonoMarsTimeSpeedUpBagItemContainer monoItemContainer;
        [ALHeader("数量操作部分")]
        public GGUIMonoBagPopCounter monoPopCounter;
        [ALHeader("总共时间减少多少")]
        public Text txtTotalTimeReduce;

        [ALHeader("可以立刻完成显示物体列表")]
        public List<GameObject> canCompleteNowShowList;
        [ALHeader("立即完成")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成消耗物品")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        
        [ALHeader("使用按钮")]
        public GameObject btnUse;
        [ALHeader("不可使用时置灰列表")]
        public List<MaskableGraphic> cannotUseGrayList;
        
        [ALHeader("一键使用按钮")]
        public GameObject btnAutoUse;
        
        [ALHeader("联盟互助次数")]
        public Text txtHelpCount;
        [ALHeader("没有发起互助需要隐藏的列表")]
        public List<GameObject> notAskHelpHideList;
        
        
        [ALHeader("联盟求助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7122); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7122);} }
    }
}
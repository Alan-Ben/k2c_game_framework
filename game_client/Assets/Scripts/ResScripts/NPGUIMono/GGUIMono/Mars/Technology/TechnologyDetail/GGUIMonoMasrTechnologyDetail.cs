
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 科技详情页面
    /// </summary>
    public class GGUIMonoMasrTechnologyDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("属性总览按钮")]
        public GameObject btnPropertyOverview;
        
        [ALHeader("科技信息Mono")]
        public GGUIMonoMarsTechnologyInfo monoTechnologyInfo;

        [ALHeader("当前等级文本")]
        public TextEx txtNowLevel;
        [ALHeader("下一级等级文本")]
        public TextEx txtNextLevel;
        
        [ALHeader("属性列表")]
        public GGUIMonoMarsPropertyShowItemContainer monoPropertyContainer;
        
        [ALHeader("升级消耗物品列表")]
        public GGUIMonoMarsCostItemContainer monoCostItemContainer;
        
        [ALHeader("升级条件列表")]
        public GGUIMonoConditionDescContainer monoConditionContainer;

        [ALHeader("立即完成按钮")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成的消耗")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("可立即完成时显示物体列表")]
        public List<GameObject> canCompleteNowShowGoList;
        [ALHeader("不可立即完成时显示物体列表")]
        public List<GameObject> cannotCompleteNowShowGoList;
        [ALHeader("不可立即完成时置灰列表")]
        public List<MaskableGraphic> cannotCompleteNowGrayList;
        
        [ALHeader("升级需要的初始时间")]
        public TextEx txtUpgradeOriTime;
        [ALHeader("升级需要的实际时间")]
        public TextEx txtUpgradeRealTime;

        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("可升级时显示物体列表")]
        public List<GameObject> canUpgradeShowGoList;
        [ALHeader("不可升级时显示物体列表")]
        public List<GameObject> cannotUpgradeShowGoList;
        [ALHeader("不可升级时置灰列表")]
        public List<MaskableGraphic> cannotUpgradeGrayList;

        [ALHeader("升级倒计时进度条")]
        public NPGGUIMonoProgress monoUpgradeCountDownProgress;
        [ALHeader("取消升级按钮")]
        public GameObject btnCancelUpgrade;
        
        [ALHeader("升级加速按钮")]
        public GameObject btnUpgradeSpeedUp;

        [ALHeader("升级完成确认按钮")]
        public GameObject btnUpgradeCompleteConfirm;
        [ALHeader("联盟互助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;

        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7307); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7307); } }
    }    
}

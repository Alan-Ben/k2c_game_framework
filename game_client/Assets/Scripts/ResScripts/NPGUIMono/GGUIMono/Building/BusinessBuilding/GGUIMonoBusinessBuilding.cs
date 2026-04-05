
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIMonoBusinessBuildingVideoShowLockMask
    {
        [ALHeader("遮罩对象")]
        public GameObject goMask;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;
        [ALHeader("视频变化时的特效父节点和特效 id ")]
        public Transform videoChgEffectPos;
        public long videoChgEffectSfxId;
    }
    [Serializable]
    public class GGUIMonoBusinessBuildingVideoShow
    {
        [ALHeader("监控器的画面")]
        public GGUIMonoCommonShowCase monoMonitorShowcase;
        [ALHeader("切换下一个或上一个视频的按钮")]
        public GameObject btnNext;
        public GameObject btnPrev;
        [ALHeader("当前的时间")]
        public Text txtTimeNow;
        [ALHeader("当前播放的视频名字")]
        public Text txtVideoName;
        [ALHeader("自动轮播的勾选框和自动轮播的时间间隔(秒)")]
        public NPGGUIMonoCommonToggleEx monoAutoPlay;
        public float autoPlayInterval = 5f;
        [ALHeader("切换视频的小点容器")]
        public GGUIMonoBusinessBuildingVideoIndexContainer monoIndexContainer;
        [ALHeader("解锁遮罩列表"),
         ALInfo("develop 表配了几条，这边就要有几个")]
        public List<GGUIMonoBusinessBuildingVideoShowLockMask> listLockMask;
        [ALHeader("分割线的对象列表")]
        public List<GameObject> listSplitLineShow;
    }
    public class GGUIMonoBusinessBuilding : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnBack;
        [ALHeader("建筑相性 icon ")]
        public RawImage imgAttrIcon;
        [ALHeader("建筑名字和等级")]
        public Text txtName;
        public Text txtLevel;
        [ALHeader("建筑的每秒总收益")]
        public Text txtTotalEarningsPerS;
        [ALHeader("查看收益详情按钮")]
        public GameObject btnEarningDetail;
        [ALHeader("每个员工的收益")]
        public Text txtEmployeeEarningsPerPerson;
        [ALHeader("员工数量")]
        public GGUIMonoSmoothDampLongText txtEmployeeCount;
        [ALHeader("收益倍率")]
        public Text txtEarningsBonus;
        [ALHeader("下一等级的收益倍率")]
        public Text txtNextEarningsBonus;
        [ALHeader("连续十次雇佣 toggle ")]
        public NPGGUIMonoCommonToggleEx monoTenTimesHire;
        [ALHeader("雇佣花费")]
        public NPGGUIMonoCommonItem monoHireCost;
        [ALHeader("雇佣按钮")]
        public GameObject btnHire;
        [ALHeader("升级花费")]
        public NPGGUIMonoCommonItem monoUpgradeCost;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("委派大臣列表")]
        public GGUIMonoBusinessBuildingOperatingHeroContainer monoOperatingHeroContainer;
        [ALHeader("委派大臣按钮")]
        public GameObject btnSetHero;
        [ALHeader("下一个大臣槽位解锁所需的员工")]
        public Text txtNextHeroSlotUnlockRequire;
        [ALHeader("上面这条文本相关的动画"),
         ALInfo("界面启动就会自动播放")]
        public Animation animNextHeroSlotUnlockRequire;
        public string animNameNextHeroSlotUnlockRequire;
        [ALHeader("当前大臣槽位的情况")]
        public Text txtHeroSlotState;
        [ALHeader("大臣槽位全解锁隐藏列表")]
        public List<GameObject> listHeroSlotAllUnlockHide;
        [ALHeader("有大臣可以委派时展示的列表")]
        public List<GameObject> listCanHeroSettleShow;
        [ALHeader("研发按钮")]
        public GameObject btnRnD;
        [ALHeader("研发按钮解锁产品进度")]
        public NPGGUIMonoProgress monoRnDProgress;
        [ALHeader("有产品可以解锁时展示的列表")]
        public List<GameObject> listCanUnlockProductShow;
        [ALHeader("当等级全满时要隐藏的列表")]
        public List<GameObject> listLevelMaxHide;
        [ALHeader("当等级全满时要显示的列表")]
        public List<GameObject> listLevelMaxShow;
        [ALHeader("当满级并且招聘满员时要隐藏的列表")]
        public List<GameObject> listLevelEmployeeMaxHide;
        [ALHeader("当满级并且招聘满员时要显示的列表")]
        public List<GameObject> listLevelEmployeeMaxShow;
        [ALHeader("视频展示")]
        public GGUIMonoBusinessBuildingVideoShow videoShow;
        [ALHeader("前后切换建筑的按钮")]
        public GameObject btnGotoNextBuilding;
        public GameObject btnGotoPrevBuilding;
        [ALHeader("可以切换建筑时显示的对象")]
        public List<GameObject> listCanGotoShow;
        [ALHeader("雇佣特效父节点和特效 id ")]
        public Transform hireEffectParent;
        public long hireEffectSfxId;
        public long tenTimesHireEffectSfxId;
        [ALHeader("雇佣动画的动画组件和动画名，以及延迟播放的时间")]
        public Animation animHire;
        public string animNameHire;
        public string animNameTenTimesHire;
        public float animHireDelay = 0.5f;
        [ALHeader("已研发的业务列表")]
        public GGUIMonoBusinessBuildingProductGrid monoProductGrid;
        [ALHeader("新派遣顾问气泡")]
        public GGUIMonoHeroVoiceBubble monoHeroBubble;
        [ALHeader("延时展示下一个气泡的时间")]
        public float delayShowNextHeroBubbleSecond = 2;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1100); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1100); } }
    }
}
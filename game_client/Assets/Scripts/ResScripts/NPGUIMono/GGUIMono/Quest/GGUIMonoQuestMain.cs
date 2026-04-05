using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Spine.Unity;

namespace GOE
{
    /// <summary>
    /// 任务步骤状态
    /// </summary>
    public enum ENPQuestStepStatusEnum
    {
        [InspectorName("QUEST_CANGET（可领奖）")]
        QUEST_CANGET,//可领奖
        [InspectorName("QUEST_FAIL（失败）")]
        QUEST_FAIL,//失败
        [InspectorName("QUEST_ISGOING（正在进行中）")]
        QUEST_ISGOING,//正在进行中
        [InspectorName("QUEST_NONE（未接受）")]
        QUEST_NONE, //未接受
    }

    /// <summary>
    /// 主线任务界面页签类型
    /// </summary>
    public enum EQuestMainTab
    {
        [InspectorName("MAIN_QUEST（主线任务）")]
        MAIN_QUEST,//主线任务
        [InspectorName("FUNC_PREVIEW（功能预告）")]
        FUNC_PREVIEW,//功能预告
    }

    /// <summary>
    /// 页签类型
    /// </summary>
    [System.Serializable]
    public class GGUIQuestMainTabMono
    {
        [ALHeader("页签类型")]
        public EQuestMainTab tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 任务主界面
    /// </summary>
    public class GGUIMonoQuestMain : _AALBasicUIWndMono
    {
        [ALHeader("页签列表")]
        public List<GGUIQuestMainTabMono> monoTabList;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("默认选中页签")]
        public EQuestMainTab defaultTab = EQuestMainTab.MAIN_QUEST;
        [ALHeader("功能全部解锁后需要显示的GO列表")]
        public List<GameObject> goAllUnlockShowList;
        [ALHeader("功能全部解锁后需要隐藏的GO列表")]
        public List<GameObject> goAllUnlockHideList;

        [ALHeader("spine动画")]
        public SkeletonGraphic spineAnimation;
        [ALHeader("spine开始动画名")]
        public string spineStartName;
        [ALHeader("spine停留动画名")]
        public string spineIdleName;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2401); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2401); } }
    }
}

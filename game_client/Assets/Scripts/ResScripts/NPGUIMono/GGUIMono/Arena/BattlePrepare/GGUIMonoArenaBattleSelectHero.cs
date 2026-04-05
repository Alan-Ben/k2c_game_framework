using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴弹窗
    /// </summary>
    public class GGUIMonoArenaBattleSelectHero : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("关闭按钮")]
        public GameObject btnCloseTwo;
        [ALHeader("伙伴实力详情按钮")]
        public GameObject btnHeroPowerDetail;
        [ALHeader("选中的伙伴形象")]
        public RawImage imgSelectHero;
        [ALHeader("选中的伙伴形象")]
        public GGUIMonoCommonShowCase monoSelectHeroShowCase;
        [ALHeader("选择伙伴列表")]
        public GGUIMonoArenaBattleSelectHeroGrid monoHeroGrid;
        [ALHeader("伙伴觉醒技能实力加成")]
        public GGUIMonoArenaStarSkillAddPowerContainer monoStarSkillContainer;
        [ALHeader("筛选子窗口")]
        public GGUIMonoHeroMainFilter monoFilter;
        [ALHeader("下一步按钮")]
        public GameObject btnNext;
        [ALHeader("点击下一步需要播放的动画")]
        public CommonAnimationSingleInfo aniClickNext;
        [ALHeader("随机谈判时需要显示的GO列表")]
        public List<GameObject> goRandomShowList;
        [ALHeader("随机谈判时需要隐藏的GO列表")]
        public List<GameObject> goRandomHideList;
        [ALHeader("伙伴实力详情tip的X偏移")]
        public float powerDetailIntervalX;
        [ALHeader("伙伴实力详情tip的Y偏移")]
        public float powerDetailIntervalY;
        [ALHeader("未选择伙伴时需要显示的GO列表")]
        public List<GameObject> goNoSelectHeroShowList;
        [ALHeader("未选择伙伴时需要隐藏的GO列表")]
        public List<GameObject> goNoSelectHeroHideList;

        [ALInfo("========觉醒技能实力加成展示配置========")]
        [ALHeader("觉醒技能实力加成列表每批展示数量")]
        public int eachBatchSkillAddPowerShowCount = 4;
        [ALHeader("觉醒技能实力加成列表每批展示时间")]
        public float eachBatchSkillAddPowerShowSec = 2.6f;

        [ALInfo("========指定谈判配置========")]
        [ALHeader("指定谈判消耗按钮")]
        public GameObject btnCostNext;
        [ALHeader("指定谈判消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("普通谈判勾选框")]
        public NPGGUIMonoCommonToggleEx monoSelectNormalCost;
        [ALHeader("高级谈判勾选框")]
        public NPGGUIMonoCommonToggleEx monoSelectAdvancedCost;
        [ALHeader("特级谈判勾选框")]
        public NPGGUIMonoCommonToggleEx monoSelectSuperCost;
        [ALHeader("指定谈判获得影响力数量")]
        public Text txtInfluenceCount;
        [ALHeader("指定谈判获得道具数量")]
        public Text txtItemCount;
        [ALHeader("指定谈判获得实力数量")]
        public Text txtPowerCount;

        [ALInfo("========初始增益页面配置========")]
        [ALHeader("返回上一步按钮")]
        public GameObject btnLastStep;
        [ALHeader("点击上一步需要播放的动画")]
        public CommonAnimationSingleInfo aniClickLast;
        [ALHeader("开始按钮")]
        public GameObject btnStart;
        [ALHeader("不选择增益item")]
        public GGUIMonoArenaBattleInitBuffItem monoNoSelectItem;
        [ALHeader("选择增益item列表")]
        public List<GGUIMonoArenaBattleInitBuffItem> monoBuffItemList;
        [ALHeader("当前血量值")]
        public Text txtCurBlood;

        [ALInfo("========页面滚动配置========")]
        [ALHeader("滚动scroll")]
        public ScrollRect scrollRect;
        [ALHeader("滚动时间，配成0是直接切换")]
        public float moveTimeSec = 0.1f;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5208); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5208); } }
    }
}

using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeGame : _AHotfixBaseMono
    {
        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
        [HotfixMono("当前奖券的数量")]
        public GGUIMonoCommonSimpleItem monoTicketItem;
        [HotfixMono("奖券增加的提示父对象")]
        public Transform transTicketAddTipParent;
        [HotfixMono("奖券增加提示id")]
        public long ticketAddTipId;
        [HotfixMono("最高得分")]
        public Text txtHighestScore;
        [HotfixMono("得分详情按钮")]
        public GameObject btnScoreDetail;
        [HotfixMono("体力")]
        public GGUIMonoCommonLazyCDCountResume monoStamina;
        [HotfixMono("快速模式切换按钮")]
        public NPGGUIMonoCommonToggleEx toggleAdvanceMode;
        [HotfixMono("快速模式未解锁时显示的内容")]
        public List<GameObject> listAdvanceModeLockShow;
        [HotfixMono("急速模式切换按钮")]
        public NPGGUIMonoCommonToggleEx toggleUltraMode;
        [HotfixMono("急速模式未解锁时显示的内容")]
        public List<GameObject> listUltraModeLockShow;
        [HotfixMono("模式切换动画")]
        public Animation modeChangeAnim;
        [HotfixMono("普通模式的动画")]
        public string normalModeAnimName;
        [HotfixMono("快速模式的动画")]
        public string advanceModeAnimName;
        [HotfixMono("急速模式的动画")]
        public string ultraModeAnimName;
        [HotfixMono("整理物品道具")]
        public GGUIMonoCommonSimpleItem monoOrganizeItem;
        [HotfixMono("整理按钮")]
        public GameObject btnOrganize;
        [HotfixMono("消除物品道具")]
        public GGUIMonoCommonSimpleItem monoEliminateItem;
        [HotfixMono("消除按钮")]
        public GameObject btnEliminate;
        [HotfixMono("游戏玩法子窗口")]
        public GGUIHotfixCommonMono monoGamePlay;
        [HotfixMono("图鉴按钮")]
        public GameObject btnHandbook;
        [HotfixMono("宝箱奖励子窗口")]
        public GGUIHotfixCommonMono monoBoxReward;
        [HotfixMono("使用道具提示动画")]
        public Animation useItemTipAnim;
        [HotfixMono("使用道具提示动画名")]
        public string useItemTipAnimName;
    }
}
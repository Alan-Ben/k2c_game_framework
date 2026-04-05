
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeGamePlayBlockItem : _AHotfixBaseMono
    {
        [HotfixMono("棋子行为动画")]
        public Animation behaviorAnim;
        [HotfixMono("重置所有动画用的动画")] 
        public string resetAllAnimName;
        [HotfixMono("刷新出现时的动画")]
        public string spawnAnimName;
        [HotfixMono("重排时刷新出现的棋子动画")]
        public string organizeSpawnAnimName;
        [HotfixMono("销毁时的动画")]
        public string destroyAnimName;
        [HotfixMono("重排时销毁的动画")]
        public string organizeDestroyAnimName;
        [HotfixMono("游戏结束销毁时的动画")]
        public string gameOverDestroyAnimName;
        [HotfixMono("棋子图标")]
        public RawImage imgBlockIcon;
        [HotfixMono("点击按钮")]
        public GameObject btnClick;
        [HotfixMono("有 buff 时的显示")]
        public List<GameObject> listHasBuffShow;
        [HotfixMono("有 buff 时的隐藏")]
        public List<GameObject> listHasBuffHide;
        [HotfixMono("buff剩余步数文本")]
        public Text txtBuffRemainStep;
        [HotfixMono("特效的父节点")]
        public Transform sfxParent;
        
        
        public void setHasBuff(bool _hasBuff)
        {
            ALUGUICommon.setGameObjEnable(listHasBuffShow, false);
            ALUGUICommon.setGameObjEnable(listHasBuffHide, true);
            ALUGUICommon.setGameObjEnable(_hasBuff ? listHasBuffShow : listHasBuffHide, true);
        }
    }
}
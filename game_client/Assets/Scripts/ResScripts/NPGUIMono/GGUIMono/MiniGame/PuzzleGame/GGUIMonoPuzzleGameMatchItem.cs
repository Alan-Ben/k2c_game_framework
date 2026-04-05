using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoPuzzleGameMatchItem : _AALBasicUIWndMono
    {
        [ALHeader("匹配item的id, 与GGUIMonoPuzzleGameMatchItemBg中配置的id对应")]
        public long matchItemId;
        
        [ALHeader("游戏状态显示")]
        public MultiStateShow<EPuzzleGameMatchItemState> gameStateShow;

        [ALHeader("事件响应对象")]
        public GameObject eventTriggerTarget;
        [ALHeader("操作屏蔽对象")]
        public GameObject opMask;

        [ALHeader("匹配区间")]
        public RectTransform matchRectTransform;

        [ALHeader("重置item位置的延迟时间")]
        public float resumeItemPositionDelayTime;
        [ALHeader("重置item位置的飞行时间")]
        public float resumeItemPositionFlyTime;
        [ALHeader("当匹配错误时, 是否需要先将位置设置到错误的匹配位置")]
        public bool onMatchWrongNeedSetPosition;

        /// <summary>
        /// 设置当前状态
        /// </summary>
        public void setState(EPuzzleGameMatchItemState _state)
        {
            if (gameStateShow == null)
                return;
            
            gameStateShow.setShowData(_state);
        }
    }
}
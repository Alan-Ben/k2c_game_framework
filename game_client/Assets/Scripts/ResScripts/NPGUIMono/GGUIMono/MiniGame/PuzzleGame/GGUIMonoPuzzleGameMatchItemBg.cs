using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoPuzzleGameMatchItemBg : _AALBasicUIWndMono
    {
        [ALHeader("匹配item的id, 与GGUIMonoPuzzleGameMatchItem中配置的id对应")]
        public long matchItemId;
        
        [ALHeader("游戏状态显示")]
        public MultiStateShow<EPuzzleGameMatchItemBgState> gameStateShow;
        
        [ALHeader("匹配区间")]
        public RectTransform matchRectTransform;
        
        /// <summary>
        /// 设置当前状态
        /// </summary>
        public void setState(EPuzzleGameMatchItemBgState _state)
        {
            if (gameStateShow == null)
                return;
            
            gameStateShow.setShowData(_state);
        }
    }
}
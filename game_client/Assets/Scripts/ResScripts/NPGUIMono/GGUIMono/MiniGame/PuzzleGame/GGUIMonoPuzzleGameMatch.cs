using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;

namespace GOE
{
    /// <summary>
    /// 拼图游戏窗口
    /// </summary>
    public class GGUIMonoPuzzleGameMatch : _AALBasicUIWndMono
    {
        [ALHeader("游戏状态显示")]
        public MultiStateShow<EPuzzleGameState> gameStateShow;
        
        [ALHeader("匹配item列表")]
        public List<GGUIMonoPuzzleGameMatchItem> matchItemList;
        
        [ALHeader("匹配item背景列表")]
        public List<GGUIMonoPuzzleGameMatchItemBg> matchItemBgList;

        [ALHeader("游戏成功后退出游戏时间")]
        public float gameSuccessExitTime;
        
        /// <summary>
        /// 设置当前状态
        /// </summary>
        public void setState(EPuzzleGameState _state)
        {
            if (gameStateShow == null)
                return;
            
            gameStateShow.setShowData(_state);
        }
    }
}
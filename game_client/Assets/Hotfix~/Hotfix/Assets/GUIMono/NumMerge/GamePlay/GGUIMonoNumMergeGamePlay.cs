
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeGamePlay : _AHotfixBaseMono
    {
        [HotfixMono("游戏得分")]
        public Text txtGameScore;
        [HotfixMono("游戏加分提示父节点")]
        public Transform transGameScoreAddTipParent;
        [HotfixMono("游戏加分提示 id ")]
        public long gameScoreAddTipId;
        [HotfixMono("游戏棋盘左下角那格的位置")]
        public Transform transGameBoardParent;
        [HotfixMono("棋盘单元格大小")]
        public float gameBoardCellSize = 100f;
        [HotfixMono("棋子模板")]
        public GGUIHotfixCommonMono monoItemTemplate;
        [HotfixMono("拖拽区域")]
        public GameObject btnDragArea;
        [HotfixMono("拖拽阈值")]
        public float dragThreshold;
        [HotfixMono("消除模式下显示的内容")]
        public List<GameObject> listEliminateModeShow;
        [HotfixMono("消除模式下隐藏的内容")]
        public List<GameObject> listEliminateModeHide;
        [HotfixMono("取消消除模式按钮")]
        public GameObject btnCancelEliminateMode;
        [HotfixMono("棋子的移动时间")]
        public float itemMoveTime;
        [HotfixMono("整个棋盘的棋子播放动画时，棋子之间的间隔时间")]
        public float itemAnimSpace;
        [HotfixMono("动画组件")]
        public Animation behaviorAnim;
        [HotfixMono("游戏结束动画")]
        public string gameOverAnimName;
        [HotfixMono("得分动画的动画组件")]
        public Animation buffScoreAnim;
        [HotfixMono("得分增加动画")]
        public string buffScoreAnimName;
        [HotfixMono("得分增加动画的延迟时间")]
        public float buffScoreAnimDelay;
        [HotfixMono("移动音效 id")]
        public long moveAudioId;


        public void setEliminateModeShow(bool _isEliminateMode)
        {
            ALUGUICommon.setGameObjEnable(listEliminateModeShow, false);
            ALUGUICommon.setGameObjEnable(listEliminateModeHide, false);
            ALUGUICommon.setGameObjEnable(_isEliminateMode ? listEliminateModeShow : listEliminateModeHide, true);
        }
    }
}
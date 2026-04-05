using ALPackage;
using UnityEngine;

namespace GOE
{
    public class TreasureHuntGameController
    {
        private readonly TreasureHuntGameLogic _m_gameLogic;
        private int _m_pauseSerialize;
        
        
        public TreasureHuntGameController(TreasureHuntGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }


        public void startGame(bool _isAdvance)
        {
            _m_gameLogic?.startGame(_isAdvance);
        }
        /// <summary>
        /// 设置水平移动的目标值
        /// </summary>
        public void setMoveTarget(Vector2 _screenPos)
        {
            Vector3 worldPos = GCommon.getTDOnlyGroundPos(_screenPos, _m_gameLogic?.playerUnit.worldPositionY ?? 0);
            Transform unitRoot = MainAdditionTreasureHuntGameTDScene.instance.getUnitRoot();
            if (unitRoot == null)
                return;
            
            Vector3 localPosition = unitRoot.InverseTransformPoint(worldPos);
            _m_gameLogic?.playerUnit.setMoveTargetHorizontal(localPosition.x);
        }
        public void pauseGame()
        {
            if (_m_gameLogic == null || _m_gameLogic.isPause)
                return;
            
            _m_gameLogic.pauseGame();
            
            int pauseSerialize = _m_pauseSerialize = ALSerializeOpMgr.next();
            long serialize = _m_gameLogic.startSerialize;
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndTreasureHuntGamePlayQuitCheck.instance, () =>
            {
                if (serialize != _m_gameLogic.startSerialize || pauseSerialize != _m_pauseSerialize)
                    return;
                
                resumeGame();
            }, UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_QUIT_CHECK));
        }
        public void resumeGame()
        {
            if (_m_gameLogic == null || !_m_gameLogic.isPause)
                return;
                
            int pauseSerialize = _m_pauseSerialize = ALSerializeOpMgr.next();
            long serialize = _m_gameLogic.startSerialize;
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndTreasureHuntGamePlayResumeCountDown.instance, () =>
            {
                if (serialize != _m_gameLogic.startSerialize || pauseSerialize != _m_pauseSerialize)
                    return;
                
                _m_gameLogic.resumeGame();
            }, UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_RESUME_COUNT_DOWN));
        }
    }
}
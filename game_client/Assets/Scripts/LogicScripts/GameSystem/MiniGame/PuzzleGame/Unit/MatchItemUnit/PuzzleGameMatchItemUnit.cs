using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemUnit : _APuzzleGameUnit
    {
        [NotNull] private GGUIWndPuzzleGameMatchItem _m_matchItemWnd;
        
        // 状态机
        [NotNull] private readonly _TALSimpleStateMachine<EPuzzleGameMatchItemState> _m_stateMachine;
        
        private Vector3 _m_vItemUIOriginWorldPos;//item的初始位置
        
        [NotNull] internal  _TALSimpleStateMachine<EPuzzleGameMatchItemState> stateMachine { get { return _m_stateMachine; } }
        internal long matchItemId { get { return _m_matchItemWnd.matchItemId; } }
        internal RectTransform matchRectTransform { get { return _m_matchItemWnd.matchRectTransform; } }

        internal Vector3 itemUIWorldPos { get { return _m_matchItemWnd.position; } }
        
        internal EPuzzleGameMatchItemState itemState { get { return _m_stateMachine.curState.state; } }
        
        public PuzzleGameMatchItemUnit([NotNull] PuzzleGameLogic _gameLogic, [NotNull] PuzzleGameController _gameController, [NotNull] GGUIWndPuzzleGameMatchItem _matchItemWnd) : base(_gameLogic, _gameController)
        {
            _m_matchItemWnd = _matchItemWnd;
            
            _m_stateMachine = new _TALSimpleStateMachine<EPuzzleGameMatchItemState>();
            _m_stateMachine.changeState(new PuzzleGameMatchItemNoneState(this));
        }

        public override void init()
        {
            _m_matchItemWnd.showWnd();
            
            _m_vItemUIOriginWorldPos = itemUIWorldPos;
            _m_stateMachine.changeState(new PuzzleGameMatchItemIdleState(this));
        }

        public override void discard()
        {
            _m_stateMachine.changeState(new PuzzleGameMatchItemNoneState(this));

            _m_matchItemWnd.hideWnd();
        }

        internal void setItemPosition(Vector3 _position)
        {
            _m_matchItemWnd.setItemPosition(_position);
        }
        
        /// <summary>
        /// 移动位置到最后面
        /// </summary>
        internal void moveTransformToLast()
        {
            _m_matchItemWnd.moveTransformToLast();
        }

        /// <summary>
        /// 打开操作屏蔽
        /// </summary>
        internal void openOpMask()
        {
            _m_matchItemWnd.openOpMask();
        }
        
        /// <summary>
        /// 关闭操作屏蔽
        /// </summary>
        internal void closeOpMask()
        {
            _m_matchItemWnd.closeOpMask();
        }
        
        internal void setItemShowState(EPuzzleGameMatchItemState _state, bool _forceChg = false)
        {
            _m_matchItemWnd.setItemState(_state, _forceChg);
        }

        /// <summary>
        /// 当item匹配错误时
        /// </summary>
        /// <param name="_wrongPosition">错误匹配的坐标位置</param>
        internal void onItemMatchWrong(Vector3 _wrongPosition, bool _needDelay, Action _complete)
        {
            if (_m_matchItemWnd.onMatchWrongNeedSetPosition)
            {
                _m_matchItemWnd.setItemPosition(_wrongPosition);
            }

            resumeItemOriPosition(_needDelay, _complete);
        }
        
        
        internal void resumeItemOriPosition(bool _needDelay, Action _onMoveDone)
        {
            _m_matchItemWnd.resumeItemOriPosition(_m_vItemUIOriginWorldPos, _needDelay, getGameSerialize, _onMoveDone);
        }

        public long getGameSerialize()
        {
            return _m_puzzleGameLogic.gameSerializeId;
        }
    }
}
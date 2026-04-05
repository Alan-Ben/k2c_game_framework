using JetBrains.Annotations;
using ALPackage;
using UnityEngine;

namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgUnit : _APuzzleGameUnit
    {
        [NotNull] private GGUIWndPuzzleGameMatchItemBg _m_matchItemBgWnd;
        // 状态机
        [NotNull] private readonly _TALSimpleStateMachine<EPuzzleGameMatchItemBgState> _m_stateMachine;

        [NotNull] internal  _TALSimpleStateMachine<EPuzzleGameMatchItemBgState> stateMachine { get { return _m_stateMachine; } }
        internal long matchItemId { get { return _m_matchItemBgWnd.matchItemId; } }
        internal RectTransform matchRectTransform { get { return _m_matchItemBgWnd.matchRectTransform; } }
        internal Vector3 itemUIWorldPos { get { return _m_matchItemBgWnd.position; } }
        internal EPuzzleGameMatchItemBgState itemState { get { return _m_stateMachine.curState.state; } }
        
        public PuzzleGameMatchItemBgUnit([NotNull] PuzzleGameLogic _gameLogic, [NotNull] PuzzleGameController _gameController, [NotNull] GGUIWndPuzzleGameMatchItemBg _matchIteBgmWnd) : base(_gameLogic, _gameController)
        {
            _m_matchItemBgWnd = _matchIteBgmWnd;

            _m_stateMachine = new _TALSimpleStateMachine<EPuzzleGameMatchItemBgState>();
            _m_stateMachine.changeState(new PuzzleGameMatchItemBgNoneState(this));
        }

        public override void init()
        {
            _m_matchItemBgWnd.showWnd();
            
            _m_stateMachine.changeState(new PuzzleGameMatchItemBgIdleState(this));
        }

        public override void discard()
        {
            _m_stateMachine.changeState(new PuzzleGameMatchItemBgNoneState(this));
            
            _m_matchItemBgWnd.hideWnd();
        }
        
        internal void setItemShowState(EPuzzleGameMatchItemBgState _state, bool _forceChg = false)
        {
            _m_matchItemBgWnd.setItemState(_state, _forceChg);
        }
        
        public long getGameSerialize()
        {
            return _m_puzzleGameLogic.gameSerializeId;
        }
    }
}
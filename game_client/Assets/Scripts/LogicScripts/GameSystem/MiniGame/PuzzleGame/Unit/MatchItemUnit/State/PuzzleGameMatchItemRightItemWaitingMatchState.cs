namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemRightItemWaitingMatchState : _APuzzleGameMatchItemState
    {
        public PuzzleGameMatchItemRightItemWaitingMatchState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH or EPuzzleGameMatchItemState.WRONG_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemState.NO_MATCH_RESUME or EPuzzleGameMatchItemState.MATCH_SUCCESS
                or EPuzzleGameMatchItemState.MATCH_FAIL_RESUME or EPuzzleGameMatchItemState.NONE;
        }
    }
}
namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemNoItemWaitingMatchState : _APuzzleGameMatchItemState
    {
        public PuzzleGameMatchItemNoItemWaitingMatchState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH or EPuzzleGameMatchItemState.WRONG_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemState.NO_MATCH_RESUME or EPuzzleGameMatchItemState.MATCH_SUCCESS
                or EPuzzleGameMatchItemState.MATCH_FAIL_RESUME or EPuzzleGameMatchItemState.NONE;
        }
    }
}
namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgWrongItemWaitingMatchState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgWrongItemWaitingMatchState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.WRONG_ITEM_WAITING_MATCH; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.WRONG_ITEM_WAITING_MATCH);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.NO_ITEM_WAITING_MATCH  or EPuzzleGameMatchItemBgState.RIGHT_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemBgState.NO_MATCH_RESUME or EPuzzleGameMatchItemBgState.MATCH_SUCCESS
                or EPuzzleGameMatchItemBgState.MATCH_FAIL_RESUME or EPuzzleGameMatchItemBgState.NONE;
        }
    }
}
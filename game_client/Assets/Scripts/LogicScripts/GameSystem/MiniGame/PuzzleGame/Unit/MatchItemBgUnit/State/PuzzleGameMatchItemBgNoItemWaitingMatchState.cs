namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgNoItemWaitingMatchState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgNoItemWaitingMatchState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.NO_ITEM_WAITING_MATCH; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.NO_ITEM_WAITING_MATCH);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.RIGHT_ITEM_WAITING_MATCH or EPuzzleGameMatchItemBgState.WRONG_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemBgState.NO_MATCH_RESUME or EPuzzleGameMatchItemBgState.MATCH_SUCCESS
                or EPuzzleGameMatchItemBgState.MATCH_FAIL_RESUME or EPuzzleGameMatchItemBgState.NONE;
        }
    }
}
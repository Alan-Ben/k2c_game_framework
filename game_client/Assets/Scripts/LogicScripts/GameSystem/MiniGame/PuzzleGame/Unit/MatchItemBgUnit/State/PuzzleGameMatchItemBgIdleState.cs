namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgIdleState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgIdleState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.IDLE; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.IDLE);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.NO_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemBgState.RIGHT_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemBgState.WRONG_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemBgState.NONE;
        }
    }
}
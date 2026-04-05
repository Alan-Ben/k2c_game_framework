namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgNoneState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgNoneState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.NONE; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.NONE, true);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.IDLE;
        }
    }
}
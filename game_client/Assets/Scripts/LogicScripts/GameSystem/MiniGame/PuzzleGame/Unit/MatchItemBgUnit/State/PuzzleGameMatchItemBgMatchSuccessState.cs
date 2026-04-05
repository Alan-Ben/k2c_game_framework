namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgMatchSuccessState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgMatchSuccessState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.MATCH_SUCCESS; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.MATCH_SUCCESS);

        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.NONE;
        }
    }
}
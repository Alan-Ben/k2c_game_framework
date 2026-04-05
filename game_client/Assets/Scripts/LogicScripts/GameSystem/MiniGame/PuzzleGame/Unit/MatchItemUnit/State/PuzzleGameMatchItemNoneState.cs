namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemNoneState : _APuzzleGameMatchItemState
    {
        public PuzzleGameMatchItemNoneState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.NONE; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemUnit.openOpMask();//打开操作屏蔽
            
            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.NONE, true);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.IDLE;
        }
    }
}
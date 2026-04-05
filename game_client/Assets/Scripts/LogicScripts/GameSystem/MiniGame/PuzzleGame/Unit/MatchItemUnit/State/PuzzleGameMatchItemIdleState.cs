namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemIdleState : _APuzzleGameMatchItemState
    {
        public PuzzleGameMatchItemIdleState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.IDLE; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemUnit.closeOpMask();//关闭操作屏蔽

            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.IDLE);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemState.WRONG_ITEM_WAITING_MATCH
                or EPuzzleGameMatchItemState.NONE;
        }
    }
}
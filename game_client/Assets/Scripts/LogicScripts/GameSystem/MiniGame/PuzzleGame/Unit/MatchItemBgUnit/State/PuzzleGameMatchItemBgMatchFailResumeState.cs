namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemBgMatchFailResumeState : _APuzzleGameMatchItemBgState
    {
        public PuzzleGameMatchItemBgMatchFailResumeState(PuzzleGameMatchItemBgUnit _matchItemBgUnit) : base(_matchItemBgUnit)
        {
        }

        public override EPuzzleGameMatchItemBgState state { get { return EPuzzleGameMatchItemBgState.MATCH_FAIL_RESUME; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemBgUnit.setItemShowState(EPuzzleGameMatchItemBgState.MATCH_FAIL_RESUME);

            long gameSerialize = _m_matchItemBgUnit.getGameSerialize();
            // 暂时不需要什么表现, 直接变回idle状态, 因为有可能在同一帧进行状态切换, 所以这里延后一帧执行状态变化
            CommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                if (gameSerialize != _m_matchItemBgUnit.getGameSerialize())
                    return;
                    
                _m_matchItemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgIdleState(_m_matchItemBgUnit));
            });
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemBgState _newState)
        {
            return _newState is EPuzzleGameMatchItemBgState.IDLE or EPuzzleGameMatchItemBgState.NONE;
        }
    }
}
using UnityEngine;

namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemNoMatchResumeState : _APuzzleGameMatchItemState
    {
        public PuzzleGameMatchItemNoMatchResumeState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.NO_MATCH_RESUME; } }
        
        protected override void _onEnterSub()
        {
            _m_matchItemUnit.openOpMask();//打开操作屏蔽
            
            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.NO_MATCH_RESUME);
            
            long gameSerialize = _m_matchItemUnit.getGameSerialize();
            _m_matchItemUnit.resumeItemOriPosition(false, () =>
            {
                // 移动完成后, 回到idle状态, 因为有可能在同一帧进行状态切换, 所以这里延后一帧执行状态变化
                CommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    if (gameSerialize != _m_matchItemUnit.getGameSerialize())
                        return;
                    
                    _m_matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemIdleState(_m_matchItemUnit));
                });
            });
        }
        
        protected override void _onExitSub()
        {
            _m_matchItemUnit.closeOpMask();//关闭操作屏蔽
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.IDLE or EPuzzleGameMatchItemState.NONE;
        }
    }
}
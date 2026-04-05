using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_None : _ATileMatchGameStateMachineBaseState
        {
            public TileMatchGameStateMachineState_None([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.None; } }

            protected override void _onEnter()
            {
            }

            protected override void _onExit()
            {
            }

            protected override void _onTick(float _deltaTime)
            {
            }

            public override bool canEnterState(_ATHotfixStateBase<ETileMatchGameState> _newState)
            {
                return false;
            }

            public override void resetData()
            {
            }
            
            public override void doPress(TileMatchBlockInfo _cubeInfo)
            {
            }
        }
    }
}
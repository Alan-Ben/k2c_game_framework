using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public partial class TileMatchGameStateMachine : _THotfixStateMachine<_ATileMatchGameStateMachineBaseState, ETileMatchGameState>
        {
            public TileMatchGameStateMachine([NotNull] TileMatchGameLogic _gameLogic) : base(new TileMatchGameStateMachineStateFactory(_gameLogic))
            {
            }
        }
    }
}
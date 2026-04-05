using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public abstract class _ATileMatchGameStateMachineBaseState : _ATHotfixStateBase<ETileMatchGameState>
        {
            [NotNull] protected TileMatchGameLogic _m_gameLogic;
            
            public _ATileMatchGameStateMachineBaseState([NotNull] TileMatchGameLogic _gameLogic)
            {
                _m_gameLogic = _gameLogic;
            }
            
            public abstract void doPress(TileMatchBlockInfo _cubeInfo);
        }
    }
}
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class DragBoxGameController : _AMiniGameController
    {
        [NotNull] private DragBoxGameLogic _m_gameLogic;
        
        public DragBoxGameController([NotNull] DragBoxGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
    }
}
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameController : _AMiniGameController
    {
        [NotNull] private PuzzleGameLogic _m_puzzleGameLogic;
        
        public PuzzleGameController([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_puzzleGameLogic = _gameLogic;
        }

        public void onMatchItemPointDown(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _m_puzzleGameLogic.onMatchItemPointDown(_itemWnd);
        }
        
        public void onMatchItemDrag(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _m_puzzleGameLogic.onMatchItemDrag(_itemWnd);
        }
        
        public void onMatchItemPointUp(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _m_puzzleGameLogic.onMatchItemPointUp(_itemWnd);
        }
    }
}
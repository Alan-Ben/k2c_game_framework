using System;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameMainWndUnit : _APuzzleGameUnit
    {
        [NotNull] GGUIWndPuzzleGame _m_puzzleGameMainWnd = GGUIWndPuzzleGame.instance;
        private GGUIWndPuzzleGameMatch _m_wGameMatchWnd;

        [NotNull] internal GGUIWndPuzzleGame puzzleGameMainWnd { get { return _m_puzzleGameMainWnd; } }
        
        public PuzzleGameMainWndUnit([NotNull] PuzzleGameLogic _gameLogic, [NotNull] PuzzleGameController _controller) : base(_gameLogic, _controller)
        {
        }

        public void preLoadWnd(Action _complete, Action _fail)
        {
            // GGUIWndPuzzleGame窗口预加载
            _m_puzzleGameMainWnd.load(() =>
            {
                // 加载完GGUIWndPuzzleGame窗口后再加载GGUIWndPuzzleGameMatch窗口
                _loadGameMatchWnd((_wnd) =>
                {
                    if (_wnd == null)
                    {
                        _fail?.Invoke();
                        return;
                    }
                    
                    _m_wGameMatchWnd = _wnd;
                    _m_wGameMatchWnd.setGameController(_m_puzzleGameController);
                    _complete?.Invoke();
                });
            });
        }

        public void discardPreLoadWnd()
        {
            _m_puzzleGameMainWnd.discard();
            
            // _m_wGameMatchWnd窗口会在_m_puzzleGameMainWnd窗口释放时释放 所以这里不需要再次释放, 只需要制空引用就行
            _m_wGameMatchWnd = null;
        }

        public override void init()
        {
            _m_puzzleGameLogic.uiScene.showMainWnd(_m_puzzleGameMainWnd);
        }

        public override void discard()
        {
            _m_puzzleGameMainWnd.hideWnd();
        }
        
        /// <summary>
        /// 设置游戏表现状态
        /// </summary>
        /// <param name="_state"></param>
        /// <param name="_forceChg"></param>
        internal void setGameShowState(EPuzzleGameState _state, bool _forceChg = false)
        {
            _m_wGameMatchWnd?.setGameState(_state, _forceChg);
        }
        
        /// <summary>
        /// 游戏成功表现
        /// </summary>
        internal void playGameSuccess(Action _playDone)
        {
            _m_wGameMatchWnd?.playGameSuccess(getGameSerialize, _playDone);
        }
        
        public long getGameSerialize()
        {
            return _m_puzzleGameLogic.gameSerializeId;
        }
        
        #region 对GGUIWndPuzzleGameMatch操作

        private void _loadGameMatchWnd(Action<GGUIWndPuzzleGameMatch> _onLoadDone)
        {
            if (_m_puzzleGameLogic.puzzleGameRefObj == null)
            {
                _onLoadDone?.Invoke(null);
                return;
            }
            
            _m_puzzleGameMainWnd.loadGameMatchWnd(_m_puzzleGameLogic.puzzleGameRefObj.ui_match_prefab, _onLoadDone);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_action"></param>
        internal void dealAllMatchItem(Func<GGUIWndPuzzleGameMatchItem, bool> _action)
        {
            if (_action == null)
                return;

            _m_wGameMatchWnd?.dealAllMatchItem(_action);
        }
        
        internal void dealAllMatchItemBg(Func<GGUIWndPuzzleGameMatchItemBg, bool> _action)
        {
            if (_action == null)
                return;

            _m_wGameMatchWnd?.dealAllMatchItemBg(_action);
        }
        
        #endregion
    }
}
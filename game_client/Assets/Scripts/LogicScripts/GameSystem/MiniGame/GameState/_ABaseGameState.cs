using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _ABaseGameState<T_STATE, T_GAME_LOGIC> : _ASimpleState<T_STATE>
        where T_STATE : Enum
        where T_GAME_LOGIC : _AMiniGameLogic
    {
        [NotNull] protected readonly T_GAME_LOGIC _m_gameLogic;
        
        protected _ABaseGameState([NotNull] T_GAME_LOGIC _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        [NotNull] public T_GAME_LOGIC gameLogic { get { return _m_gameLogic;  } }
    }
}
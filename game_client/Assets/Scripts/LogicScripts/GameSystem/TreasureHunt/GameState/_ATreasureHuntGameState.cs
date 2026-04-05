using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class TreasureHuntGameLogic
    {
        public enum TreasureHuntGameStateType
        {
            NONE, // 空状态
            IDLE, // 待机状态
            RUN_UP, // 助跑状态
            GAMING, // 可操控状态
            END, // 结束状态
        }


        public abstract class _ATreasureHuntGameState : _ASimpleState<TreasureHuntGameStateType>
        {
            [NotNull] private readonly TreasureHuntGameLogic _m_gameLogic;


            protected _ATreasureHuntGameState([NotNull] TreasureHuntGameLogic _gameLogic)
            {
                _m_gameLogic = _gameLogic;
            }


            [NotNull] public TreasureHuntGameLogic gameLogic { get { return _m_gameLogic; } }
        }
        public abstract class _ATreasureHuntGameState<T> : _ASimpleState<TreasureHuntGameStateType, T>
        {
            [NotNull] private readonly TreasureHuntGameLogic _m_gameLogic;


            protected _ATreasureHuntGameState([NotNull] TreasureHuntGameLogic _gameLogic)
            {
                _m_gameLogic = _gameLogic;
            }


            [NotNull] public TreasureHuntGameLogic gameLogic { get { return _m_gameLogic; } }
        }
        public abstract class _ATreasureHuntGameState<T, Y> : _ASimpleState<TreasureHuntGameStateType, T, Y>
        {
            [NotNull] private readonly TreasureHuntGameLogic _m_gameLogic;


            protected _ATreasureHuntGameState([NotNull] TreasureHuntGameLogic _gameLogic)
            {
                _m_gameLogic = _gameLogic;
            }


            [NotNull] public TreasureHuntGameLogic gameLogic { get { return _m_gameLogic; } }
        }
        public abstract class _ATreasureHuntGameState<T, Y, U> : _ASimpleState<TreasureHuntGameStateType, T, Y, U>
        {
            [NotNull] private readonly TreasureHuntGameLogic _m_gameLogic;


            protected _ATreasureHuntGameState([NotNull] TreasureHuntGameLogic _gameLogic)
            {
                _m_gameLogic = _gameLogic;
            }


            [NotNull] public TreasureHuntGameLogic gameLogic { get { return _m_gameLogic; } }
        }
    }
}
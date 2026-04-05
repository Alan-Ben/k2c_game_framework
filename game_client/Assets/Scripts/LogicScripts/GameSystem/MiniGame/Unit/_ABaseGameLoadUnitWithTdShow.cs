using System;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    /// <summary>
    /// 一个基础游戏单位，带有一个 TDShow
    /// </summary>
    public abstract class _ABaseGameLoadUnitWithTdShow<T_TDShow> : _ABaseGameLoadUnit
        where T_TDShow : _IGameUnitTDShow
    {
        protected T_TDShow _m_tdShow;
        
        protected _ABaseGameLoadUnitWithTdShow([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
        }
        
        public T_TDShow tdShow { get { return _m_tdShow; } }

        /// <summary>
        /// 加载操作
        /// </summary>
        protected sealed override void _loadOp( Action _complete)
        {
            _createTDShow(_tdShow =>
            {
                _m_tdShow = _tdShow;               
                _complete(); 
            });   
        }
        /// <summary>
        /// 卸载操作
        /// </summary>
        protected sealed override void _unloadOp()
        {
            _discardTDShow(_m_tdShow);
            _m_tdShow = default;
        }
        protected abstract void _createTDShow([NotNull] Action<T_TDShow> _complete);
        protected abstract void _discardTDShow(T_TDShow _tdShow);
    }
}
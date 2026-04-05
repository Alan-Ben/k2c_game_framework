using System;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _ABaseGameLoadUnitWithUIShow<T_UIShow> : _ABaseGameLoadUnit
        where T_UIShow : _IGameUnitUIShow
    {
        protected T_UIShow _m_uiShow;
        
        protected _ABaseGameLoadUnitWithUIShow([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
        }
        
        public T_UIShow uiShow { get { return _m_uiShow; } }

        /// <summary>
        /// 加载操作
        /// </summary>
        protected sealed override void _loadOp( Action _complete)
        {
            _createUIShow(_uiShow =>
            {
                _m_uiShow = _uiShow;               
                _complete(); 
            });   
        }
        /// <summary>
        /// 卸载操作
        /// </summary>
        protected sealed override void _unloadOp()
        {
            _discardUIShow(_m_uiShow);
            _m_uiShow = default;
        }
        protected abstract void _createUIShow([NotNull] Action<T_UIShow> _complete);
        protected abstract void _discardUIShow(T_UIShow _uiShow);
    }
}
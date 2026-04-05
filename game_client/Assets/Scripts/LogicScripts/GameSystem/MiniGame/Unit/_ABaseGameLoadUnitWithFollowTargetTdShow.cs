using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    /// <summary>
    /// 一个基础游戏单位，带有一个 TDShow 和一个 UI跟随物体
    /// </summary>
    public abstract class _ABaseGameLoadUnitWithFollowTargetTdShow<T_TDShow, T_UIFollower> : _ABaseGameLoadUnit
        where T_TDShow : _IGameUnitFollowTargetTDShow
        where T_UIFollower : _AALGGUICommonFollowItemController, new()
    {
        protected T_TDShow _m_tdShow;
        protected T_UIFollower _m_uiFollower;

        [NotNull] private _IALCommonFollowInstanceContainer _m_followInstanceContainer;
        private _AALCommonFollowInstance _m_followTarget;
        
        protected _ABaseGameLoadUnitWithFollowTargetTdShow([NotNull] _AGameLogic _gameLogic, [NotNull]_IALCommonFollowInstanceContainer _followInstanceContainer) : base(_gameLogic)
        {
            _m_followInstanceContainer = _followInstanceContainer;
        }

        public T_TDShow tdShow { get { return _m_tdShow; } }
        public T_UIFollower uiFollower { get { return _m_uiFollower; } }

        /// <summary>
        /// 加载操作
        /// </summary>
        protected sealed override void _loadOp(Action _complete)
        {
            _createTDShow(_tdShow =>
            {
                _m_tdShow = _tdShow;
                _m_followTarget = _m_tdShow?.getFollowTarget();
                if (_m_followTarget != null)
                {
                    _m_followInstanceContainer.regInstance(_m_followTarget);
                    _m_uiFollower = new T_UIFollower();
                    _m_followTarget.addController(_m_uiFollower);
                }
                _complete();
            });
        }

        /// <summary>
        /// 卸载操作
        /// </summary>
        protected sealed override void _unloadOp()
        {
            _m_followInstanceContainer.removeInstance(_m_followTarget);
            _m_followTarget?.discard();
            _m_followTarget = null;
            _m_uiFollower = null;
            _discardTDShow(_m_tdShow);
            _m_tdShow = default;
        }

        protected abstract void _createTDShow([NotNull] Action<T_TDShow> _complete);
        protected abstract void _discardTDShow(T_TDShow _tdShow);
    }
}
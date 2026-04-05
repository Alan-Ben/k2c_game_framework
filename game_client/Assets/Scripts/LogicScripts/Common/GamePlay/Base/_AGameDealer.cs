using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 游戏逻辑处理器
    /// </summary>
    public abstract class _AGameDealer
    {
        // 游戏逻辑层实例
        [NotNull] private _AGameLogic _m_gameLogic;
        
        /* 原来访问权限为protected, 但是想在热更工程使用的话, 无法访问带参的protected构造方法, 所以先改成public
         *
         */public _AGameDealer([NotNull] _AGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        /// <summary>
        /// 游戏逻辑层实例
        /// </summary>
        [NotNull] public _AGameLogic gameLogic { get { return _m_gameLogic; } }
        /// <summary>
        /// 处理器是否运行起来了
        /// </summary>
        public bool isRunning { get { return _m_gameLogic.isRunning; } }

        /// <summary>
        /// 处理器启动时触发的逻辑
        /// </summary>
        protected abstract void _onStart();
        /// <summary>
        /// 处理器停止时触发的逻辑
        /// </summary>
        protected abstract void _onStop();
        /// <summary>
        /// 处理器每帧的 tick 逻辑
        /// </summary>
        protected abstract void _onTick(float _deltaTime);
        /// <summary>
        /// 当有新的单位创建时的逻辑
        /// </summary>
        protected abstract void _onAddGameUnit(_AGameUnit _unit);
        /// <summary>
        /// 当单位被移除时的逻辑
        /// </summary>
        protected abstract void _onRemoveGameUnit(_AGameUnit _unit);
        

        /// <summary>
        /// 启动处理器
        /// </summary>
        internal void start()
        {
            _onStart();
        }
        /// <summary>
        /// 停止处理器
        /// </summary>
        internal void stop()
        {
            _onStop();
        }
        /// <summary>
        /// 处理器每帧 tick
        /// </summary>
        internal void tick(float _deltaTime)
        {
            _onTick(_deltaTime);
        }
        /// <summary>
        /// 新的单位加入处理器
        /// </summary>
        internal void addGameUnit(_AGameUnit _unit)
        {
            _onAddGameUnit(_unit);
        }
        /// <summary>
        /// 旧的单位加入处理器
        /// </summary>
        internal void removeGameUnit(_AGameUnit _unit)
        {
            _onRemoveGameUnit(_unit);
        }
    }
}
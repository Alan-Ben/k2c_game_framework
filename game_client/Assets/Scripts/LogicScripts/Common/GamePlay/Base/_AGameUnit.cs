
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一个游戏逻辑层单位
    /// </summary>
    public abstract class _AGameUnit
    {
        // 这个 unit 所属的逻辑层
        [NotNull] private _AGameLogic _m_gameLogic;

        /* 原来访问权限为protected, 但是想在热更工程使用的话, 无法访问带参的protected构造方法, 所以先改成public
         * 
         */public _AGameUnit([NotNull] _AGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        /// <summary>
        /// 游戏逻辑管理器
        /// </summary>
        [NotNull] public _AGameLogic gameLogic { get { return _m_gameLogic; } }

        public abstract void init();
        public abstract void discard();
    }
}


using System;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一个基础游戏单位
    /// </summary>
    /// <remarks>
    /// 外部不要直接把这个类加入到 GameLogic 中，而是调用它的 load 方法，在加载完后会自动添加到 GameLogic 中
    /// </remarks>
    public abstract class _AGameLoadUnit : _AGameUnit
    {
        // 是否完成了加载
        private bool _m_isLoaded;
        // 是否正在加载
        private bool _m_isLoading;
        // 加载计数
        private int _m_loadCount;
        // 加载完成的回调
        private Action _m_onLoaded;
        

        /* 原来访问权限为protected, 但是想在热更工程使用的话, 无法访问带参的protected构造方法, 所以先改成public
         *
         */public _AGameLoadUnit([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_isLoaded = false;
            _m_isLoading = false;
            _m_loadCount = 0;
            _m_onLoaded = null;
        }

        protected bool isLoaded { get { return _m_isLoaded; } }

        /// <summary>
        /// 加载这个游戏单位
        /// </summary>
        /// <remarks>
        /// 加载完成后会自动添加到 GameLogic 中
        /// </remarks>
        public void load(Action _complete = null)
        {
            // 增加加载请求
            _m_loadCount++;

            // 如果已经加载完了，就调用加载完成事件
            if (_m_isLoaded)
                _complete?.Invoke();

            // 先把加载事件存起来
            _m_onLoaded += _complete;
            // 如果没有加载完成，同时也没有正在加载，就开始加载
            if (!_m_isLoaded && !_m_isLoading)
            {
                // 标记正在加载
                _m_isLoading = true;
                // 进行加载操作
                _loadOp(() =>
                {
                    // 设置已经加载完成
                    _m_isLoading = false;
                    _m_isLoaded = true;//这里应该先设置加载已经完成, 不然在调用_m_onLoaded时是未完成状态, 后面再判断是否需要销毁
                    
                    // 调用加载完成事件
                    _m_onLoaded?.Invoke();
                    _m_onLoaded = null;

                    // 如果现在计数已经小于等于 0，说明已经用不着这个资源了，就直接释放
                    if (_m_loadCount <= 0)
                    {
                        _unloadOp();
                        _m_isLoaded = false;
                    }
                    else
                    {
                        _m_isLoaded = true;
                        gameLogic.addGameUnit(this);
                    }
                });
            }
        }

        /// <summary>
        /// 卸载这个游戏单位
        /// </summary>
        /// <remarks>
        /// 卸载后会自动从 GameLogic 中移除
        /// </remarks>
        public void unload()
        {
            // 减少加载统计
            _m_loadCount--;
            // 判断数量是否仍然大于 0，大于 0 说明资源仍然在使用，不进行后续处理
            if (_m_loadCount > 0)
                return;

            // 重置加载统计
            if (_m_loadCount < 0)
                _m_loadCount = 0;

            // 如果还没加载完成，就不进行后续处理
            if (!_m_isLoaded)
                return;

            gameLogic.removeGameUnit(this);
            // 调用卸载事件
            _unloadOp();
            _m_isLoaded = false;
        }

        /// <summary>
        /// 加载操作
        /// </summary>
        protected abstract void _loadOp([NotNull] Action _complete);

        /// <summary>
        /// 卸载操作
        /// </summary>
        protected abstract void _unloadOp();
    }
}
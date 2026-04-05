using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    /// <summary>
    /// 一个基础游戏单位
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class _ABaseGameLoadUnit : _AGameUnit
    {
        // 是否完成了加载
        private bool _m_isLoaded;
        // 是否正在加载
        private bool _m_isLoading;
        // 加载计数
        private int _m_loadCount;
        // 加载完成的回调
        private Action _m_onLoaded;

        protected _ABaseGameLoadUnit([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_isLoaded = false;
            _m_isLoading = false;
            _m_loadCount = 0;
            _m_onLoaded = null;
        }

        public void regLoadedDone(Action _action)
        {
            if (_m_isLoaded)
            {
                _action?.Invoke();
                return;
            }
            
            _m_onLoaded += _action;
        }
        
        public void unRegLoadedDone(Action _action)
        {
            _m_onLoaded -= _action;
        }
        
        /// <summary>
        /// 加载操作
        /// </summary>
        public void load(Action _complete)
        {
            _m_onLoaded += _complete;
            load();
        }
        
        /// <summary>
        /// 加载操作
        /// </summary>
        public void load()
        {
            // 增加加载请求
            _m_loadCount++;
            
            // 如果已经加载完了，就调用加载完成事件
            if (_m_isLoaded)
            {
                _m_onLoaded?.Invoke();
                return;
            }

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
                    }
                });
            }
        }

        /// <summary>
        /// 卸载操作
        /// </summary>
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
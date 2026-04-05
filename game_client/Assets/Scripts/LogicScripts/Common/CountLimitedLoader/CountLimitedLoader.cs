
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 限制同时加载数量的加载器
    /// </summary>
    public class CountLimitedLoader
    {
        // 待加载的列表
        [NotNull] private readonly List<_ICountLimitedLoaderTask> _m_loadTasks;
        // 任务的加载计数
        [NotNull] private readonly Dictionary<_ICountLimitedLoaderTask, int> _m_taskCounter;
        // 当前正在加载的任务列表
        [NotNull] private readonly HashSet<_ICountLimitedLoaderTask> _m_currentLoadTask;
        // 同时可以加载的最大数量
        private readonly int _m_maxCount;
        // 当前正在加载的任务
        private int _m_curLoadCount;
        
        /// <summary>
        /// 指定可以接受的限制数量来构建
        /// </summary>
        public CountLimitedLoader(int _maxCount)
        {
            _m_maxCount = _maxCount;
            _m_curLoadCount = 0;
            
            _m_loadTasks = new List<_ICountLimitedLoaderTask>();
            _m_taskCounter = new Dictionary<_ICountLimitedLoaderTask, int>();
            _m_currentLoadTask = new HashSet<_ICountLimitedLoaderTask>();
        }
        
        /// <summary>
        /// 加载某个对象
        /// </summary>
        public void load(_ICountLimitedLoaderTask _loader)
        {
            if (_loader == null)
                return;

            // 查看引用计数，现在是否应该加载
            if (!_m_taskCounter.ContainsKey(_loader) || _m_taskCounter[_loader] <= 0)
            {
                _m_taskCounter[_loader] = 1;

                // 检查是否在正在加载列表里，在的话就不处理，加载完直接采用即可
                if (!_m_currentLoadTask.Contains(_loader))
                {
                    // 添加到待处理列表中
                    _m_loadTasks.Add(_loader);
                    // 判断是否要加载，并开始
                    _checkAndLoad();
                }
            }
            else
            {
                // 已经在加载或加载完成的，就计数加 1
                _m_taskCounter[_loader] += 1;
            }
        }
        /// <summary>
        /// 卸载某个对象
        /// </summary>
        public void discard(_ICountLimitedLoaderTask _loader)
        {
            if (_loader == null)
                return;

            // 如果之前都没有计数过，就直接不处理
            if (!_m_taskCounter.ContainsKey(_loader))
                return;

            // 如果计数再减就没了，就把它移除，如果加载完了就销毁
            if (_m_taskCounter[_loader] <= 1)
            {
                _m_taskCounter.Remove(_loader);
                // 从待加载列表中尝试移除
                if (!_m_loadTasks.Remove(_loader))
                {
                    // 如果没有移除成功，就尝试去正在加载的列表中找，找到了的话就不处理了，等待加载完后自动处理
                    if (!_m_currentLoadTask.Contains(_loader))
                    {
                        // 没找到就直接卸载，因为有计数存在，这个 _loader 就必定是已经加载完的
                        _loader.discard();
                    }
                }
            }
            else
            {
                // 不删除的话就计数减 1
                _m_taskCounter[_loader] -= 1;
            }
        }
        /// <summary>
        /// 输出调试字符串，EDITOR 模式下会打印正在加载的任务
        /// </summary>
        public string getDebugString()
        {
            string result = $"现在正在加载的任务数量为 {_m_curLoadCount} 个";
#if UNITY_EDITOR
            result += "分别为：\n";
            foreach (_ICountLimitedLoaderTask task in _m_currentLoadTask)
            {
                result += task.debugName + "\n";
            }
#endif
            return result;
        }
        
        // 检查并开始加载
        private void _checkAndLoad()
        {
            // 如果同时加载的数量达到上限了，或者没有要加载的任务，就返回
            if (_m_curLoadCount >= _m_maxCount || _m_loadTasks.Count <= 0)
                return;

            // 直到到达并行上限，都循环下去，获取当前要加载的内容
            while (_m_curLoadCount < _m_maxCount)
            {
                // 如果没有要加载的内容了，就退出
                if (_m_loadTasks.Count <= 0)
                    break;
                
                // 获取第一个加载任务
                _ICountLimitedLoaderTask task = _m_loadTasks[0];
                _m_loadTasks.RemoveAt(0);
                
                // 添加到正在加载的列表中
                _m_currentLoadTask.Add(task);
                
                // 增加加载计数
                _m_curLoadCount++;
                // 开始加载
                task.load(() =>
                {
                    // 加载完成后减掉加载计数
                    _m_curLoadCount--;
                    
                    // 从正在加载的列表中移除
                    _m_currentLoadTask.Remove(task);
                    
                    // 如果已经不存在计数了，就直接销毁
                    if (!_m_taskCounter.ContainsKey(task) || _m_taskCounter[task] <= 0)
                        task.discard();
                    
                    // 结束后再检查一次，是否可以运行
                    _checkAndLoad();
                });
            }
        }
    }
}
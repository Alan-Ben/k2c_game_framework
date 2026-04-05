
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 一个可以被 cache 回收的类
    /// </summary>
    public abstract class _AHotfixCacheClass
    {
        /// <summary>
        /// 被 cache 回收时调用
        /// </summary>
        public abstract void reset();
        /// <summary>
        /// 被 cache 销毁时调用
        /// </summary>
        public abstract void discard();
    }
    
    /// <summary>
    /// 一个可以被 cache 回收的类的模板
    /// </summary>
    /// <remarks>
    /// 适用于某个类需要频繁的 new
    /// </remarks>
    public abstract class HotfixCacheClass<T> : _AHotfixCacheClass
        where T : _AHotfixCacheClass, new()
    {
        // 内部的静态缓存
        private static Cache _g_cache;

        /// <summary>
        /// 获取一个这个类的实例
        /// </summary>
        [NotNull] 
        public static T pop()
        {
            if (_g_cache == null)
            {
                Debug.LogError($"{typeof(T).FullName}.pop 时，没有 cache 存在哦，先调用 {typeof(T).FullName}.createCache 方法，再使用这个方法");
                return new T();
            }
            
            return _g_cache?.popItem();
        }
        /// <summary>
        /// 放回这个类的实例
        /// </summary>
        public static void pushBack(T _task)
        {
            if (_task == null)
                return;
            
            _g_cache?.pushBackCacheItem(_task);
        }
        /// <summary>
        /// 构建这个类的 cache
        /// </summary>
        public static void createCache(int _initCount, int _maxCount)
        {
            if (_g_cache != null)
            {
                Debug.LogError($"{typeof(T).FullName}.createCache 时，已经存在 cache 了，需要先调用 {typeof(T).FullName}.discardCache 再调用这个方法");
                return;
            }
            _g_cache = new Cache(_initCount, _maxCount);
        }
        /// <summary>
        /// 销毁这个类的 cache
        /// </summary>
        public static void discardCache()
        {
            _g_cache?.discard();
            _g_cache = null;
        }
        
        /// <summary>
        /// 内部的缓存类
        /// </summary>
        private class Cache : _AHotfixUnsafeCacheController<T, T>
        {
            public Cache(int _minCount, int _maxCount) : base(_minCount, _maxCount)
            {
            }

            public Cache(int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
            {
            }

            protected override string _warningTxt { get { return typeof(T).FullName; } }
            protected override void _onInit(T _template)
            {
            }

            protected override T _createItem(T _template)
            {
                return new T();
            }

            protected override void _discardItem(T _item)
            {
                _item?.discard();
            }

            protected override void _resetItem(T _item)
            {
                _item?.reset();
            }
        }
    }
}
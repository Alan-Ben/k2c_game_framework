namespace Hotfix
{
    /// <summary>
    /// 一个状态通用的基类((仿造主工程_ATALBasicStateCacheController))
    /// </summary>
    /// 外部实现的时候不要直接继承这个类，继承<see cref="_AState{T}"/>系列的类
    public abstract class _ATHotfixBasicStateCacheController<T> : _AHotfixUnsafeCacheController<T, T>
    {
        protected _ATHotfixBasicStateCacheController(int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            init(createObj());
        }
        
        /// <summary>
        /// 纯粹的直接创建对象的处理
        /// </summary>
        /// <returns></returns>
        protected abstract T createObj();
    }
}
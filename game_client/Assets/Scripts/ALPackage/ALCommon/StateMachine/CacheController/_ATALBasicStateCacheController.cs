using System;
using ALPackage;

namespace ALPackage
{
    /// <summary>
    /// 一个状态通用的基类
    /// </summary>
    /// <remarks>
    /// 外部实现的时候不要直接继承这个类，继承<see cref="_AState{T}"/>系列的类
    /// </remarks>
    public abstract class _ATALBasicStateCacheController<T> : _AALUnsafeCacheController<T, T>
    {
        public _ATALBasicStateCacheController(int _minCount ,int _maxCount) : base(_minCount, _maxCount) { init(createObj()); }

        protected override T _createItem(T _template) { return createObj(); }
        protected override void _discardItem(T _item) { }
        protected override void _onInit(T _template) { }

        /// <summary>
        /// 纯粹的直接创建对象的处理
        /// </summary>
        /// <returns></returns>
        protected abstract T createObj();
    }
}


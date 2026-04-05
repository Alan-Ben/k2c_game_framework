using System;
using ALPackage;

namespace ALPackage
{
    /// <summary>
    /// 状态机状态的缓存类
    /// </summary>
    public class _TALBasicStateCacheController<STATE_T, T> : _ATALBasicStateCacheController<STATE_T> where T : Enum where STATE_T : _ATALStateBase<T>
    {
        //创建函数对象
        private Func<STATE_T> _m_fCreateFunc;

        protected override string _warningTxt => typeof(STATE_T).ToString();

        public _TALBasicStateCacheController(Func<STATE_T> _createFunc, int _minCount ,int _maxCount) : base(_minCount, _maxCount)
        {
            //设置创建函数
            _m_fCreateFunc = _createFunc;
        }

        protected override STATE_T _createItem(STATE_T _template) { return createObj(); }
        protected override void _discardItem(STATE_T _item) { }
        protected override void _onInit(STATE_T _template) { }

        /// <summary>
        /// 纯粹的直接创建对象的处理
        /// </summary>
        /// <returns></returns>
        protected override STATE_T createObj()
        {
            if (null != _m_fCreateFunc)
                return _m_fCreateFunc();

            return default(STATE_T);
        }
        //设置对象无效
        protected override void _resetItem(STATE_T _item)
        {
            if (null != _item)
                _item.resetData();
        }
    }
}


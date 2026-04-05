using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************
     * 缓存池模板对象
     **/
    public abstract class _AALUICacheController<T, TEMP> : _AALCacheController<T, TEMP>
        where T : _IALBasicUIWndInterface
        where TEMP : _AALBasicUIWndMono
    {
        protected _AALUICacheController(int _minCount, int _maxCount)
            : base(_minCount, _maxCount)
        {
        }
        protected _AALUICacheController(int _minCount, int _maxCount, int _addUnit)
            : base(_minCount, _maxCount, _addUnit)
        {
        }

        //设置对象无效
        protected override void _resetItem(T _item)
        {
            //重置对象
            _item.resetWnd();
        }
    }
}

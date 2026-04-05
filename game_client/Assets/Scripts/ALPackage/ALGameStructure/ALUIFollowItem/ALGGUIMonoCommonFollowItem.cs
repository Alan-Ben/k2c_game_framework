using UnityEngine;
using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 基础框架中跟随节点的UI基本Mono对象
    /// 所有需要跟随3D坐标的UI对象脚本都需要继承自这个脚本
    /// 
    /// 注意这类跟随脚本的坐标定位方式需要统一
    /// </summary>
    public class ALGGUIMonoCommonFollowItem : _AALBasicUIWndMono
    {
        //资源数据对象，不做序列化，在实际逻辑调用的时候会赋值，方便释放的时候自动归位
        [System.NonSerialized]
        private _AALBasicLoadResIndexInfo _m_iResIndex;

        protected internal _AALBasicLoadResIndexInfo _resIndex { get { return _m_iResIndex; } }

        /// <summary>
        /// 设置资源索引数据
        /// </summary>
        /// <param name="_index"></param>
        protected internal void _setResIndex(_AALBasicLoadResIndexInfo _index)
        {
            _m_iResIndex = _index;
        }
    }
}


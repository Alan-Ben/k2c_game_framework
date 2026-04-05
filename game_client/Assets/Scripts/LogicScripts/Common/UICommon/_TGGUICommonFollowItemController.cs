using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用的controller模板类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="W"></typeparam>
    public class _TGGUICommonFollowItemController<T, W> : _ATALGGUICommonFollowItemController<T,W>
        where T : ALGGUIMonoCommonFollowItem
        where W : _ATALGGUIWndCommonFollowItem<T>
    {
        //资源id
        private GResPathIndex _m_followIndex;
        
        //对应创建函数
        private Func<T, W> _m_creatWnd;

        public _TGGUICommonFollowItemController(int _resPathId, Func<T, W> _creatWnd)
        {
            _m_followIndex = new GResPathIndex(_resPathId);
            _m_creatWnd = _creatWnd;
        }
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_followIndex; } }
        
        
        protected override W _createItemWnd(T _wndMono)
        {
            if (null != _m_creatWnd)
            {
                W wWnd = _m_creatWnd(_wndMono);

                return wWnd;
            }

#if UNITY_EDITOR
            Debug.LogError("controller的创建委托没传");
#endif
            
            return null;
        }
    }
}
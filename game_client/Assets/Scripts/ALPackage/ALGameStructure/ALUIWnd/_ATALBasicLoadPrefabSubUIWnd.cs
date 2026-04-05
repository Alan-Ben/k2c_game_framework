using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public abstract class _ATALBasicLoadPrefabSubUIWnd<T> : _ATALBasicLoadUIWnd<T> where T : _AALBasicUIWndMono
    {
        /** 父对象 */
        private Transform _m_tParentTransform;

        protected _ATALBasicLoadPrefabSubUIWnd(Transform _parent)
        {
            _m_tParentTransform = _parent;
        }

        /// <summary>
        /// 获取本对象加载后所挂载的父节点
        /// </summary>
        protected override Transform _getParentTransForm()
        {
            return _m_tParentTransform;
        }
    }
}

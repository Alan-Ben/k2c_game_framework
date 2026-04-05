using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

/// <summary>
/// 模板窗口cache管理对象
/// </summary>
public abstract class _AALUITemplateWndUnsafeCache<T_W, T_M> : _AALUnsafeCacheController<T_W, T_M>
    where T_M : _AALBasicUIWndMono
    where T_W : _ATALBasicUISubWnd<T_M>
{
    /// <summary>
    /// 父节点对象
    /// </summary>
    private Transform _m_tParentTrans;

    public _AALUITemplateWndUnsafeCache(Transform _parentTrans, int _minCount, int _maxCount, int _addUnit)
        : base(_minCount, _maxCount, _addUnit)
    {
        if(null != _parentTrans)
            _m_tParentTrans = _parentTrans;
    }

    /// <summary>
    /// 创建实例对象
    /// </summary>
    /// <param name="_template"></param>
    /// <returns></returns>
    protected override T_W _createItem(T_M _template)
    {
        if(_template == null)
            return null;

        T_M monoClone = GameObject.Instantiate(_template) as T_M;
        if(null == monoClone)
            return null;

        monoClone.transform.SetParent(_m_tParentTrans);
        monoClone.transform.localPosition = Vector3.zero;
        monoClone.transform.localScale = Vector3.one;

        //创建一个子窗口管理对象
        T_W newWnd = _createWndByMono(monoClone);
        if(null == newWnd)
        {
            //创建对象无效，删除创建对象资源并退出
            ALUnityCommon.releaseGameObj(monoClone);
            return null;
        }

        ///初始化窗口信息
        _initNewWnd(newWnd);

        return newWnd;
    }

    protected override void _discardItem(T_W _item)
    {
        if(null != _item)
        {
            T_M mono = _item.wnd;
            _item.discard();
            //释放资源
            ALUnityCommon.releaseGameObj(mono);
        }
    }

    /// <summary>
    /// 根据脚本创建窗口对象
    /// </summary>
    /// <param name="_mono"></param>
    /// <returns></returns>
    protected abstract T_W _createWndByMono(T_M _mono);
    /// <summary>
    /// 初始化实例化窗口对象
    /// </summary>
    /// <param name="_wnd"></param>
    protected abstract void _initNewWnd(T_W _wnd);
}

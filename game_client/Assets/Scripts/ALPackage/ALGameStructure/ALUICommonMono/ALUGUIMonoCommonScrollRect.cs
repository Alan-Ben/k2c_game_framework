using System;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*******************
 * 拖拽对象的自定义实现
 **/
public class ALUGUIMonoCommonScrollRect : ScrollRect
{
    //是否正在拖拽
    //private bool _m_bIsDragging;
    //结束拖拽时的处理函数
    private Action _m_dOnStartDragDelegate;
    //结束拖拽时的处理函数
    private Action _m_dOnEndDragDelegate;

    public Action onStartDragDelegate { get { return _m_dOnStartDragDelegate; } set { _m_dOnStartDragDelegate = value; } }
    public Action onEndDragDelegate { get { return _m_dOnEndDragDelegate; } set { _m_dOnEndDragDelegate = value; } }
    //
    // Summary:
    //     ///
    //     Handling for when the content is beging being dragged.
    //     ///
    //
    // Parameters:
    //   eventData:
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        //设置正在拖拽
        //_m_bIsDragging = true;
        //调用结束拖拽函数
        if (null != _m_dOnStartDragDelegate)
            _m_dOnStartDragDelegate();
    }
    //
    // Summary:
    //     ///
    //     Handling for when the content is dragged.
    //     ///
    //
    // Parameters:
    //   eventData:
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }
    //
    // Summary:
    //     ///
    //     Handling for when the content has finished being dragged.
    //     ///
    //
    // Parameters:
    //   eventData:
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        //设置正在拖拽
        //_m_bIsDragging = false;
        //调用结束拖拽函数
        if (null != _m_dOnEndDragDelegate)
            _m_dOnEndDragDelegate();
    }
}

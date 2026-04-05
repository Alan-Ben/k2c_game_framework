using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public abstract class _ANPGGUIBasicSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATALUGUISubWndBasicContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _TALUGUIMonoContainerWnd<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        protected _ANPGGUIBasicSubWndContainer(_T_CONTAINER_MONO _containerMono)
                : base(_containerMono)
        {
        }

        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {

        }
    }
}

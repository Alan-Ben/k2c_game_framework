using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 背包物品出售面板
    /// </summary>
    public abstract class _ABasicBagItemBar : _AALUGUIGridBarController
    {
        /// <summary>
        /// 数据存储对象
        /// </summary>
        protected BagItem _m_biBagItem = null;

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public abstract void init(Action _doneDelegate);
        /// <summary>
        /// 刷新物品信息
        /// </summary>
        /// <param name="_item"></param>
        protected abstract void _refreshItem(BagItem _item);

        /// <summary>
        /// 设置物品信息
        /// </summary>
        /// <param name="_item"></param>
        public void setItem(BagItem _item)
        {
            _m_biBagItem = _item;

            //刷新显示
            _refreshItem(_m_biBagItem);
        }
        
        protected override int _compareWhenEqualIdx(_AALUGUIGridBarController _other)
        {
            if(_other is BagItemTypeBarController);//类型bar会在后面显示
                return -1;
            
            return base._compareWhenEqualIdx(_other);
        }

        public abstract RectTransform getWndRectTransform();
    }
}

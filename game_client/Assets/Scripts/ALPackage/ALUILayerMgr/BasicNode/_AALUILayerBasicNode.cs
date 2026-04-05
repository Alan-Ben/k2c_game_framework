﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的所有层级节点的接口类
     **/
    public abstract class _AALUILayerBasicNode
    {
        //父节点
        private _AALUILayerBasicNode _m_pParent;
        //本节点的对象
        private _AALUILayerBasicNodeObj _m_noNodeObj;

        //子节点队列
        protected List<_AALUILayerBasicNode> _m_lChildList;

        //当前的sortingLayer层级
        private int _m_iCurSortingLayerId;
        //当前的层级
        protected int _m_iCurOrderInLayer;

        public _AALUILayerBasicNode(_AALUILayerBasicNodeObj _nodeObj)
        {
            _m_pParent = null;
            _m_noNodeObj = _nodeObj;

            _m_lChildList = new List<_AALUILayerBasicNode>();

            _m_iCurSortingLayerId = 0;
            _m_iCurOrderInLayer = 0;
        }
        public _AALUILayerBasicNode(_AALUILayerBasicNodeObj _nodeObj, int _sortingLayerId, int _basicOrderLayer)
        {
            _m_pParent = null;
            _m_noNodeObj = _nodeObj;

            _m_lChildList = new List<_AALUILayerBasicNode>();

            _m_iCurSortingLayerId = _sortingLayerId;
            _m_iCurOrderInLayer = _basicOrderLayer;
        }


        /// <summary>
        /// 当前所在节点的Go的InstanceId
        /// </summary>
        public virtual int goInstanceID { get { return nodeObj.transF.GetInstanceID(); } }
        /// <summary>
        /// node挂载对象
        /// </summary>
        public _AALUILayerBasicNodeObj nodeObj { get { return _m_noNodeObj; } }
        /// <summary>
        /// 父节点对象
        /// </summary>
        public _AALUILayerBasicNode parentNode { get { return _m_pParent; } }
        /// <summary>
        /// 当前所在节点对应的canvas对象
        /// </summary>
        public Transform transF { get { return null == _m_noNodeObj ? null : _m_noNodeObj.transF; } }
        /// <summary>
        /// 当前所在层级
        /// </summary>
        public virtual int sortingLayerId { get { return _m_iCurSortingLayerId; } }
        /// <summary>
        /// 当前所在层级
        /// </summary>
        public virtual int orderInLayer { get { return _m_iCurOrderInLayer; } }
        /// <summary>
        /// 当前对象原始相对层级
        /// </summary>
        public virtual int srcOrderInLayer { get { return _m_iCurOrderInLayer; } }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="_node"></param>
        public void addChild(_AALUILayerBasicNode _node)
        {
            if(null == _node)
                return;

            //设置父节点
            _node._setParent(this);
            //添加到队列
            _m_lChildList.Add(_node);
        }

        /// <summary>
        /// 移除已存在的子节点
        /// </summary>
        /// <param name="_node"></param>
        public void rmvChild(_AALUILayerBasicNode _node)
        {
            if(null == _node)
                return;

            if(!_m_lChildList.Remove(_node))
                return;

            //调用移除事件操作
            _onRmvChild(_node);

            //重置父节点
            _node._m_pParent = null;
        }

        /// <summary>
        /// 设置当前节点的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        public void setOrderInLayer(int _sortingLayer, int _layer)
        {
            //设置自己的层级
            _setOrderInLayer(_sortingLayer, _layer);

            //遍历设置子节点的层级
            _AALUILayerBasicNode tmpNode = null;
            for(int i = 0; i < _m_lChildList.Count; i++)
            {
                tmpNode = _m_lChildList[i];
                if(null == tmpNode)
                    continue;

                //设置层级
                tmpNode.setOrderInLayer(_sortingLayer, _layer + tmpNode.orderLayerToParentNode);

                //调用设置函数
                tmpNode._onSetNewLayer(tmpNode.orderLayerToParentNode, tmpNode.orderInLayer);
            }
        }

        /// <summary>
        /// 移除所有子节点的管理数据
        /// </summary>
        public void clearAllChild(List<_IALUILayerBasicNodeMonoInterface> _recInterfaceList = null)
        {
            //遍历移除子节点
            _AALUILayerBasicNode tmpNode = null;
            while(_m_lChildList.Count > 0)
            {
                tmpNode = _m_lChildList[0];
                if(null == tmpNode)
                {
                    _m_lChildList.RemoveAt(0);
                    continue;
                }

                //移除层级
                ALUILayerMgr.instance.rmvLayerNode(tmpNode, _recInterfaceList);
            }

            _m_lChildList.Clear();
        }

        /// <summary>
        /// 资源释放函数
        /// </summary>
        public void discard()
        {
            _m_pParent = null;

            if(null != _m_noNodeObj)
                _m_noNodeObj.discard();
            _m_noNodeObj = null;

            //释放子节点
            clearAllChild();

            //调用释放处理
            _doDiscard();
        }

        /// <summary>
        /// 设置父节点对象
        /// </summary>
        /// <param name="_parent"></param>
        protected internal void _setParent(_AALUILayerBasicNode _parent)
        {
            //如原先有父节点则退出
            if(null != _m_pParent)
            {
                _m_pParent.rmvChild(this);
            }

            _m_pParent = _parent;

            //初始化设置层级
            _setOrderInLayer(_parent.sortingLayerId, _parent.srcOrderInLayer + orderLayerToParentNode);

            //调用设置函数
            _onSetNewLayer(orderLayerToParentNode, orderInLayer);
        }
        /// <summary>
        /// 设置父节点，且根据带入的层级直接设置对应层级
        /// 带入的是相对最原始层级的附加层级
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_layer"></param>
        protected internal virtual void _setParent(_AALUILayerBasicNode _parent, int _layer)
        {
            //如原先有父节点则退出
            if(null != _m_pParent)
            {
                _m_pParent.rmvChild(this);
            }

            _m_pParent = _parent;

            //初始化设置层级
            _setOrderInLayer(_parent.sortingLayerId, _parent.srcOrderInLayer + _layer);

            //调用设置函数，带入的第一个参数是相对父节点的层级
            _onSetNewLayer(_layer, orderInLayer);
        }

        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        protected virtual void _setOrderInLayer(int _sortingLayer, int _layer)
        {
            if(null == _m_noNodeObj)
                return;

            _m_iCurSortingLayerId = _sortingLayer;
            _m_iCurOrderInLayer = _layer;
            _m_noNodeObj.setOrderInLayer(_sortingLayer, _layer);
        }

        /// <summary>
        /// 相对父节点的层级对比
        /// </summary>
        public abstract int orderLayerToParentNode { get; }

        /// <summary>
        /// 子类的资源释放处理函数
        /// </summary>
        protected abstract void _doDiscard();
        /// <summary>
        /// 当移除子节点的时候触发的处理函数
        /// </summary>
        /// <param name="_node"></param>
        protected abstract void _onRmvChild(_AALUILayerBasicNode _node);
        /// <summary>
        /// 当修改当前层级的时候，触发的函数
        /// </summary>
        /// <param name="_orderLayerToParentNode"></param>
        protected abstract void _onSetNewLayer(int _orderLayerToParentNode, int _finalLayer);
    }
}

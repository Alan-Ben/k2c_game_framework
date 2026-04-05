﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 基于UI系统层级管理对象上衍生出的动态节点的管理节点对象
    /// 本类在添加子对象的时候会逐个添加子对象的层级，而不是
    /// </summary>
    public abstract class _AALUILayerBasicDynamicNode : _AALUILayerBasicNode, _IALUILayerDynamicNode, _IALUILayerParentNode
    {
        //子节点队列
        private List<_AALUILayerBasicNode> _m_lDynamicList;
        
        /// <summary>
        /// 当前动态生成的层级索引
        /// </summary>
        private int _m_iCurDynamicOrder;

        public _AALUILayerBasicDynamicNode(_AALUILayerBasicNodeObj _nodeObj)
            : base(_nodeObj)
        {
            _m_lDynamicList = new List<_AALUILayerBasicNode>();

            //所有动态层数都是从0开始
            _m_iCurDynamicOrder = 0;
        }
        public _AALUILayerBasicDynamicNode(_AALUILayerBasicNodeObj _nodeObj, int _sortingLayerId, int _basicOrderLayer)
            : base(_nodeObj, _sortingLayerId, _basicOrderLayer)
        {
            _m_lDynamicList = new List<_AALUILayerBasicNode>();

            //所有动态层数都是从0开始
            _m_iCurDynamicOrder = 0;
        }

        /// <summary>
        /// 当前所在层级，基础层级需要加上累加层级
        /// </summary>
        public override int orderInLayer { get { return srcOrderInLayer + _m_iCurDynamicOrder; } }


        #region 动态节点的处理
        /// <summary>
        /// 添加动态层级的子节点
        /// </summary>
        /// <param name="_node"></param>
        public void addDynamicChild(_AALUILayerBasicNode _node, int _additionLayer)
        {
            if(null == _node)
                return;

            //设置父节点,带入的层级是完整的计算层级，不需要重新计算
            _node._setParent(this, _makeNewLocalOrder(_additionLayer));
            //添加到队列
            _m_lDynamicList.Add(_node);
            //添加到队列
            _m_lChildList.Add(_node);
        }

        /// <summary>
        /// 创建新的order序列
        /// </summary>
        /// <returns></returns>
        protected int _makeNewLocalOrder(int _additionLayer)
        {
            if(_additionLayer >= 0)
            {
                _m_iCurDynamicOrder += _additionLayer;

                return _m_iCurDynamicOrder;
            }
            else
            {
                return _m_iCurDynamicOrder + _additionLayer;
            }
        }
        #endregion 动态节点的处理

        /// <summary>
        /// 当移除子节点的时候触发的处理函数
        /// </summary>
        /// <param name="_node"></param>
        protected override void _onRmvChild(_AALUILayerBasicNode _node)
        {
            //此时需要检查是否删除的是动态子节点，如是则需要从动态子节点中删除，并判断是否与当前的最新层级一致，如一致则需要重新核算最高层级
            if(!_m_lDynamicList.Remove(_node))
                return;

            //判断是否最新层级
            if(_node.orderLayerToParentNode != _m_iCurDynamicOrder)
                return;

            //重新检测最新层级
            int minV = 0;
            _AALUILayerBasicNode tmpNode = null;
            for(int i = 0; i < _m_lDynamicList.Count; i++)
            {
                tmpNode = _m_lDynamicList[i];
                if(null == tmpNode)
                    continue;

                if(tmpNode.orderLayerToParentNode > minV)
                    minV = tmpNode.orderLayerToParentNode;
            }

            //设置值
            _m_iCurDynamicOrder = minV;
        }
    }
}

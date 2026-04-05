﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的每个UI层级的根节点对象
     **/
    public class ALUILayerNode : _AALUILayerBasicNode, _IALUILayerParentNode
    {
        //相对父节点的额外层级
        private int _m_iOrderLayerToParent;

        public ALUILayerNode(_AALUILayerBasicNodeObj _nodeObj)
            : base(_nodeObj)
        {
            _m_iOrderLayerToParent = 0;
        }
        public ALUILayerNode(_AALUILayerBasicNodeObj _nodeObj, int _orderLayerToParent)
            : base(_nodeObj)
        {
            _m_iOrderLayerToParent = _orderLayerToParent;
        }

        /// <summary>
        /// 相对父节点的层级对比
        /// </summary>
        public override int orderLayerToParentNode { get { return _m_iOrderLayerToParent; } }

        /// <summary>
        /// 子类的资源释放处理函数
        /// </summary>
        protected override void _doDiscard()
        {

        }

        /// <summary>
        /// 当修改当前层级的时候，触发的函数
        /// </summary>
        /// <param name="_orderLayerToParentNode"></param>
        protected override void _onSetNewLayer(int _orderLayerToParentNode, int _finalLayer)
        {
        }

        /// <summary>
        /// 当移除子节点的时候触发的处理函数
        /// </summary>
        /// <param name="_node"></param>
        protected override void _onRmvChild(_AALUILayerBasicNode _node)
        {

        }
    }
}

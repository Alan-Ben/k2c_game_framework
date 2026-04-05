﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 动态生成的子节点数据对象，此对象后续还可以动态派生其他的动态子节点
     * 此节点的后续还可以继续派生Dynamic节点（可以自增层级）
     * 
     * 与ALUILayerDynamicStaticNode的区别在于，本节点的子节点可以自增层级
     **/
    public class ALUILayerDynamicNode : _AALUILayerBasicDynamicNode
    {
        //相对父节点的额外层级
        private int _m_iOrderLayerToParent;

        public ALUILayerDynamicNode(_AALUILayerBasicNodeObj _nodeObj)
            : base(_nodeObj)
        {
            _m_iOrderLayerToParent = 0;
        }

        /// <summary>
        /// 相对父节点的层级对比
        /// </summary>
        public override int orderLayerToParentNode { get { return _m_iOrderLayerToParent; } }

        /// <summary>
        /// 当修改当前层级的时候，触发的函数
        /// </summary>
        /// <param name="_orderLayerToParentNode"></param>
        protected override void _onSetNewLayer(int _orderLayerToParentNode, int _finalLayer)
        {
            _m_iOrderLayerToParent = _orderLayerToParentNode;
        }

        /// <summary>
        /// 子类的资源释放处理函数
        /// </summary>
        protected override void _doDiscard()
        {

        }
    }
}

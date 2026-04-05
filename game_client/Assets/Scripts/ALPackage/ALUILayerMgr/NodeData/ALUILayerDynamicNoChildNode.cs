﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 动态生成的子节点数据对象
     * 此节点不能作为父节点，只能作为无子节点的叶子节点
     **/
    public class ALUILayerDynamicNoChildNode : ALUILayerNoChildNode
    {
        //相对父节点的额外层级
        private int _m_iOrderLayerToParent;

        public ALUILayerDynamicNoChildNode(_AALUILayerBasicNodeObj _nodeObj)
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
        /// 第一个参数是当前设置的层级，当静态设置的时候就是父节点层级，当是动态设置的时候就是父节点生成的新层级
        /// 第二个参数是当前设置之后的实际层级
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

        /// <summary>
        /// 当移除子节点的时候触发的处理函数
        /// </summary>
        /// <param name="_node"></param>
        protected override void _onRmvChild(_AALUILayerBasicNode _node)
        {

        }
    }
}

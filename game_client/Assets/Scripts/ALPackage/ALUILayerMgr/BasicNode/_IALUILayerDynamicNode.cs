﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 动态分配节点层级的动态根节点对象
     * 本接口只是设置可以添加动态子节点，此接口只有在_AALUILayerDynamicNodeMono类中会进行调用
     **/
    public interface _IALUILayerDynamicNode
    {
        /// <summary>
        /// 添加动态层级的子节点
        /// </summary>
        /// <param name="_node"></param>
        void addDynamicChild(_AALUILayerBasicNode _node, int _additionLayer);
    }
}

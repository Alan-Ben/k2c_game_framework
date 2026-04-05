﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的所有层级节点的接口类
     **/
    public class ALUILayerNodeGameObj : _AALUILayerBasicNodeObj
    {
        private Transform _m_cgoTrans;

        public ALUILayerNodeGameObj(Transform _trans, _AALUILayerBasicMonoData _monoData, _IALUILayerBasicNodeMonoInterface _nodeInterface)
            : base(_monoData, _nodeInterface)
        {
            _m_cgoTrans = _trans;
        }

        /// <summary>
        /// 当前所在节点对应的canvas对象
        /// </summary>
        public override Transform transF { get { return _m_cgoTrans; } }
        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        public override void setOrderInLayer(int _sortingLayer, int _layer)
        {
            
        }
        /// <summary>
        /// 资源释放函数
        /// </summary>
        public override void discard()
        {
            _m_cgoTrans = null;
        }
    }
}

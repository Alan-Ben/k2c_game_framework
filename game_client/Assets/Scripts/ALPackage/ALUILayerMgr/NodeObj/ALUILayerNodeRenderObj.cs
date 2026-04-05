﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的所有层级节点的接口类
     **/
    public class ALUILayerNodeRenderObj : _AALUILayerBasicNodeObj
    {
        private Renderer _m_coRenderObj;
        private Transform _m_cgoRenderTrans;

        public ALUILayerNodeRenderObj(Renderer _render, _AALUILayerBasicMonoData _monoData, _IALUILayerBasicNodeMonoInterface _nodeInterface)
            : base(_monoData, _nodeInterface)
        {
            _m_coRenderObj = _render;

            if(null != _m_coRenderObj)
                _m_cgoRenderTrans = _m_coRenderObj.transform;
        }

        /// <summary>
        /// 当前所在节点对应的canvas对象
        /// </summary>
        public override Transform transF { get { return _m_cgoRenderTrans; } }
        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        public override void setOrderInLayer(int _sortingLayer, int _layer)
        {
            if(null == _m_coRenderObj || null == _m_cgoRenderTrans)
                return;

            _m_coRenderObj.sortingLayerID = _sortingLayer;
            _m_coRenderObj.sortingOrder = _layer;
        }
        /// <summary>
        /// 资源释放函数
        /// </summary>
        public override void discard()
        {
            _m_coRenderObj = null;
            _m_cgoRenderTrans = null;
        }
    }
}

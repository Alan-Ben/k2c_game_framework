﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的所有层级节点的接口类
     **/
    public class ALUILayerNodeCanvasObj : _AALUILayerBasicNodeObj
    {
        private Canvas _m_coCanvasObj;
        private Transform _m_cgoRootTrans;

        public ALUILayerNodeCanvasObj(Canvas _canvas, _AALUILayerBasicMonoData _monoData, _IALUILayerBasicNodeMonoInterface _nodeInterface)
            : base(_monoData, _nodeInterface)
        {
            _m_coCanvasObj = _canvas;

            if (null != _m_coCanvasObj)
                _m_cgoRootTrans = _m_coCanvasObj.transform;
        }
        public ALUILayerNodeCanvasObj(Canvas _canvas, Transform _rootTransform)
            : base(null, null)
        {
            _m_coCanvasObj = _canvas;

            _m_cgoRootTrans = _rootTransform;
        }

        /// <summary>
        /// 当前所在节点对应的canvas对象
        /// </summary>
        public override Transform transF { get { return _m_cgoRootTrans; } }
        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        public override void setOrderInLayer(int _sortingLayer, int _layer)
        {
            if (null == _m_coCanvasObj || null == _m_cgoRootTrans)
                return;

            //设置需要切换
            _m_coCanvasObj.overrideSorting = true;
            _m_coCanvasObj.sortingLayerID = _sortingLayer;
            _m_coCanvasObj.sortingOrder = _layer;
        }
        /// <summary>
        /// 资源释放函数
        /// </summary>
        public override void discard()
        {
            _m_coCanvasObj = null;
            _m_cgoRootTrans = null;
        }
    }
}

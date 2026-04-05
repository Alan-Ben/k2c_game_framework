﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象基础管理接口对象
     **/
    public abstract class _AALUILayerBasicNodeObj
    {
        //对应的配置数据对象，注意根节点此数据为null
        private _AALUILayerBasicMonoData _m_data;
        //对应的节点接口对象
        private _IALUILayerBasicNodeMonoInterface _m_iNodeInterface;

        protected _AALUILayerBasicNodeObj(_AALUILayerBasicMonoData _data, _IALUILayerBasicNodeMonoInterface _nodeInterface)
        {
            _m_data = _data;
            _m_iNodeInterface = _nodeInterface;
        }

        public _AALUILayerBasicMonoData monoData { get { return _m_data; } }
        public _IALUILayerBasicNodeMonoInterface nodeInterface { get { return _m_iNodeInterface; } }

        /// <summary>
        /// 当前所在节点对应的canvas对象
        /// </summary>
        public abstract Transform transF { get; }
        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        public abstract void setOrderInLayer(int _sortingLayer, int _layer);
        /// <summary>
        /// 资源释放函数
        /// </summary>
        public abstract void discard();
    }
}

using GOE;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ALPackage
{
    /*********************
     * UI系统层级管理对象的每个UI层级的根节点对象
     **/
    public class ALUILayerRootNode : _AALUILayerBasicDynamicNode
    {
        private Transform _m_tRootTransform;
        private Canvas _m_coCanvasObj;

        //是否每个子窗口对象都会创建一个Canvas用于分层
        public bool _m_bIsAllChildCreateCanvas = false;
        //每个子窗口分层之间的间隔
        public int _m_iLayerInterval = 0;

        public ALUILayerRootNode(ALUILayerInfo _layerInfo)
            : base(new ALUILayerNodeCanvasObj(_layerInfo.cavasObj, _layerInfo.layerRootGo), _layerInfo.cavasObj.sortingLayerID, _layerInfo.cavasObj.sortingOrder)
        {
            if(null == _layerInfo)
                return;

            _m_tRootTransform = _layerInfo.layerRootGo;
            _m_coCanvasObj = _layerInfo.cavasObj;

            //设置是否每个子窗口对象都会创建一个Canvas用于分层以及分层之间的间隔
            _m_bIsAllChildCreateCanvas = _layerInfo.isAllChildCreateCanvas;
            _m_iLayerInterval = _layerInfo.layerInterval;
        }

        /// <summary>
        /// 返回根节点的Root的Transform对象
        /// </summary>
        public Transform rootTF { get{return _m_tRootTransform; } }

        /// <summary>
        /// 相对父节点的层级对比
        /// </summary>
        public override int orderLayerToParentNode { get { return 0; } }


        /// <summary>
        /// 设置当前的层级信息
        /// </summary>
        /// <param name="_layer"></param>
        protected override void _setOrderInLayer(int _sortingLayer, int _layer)
        {
            //设置层级
            _m_coCanvasObj.sortingLayerID = _sortingLayer;
            _m_coCanvasObj.sortingOrder = _layer;
        }

        /// <summary>
        /// 当修改当前层级的时候，触发的函数
        /// </summary>
        /// <param name="_orderLayerToParentNode"></param>
        protected override void _onSetNewLayer(int _orderLayerToParentNode, int _finalLayer)
        {
        }

        /// <summary>
        /// 子类的资源释放处理函数
        /// </summary>
        protected override void _doDiscard()
        {

        }

        /// <summary>
        /// 当向本根节点添加子对象的时候，需要根据子对象的附加脚本以及信息做一定的处理
        /// 当本节点表示必须在每个子节点进行canvas分层的时候，将保证每个子节点都有一个canvas
        /// 且每个canvas的层级是依照配置递增的
        /// </summary>
        /// <param name="_wndMono"></param>
        public void checkChildUIWnd(_AALBasicUIWndMono _wndMono)
        {
            if (null == _wndMono)
                return;

            //获取对应的Transform对象
            Transform wndTF = _wndMono.transform;
            if(null == wndTF)
                return;

            //判断附加对象是否已经有脚本，有则不处理
            _IALUILayerBasicNodeMonoInterface nodeMono = wndTF.GetComponent<_IALUILayerBasicNodeMonoInterface>();
            if (null != nodeMono)
                return;

            //如果无固定脚本，则判断本节点是否需要对子节点进行检测
            if (!_m_bIsAllChildCreateCanvas)
                return;

            //检测子对象是否有canvas，有则根据不同类型添加不同脚本
            Canvas uiCanvas = wndTF.GetComponent<Canvas>();
            //如果没有Canvas，则添加一个，如果添加的canvas需要默认把raycast放入队列
            if (null == uiCanvas)
            {
                uiCanvas = wndTF.AddMissingComponent<Canvas>();
                //如果不存在canvas才需要添加raycast
                GraphicRaycaster raycaster = wndTF.AddMissingComponent<GraphicRaycaster>();

                //向Mono的RayCast列表增加新对象
                _wndMono.uiGraphicRaycasterList.Add(raycaster);
            }

            //判断以及配置canvas的层级
            uiCanvas.overrideSorting = true;
            uiCanvas.sortingLayerID = sortingLayerId;

            //增加默认dynamic脚本
            ALUILayerCanvasNodeMono mono = wndTF.AddMissingComponent<ALUILayerCanvasNodeMono>();
            mono.nodeData = new ALUILayerCanvasMonoData();
            mono.nodeData.canvasObj = uiCanvas;
            mono.nodeData.addOrderInLayer_V = _m_iLayerInterval;

            mono.nodeData.isDynamicNode_V = true;
            mono.nodeData.isNoChildNode_V = false;
        }
    }
}

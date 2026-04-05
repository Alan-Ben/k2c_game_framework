using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 层级管理节点数据基类
    /// </summary>
    public abstract class _AALUILayerBasicMonoData
    {
        protected _AALUILayerBasicNode _m_nNode = null;
        
        public void discard()
        {
            //释放节点
            if(null != _m_nNode)
            {
                _m_nNode.discard();
                //处理释放函数
                _doDiscard();
            }
            _m_nNode = null;
        }

        /// <summary>
        /// 将本对象注册到层级管理器中
        /// </summary>
        public void regIntoLayer(_IALUILayerBasicNodeMonoInterface _nodeInterface)
        {
            //对象有效，此时进行注册处理
            if(isDynamicNode_V)
                ALUILayerMgr.instance.addUILayerDynamicNode(createUILayerNode(_nodeInterface), addOrderInLayer_V);
            else
                ALUILayerMgr.instance.addUILayerNode(createUILayerNode(_nodeInterface));
        }
        /// <summary>
        /// 将本对象从层级管理器中移除
        /// </summary>
        public void unregFromLayer(List<_IALUILayerBasicNodeMonoInterface> _recInterfaceList = null)
        {
            //此时执行注销操作
            ALUILayerMgr.instance.rmvLayerNode(goInstanceId, _recInterfaceList);
        }

        /// <summary>
        /// 创建一个新的层级节点对象
        /// </summary>
        /// <returns></returns>
        protected _AALUILayerBasicNode createUILayerNode(_IALUILayerBasicNodeMonoInterface _nodeInterface)
        {
            if (null != _m_nNode)
                return _m_nNode;

            if (isDynamicNode_V)
            {
                if(isNoChildNode_V)
                    _m_nNode = new ALUILayerDynamicNoChildNode(_createUILayerNodeObj(_nodeInterface));
                else
                    _m_nNode = new ALUILayerDynamicNode(_createUILayerNodeObj(_nodeInterface));
            }
            else
            {
                if (isNoChildNode_V)
                    _m_nNode = new ALUILayerNoChildNode(_createUILayerNodeObj(_nodeInterface), addOrderInLayer_V);
                else
                    _m_nNode = new ALUILayerNode(_createUILayerNodeObj(_nodeInterface), addOrderInLayer_V);
            }

            return _m_nNode;
        }

        /// <summary>
        /// 相对父节点增加的层级
        /// </summary>
        public abstract int addOrderInLayer_V { get; set; }
        /// <summary>
        /// 是否为动态节点
        /// </summary>
        public abstract bool isDynamicNode_V { get; set; }
        /// <summary>
        /// 是否为无子节点
        /// </summary>
        public abstract bool isNoChildNode_V { get; set; }

        /// <summary>
        /// 获取对应需要控制对象的实例Id
        /// </summary>
        public abstract int goInstanceId { get; }

        /// <summary>
        /// 创建对应控制节点的控制对象，各子类根据自己控制对象的不同返回不同控制类对象
        /// </summary>
        /// <returns></returns>
        protected abstract _AALUILayerBasicNodeObj _createUILayerNodeObj(_IALUILayerBasicNodeMonoInterface _nodeInterface);
        /// <summary>
        /// 释放函数
        /// </summary>
        protected abstract void _doDiscard();
    }
}
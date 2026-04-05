using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// UI系统层级管理对象挂载脚本基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AALUILayerBasicNodeMono<T> : MonoBehaviour, _IALUILayerBasicNodeMonoInterface where T : _AALUILayerBasicMonoData
    {
        [SerializeField]
        protected T node = null;

        /// <summary>
        /// 获取在Mono中的Transform对象
        /// </summary>
        public Transform trans { get { return null == this ? null : transform; } }
        /// <summary>
        /// 返回数据对象，用于让外部可以通过程序进行一些默认初始化处理
        /// </summary>
        public T nodeData { get { return node; } set { node = value; } }

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            //到管理对象中进行处理
            ALUILayerMgr.instance.addCheckInfo(this);
        }

        private void OnDisable()
        {
            //如果执行无效操作的时候，直接注销显示对象
            if (node != null)
                node.unregFromLayer();
        }

        private void OnDestroy()
        {
            //如果执行无效操作的时候，直接注销显示对象
            if (node != null)
                node.unregFromLayer();

            if (node != null)
                node.discard();
        }

        /// <summary>
        /// 检测函数
        /// </summary>
        public void check()
        {
            if(null == this || null == gameObject)
            {
                return;
            }

            //判断状态是否变更，如未变更则不处理
            //Alzq: 此处暂时屏蔽，因为可能在部分流程中会先把对象隐藏，然后清理出父节点，再显示，此时会导致对象的状态变更在check中无法被正确处理
            //if(_m_bDealedEnableState == gameObject.activeInHierarchy)
            //    return;

            if(gameObject.activeInHierarchy)
            {
                //对象有效，此时进行注册处理
                if(node != null)
                    node.regIntoLayer(this);
            }
            else
            {
                //此时执行注销操作
                if (node != null)
                    node.unregFromLayer();
            }
        }
    }
}

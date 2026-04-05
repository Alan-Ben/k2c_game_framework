using System;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// UI系统层级管理对象的每个UI层级的根节点对象  的管理GameObject层级的数据类
    /// </summary>
    [Serializable]
    public class ALUILayerGameObjMonoData : _AALUILayerBasicMonoData_Value
    {
        public Transform transform;
        /// <summary>
        /// 获取对应需要控制对象的实例Id
        /// </summary>
        public override int goInstanceId { get { if(null == transform) return 0; else return transform.GetInstanceID(); } }
        /// <summary>
        /// 创建对应控制节点的控制对象，各子类根据自己控制对象的不同返回不同控制类对象
        /// </summary>
        /// <returns></returns>
        protected override _AALUILayerBasicNodeObj _createUILayerNodeObj(_IALUILayerBasicNodeMonoInterface _nodeInterface)
        {
            if(null == transform)
            {
                ALLog.Error(" did not set the game obj!!");
                return null;
            }

            return new ALUILayerNodeGameObj(transform, this, _nodeInterface);
        }

        /// <summary>
        /// 释放函数
        /// </summary>
        protected override void _doDiscard()
        {
        }
    }
}
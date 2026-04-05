using System;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// UI系统层级管理对象的每个UI层级的根节点对象  的管理Canvas层级的数据类
    /// Simple简易的UI层级管理对象的Canvas层级的数据类
    /// </summary>
    [Serializable]
    public class SimpleUILayerCanvasMonoData : _AALUILayerBasicMonoData
    {
        [ALHeader("相对父节点增加的层级")]
        public int addOrderInLayer = 10;
        [ALHeader("修改层级的canvas对象")]
        public Canvas canvasObj;

        public override int addOrderInLayer_V { get { return addOrderInLayer; } set { addOrderInLayer = value; } }
        public override bool isDynamicNode_V { get { return false; } set { UnityEngine.Debug.LogError($"SimpleUILayerCanvasMonoData 不支持set isDynamicNode_V value ={value}"); return; } }
        public override bool isNoChildNode_V { get { return true; } set { UnityEngine.Debug.LogError($"SimpleUILayerCanvasMonoData 不支持set isNoChildNode_V value ={value}"); return; } }

        /// <summary>
        /// 获取对应需要控制对象的实例Id
        /// </summary>
        public override int goInstanceId { get { if(null == canvasObj || null == canvasObj.transform) return 0; else return canvasObj.transform.GetInstanceID(); } }
        /// <summary>
        /// 创建对应控制节点的控制对象，各子类根据自己控制对象的不同返回不同控制类对象
        /// </summary>
        /// <returns></returns>
        protected override _AALUILayerBasicNodeObj _createUILayerNodeObj(_IALUILayerBasicNodeMonoInterface _nodeInterface)
        {
            if (null == canvasObj)
            {
                ALLog.Error(" did not set the canvas obj!!");
                return null;
            }

            return new ALUILayerNodeCanvasObj(canvasObj, this, _nodeInterface);
        }

        /// <summary>
        /// 释放函数
        /// </summary>
        protected override void _doDiscard()
        {
        }
    }
}
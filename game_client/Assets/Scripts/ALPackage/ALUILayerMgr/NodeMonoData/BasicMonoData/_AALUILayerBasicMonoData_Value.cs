using System;

namespace ALPackage
{
    /// <summary>
    /// 层级管理节点数据基类
    /// </summary>
    public abstract class _AALUILayerBasicMonoData_Value : _AALUILayerBasicMonoData
    {
        [ALHeader("相对父节点增加的层级")]
        public int addOrderInLayer = 10;
        [ALHeader("是否为动态节点")]
        public bool isDynamicNode = false;
        [ALHeader("是否为无子节点")]
        public bool isNoChildNode = false;

        /// <summary>
        /// 相对父节点增加的层级
        /// </summary>
        public override int addOrderInLayer_V { get { return addOrderInLayer; } set { addOrderInLayer = value; } }
        /// <summary>
        /// 是否为动态节点
        /// </summary>
        public override bool isDynamicNode_V { get { return isDynamicNode; } set { isDynamicNode = value; } }
        /// <summary>
        /// 是否为无子节点
        /// </summary>
        public override bool isNoChildNode_V { get { return isNoChildNode; } set { isNoChildNode = value; } }
    }
}
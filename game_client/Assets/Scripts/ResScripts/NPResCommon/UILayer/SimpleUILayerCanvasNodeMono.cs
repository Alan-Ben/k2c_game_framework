using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// UI系统层级管理对象的每个UI层级的根节点对象  的管理Canvas层级的Mono
    /// Simple简易的UI层级管理对象的Canvas层级的数据类
    /// 策划只需要关心orderlayer
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class SimpleUILayerCanvasNodeMono : _AALUILayerBasicNodeMono<SimpleUILayerCanvasMonoData>
    {
    }
}
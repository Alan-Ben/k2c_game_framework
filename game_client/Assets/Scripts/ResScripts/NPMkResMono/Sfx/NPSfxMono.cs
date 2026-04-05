using System;
using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效控制脚本，用于控制特效所有对象的大小以及特效的层级
/// </summary>
public class NPSfxMono : MonoBehaviour
{
    /** 特效子对象的列表 */
    [ALHeader("特效子对象的列表")]
    public List<GameObject> childGoList;
    [ALHeader("是否UI特效,是的话会自动动态修改orderlayer")]
    public bool isUISfx;

    /// <summary>
    /// 是否处理了UI层级检测
    /// </summary>
    private bool _m_bIsDealUICheckLayer = false;

    private void OnEnable()
    {
        //UI特效要注册下动态脚本
        if (isUISfx)
            checkUILayerMono();
    }

    /******************
     * 重置特效
     **/
    public void resetSfx()
    {
        if (null != gameObject)
            gameObject.SetActive(false);
        //重置比例
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
        
        //重置层级
        //这边都需要重置层级，因为使用时候都会设置层级
        setLayer(WCGResCommon.WCG_C_LAYER_IGNORE);
    }

    /******************
     * 设置特效有效
     **/
    public void setEnable()
    {
        if(null != gameObject)
            gameObject.SetActive(true);
    }

    /******************
     * 设置特效旋转角度
     **/
    public void setRotation(Vector3 _direction)
    {
        if (null == gameObject)
            return;

        transform.rotation = Quaternion.LookRotation(_direction);
    }

    /*******************
     * 设置特效层级
     **/
    public void setLayer(int _layer)
    {
        for (int i = 0; i < childGoList.Count; i++)
        {
            if (null == childGoList[i])
                continue;

            childGoList[i].layer = _layer;
        }
    }
    
    /// <summary>
    /// 设置父节点
    /// </summary>
    public void setParent(Transform _tf, bool _worldPositionStays = true)
    {
        if(null == gameObject)
            return;

        gameObject.transform.SetParent(_tf, _worldPositionStays);
    }

    /// <summary>
    /// 检测UI的层级管理mono是否有添加，如没添加则根据规则自动添加
    /// </summary>
    public void checkUILayerMono()
    {
        //检测是否已经处理过
        if (_m_bIsDealUICheckLayer)
            return;

        //设置了检测层级管理的标志
        _m_bIsDealUICheckLayer = true;

        GameObject tmpGo = null;
        for (int i = 0; i < childGoList.Count; i++)
        {
            tmpGo = childGoList[i];
            if (null == tmpGo)
                continue;

            //检查脚本类型，是ui对象还是render对象
            //暂时只处理render对象
            Renderer sfxRender = tmpGo.GetComponent<Renderer>();
            if (null != sfxRender)
            {
                //添加层级管理脚本，添加的脚本不能是一个父节点的脚本，只能是叶子节点脚本
                ALUILayerRenderNodeMono mono = tmpGo.AddMissingComponent<ALUILayerRenderNodeMono>();
                mono.nodeData = new ALUILayerRenderMonoData();
                mono.nodeData.renderObj = sfxRender;
                mono.nodeData.addOrderInLayer = sfxRender.sortingOrder;

                mono.nodeData.isDynamicNode = false;
                mono.nodeData.isNoChildNode = true;
            }
            else
            {
                //判断canvas
                Canvas sfxCanvas = tmpGo.GetComponent<Canvas>();
                if (null != sfxCanvas)
                {
                    //添加层级管理脚本，添加的脚本不能是一个父节点的脚本，只能是叶子节点脚本
                    ALUILayerCanvasNodeMono mono = tmpGo.AddMissingComponent<ALUILayerCanvasNodeMono>();
                    mono.nodeData = new ALUILayerCanvasMonoData();
                    mono.nodeData.canvasObj = sfxCanvas;
                    mono.nodeData.addOrderInLayer = sfxCanvas.sortingOrder;

                    mono.nodeData.isDynamicNode = false;
                    mono.nodeData.isNoChildNode = true;
                }
            }
        }
    }
}

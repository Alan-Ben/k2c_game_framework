using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 
 **/
public class WCGRectTransformSizeFixMono : MonoBehaviour
{
    //跟随的窗口对象
    public RectTransform followRectTrans;
    //附加的窗口尺寸
    public Vector2 additionSize;

    public GameObject targetObj;//跟随的对象的目标
    private RectTransform targetObjRectTransform;//跟随的对象的目标的RectTransform

     //需要需要每一帧更新
    public bool is_need_update_per_frame = true;

    void Awake () {
        targetObjRectTransform = targetObj.GetComponent<RectTransform>();

        fixTransformSize();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(is_need_update_per_frame)
            fixTransformSize();
    }

    public void fixTransformSize () {
        if (null == targetObj || null == targetObjRectTransform || null == followRectTrans)
            return;

        followRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetObjRectTransform.rect.width + additionSize.x);
        followRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetObjRectTransform.rect.height + additionSize.y);

    }
}

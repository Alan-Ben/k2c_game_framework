using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 曲线弧形的LayoutGroup扩展
    /// </summary>
    [AddComponentMenu("Layout/Extensions/Curve Layout")]
    public class CurveLayout : LayoutGroup
    {
        [ALHeader("间隔")]
        public Vector2 spaceSize;
        [ALHeader("曲线对应的显示区域大小")]
        public RectTransform curveAreaObj;
        [ALHeader("列表显示区域比遮罩区域小的时候，是否居中显示content")]
        public bool isContentToCenter = true;
        [ALHeader("显示对象方向枚举")]
        public ALGUIListLayoutStyle layoutStyle;
        [ALHeader("默认向上或向左弯曲")]
        public bool isDefaultCurve;
        [ALHeader("曲线")] 
        public AnimationCurve curve;
        [ALHeader("最小高度限制")] 
        public float minHeight;
        [ALHeader("最大高度限制")] 
        public float maxHeight;
        [ALHeader("最小缩放")]
        public float minScale = 1;
        [ALHeader("最大缩放")]
        public float maxScale = 1;

        protected override void OnEnable()
        {
            base.OnEnable();
            _calculateRadial();
        }

        private void Update()
        {
            _calculateRadial();
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _calculateRadial();
        }
#endif
        public override void CalculateLayoutInputVertical()
        {
            _calculateRadial();
        }
        public override void CalculateLayoutInputHorizontal()
        {
            _calculateRadial();
        }

        public override void SetLayoutHorizontal()
        {
            
        }

        public override void SetLayoutVertical()
        {
            
        }
        //计算一次位置
        private void _calculateRadial()
        {
#if NP_GAME
            
            if (transform.childCount == 0)
                return;
            float totalWidth = 0;
            float totalHeight = 0;
            // rectTransform.pivot = new Vector2(0.5f, 0.5f);
            // rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            // rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                {
                    totalWidth += child.rect.width;
                    totalHeight += child.rect.height;
                    if (i != transform.childCount - 1)
                    {
                        totalWidth += spaceSize.x;
                        totalHeight += spaceSize.y;
                    }
                }
            }

            Vector2 sizeDelta = rectTransform.sizeDelta;
            bool needRefreshToCenter = false;
            float offSetMax = 0;
            if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                needRefreshToCenter = totalWidth < curveAreaObj.rect.width;
                if (isContentToCenter && needRefreshToCenter)
                {
                    offSetMax = (curveAreaObj.rect.width - totalWidth) / 2;
                    totalWidth = curveAreaObj.rect.width;
                }
                rectTransform.sizeDelta = new Vector2(totalWidth ,sizeDelta.y);
            }
            else
            {
                needRefreshToCenter = totalHeight < curveAreaObj.rect.height;
                if (isContentToCenter && needRefreshToCenter)
                {
                    offSetMax = (curveAreaObj.rect.height - totalHeight) / 2;
                    totalHeight = curveAreaObj.rect.height;
                }
                rectTransform.sizeDelta = new Vector2(sizeDelta.x ,totalHeight);
            }
            
            float currentX = 0;
            float currentY = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                {
                    //设置显示对象位置
                    if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                    {
                        child.localPosition = new Vector2( offSetMax- rectTransform.rect.width * rectTransform.pivot.x + currentX + child.pivot.x * child.rect.width,0);
                        // >>>再根据曲线及当前方向上的坐标值设置另一个坐标值<<<
                         Vector2 _screenPos = RectTransformUtility.WorldToScreenPoint(MainCameraMono.instance?.fullCanvas?.worldCamera, child.position);
                         //屏幕坐标转换到UGUI坐标
                         Vector2 uiPos;
                         RectTransformUtility.ScreenPointToLocalPointInRectangle(
                             curveAreaObj,
                             _screenPos,
                             MainCameraMono.instance?.uiCamera,
                             out uiPos);
                         // float _calcX = uiPos.x;  //最终相对坐标
                         // float _calcY = uiPos.y;  //最终相对坐标
                         float _calcX = curveAreaObj.rect.width / 2 + uiPos.x;  //最终相对坐标
                         float _calcY = curveAreaObj.rect.height / 2 - uiPos.y;  //最终相对坐标
                         float x = _calcX + (0.5f - child.pivot.x) * child.rect.width;   //增加item宽度的一半计算   TODO 因为ui上pivot配置的是0,1
                         float y = _calcY + (0.5f - child.pivot.y) * child.rect.height;  //增加item高度的一半计算
                        
                         float proportion = curve.Evaluate(x / curveAreaObj.rect.width);   //最终比例
                         float deviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                         if (!isDefaultCurve)
                             deviation = -deviation;
                         //横向位置设置
                         child.localPosition = new Vector2(child.localPosition.x, deviation);
                         //设置 scale
                         deviation = (maxScale - minScale) * proportion + minScale;    //最终缩放
                         child.localScale = new Vector3(deviation, deviation, deviation);
                    }
                    else
                    {
                        child.localPosition = new Vector2(0,- offSetMax + rectTransform.rect.height * (1 - rectTransform.pivot.y) -(currentY + (1 - child.pivot.y) * child.rect.height));
                        //>>>再根据曲线及当前方向上的坐标值设置另一个坐标值<<<
                        Vector2 _screenPos = RectTransformUtility.WorldToScreenPoint(MainCameraMono.instance.fullCanvas.worldCamera, child.position);
                        //屏幕坐标转换到UGUI坐标
                        Vector2 uiPos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            curveAreaObj,
                            _screenPos,
                            MainCameraMono.instance.uiCamera,
                            out uiPos);
                        // float _calcX = uiPos.x;  //最终相对坐标
                        // float _calcY = uiPos.y;  //最终相对坐标
                        float _calcX = curveAreaObj.rect.width / 2 + uiPos.x;  //最终相对坐标
                        float _calcY = curveAreaObj.rect.height / 2 - uiPos.y;  //最终相对坐标
                        float x = _calcX + (0.5f - child.pivot.x) * child.rect.width;   //增加item宽度的一半计算   TODO 因为ui上pivot配置的是0,1
                        float y = _calcY + (0.5f - child.pivot.y) * child.rect.height;  //增加item高度的一半计算

                        float proportion = curve.Evaluate(y / curveAreaObj.rect.height);   //最终比例
                        float deviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                        if (isDefaultCurve)
                            deviation = -deviation;
                        //纵向位置设置
                        child.localPosition = new Vector2(deviation, child.localPosition.y);
                        
                        //设置 scale
                        deviation = (maxScale - minScale) * proportion + minScale;    //最终缩放
                        child.localScale = new Vector3(deviation, deviation, deviation);
                    }
                    currentX += child.rect.width;
                    currentY += child.rect.height;
                    if (i != transform.childCount - 1)
                    {
                        currentX += spaceSize.x;
                        currentY += spaceSize.y;
                    }
                }
            }
            
#endif
        }
    }
}
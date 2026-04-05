using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 曲线弧形的网格LayoutGroup扩展，支持多行或多列
    /// </summary>
    [AddComponentMenu("Layout/Extensions/Curve Grid Layout")]
    public class CurveGridLayout : LayoutGroup
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
        [ALHeader("每行或每列的item数量")]
        public int perColumnOrRawItemCount = 1;
        [ALHeader("每个item的大小")]
        public Vector2 itemSize;

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
            
            if (perColumnOrRawItemCount < 1)
                perColumnOrRawItemCount = 1;

            // 计算活动子对象数量
            int activeChildCount = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                {
                    activeChildCount++;
                }
            }

            if (activeChildCount == 0)
                return;

            float totalWidth = 0;
            float totalHeight = 0;
            
            // 根据布局方向和每行/列item数量计算总尺寸
            if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                // 横向布局：计算需要多少列
                int columnCount = Mathf.CeilToInt((float)activeChildCount / perColumnOrRawItemCount);
                totalWidth = columnCount * itemSize.x + (columnCount - 1) * spaceSize.x;
                totalHeight = perColumnOrRawItemCount * itemSize.y + (perColumnOrRawItemCount - 1) * spaceSize.y;
            }
            else
            {
                // 纵向布局：计算需要多少行
                int rowCount = Mathf.CeilToInt((float)activeChildCount / perColumnOrRawItemCount);
                totalWidth = perColumnOrRawItemCount * itemSize.x + (perColumnOrRawItemCount - 1) * spaceSize.x;
                totalHeight = rowCount * itemSize.y + (rowCount - 1) * spaceSize.y;
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
                rectTransform.sizeDelta = new Vector2(totalWidth, sizeDelta.y);
            }
            else
            {
                needRefreshToCenter = totalHeight < curveAreaObj.rect.height;
                if (isContentToCenter && needRefreshToCenter)
                {
                    offSetMax = (curveAreaObj.rect.height - totalHeight) / 2;
                    totalHeight = curveAreaObj.rect.height;
                }
                rectTransform.sizeDelta = new Vector2(sizeDelta.x, totalHeight);
            }
            
            int activeIndex = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                {
                    //设置显示对象位置
                    if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                    {
                        // 横向布局：每列多个item
                        int columnIndex = activeIndex / perColumnOrRawItemCount;
                        int rowIndex = activeIndex % perColumnOrRawItemCount;
                        
                        float currentX = columnIndex * (itemSize.x + spaceSize.x);
                        float currentY = rowIndex * (itemSize.y + spaceSize.y);
                        
                        child.localPosition = new Vector2(
                            offSetMax - rectTransform.rect.width * rectTransform.pivot.x + currentX + child.pivot.x * itemSize.x,
                            -currentY - (1 - child.pivot.y) * itemSize.y
                        );
                        
                        // >>>再根据曲线及当前方向上的坐标值设置另一个坐标值<<<
                        if (MainCameraMono.instance != null && MainCameraMono.instance.fullCanvas != null && MainCameraMono.instance.uiCamera != null)
                        {
                            Vector2 _screenPos = RectTransformUtility.WorldToScreenPoint(MainCameraMono.instance.fullCanvas.worldCamera, child.position);
                            //屏幕坐标转换到UGUI坐标
                            Vector2 uiPos;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                curveAreaObj,
                                _screenPos,
                                MainCameraMono.instance.uiCamera,
                                out uiPos);
                            
                            float _calcX = curveAreaObj.rect.width / 2 + uiPos.x;  //最终相对坐标
                            float x = _calcX + (0.5f - child.pivot.x) * itemSize.x;   //增加item宽度的一半计算
                        
                            float proportion = curve.Evaluate(x / curveAreaObj.rect.width);   //最终比例
                            float deviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                            if (!isDefaultCurve)
                                deviation = -deviation;
                            //横向位置设置
                            child.localPosition = new Vector2(child.localPosition.x, child.localPosition.y + deviation);
                            //设置 scale
                            deviation = (maxScale - minScale) * proportion + minScale;    //最终缩放
                            child.localScale = new Vector3(deviation, deviation, deviation);
                        }
                    }
                    else
                    {
                        // 纵向布局：每行多个item
                        int rowIndex = activeIndex / perColumnOrRawItemCount;
                        int columnIndex = activeIndex % perColumnOrRawItemCount;
                        
                        float currentX = columnIndex * (itemSize.x + spaceSize.x);
                        float currentY = rowIndex * (itemSize.y + spaceSize.y);
                        
                        child.localPosition = new Vector2(
                            currentX + child.pivot.x * itemSize.x,
                            -offSetMax + rectTransform.rect.height * (1 - rectTransform.pivot.y) - (currentY + (1 - child.pivot.y) * itemSize.y)
                        );
                        
                        //>>>再根据曲线及当前方向上的坐标值设置另一个坐标值<<<
                        if (MainCameraMono.instance != null && MainCameraMono.instance.fullCanvas != null && MainCameraMono.instance.uiCamera != null)
                        {
                            Vector2 _screenPos = RectTransformUtility.WorldToScreenPoint(MainCameraMono.instance.fullCanvas.worldCamera, child.position);
                            //屏幕坐标转换到UGUI坐标
                            Vector2 uiPos;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                curveAreaObj,
                                _screenPos,
                                MainCameraMono.instance.uiCamera,
                                out uiPos);
                            
                            float _calcY = curveAreaObj.rect.height / 2 - uiPos.y;  //最终相对坐标
                            float y = _calcY + (0.5f - child.pivot.y) * itemSize.y;  //增加item高度的一半计算

                            float proportion = curve.Evaluate(y / curveAreaObj.rect.height);   //最终比例
                            float deviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                            if (isDefaultCurve)
                                deviation = -deviation;
                            //纵向位置设置
                            child.localPosition = new Vector2(child.localPosition.x + deviation, child.localPosition.y);
                            
                            //设置 scale
                            deviation = (maxScale - minScale) * proportion + minScale;    //最终缩放
                            child.localScale = new Vector3(deviation, deviation, deviation);
                        }
                    }
                    
                    activeIndex++;
                }
            }
            
#endif
        }
    }
}

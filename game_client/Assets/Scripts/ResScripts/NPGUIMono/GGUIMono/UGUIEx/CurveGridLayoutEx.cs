using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 曲线弧形的GridLayoutGroup扩展
    /// </summary>
    [AddComponentMenu("Layout/Extensions/Curve Grid Layout Ex")]
    public class CurveGridLayoutEx : LayoutGroup
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
        [ALHeader("每行或每列展示item数量")]
        public int perColumnOrRawItemCount = 1;

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
            
            // 确保perColumnOrRawItemCount至少为1
            int itemsPerGroup = Mathf.Max(1, perColumnOrRawItemCount);
            
            float totalWidth = 0;
            float totalHeight = 0;
            // rectTransform.pivot = new Vector2(0.5f, 0.5f);
            // rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            // rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            
            // 计算主轴方向的总尺寸
            if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                // 横向布局：计算总宽度（按列计算）
                int columnCount = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    RectTransform child = (RectTransform)transform.GetChild(i);
                    if (child != null && child.gameObject.activeSelf)
                    {
                        int columnIndex = i / itemsPerGroup;
                        if (i % itemsPerGroup == 0)
                        {
                            if (columnCount > 0)
                                totalWidth += spaceSize.x;
                            totalWidth += child.rect.width;
                            columnCount++;
                        }
                    }
                }
            }
            else
            {
                // 纵向布局：计算总高度（按行计算）
                int rowCount = 0;
                for (int i = 0; i < transform.childCount; i++)
                {
                    RectTransform child = (RectTransform)transform.GetChild(i);
                    if (child != null && child.gameObject.activeSelf)
                    {
                        int rowIndex = i / itemsPerGroup;
                        if (i % itemsPerGroup == 0)
                        {
                            if (rowCount > 0)
                                totalHeight += spaceSize.y;
                            totalHeight += child.rect.height;
                            rowCount++;
                        }
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
                if (child != null && child.gameObject.activeSelf)
                {
                    int groupIndex = i % itemsPerGroup; // 在当前组内的索引
                    int mainAxisIndex = i / itemsPerGroup; // 主轴上的索引（列索引或行索引）
                    
                    //设置显示对象位置
                    if (layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                    {
                        // 横向布局：每列多个item
                        // 设置X坐标（主轴）
                        float baseX = offSetMax - rectTransform.rect.width * rectTransform.pivot.x + currentX + child.pivot.x * child.rect.width;
                        child.localPosition = new Vector2(baseX, 0);
                        
                        // >>>先计算曲线和缩放<<<
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
                        float curveDeviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                        if (!isDefaultCurve)
                            curveDeviation = -curveDeviation;
                        
                        //计算缩放
                        float scale = (maxScale - minScale) * proportion + minScale;    //最终缩放
                        
                        // 计算Y坐标偏移（副轴），考虑scale影响
                        float totalColumnYOffset = 0;
                        for (int j = i - groupIndex; j < i; j++)
                        {
                            RectTransform tempChild = (RectTransform)transform.GetChild(j);
                            if (tempChild != null && tempChild.gameObject.activeSelf)
                            {
                                RectTransform nextChild = (RectTransform)transform.GetChild(j + 1);
                                if (nextChild != null && nextChild.gameObject.activeSelf)
                                {
                                    // 计算前一个item的缩放
                                    float tempChildScale = tempChild.localScale.x;
                                    // 当前item使用已计算的scale
                                    float nextChildScale = (j + 1 == i) ? scale : nextChild.localScale.x;
                                    
                                    // 考虑缩放后的实际高度
                                    totalColumnYOffset += (tempChild.rect.height * tempChildScale + nextChild.rect.height * nextChildScale) / 2 + spaceSize.y;
                                }
                            }
                        }
                        
                        //横向位置设置，加上列内Y轴偏移
                        child.localPosition = new Vector2(child.localPosition.x, curveDeviation + totalColumnYOffset);
                        //设置 scale
                        child.localScale = new Vector3(scale, scale, scale);
                        
                        // 更新currentX（只在每列的第一个item时更新）
                        if (groupIndex == itemsPerGroup - 1 || i == transform.childCount - 1)
                        {
                            currentX += child.rect.width;
                            if (i < transform.childCount - 1)
                            {
                                // 检查下一个child是否存在且激活
                                RectTransform nextChild = (RectTransform)transform.GetChild(i + 1);
                                if (nextChild != null && nextChild.gameObject.activeSelf)
                                {
                                    currentX += spaceSize.x;
                                }
                            }
                        }
                    }
                    else
                    {
                        // 纵向布局：每行多个item
                        // 设置Y坐标（主轴）
                        float baseY = - offSetMax + rectTransform.rect.height * (1 - rectTransform.pivot.y) - (currentY + (1 - child.pivot.y) * child.rect.height);
                        child.localPosition = new Vector2(0, baseY);
                        
                        //>>>先计算曲线和缩放<<<
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

                        float proportion = curve.Evaluate(y / curveAreaObj.rect.height);   //最终比例
                        float curveDeviation = (maxHeight - minHeight) * proportion + minHeight;    //最终高度
                        if (isDefaultCurve)
                            curveDeviation = -curveDeviation;
                        
                        //计算缩放
                        float scale = (maxScale - minScale) * proportion + minScale;    //最终缩放
                        
                        // 计算X坐标偏移（副轴），考虑scale影响
                        float totalRowXOffset = 0;
                        for (int j = i - groupIndex; j < i; j++)
                        {
                            RectTransform tempChild = (RectTransform)transform.GetChild(j);
                            if (tempChild != null && tempChild.gameObject.activeSelf)
                            {
                                RectTransform nextChild = (RectTransform)transform.GetChild(j + 1);
                                if (nextChild != null && nextChild.gameObject.activeSelf)
                                {
                                    // 计算前一个item的缩放
                                    float tempChildScale = tempChild.localScale.x;
                                    // 当前item使用已计算的scale
                                    float nextChildScale = (j + 1 == i) ? scale : nextChild.localScale.x;
                                    
                                    // 考虑缩放后的实际宽度
                                    totalRowXOffset += (tempChild.rect.width * tempChildScale + nextChild.rect.width * nextChildScale) / 2 + spaceSize.x;
                                }
                            }
                        }
                        
                        //纵向位置设置，加上行内X轴偏移
                        child.localPosition = new Vector2(curveDeviation + totalRowXOffset, child.localPosition.y);
                        
                        //设置 scale
                        child.localScale = new Vector3(scale, scale, scale);
                        
                        // 更新currentY（只在每行的第一个item时更新）
                        if (groupIndex == itemsPerGroup - 1 || i == transform.childCount - 1)
                        {
                            currentY += child.rect.height;
                            if (i < transform.childCount - 1)
                            {
                                // 检查下一个child是否存在且激活
                                RectTransform nextChild = (RectTransform)transform.GetChild(i + 1);
                                if (nextChild != null && nextChild.gameObject.activeSelf)
                                {
                                    currentY += spaceSize.y;
                                }
                            }
                        }
                    }
                }
            }
            
#endif
        }
    }
}
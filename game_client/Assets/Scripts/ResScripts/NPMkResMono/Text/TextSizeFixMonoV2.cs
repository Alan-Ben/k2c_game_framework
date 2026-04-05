using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 监听Layout接口的方式，根据文字自适应修改指定transform大小
    /// </summary>
    [ExecuteAlways]
    public class TextSizeFixMonoV2 : UIBehaviour, ILayoutSelfController
    {
        [ALHeader("跟随的窗口对象")]
        public RectTransform followRectTrans;
        [ALHeader("附加的窗口尺寸")]
        public Vector2 additionSize;
    
        [ALHeader("是否使用限制perferWidth")]
        public bool is_limit_preferred_width = false;
        public float maxPreferredWidth;
    
        [ALHeader("是否使用限制perfer Height")]
        public bool is_limit_preferred_height = false;
        public float maxPreferredHeight;
        
        [ALHeader("是否使用限制minWidth")]
        public bool is_limit_min_width = false;
        public float fixedMinWidth;
    
        [ALHeader("是否使用限制minHeight")]
        public bool is_limit_min_height = false;
        public float fixedMinHeight;
        
        [ALHeader("是否使用限制MaxWidth")]
        public bool is_limit_max_width = false;
        public float fixedMaxWidth;
    
        [ALHeader("是否使用限制maxHeight")]
        public bool is_limit_max_height = false;
        public float fixedMaxHeight;
        
        [ALInfo("如果启用该功能，填入参考分辨率，最终的相关 fixed size 会根据填入的参考分辨率计算出比例，确保游戏运行时这个比例是不变的")]
        [ALHeader("是否需要根据当前分辨率修改 fixed size ")]
        public bool is_need_fix_size_by_resolution = false;
        public Vector2 fixedSizeByResolution = new Vector2(1080, 1920);
        
        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            // awake的时候设置一边自适应，避免TextEx 在Awake的时候翻译的文本，没有触发LayoutRebuild，导致未触发自适应
            SetLayoutHorizontal();
            SetLayoutVertical();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            LayoutRebuilder.MarkLayoutForRebuild(followRectTrans);
            base.OnDisable();
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            if(followRectTrans != null)
            {
                if(axis == 0)
                {
                    //算出text的preferred宽高
                    float preferredWidth = LayoutUtility.GetPreferredSize(rectTransform, axis);
                    //限制最大preferred宽度
                    if (is_limit_preferred_width && preferredWidth > maxPreferredWidth)
                        preferredWidth = maxPreferredWidth;
                    //加上附加的宽高
                    float finalWidth = preferredWidth + additionSize.x;
                    //限制最小宽度
                    if (is_limit_min_width)
                        finalWidth = Mathf.Max(finalWidth, getFixedMinWidth());
                    //限制最大宽度
                    if (is_limit_max_width)
                        finalWidth = Mathf.Min(finalWidth, getFixedMaxWidth());
                    followRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalWidth);
                }
                else
                {
                    float preferredHeight = LayoutUtility.GetPreferredSize(rectTransform, axis);
                    //限制最大preferred高度
                    if (is_limit_preferred_height && preferredHeight > maxPreferredHeight)
                        preferredHeight = maxPreferredHeight;
                    float finalHeight = preferredHeight + additionSize.y;
                    //限制最小高度
                    if (is_limit_min_height)
                        finalHeight = Mathf.Max(finalHeight, getFixedMinHeight());
                    //限制最大高度
                    if (is_limit_max_height)
                        finalHeight = Mathf.Min(finalHeight, getFixedMaxHeight());
                    followRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
                }
                SetDirty();
            }
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(followRectTrans);
        }
        
        private float getFixedMinWidth()
        {
            if (is_need_fix_size_by_resolution && fixedSizeByResolution.x != 0)
                return fixedMinWidth * Screen.width / fixedSizeByResolution.x;
            return fixedMinWidth;
        }
        private float getFixedMinHeight()
        {
            if (is_need_fix_size_by_resolution && fixedSizeByResolution.y != 0)
                return fixedMinHeight * Screen.height / fixedSizeByResolution.y;
            return fixedMinHeight;
        }
        private float getFixedMaxWidth()
        {
            if (is_need_fix_size_by_resolution && fixedSizeByResolution.x != 0)
                return fixedMaxWidth * Screen.width / fixedSizeByResolution.x;
            return fixedMaxWidth;
        }
        private float getFixedMaxHeight()
        {
            if (is_need_fix_size_by_resolution && fixedSizeByResolution.y != 0)
                return fixedMaxHeight * Screen.height / fixedSizeByResolution.y;
            return fixedMaxHeight;
        }

    #if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

    #endif
    }
}
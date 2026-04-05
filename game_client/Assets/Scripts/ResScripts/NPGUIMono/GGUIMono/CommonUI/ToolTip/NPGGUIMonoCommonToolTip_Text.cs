using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带文本的随子窗口基类
    /// </summary>
    public class NPGGUIMonoCommonToolTip_Text : NPGGUIMonoCommonToolTip
    {
        [ALHeader("物品描述")]
        public Text txtDesc;
        
        [ALHeader("描述的最小高度")]
        public float fixedMinWidth = 90f;
        [ALHeader("除去文字的部分的高度")]
        public float heightWithoutText = 150f;
        
        
        //计算文本高度
        public float getTextHeight(string _text)
        {
            if (txtDesc == null || txtDesc.rectTransform == null)
                return 0;
            
            TextGenerationSettings ts = txtDesc.GetGenerationSettings(new Vector2(txtDesc.GetPixelAdjustedRect().size.x, 0));
            ts.horizontalOverflow = HorizontalWrapMode.Wrap;
            ts.verticalOverflow = VerticalWrapMode.Overflow;
            float descHeight = 0;
            if(txtDesc.cachedTextGeneratorForLayout != null)
            {
                descHeight = txtDesc.cachedTextGeneratorForLayout.GetPreferredHeight(_text, ts) / txtDesc.pixelsPerUnit;
            }
            return descHeight > fixedMinWidth ? descHeight : fixedMinWidth;
        }
    }
}
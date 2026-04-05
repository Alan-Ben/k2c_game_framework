using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// icon点击查看详情的mono
    /// </summary>
    public class NPGGUIMonoCommonToolTip_IconDetail : NPGGUIMonoCommonToolTip
    {
        [Header("图片")]
        public RawImage icon;              // 图片
        [Header("名称")]
        public Text txtName;            // 名称
        [Header("描述")]
        public Text txtDesc;             // 描述
        
        [ALHeader("描述的最小宽度")]
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
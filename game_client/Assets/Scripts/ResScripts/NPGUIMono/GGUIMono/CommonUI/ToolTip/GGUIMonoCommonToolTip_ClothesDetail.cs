using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIClothesAttrDetail
    {
        [ALHeader("属性类型")]
        public EBasicAttrType type;
        [ALHeader("属性图标")]
        public RawImage imgIcon;
        [ALHeader("属性名")]
        public Text txtName;
        [ALHeader("属性加成值")]
        public Text txtValue;
    }

    /// <summary>
    /// 时装详情跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_ClothesDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("属性列表")]
        public List<GGUIClothesAttrDetail> attrList;
        [ALHeader("属性有加成值时文本颜色")]
        public Color attrHaveValueColor = Color.yellow;
        [ALHeader("属性没有加成值时文本颜色")]
        public Color attrNotHaveValueColor = Color.white;
        [ALHeader("没有属性加成时需要隐藏的GO列表")]
        public List<GameObject> noAttrHideList;

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
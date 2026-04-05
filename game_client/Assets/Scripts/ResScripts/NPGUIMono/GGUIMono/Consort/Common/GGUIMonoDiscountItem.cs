
    using System.Collections.Generic;
    using ALPackage;
    using UnityEngine;

    public class GGUIMonoDiscountItem : _AALBasicUIWndMono
    {
        [ALHeader("消耗item")] 
        public NPGGUIMonoCommonItem costItem; 
        [ALHeader("消耗的原价数值，显示{0}/{1},{0}当前拥有数值，{1}折前数值")] 
        public TextExStrikLine txtSourceItemCount; 
        [ALHeader("原价数值有折扣的key，{0}/<s>{1}</s>,{0}当前拥有数值，{1}折前数值")] 
        public string hasDiscountShowKey = "{0}/<s>{1}</s>"; 
        [ALHeader("原价数值没有折扣的key,{0}/{1},{0}当前拥有数值，{1}折前数值")] 
        public string noDiscountShowKey = "{0}/{1}"; 
        [ALHeader("消耗的折后数值")] 
        public TextEx txtDiscountItemCount;
        [ALHeader("大数字是否缩写")]
        public bool showLargeNum = false;
        [ALHeader("折扣")] 
        public TextEx txtDiscount;
        [ALHeader("有折扣显示的列表，没折扣不显示")] 
        public List<GameObject> goDiscountList;
        [ALHeader("物品数量不足时的文字颜色")]
        public Color notEnoughTxtColor = Color.red;
    }
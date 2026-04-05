using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会举办item
    /// </summary>
    public class GGUIMonoDinnerCreateItem_Props : _AGGUIMonoDinnerCreateItemBase
    {
        [ALHeader("名称")]
        public TextEx txtName;
        [ALHeader("宴会描述")]
        public TextEx txtDesc;
        [ALHeader("Banner图")]
        public RawImage texBanner;
        
        [ALHeader("消耗列表")]
        public NPGGUIMonoCommonItemContainer costItemContainer;
        [ALHeader("消耗不足，或者cd中的时候置灰对象")]
        public List<MaskableGraphic> goListGray;
        
        
        /// <summary>
        /// 资源加载路径
        /// </summary>
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2919); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2919);} }
    }
}

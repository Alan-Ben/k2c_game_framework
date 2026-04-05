using System.Collections.Generic;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class NPGGUIMonoCommonToolTipDressParentParam
    {
        [ALHeader("物品类型")]
        public ENPItemType itemType;
        [ALHeader("预制体父节点")]
        public Transform goDressParent;
    }
    /// <summary>
    /// 物品详情
    /// </summary>
    public class NPGGUIMonoCommonToolTip_ItemDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("物品图标")]
        public RawImage imgItemIcon;

        [ALHeader("物品品质底图")]
        public Image imgQualityBg;

        [ALHeader("物品名称")]
        public Text txtItemName;

        [ALHeader("物品持有数量文本")]
        public Text txtNum;

        [ALHeader("物品产出文本")]
        public Text txtAccess;

        [ALHeader("物品详细描述文本")]
        public Text txtItemDesc;

        [ALHeader("物品详细按钮")] 
        public GameObject btnDetail;

        [ALHeader("装扮加载GO父节点列表")]
        public List<NPGGUIMonoCommonToolTipDressParentParam> goDressParentList;

        /// <summary>
        /// 获取装扮预制体父节点
        /// </summary>
        /// <param name="_itemType"></param>
        /// <returns></returns>
        public Transform getDressParent(ENPItemType _itemType)
        {
            if (goDressParentList == null)
                return null;

            // 默认父节点，如果没有匹配的类型，则返回默认父节点
            Transform defaultParent = null;
            for (int i = 0; i < goDressParentList.Count; i++)
            {
                if(goDressParentList[i] == null)
                    continue;

                // 设置默认父节点
                if (goDressParentList[i].itemType == ENPItemType.NONE)
                    defaultParent = goDressParentList[i].goDressParent;

                if (goDressParentList[i].itemType == _itemType && goDressParentList[i].goDressParent != null)
                    return goDressParentList[i].goDressParent;
            }
            return defaultParent;
        }
    }
}
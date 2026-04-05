using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 召唤商店物品详细信息prefab窗口加载路径
    /// </summary>
    [Serializable]
    public class RecruitItemDetailInfoWndAssetPath
    {
        [ALHeader("召唤商店物品item")]
        public ERecruitItemType recruitItemType;
        
        [ALHeader("详细信息prefab窗口加载路径")]
        public NPCommonAssetPathInfo assetPathInfo;
    }
    
    /// <summary>
    /// 召唤商店
    /// </summary>
    public class GGUIMonoRecruitShopNew : _AALBasicUIWndMono
    {
        [ALHeader("招募消耗的物品")]
        public NPGGUIMonoCommonItem monoRecruitCostItem;

        [ALHeader("招募商店的物品容器")]
        public GGUIMonoRecruitShopIconItemContainer monoRecruitShopIconItemContainer;

        [ALHeader("详细信息加载父节点")]
        public Transform detailInfoParent;
        [ALHeader("详细信息prefab窗口加载路径列表")]
        public List<RecruitItemDetailInfoWndAssetPath> recruitItemDetailInfoWndAssetPathList;
        
        [ALHeader("左切按钮")]
        public GameObject btnLeft;
        [ALHeader("右切按钮")]
        public GameObject btnRight;
        
        [ALHeader("兑换按钮")]
        public GGUISubMonoRecruitExchangeBtn monoExchangeBtn;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;

        public NPCommonAssetPathInfo getRecruitItemDetailInfoWndAssetPath(ERecruitItemType itemType)
        {
            if (recruitItemDetailInfoWndAssetPathList == null)
                return null;

            foreach (var item in recruitItemDetailInfoWndAssetPathList)
            {
                if (item != null && item.recruitItemType == itemType)
                {
                    return item.assetPathInfo;
                }
            }

            return null;
        }
    }
}
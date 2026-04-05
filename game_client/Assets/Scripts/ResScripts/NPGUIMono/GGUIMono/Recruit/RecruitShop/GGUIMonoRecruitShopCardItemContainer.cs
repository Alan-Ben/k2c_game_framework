using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 兑换商店道具card对应的Prefab
    /// </summary>
    [Serializable]
    public class RecruitShopCardItemPrefab
    {
        [ALHeader("道具类型")]
        public ENPItemType itemType;

        [ALHeader("展示模板")]
        public GGUIMonoRecruitShopCardItem prefab;
    }
    
    public class GGUIMonoRecruitShopCardItemContainer : _ATNPGGUIMonoBasicDiffItemContainer
    {
        [ALHeader("不同道具类型对应的Prefab")]
        public List<RecruitShopCardItemPrefab> itemPrefabList;

        [ALHeader("没有item时显示")]
        public List<GameObject> noItemShow;
        
        /// <summary>
        /// 获取道具类型对应的Prefab
        /// </summary>
        /// <param name="_itemType"></param>
        /// <returns></returns>
        public GGUIMonoRecruitShopCardItem getItemPrefab(ENPItemType _itemType)
        {
            if (itemPrefabList == null)
                return null;

            foreach (var item in itemPrefabList)
            {
                if (item != null && item.itemType == _itemType)
                    return item.prefab;
            }

            return null;
        }
    }
}
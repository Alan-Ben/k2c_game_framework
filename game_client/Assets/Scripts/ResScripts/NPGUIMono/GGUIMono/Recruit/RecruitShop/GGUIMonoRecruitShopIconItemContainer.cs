using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 兑换商店道具icon对应的Prefab
    /// </summary>
    [Serializable]
    public class RecruitShopIconItemPrefab
    {
        [ALHeader("兑换道具类型")]
        public ERecruitItemType recruitItemType;

        [ALHeader("展示模板")]
        public GGUIMonoRecruitShopIconItem prefab;
    }

    /// <summary>
    /// 招募
    /// </summary>
    public class GGUIMonoRecruitShopIconItemContainer : _ATNPGGUIMonoBasicDiffItemContainer
    {
        [ALHeader("不同招募类型对应的Prefab")] public List<RecruitShopIconItemPrefab> itemPrefabList;

        [ALHeader("没有item时显示")] public List<GameObject> noItemShow;

        /// <summary>
        /// 获取道具类型对应的Prefab
        /// </summary>
        /// <param name="_recruitItemType"></param>
        /// <returns></returns>
        public GGUIMonoRecruitShopIconItem getItemPrefab(ERecruitItemType _recruitItemType)
        {
            if (itemPrefabList == null)
                return null;

            foreach (var item in itemPrefabList)
            {
                if (item != null && item.recruitItemType == _recruitItemType)
                    return item.prefab;
            }

            return null;
        }
    }
}
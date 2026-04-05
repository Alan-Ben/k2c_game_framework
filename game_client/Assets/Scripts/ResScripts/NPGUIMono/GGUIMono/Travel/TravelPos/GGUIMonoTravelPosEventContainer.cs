using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;

namespace GOE
{
    /// <summary>
    /// 事件item类型与Prefab模板的映射
    /// </summary>
    [Serializable]
    public class TravelPosEventItemPrefab
    {
        [ALHeader("事件类型")]
        public ETravelEventType eventType;
        [ALHeader("展示模板")]
        public _AGGUIMonoTravelPosEventItem prefab;
    }

    /// <summary>
    /// 游历地点事件容器Mono
    /// </summary>
    public class GGUIMonoTravelPosEventContainer : _ATNPGGUIMonoBasicDiffItemContainer
    {
        [ALHeader("不同事件类型对应的Prefab")]
        public List<TravelPosEventItemPrefab> itemPrefabList;
        [ALHeader("默认事件Prefab, 上面的列表里没有对应事件类型时使用")]
        public _AGGUIMonoTravelPosEventItem defaultItemPrefab;

        /// <summary>
        /// 根据事件item类型获取对应的Prefab模板
        /// </summary>
        public _AGGUIMonoTravelPosEventItem getItemPrefab(ETravelEventType _itemType)
        {
            if (itemPrefabList == null)
                return null;

            for (int i = 0; i < itemPrefabList.Count; i++)
            {
                TravelPosEventItemPrefab item = itemPrefabList[i];
                if (item != null && item.eventType == _itemType)
                    return item.prefab;
            }
            
            return defaultItemPrefab;
        }
    }
}
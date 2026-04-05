using System;
using System.Collections.Generic;
using Common.TravelEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历事件类型对应的Prefab
    /// </summary>
    [Serializable]
    public class AkeyTravelResultItemPrefab
    {
        [ALHeader("事件类型")]
        public ETravelEventType eventType;

        [ALHeader("展示模板")]
        public _AGGUIMonoAkeyTravelResultItem prefab;
    }
    
    public class GGUIMonoAkeyTravelResultItemContainer : _ATNPGGUIMonoBasicDiffItemContainer
    {
        [ALHeader("不同事件类型对应的Prefab")]
        public List<AkeyTravelResultItemPrefab> itemPrefabList;
        
        [ALHeader("没有item时显示")]
        public List<GameObject> noItemShow;
        
        /// <summary>
        /// 获取事件类型对应的Prefab
        /// </summary>
        /// <param name="_eventType"></param>
        /// <returns></returns>
        public _AGGUIMonoAkeyTravelResultItem getItemPrefab(ETravelEventType _eventType)
        {
            if (itemPrefabList == null)
                return null;

            foreach (var item in itemPrefabList)
            {
                if (item != null && item.eventType == _eventType)
                    return item.prefab;
            }

            return null;
        }
    }
}
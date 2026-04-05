using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public struct NPGGUIGetItemResParama
    {
        [ALHeader("物品类型")]
        public ENPItemType itemType;
        [ALHeader("预制体资源ID")]
        public long assetId;
    }

    /// <summary>
    /// 物品列表，支持根据物品类型显示不同样式
    /// </summary>
    public class NPGGUIMonoGetItemContainer : _AALBasicUIWndMono
    {
        public ScrollRect scrollRect;
        public GridLayoutGroup content;
        [ALHeader("缓存池父节点")]
        public Transform cacheParent;
        [ALHeader("默认使用的模板")]
        public NPGGUIMonoCommonItem itemTemplate;
        [ALHeader("特殊物品类型预制体资源ID列表")]
        public List<NPGGUIGetItemResParama> itemTempParams;
        [ALHeader("首个item开始出现的延迟时间（秒）")]
        public float firstItemStartShowDelay;
        [ALHeader("显示间隔（秒）")]
        public float showItemIntervalTime = 0.1f;
    }
}

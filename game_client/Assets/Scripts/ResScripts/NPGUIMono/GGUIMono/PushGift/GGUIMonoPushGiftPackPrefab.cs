using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包预制体
    /// </summary>
    public class GGUIMonoPushGiftPackPrefab : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("现金礼包展示")]
        public GGUISubMonoCashGiftPack monoCashGiftPack;

        [ALHeader("特殊展示物品预制体路径配置")]
        public SerializableDictionary<ENPItemType, NPCommonAssetPathInfo> specialShowItemPrefabPathDic;
        [ALHeader("默认特殊展示物品预制体路径(在上面的字典中没有找到对应类型配置时使用)")]
        public NPCommonAssetPathInfo defaultSpecialShowItemPrefabPath;
        [ALHeader("特殊展示物品预制体加载父节点")]
        public Transform specialShowItemPrefabParent;
        
        [ALHeader("礼包有效时间倒计时文本")]
        public TextEx txtCountdown;
        [ALHeader("礼包有效时间倒计时Key文本, 一个参数, 倒计时时间")]
        public string txtCountdownKey;
        
        /// <summary>
        /// 获取特殊展示物品预制体路径
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public NPCommonAssetPathInfo getSpecialShowItemPrefabPath(ENPItemType itemType)
        {
            if (specialShowItemPrefabPathDic == null)
                return defaultSpecialShowItemPrefabPath;
            
            if(specialShowItemPrefabPathDic.TryGetValue(itemType, out NPCommonAssetPathInfo assetPathInfo))
                return assetPathInfo;
            else
                return defaultSpecialShowItemPrefabPath;
        }
    }
}
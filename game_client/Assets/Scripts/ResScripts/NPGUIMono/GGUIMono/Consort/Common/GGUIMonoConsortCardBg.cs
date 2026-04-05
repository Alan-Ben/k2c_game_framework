using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class ConsortCardBgConfig
    {
        [ALHeader("妃子对应品质")]
        public EQuality quality;

        [ALHeader("背景图片索引")]
        public NPGTextureIndex bgImgIndex;
    }
    
    /// <summary>
    /// 妃子卡牌背景子窗口
    /// </summary>
    public class GGUIMonoConsortCardBg : _AALBasicUIWndMono
    {
        [ALHeader("背景图片")]
        public RawImage imgBg;

        [ALHeader("卡牌背景配置列表(没有在列表中找到的会直接展示配表配置的默认图片)")]
        public List<ConsortCardBgConfig> cardBgConfigList;
    }
}
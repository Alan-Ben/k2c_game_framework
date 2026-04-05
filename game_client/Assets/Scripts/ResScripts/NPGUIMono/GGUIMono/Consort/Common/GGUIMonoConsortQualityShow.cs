using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class ConsortQualityShowConfig
    {
        [ALHeader("妃子对应品质")]
        public EQuality quality;

        [ALHeader("背景图片索引")]
        public NPGTextureIndex bgImgIndex;
        
        [ALHeader("名字背景颜色")]
        public Color nameBgColor = Color.white;
    }
    
    public class GGUIMonoConsortQualityShow : _AALBasicUIWndMono
    {
        [ALHeader("背景图片")]
        public RawImage imgBg;
        
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;
        
        [ALHeader("名字背景")]
        public MaskableGraphic nameBg;
        
        public List<ConsortQualityShowConfig> qualityShowConfigList;
        
        public ConsortQualityShowConfig getQualityShowConfig(EQuality quality)
        {
            if (qualityShowConfigList == null || qualityShowConfigList.Count <= 0)
            {
                Debug.LogError($"[GGUIMonoConsortQualityShow getQualityShowConfig] 找不到品质:{quality}对应的UI配置", this);
                return null;
            }
            ConsortQualityShowConfig showConfig = null;
            for (int i = 0; i < qualityShowConfigList.Count; i++)
            {
                showConfig = qualityShowConfigList[i];
                if (showConfig != null && showConfig.quality == quality)
                    return showConfig;
            }

            Debug.LogError($"[GGUIMonoConsortQualityShow getQualityShowConfig] 找不到品质:{quality}对应的UI配置", this);
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星科技树连线配置
    /// </summary>
    [Serializable]
    public class MarsTechnologyTreeLineConfig
    {
        [ALHeader("连线图片列表")]
        public List<Image> lineImageList;
        
        [ALHeader("本层使用了这组连线的科技索引列表(从1开始, 为了防止出现列表拉太长但是忘记赋值的情况)")]
        public List<int> useLineTechnologyIndexList;
        
        public void setEnable(bool _enable)
        {
            if(lineImageList == null)
                return;

            foreach (var image in lineImageList)
            {
                if(image == null || image.rectTransform == null)
                    continue;
                
                ALUGUICommon.setUIObjScale(image.rectTransform, _enable ? Vector3.one : Vector3.zero);
            }
        }
        
        public void setColor(Color _color)
        {
            if(lineImageList == null)
                return;

            foreach (var image in lineImageList)
            {
                if(image == null)
                    continue;
                
                ALUGUICommon.setUIObjColor(image, _color);
            }
        }
    }
    
    /// <summary>
    /// 科技树层级prefab
    /// </summary>
    public class GGUIMonoMarsTechnologyTreeLayerPrefab : _AALBasicUIWndMono
    {
        [ALHeader("科技树item列表")]
        public List<GGUIMonoMarsTechnologyTreeItem> monoTechnologyTreeItemList;
        
        [ALHeader("科技树连线配置列表")]
        public List<MarsTechnologyTreeLineConfig> lineConfigList;
        
        [ALHeader("连线激活(父节点1级)时颜色")]
        public Color lineActivateColor;
        [ALHeader("连线未激活(父节点0级)时颜色")]
        public Color lineNonactivatedColor;
    }
}
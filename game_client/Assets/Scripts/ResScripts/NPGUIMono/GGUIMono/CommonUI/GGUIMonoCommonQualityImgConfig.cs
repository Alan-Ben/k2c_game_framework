using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 不同品质对应的图片配置
    /// </summary>
    [Serializable]
    public class GGUIMonoCommonQualityImgConfig
    {
        [ALHeader("品质")]
        public EQuality quality;
        
        [ALHeader("RawImage使用的图片资源索引")]
        public NPGTextureIndex textureIndex;

        [ALHeader("Image使用的图片资源索引")]
        public NPGSpriteIndex spriteIndex;
    }
}
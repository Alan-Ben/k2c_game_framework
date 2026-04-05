namespace GOE
{
    /// <summary>
    /// 妃子皮肤展示相关数据
    /// </summary>
    public interface _IConsortSkinShowInfo
    {
        /// <summary>
        /// 皮肤id
        /// </summary>
        long skinId { get; }

        /// <summary>
        /// 皮肤配表
        /// </summary>
        GConsortSkinRefObj skinRefObj { get; }
        
        /// <summary>
        /// 妃子头像
        /// </summary>
        NPGTextureIndex consortHeadIcon { get; }

        /// <summary>
        /// 妃子卡牌半身像
        /// </summary>
        NPGTextureIndex consortCardImage { get; }

        /// <summary>
        /// 皮肤icon
        /// </summary>
        NPGTextureIndex skinIcon { get; }

        /// <summary>
        /// 皮肤卡牌图片
        /// </summary>
        NPGTextureIndex skinCardImg { get; }

        /// <summary>
        /// 全身形象
        /// </summary>
        NPGGoIndex tdShow { get; }

        /// <summary>
        /// 形象背景GO
        /// </summary>
        NPGGoIndex tdBgIndex { get; }
    }
}
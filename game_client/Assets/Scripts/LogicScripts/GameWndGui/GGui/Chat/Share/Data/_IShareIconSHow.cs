using ChatPackage;

namespace GOE
{
    public interface _IShareIconSHow
    {
        /// <summary>
        /// 图标
        /// </summary>
        /// <returns></returns>
        NPGTextureIndex getIcon();
        /// <summary>
        /// 头像背景
        /// </summary>
        /// <returns></returns>
        NPGSpriteIndex getIconBg();
        /// <summary>
        /// 名字
        /// </summary>
        /// <returns></returns>
        string getName();
        /// <summary>
        /// 是否需要展示特殊标记
        /// </summary>
        /// <returns></returns>
        bool needShowSpecial();
        /// <summary>
        /// 执行分享
        /// </summary>
        _AMsgDetailInfo creatShareInfo();
    }
}
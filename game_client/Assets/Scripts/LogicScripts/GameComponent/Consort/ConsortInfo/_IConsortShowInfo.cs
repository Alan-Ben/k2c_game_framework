
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 情人展示数据接口
    /// </summary>
    public interface _IConsortShowInfo
    {
        /// <summary>
        /// 妃子id
        /// </summary>
        long consortId { get; }
        
        GConsortRefObj consortRefObj { get; }
        
        /// <summary>
        /// 妃子皮肤展示数据
        /// </summary>
        _IConsortSkinShowInfo consortSkinShowInfo { get; }
        
        /// <summary>
        /// 翻译过的妃子名
        /// </summary>
        string consortTransName { get; }
        
        /// <summary>
        /// 妃子翻译过的称号
        /// </summary>
        string consortTransTitle { get; }
        
        /// <summary>
        /// 妃子品质
        /// </summary>
        EQuality consortQuality { get; }
        
        /// <summary>
        /// 名称背景
        /// </summary>
        NPGTextureIndex consortNameBg { get; }
        
        /// <summary>
        /// 亲密度
        /// </summary>
        long intimacy { get; }

        /// <summary>
        /// 展示的魅力值
        /// </summary>
        long charm { get; }
        
        /// <summary>
        /// 是否已解锁
        /// </summary>
        EGameCommonUnlockType unlockType { get; }
        
        
    }
}
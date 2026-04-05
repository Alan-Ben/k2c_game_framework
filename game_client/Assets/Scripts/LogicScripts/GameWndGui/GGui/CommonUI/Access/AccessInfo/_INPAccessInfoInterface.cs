using NPEnum;

namespace GOE
{
    /// <summary>
    /// 获取途径信息类型
    /// </summary>
    public enum ENPAccessInfoType
    {
        BAG_ITEM,
        ACCESS,
        COMBIEND
    }

    /// <summary>
    /// 获取途径信息
    /// </summary>
    public interface _INPAccessInfoInterface
    {
        /// <summary>
        /// 排序id
        /// </summary>
        long sortId { get; }
        /// <summary>
        /// 类型
        /// </summary>
        ENPAccessInfoType type { get; }
        /// <summary>
        /// 品质
        /// </summary>
        EQuality quality { get; }
    }
}


/// <summary>
/// 各种 Index 资源的类型
/// </summary>
/// <remarks>
/// 在原有的 index mainId 和 subId 的基础上，增加了一种类型，用于区分不同的资源类型
/// </remarks>
public enum EIndexType
{
    /// <summary>
    /// 默认类型，不特别指定类型时采用，index 所表示的资源路径由 mainId 和 subId 构成
    /// </summary>
    DEFAULT,
    /// <summary>
    /// 增量类型，index 所表示的资源路径会在使用 mainId 和 subId 构成后，再在路径开头加上 “add_pack/” 的路径头
    /// </summary>
    ADD,

    // 增加 indexType 时，指定的值不能超过 255 ，存储时会转换为 byte 类型
}

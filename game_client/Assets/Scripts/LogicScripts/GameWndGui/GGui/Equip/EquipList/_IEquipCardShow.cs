
namespace GOE
{
    /// <summary>
    /// 藏品卡牌展示接口
    /// </summary>
    public interface _IEquipCardShow
    {
        /// <summary>
        /// 藏品信息
        /// </summary>
        EquipInfo equipInfo { get; }
        /// <summary>
        /// 藏品配置数据
        /// </summary>
        EquipRefObj equipRef { get; }
    }
}
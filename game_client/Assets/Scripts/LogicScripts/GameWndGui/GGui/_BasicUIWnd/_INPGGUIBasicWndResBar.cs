namespace GOE
{
    /// <summary>
    /// 资源barWnd的通用接口
    /// </summary>
    public interface _INPGGUIBasicWndResBar
    {
        /// <summary>
        /// 获取玩家头像Id
        /// </summary>
        /// <returns></returns>
        long getPlayerIconResId();
        
        /// <summary>
        /// 获取资源barId
        /// </summary>
        /// <returns></returns>
        long getBarResId();
    }
}
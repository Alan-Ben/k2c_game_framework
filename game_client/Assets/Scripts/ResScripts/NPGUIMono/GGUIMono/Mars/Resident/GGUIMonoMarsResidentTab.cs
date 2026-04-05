namespace GOE
{
    public enum EMarsResidentTabType
    {
        NONE,
        [ALHeader("状态")]
        STATE,
        [ALHeader("属性")]
        ATTRIBUTE
    }
    
    /// <summary>
    /// 火星基地 - 居民tab
    /// </summary>
    public class GGUIMonoMarsResidentTab : GGUIMonoCommonPageTabItem<EMarsResidentTabType>
    {
        [ALHeader("page路径")]
        public NPCommonAssetPathInfo pagePathInfo;
    }
}
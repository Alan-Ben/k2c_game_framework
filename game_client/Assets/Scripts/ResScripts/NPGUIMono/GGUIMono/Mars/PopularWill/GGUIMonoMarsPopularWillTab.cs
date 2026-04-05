namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意页签类型
    /// </summary>
    public enum EMarsPopularWillTabType
    {
        NONE,
        [ALHeader("信件")]
        LETTER,
        [ALHeader("求助")]
        HELP
    }
    
    public class GGUIMonoMarsPopularWillTab: GGUIMonoCommonPageTabItem<EMarsPopularWillTabType>
    {
        [ALHeader("page路径")]
        public NPCommonAssetPathInfo pagePathInfo;
    }
}
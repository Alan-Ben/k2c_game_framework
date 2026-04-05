
namespace GOE
{
    /// <summary>
    /// 多个冲榜详情主界面
    /// </summary>
    public class GGUIMonoRankRushMultipleDetail : GGUIMonoRankRushDetailBase
    {
        [ALHeader("多个冲榜详情页签容器")]
        public GGUIMonoRankRushMultipleDetailTabContainer monoMultipleTabContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3908); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3908); } }
    }
}

namespace GOE
{
    public class GGUIMonoConsortBlessHeroSimpleIconContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoConsortBlessHeroSimpleIcon>
    {
        [ALHeader("最少需要显示的关联大臣数量(若妃子关联大臣没有这么多, 不足的就是没大臣数据的状态)")]
        public int leastShowItemNum = 4;
    }
}
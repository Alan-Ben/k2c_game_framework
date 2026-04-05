namespace GOE
{
    /// <summary>
    /// 伙伴家人可为空头像列表
    /// </summary>
    public class GGUIMonoHeroBlessNullableContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoHeroBlessNullableContainerItem>
    {
        [ALHeader("需要展示的item数量，会强制显示该数量的item，如果item没有值会显示空白状态")]
        public int needShowItemCount = 4;
    }
}

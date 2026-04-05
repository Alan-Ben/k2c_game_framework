namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能列表item容器
    /// </summary>
    public class GGUIMonoHeroStarSkillContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoHeroStarSkillContainerItem>
    {
        [ALHeader("当未超出容器时列表是否居中")]
        public bool needSetCenterWhenNotExceed;
    }
}

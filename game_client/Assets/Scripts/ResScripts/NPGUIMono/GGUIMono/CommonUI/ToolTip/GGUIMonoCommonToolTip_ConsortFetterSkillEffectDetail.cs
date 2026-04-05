namespace GOE
{
    public class GGUIMonoCommonToolTip_ConsortFetterSkillEffectDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("当前所处羁绊技能等级")]
        public TextEx txtNowFetterSkillLvl;
        
        [ALHeader("当前所处羁绊技能名称")]
        public TextEx txtNowFetterSkillName;
        
        [ALHeader("当前所处羁绊技能效果描述")]
        public TextEx txtNowFetterSkillEffectDesc;

        [ALHeader("羁绊技能效果描述容器")]
        public GGUIMonoConsortFetterSkillEffectDescItemContainer fettersSkillEffectDescContainer;
    }
}
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 标本转化item
    /// </summary>
    public class GGUIMonoTreasureHuntSpecimenConvertItem : _TALUGUIMonoGridItem
    {
        [ALHeader("转化的矿石信息子窗口")]
        public GGUIMonoTreasureHuntOreInfo monoOreInfo;

        [ALHeader("转化成的技能点数item")]
        public GGUISubMonoCommonItemDetail monoSkillPointItem;
    }
}
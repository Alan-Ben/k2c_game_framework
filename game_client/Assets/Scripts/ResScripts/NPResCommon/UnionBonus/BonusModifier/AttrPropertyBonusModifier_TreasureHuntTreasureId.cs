using CommonEnum;

namespace GOE
{
    public class AttrPropertyBonusModifier_TreasureHuntTreasureId : _AAttrPropertyBonusModifier_id
    {
        public override EBonusFilterType getFilterType()
        {
            return EBonusFilterType.TREASURE_HUNT_TREASURE_ID;
        }
    }
}
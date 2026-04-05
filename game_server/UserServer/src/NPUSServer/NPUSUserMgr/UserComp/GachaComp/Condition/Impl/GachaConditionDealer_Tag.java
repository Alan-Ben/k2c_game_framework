package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_Tag;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition._IGachaConditionDealer;

public class GachaConditionDealer_Tag implements _IGachaConditionDealer
{
    @Override
    public EAvatarGachaGuaranteeCondType getConditionType()
    {
        return EAvatarGachaGuaranteeCondType.TAG;
    }

    @Override
    public boolean isEnable(NPUSUserData _userdata, _IGachaCondition _condItem, RefGachaItem _itemRef)
    {
        GachaCondition_Tag condItem = (GachaCondition_Tag) _condItem;
        return _itemRef.tag_list.contains(condItem.getTag());
    }
}

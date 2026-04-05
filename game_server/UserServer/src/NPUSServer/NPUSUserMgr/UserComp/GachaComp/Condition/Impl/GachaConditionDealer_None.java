package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition._IGachaConditionDealer;

public class GachaConditionDealer_None implements _IGachaConditionDealer
{
    @Override
    public EAvatarGachaGuaranteeCondType getConditionType()
    {
        return EAvatarGachaGuaranteeCondType.NONE;
    }

    @Override
    public boolean isEnable(NPUSUserData _userdata, _IGachaCondition _condItem, RefGachaItem _itemRef)
    {
        return true;
    }
}

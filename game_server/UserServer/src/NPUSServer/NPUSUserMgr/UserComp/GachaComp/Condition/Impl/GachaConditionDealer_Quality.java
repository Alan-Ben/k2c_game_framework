package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.AvatarGacha.Cond.Impl.GachaCondition_Quality;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition._IGachaConditionDealer;

public class GachaConditionDealer_Quality implements _IGachaConditionDealer
{
    @Override
    public EAvatarGachaGuaranteeCondType getConditionType()
    {
        return EAvatarGachaGuaranteeCondType.QUALITY;
    }

    @Override
    public boolean isEnable(NPUSUserData _userdata, _IGachaCondition _condItem, RefGachaItem _itemRef)
    {
        GachaCondition_Quality condItem = (GachaCondition_Quality) _condItem;
        return CommonFunc.inRange(_itemRef.getQuality().ordinal(), condItem.getMinQuality(), condItem.getMaxQuality());
    }
}

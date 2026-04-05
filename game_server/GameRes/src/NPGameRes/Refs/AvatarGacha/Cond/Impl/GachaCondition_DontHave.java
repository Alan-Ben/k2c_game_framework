package NPGameRes.Refs.AvatarGacha.Cond.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;

public class GachaCondition_DontHave implements _IGachaCondition
{
    @Override
    public EAvatarGachaGuaranteeCondType conditionType()
    {
        return EAvatarGachaGuaranteeCondType.DONT_HAVE;
    }
}
package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public interface _IGachaConditionDealer
{
    /**
     * 获取条件类型
     * @return 条件类型
     */
    EAvatarGachaGuaranteeCondType getConditionType();

    /**
     * 是否满足条件
     * @param _userdata 玩家数据
     * @param _condItem 条件
     * @param _itemRef  抽卡物品
     * @return 是否满足条件
     */
    boolean isEnable(NPUSUserData _userdata, _IGachaCondition _condItem, RefGachaItem _itemRef);
}

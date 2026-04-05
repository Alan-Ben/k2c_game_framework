package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPGameRes.Refs.AvatarGacha.Cond.GachaGuaranteeCondObj;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl.GachaConditionDealer_DontHave;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl.GachaConditionDealer_None;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl.GachaConditionDealer_Quality;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Condition.Impl.GachaConditionDealer_Tag;

public class GachaConditionDealerMgr
{
    private static final GachaConditionDealerMgr _g_instance = new GachaConditionDealerMgr();

    public static GachaConditionDealerMgr getInstance()
    {
        return _g_instance;
    }

    private final _IGachaConditionDealer[] _m_conditionList = new _IGachaConditionDealer[EAvatarGachaGuaranteeCondType.values().length];

    public GachaConditionDealerMgr()
    {
        regDealer(new GachaConditionDealer_None());
        regDealer(new GachaConditionDealer_Tag());
        regDealer(new GachaConditionDealer_Quality());
        regDealer(new GachaConditionDealer_DontHave());
    }

    private void regDealer(_IGachaConditionDealer _dealer)
    {
        _m_conditionList[_dealer.getConditionType().ordinal()] = _dealer;
    }

    public _IGachaConditionDealer getDealer(EAvatarGachaGuaranteeCondType _conditionType)
    {
        return _m_conditionList[_conditionType.ordinal()];
    }

    public boolean isEnable(NPUSUserData _userdata, GachaGuaranteeCondObj _condObj, RefGachaItem _itemRef)
    {
        for (_IGachaCondition condItem : _condObj.getCondList())
        {
            _IGachaConditionDealer dealer = getDealer(condItem.conditionType());
            if (dealer == null)
                return false;

            if (!dealer.isEnable(_userdata, condItem, _itemRef))
                return false;
        }
        return true;
    }
}

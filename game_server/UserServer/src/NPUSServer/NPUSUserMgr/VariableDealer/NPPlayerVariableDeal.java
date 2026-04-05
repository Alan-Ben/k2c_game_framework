package NPUSServer.NPUSUserMgr.VariableDealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.Variable._ATNPBasicVariableDealMgr;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.Dealer.*;

public class NPPlayerVariableDeal
        extends _ATNPBasicVariableDealMgr<ENPPlayerVariableType, _ANPBasicPlayerVariableObj, NPPlayerVariableGroupObj, NPUSUserData>
{
    public static NPPlayerVariableDeal getInstance()
    {
        return _instance;
    }

    private static NPPlayerVariableDeal _instance = new NPPlayerVariableDeal();

    public NPPlayerVariableDeal()
    {
        super(ENPPlayerVariableType.class);

        _regDealer(new PlayerVariableDealer_CS_NUM());
        _regDealer(new PlayerVariableDealer_S_RND());
        _regDealer(new PlayerVariableDealer_CS_VALUE());
        _regDealer(new PlayerVariableDealer_CS_PROPERTY());
        _regDealer(new PlayerVariableDealer_CS_BUF_L());
        _regDealer(new PlayerVariableDealer_CS_BUFF_HAD_ACTIVE_DAY());
        _regDealer(new PlayerVariableDealer_CS_ITEM_COUNT());
        _regDealer(new PlayerVariableDealer_CS_PARAM());
        _regDealer(new PlayerVariableDealer_CS_VALUE_ID());
        _regDealer(new PlayerVariableDealer_CS_VAR_V());
        _regDealer(new PlayerVariableDealer_CS_CONDITION_BOOLEAN_V());
        _regDealer(new PlayerVariableDealer_CS_RECORD_PARAM());
        _regDealer(new PlayerVariableDealer_CS_EVENT_RECORD());
        _regDealer(new PlayerVariableDealer_CS_SCOPE_ITEM_COUNT());
        _regDealer(new PlayerVariableDealer_S_RND_ATTR_HERO());
        _regDealer(new PlayerVariableDealer_S_RND_FROM_SCOPE());
        _regDealer(new PlayerVariableDealer_S_RND_CONSORT());
        _regDealer(new PlayerVariableDealer_CS_HERO_NUM());
        _regDealer(new PlayerVariableDealer_S_RND_VALUE_BY_WEIGHT());
        _regDealer(new PlayerVariableDealer_CS_CONSORT_RES_COUNT());
        _regDealer(new PlayerVariableDealer_CS_HAS_BUILDING());
        _regDealer(new PlayerVariableDealer_CS_BUSINESS_BUILDING_WORKER_NUM());
        _regDealer(new PlayerVariableDealer_CS_BUILDING_LEVEL());
        _regDealer(new PlayerVariableDealer_CS_MARS_BUILDING_LVL());
        _regDealer(new PlayerVariableDealer_CS_MARS_BUILDING_EQUIP_LVL());
        _regDealer(new PlayerVariableDealer_CS_MARS_BUILDING_DISPATCH_NUM());
    }
}

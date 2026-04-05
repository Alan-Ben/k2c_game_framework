package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_S_RND_ATTR_HERO;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

import java.util.List;

public class PlayerVariableDealer_S_RND_ATTR_HERO extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.S_RND_ATTR_HERO;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_S_RND_ATTR_HERO obj = (NPPlayerVariable_S_RND_ATTR_HERO) _variableObj;

        List<HeroInfo> heroList;
        if (obj.attrType() == null)
        {
            heroList = _userData.getHeroComponent().getAllHeroList();
        } else
        {
            //获取有指定特长类型的大臣列表
            heroList = _userData.getHeroComponent().getAttrHeroList(obj.attrType());
        }

        //随机一个出一个大臣
        HeroInfo heroInfo = CommonFunc.randSelect(heroList);
        //返回大臣id
        return heroInfo == null ? 0 : heroInfo.getHeroId();
    }
}

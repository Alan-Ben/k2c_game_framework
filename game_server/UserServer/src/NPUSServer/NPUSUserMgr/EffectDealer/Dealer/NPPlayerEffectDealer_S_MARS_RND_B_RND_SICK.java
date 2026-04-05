package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsPeopleBuildingFunc;

public class NPPlayerEffectDealer_S_MARS_RND_B_RND_SICK extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_RND_B_RND_SICK;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
    	//随机获取一个建筑（至少需要入住1名以上居民）
    	MarsPeopleBuildingFunc func = _userData.getMarsBuildingComponent().getPeopleBuildingFuncMgr().lookupRnd(1);
    	if(null == func)
    		return;
    	
    	func.reduceRndPeople(true, _context);
    }
}

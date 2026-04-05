package NPGameRes.InitDealer;


import NPGameRes.GameObjs.Reward.RewardMgr;

/*******************
 * 初始化Reward中子集数据
 * @author Administrator
 *
 */
public class NPRewardInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        RewardMgr.getInstance().init();
    }

}

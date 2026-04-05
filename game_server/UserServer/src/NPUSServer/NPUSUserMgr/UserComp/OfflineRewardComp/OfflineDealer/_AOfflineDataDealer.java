package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_CommonReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardTakeResult;

public abstract class _AOfflineDataDealer
{
    /**
     * 离线数据枚举
     * @return
     */
    public abstract EOfflineRewardEnum getEnum();

    /**
     * 是否有效，无效数据不会进行后续的处理
     * @return
     */
    public abstract boolean isValid();
    
    /**
     * 是否同步客户端，非同步客户端处理，如果同步客户端，枚举需要加 C_ 前缀
     * @return
     */
    public abstract boolean syncToClient();

    /**
     * 预处理
     * @param _info
     * @param _context
     */
    public void preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {
        //如果已经预处理则不重复处理
        if (_info.hasPreDeal())
            return;

        _preDeal(_info, _context);

        //标记已经预处理
        _info.markHasPreDeal(_context);
    }
    
    /**
     * 预处理流程
     * @param _info
     */
    protected abstract void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context);

    /**
     * 领取奖励
     * @param _info
     * @param _context
     * @param _result
     */
    public void takeReward(OfflineRewardInfo _info, NPPlayerContext _context, OfflineRewardTakeResult _result)
    {
        //领取奖励
        Offline_CommonReward rewardItemListObj = _info.getItemList();
        if(null != rewardItemListObj)
        {
            _info.getUserData().gainItemListP(rewardItemListObj.getItemList(), _context);

            _info.getUserData().sendMsgToGC(_context.getCollector().toProto());
        }
    }
}

package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_042_ReqDealHeroRecommend;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_042_ReqDealHeroRecommend extends NPUserMsgDealer<GC2GS_021_042_ReqDealHeroRecommend>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_042_ReqDealHeroRecommend _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查大臣是否存在
        if(null != userData.getHeroComponent().lookupHero(_msg.getHeroId()))
        {
        	_commiter.commitFailRes(HeroErr.HERO_ALREADY_EXISTED.getCode());
        	return;
        }
        

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_RECOMMEND_DEAL);

        //处理大臣推荐事件
        ResultOne<Long> result = userData.getHeroRecommendComponent().dealHeroRecommend(_msg.getInstanceId(), _msg.getHeroId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_042_RetDealHeroRecommend(result.getData()));

        //记录处理大臣推荐事件
        _commiter.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.DEAL_HERO_RECOMMEND, 1, context);
    }
}

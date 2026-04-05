package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_043_ReqDelHeroRecommend;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_043_ReqDelHeroRecommend extends NPUserMsgDealer<GC2GS_021_043_ReqDelHeroRecommend>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_043_ReqDelHeroRecommend _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //处理大臣推荐事件
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_RECOMMEND_DEL);
        userData.getHeroRecommendComponent().delHeroRecommend(_msg.getInstanceId(), context);
    }
}

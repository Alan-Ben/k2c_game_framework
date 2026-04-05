package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_034_ReqFriendRecommend;
import NPCommon.ErrMain.CommErr;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerOne;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;


public class MsgDealer_GC2GS_021_034_ReqFriendRecommend extends NPUserMsgDealer<GC2GS_021_034_ReqFriendRecommend>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_034_ReqFriendRecommend _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        List<Long> recommendList = getUSServer().getFriendTipMgr().getRecommendList(userData.getCid(),
                userData.getFriendComponent().getFriendMgr().getAllFriendCidList());
        if (recommendList.isEmpty())
        {
            _commiter.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        getUSServer().getPlayerCacheGetter().getInfoListA(PlayerInfo_IconShow.class, recommendList, new HandlerOne<Map<Long, PlayerInfo_IconShow>>()
        {
            @Override
            public void handle(Map<Long, PlayerInfo_IconShow> _m_map)
            {
                if (null != _m_map)
                {
                    List<PlayerInfo_IconShow> showList = new ArrayList<>(_m_map.values());
                    //对列表进行排序，在线的排在前面，最近离线的排在前面
                    showList.sort((o1, o2) ->
                    {
                        if (o1.getIsOnline() && !o2.getIsOnline())
                            return -1;

                        if (!o1.getIsOnline() && o2.getIsOnline())
                            return 1;

                        return Long.compare(o2.getLastOfflineMs(), o1.getLastOfflineMs());
                    });

                    _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_034_RetFriendRecommend(showList));
                } else
                {
                    _commiter.commitFailRes(CommErr.SYS_ERR.getCode());
                }
            }
        });
    }
}

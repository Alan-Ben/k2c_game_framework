package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_001_ReqChapterForward;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo.ForwardResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_016_001_ReqChapterForward extends NPUserMsgDealer<GC2GS_016_001_ReqChapterForward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_001_ReqChapterForward _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取章节组件
        ChapterInfo chapterInfo = userData.getChapterComponent().getChapterInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_FORWARD);

        //执行逻辑
        ResultOne<ForwardResult> result = chapterInfo.forward(_msg.getChapterId(), _msg.getPoint(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //尝试触发事件
        RefChapterEvent refEvent = chapterInfo.tryTriggerEvent(context);
        NPPlayerContext dealEventContext = null;
        if (refEvent != null)
        {
            if (_msg.getIsAKey())
            {
                if (refEvent.detailRef != null)
                {
                    dealEventContext = NPPlayerContext.createNew(context);
                    userData.gainItemList(refEvent.detailRef.getAKeyForwordRewardList(), dealEventContext);
                }else
                {
                    USLog.error(userData.getUSServer(), "MsgDealer_GC2GS_016_001_ReqChapterForward refEvent.detailRef is null, cid:{} eventId:{}",
                            userData.getCid(), refEvent.Id());
                }
            }else
            {
                chapterInfo.createEvent(refEvent);
            }
        }

        //返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_001_RetChapterForward(_msg.getChapterId(), _msg.getPoint(),
                result.getData().coefficient, result.getData().rewardHeroExp, result.getData().rewardPlayerExp,
                result.getData().costGoldNum, refEvent == null ? 0 : refEvent.Id(), dealEventContext));
    }
}

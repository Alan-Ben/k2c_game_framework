package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_004_ReqDealChapterRewardEvent;
import NPCommon.ErrMain.ChapterErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Reward;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event._AChapterEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;

public class MsgDealer_GC2GS_016_004_ReqDealChapterRewardEvent extends NPUserMsgDealer<GC2GS_016_004_ReqDealChapterRewardEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_004_ReqDealChapterRewardEvent _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取章节组件
        ChapterInfo chapterInfo = userData.getChapterComponent().getChapterInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_FORWARD);

        _AChapterEvent event = chapterInfo.getEventInfo();
        ChapterEvent_Reward chapterEventReward = event instanceof ChapterEvent_Reward ? ((ChapterEvent_Reward) event) : null;
        if (chapterEventReward == null)
        {
            _committer.commitFailRes(ChapterErr.CHAPTER_EVENT_NOT_FOUND.getCode());
            return;
        }

        Result result = chapterEventReward.deal(context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //判断事件是否完结
        boolean isDone = chapterEventReward.isDone();
        if (isDone)
        {
            chapterInfo.disposeEvent(event, context);
        }


        //返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_004_RetDealChapterRewardEvent(context));
    }
}

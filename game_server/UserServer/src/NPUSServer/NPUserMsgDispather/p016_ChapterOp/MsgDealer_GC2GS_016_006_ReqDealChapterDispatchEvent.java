package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_006_ReqDealChapterDispatchEvent;
import NPCommon.ErrMain.ChapterErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Dispatch;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event._AChapterEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;

public class MsgDealer_GC2GS_016_006_ReqDealChapterDispatchEvent extends NPUserMsgDealer<GC2GS_016_006_ReqDealChapterDispatchEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_006_ReqDealChapterDispatchEvent _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取章节组件
        ChapterInfo chapterInfo = userData.getChapterComponent().getChapterInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_FORWARD);

        _AChapterEvent event = chapterInfo.getEventInfo();
        ChapterEvent_Dispatch chapterEventDispatch = event instanceof ChapterEvent_Dispatch ? ((ChapterEvent_Dispatch) event) : null;
        if (chapterEventDispatch == null)
        {
            _committer.commitFailRes(ChapterErr.CHAPTER_EVENT_NOT_FOUND.getCode());
            return;
        }

        ResultOne<Integer> result = chapterEventDispatch.chooseHero(_msg.getHeroList(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //判断事件是否完结
        boolean isDone = chapterEventDispatch.isDone();
        if (isDone)
        {
            chapterInfo.disposeEvent(event, context);
        }


        //返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_006_RetDealChapterDispatchEvent(result.getData(), context));
    }
}

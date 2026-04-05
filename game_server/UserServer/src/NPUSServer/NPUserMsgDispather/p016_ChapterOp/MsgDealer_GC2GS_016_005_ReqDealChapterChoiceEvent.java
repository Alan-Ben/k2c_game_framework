package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_005_ReqDealChapterChoiceEvent;
import NPCommon.ErrMain.ChapterErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Choice;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event._AChapterEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;

public class MsgDealer_GC2GS_016_005_ReqDealChapterChoiceEvent extends NPUserMsgDealer<GC2GS_016_005_ReqDealChapterChoiceEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_005_ReqDealChapterChoiceEvent _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取章节组件
        ChapterInfo chapterInfo = userData.getChapterComponent().getChapterInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_FORWARD);

        _AChapterEvent event = chapterInfo.getEventInfo();
        ChapterEvent_Choice chapterEventChoice = event instanceof ChapterEvent_Choice ? ((ChapterEvent_Choice) event) : null;
        if (chapterEventChoice == null)
        {
            _committer.commitFailRes(ChapterErr.CHAPTER_EVENT_NOT_FOUND.getCode());
            return;
        }

        Result result = chapterEventChoice.chooseOption(_msg.getOptionId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //判断事件是否完结
        boolean isDone = chapterEventChoice.isDone();
        if (isDone)
        {
            chapterInfo.disposeEvent(event, context);
        }


        //返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_005_RetDealChapterChoiceEvent(context));
    }
}

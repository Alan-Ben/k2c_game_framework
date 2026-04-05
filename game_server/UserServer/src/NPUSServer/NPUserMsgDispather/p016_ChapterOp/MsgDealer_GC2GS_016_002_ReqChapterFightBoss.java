package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_002_ReqChapterFightBoss;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;

import java.util.List;

public class MsgDealer_GC2GS_016_002_ReqChapterFightBoss extends NPUserMsgDealer<GC2GS_016_002_ReqChapterFightBoss>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_002_ReqChapterFightBoss _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取章节组件
        ChapterInfo chapterInfo = userData.getChapterComponent().getChapterInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_FORWARD);

        //执行逻辑
        ResultOne<List<NPCommon_ItemInfo>> result = chapterInfo.challengeBoss(_msg.getChapterId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_002_RetChapterFightBoss(_msg.getChapterId(), result.getData()));
    }
}

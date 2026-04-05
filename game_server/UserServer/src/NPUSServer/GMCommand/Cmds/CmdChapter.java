package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.ChapterInfo;


@ACommander(comment = "关卡", name = "chapter")
public class CmdChapter extends UsCmdBase
{
    @ACommand(comment = "修改位置[章节id][位置]")
    public String chgPos(long _chapterId, int _point)
    {
        Result result = getOwner().getChapterComponent().getChapterInfo().gmChgPos(_chapterId, _point);
        if (!result.isSucc())
            return result.getMsg();
        return "success";
    }

    @ACommand(comment = "到达boss点")
    public String toBossPoint()
    {
        Result result = getOwner().getChapterComponent().getChapterInfo().gmToBossPoint();
        if (!result.isSucc())
            return result.getMsg();
        return "success";
    }

    @ACommand(comment = "关卡信息")
    public String info()
    {
        return getOwner().getChapterComponent().getChapterInfo().toString();
    }

    @ACommand(comment = "移除事件")
    public String removeEvent()
    {
        ChapterInfo chapterInfo = getOwner().getChapterComponent().getChapterInfo();
        chapterInfo.disposeEvent(chapterInfo.getEventInfo(), getContext());
        return "done";
    }
}

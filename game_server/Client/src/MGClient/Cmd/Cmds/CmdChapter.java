package MGClient.Cmd.Cmds;

import Common.ChapterEnum.EChapterInspireType;
import GC2GS.p016_ChapterOp.GC2GS_016_001_ReqChapterForward;
import GC2GS.p016_ChapterOp.GC2GS_016_002_ReqChapterFightBoss;
import GC2GS.p016_ChapterOp.GC2GS_016_003_ReqChapterFightBossInspire;
import GS2GC.p016_ChapterOp.GS2GC_016_001_RetChapterForward;
import GS2GC.p016_ChapterOp.GS2GC_016_002_RetChapterFightBoss;
import GS2GC.p016_ChapterOp.GS2GC_016_003_RetChapterFightBossInspire;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "关卡", name = "chapter")
public class CmdChapter extends CmdBase
{
    @Command(comment = "前进")
    public void forward(long _chapterId, int _point)
    {
        GC2GS_016_001_ReqChapterForward proto = new GC2GS_016_001_ReqChapterForward();
        proto.setChapterId(_chapterId);
        proto.setPoint(_point);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_016_001_RetChapterForward>(GS2GC_016_001_RetChapterForward.class)
        {
            @Override
            public void handle(GS2GC_016_001_RetChapterForward _response)
            {
            }
        });
    }

    @Command(comment = "打boss")
    public void fightBoss(long _chapterId)
    {
        GC2GS_016_002_ReqChapterFightBoss proto = new GC2GS_016_002_ReqChapterFightBoss();
        proto.setChapterId(_chapterId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_016_002_RetChapterFightBoss>(GS2GC_016_002_RetChapterFightBoss.class)
        {
            @Override
            public void handle(GS2GC_016_002_RetChapterFightBoss _response)
            {
            }
        });
    }

    @Command(comment = "激励")
    public void inspire(EChapterInspireType _type)
    {
        GC2GS_016_003_ReqChapterFightBossInspire proto = new GC2GS_016_003_ReqChapterFightBossInspire();
        proto.setType(_type);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_016_003_RetChapterFightBossInspire>(GS2GC_016_003_RetChapterFightBossInspire.class)
        {
            @Override
            public void handle(GS2GC_016_003_RetChapterFightBossInspire _response)
            {
            }
        });
    }
}

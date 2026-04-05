
using System.Collections.Generic;
using GC2GS.p016_ChapterOp;

namespace GOE
{
    public static class GSWriter_016_ChapterOp
    {
        public static GC2GS_016_001_ReqChapterForward make_001_ReqChapterForward(long _chapterId, int _point, bool _isAKey)
        {
            return new GC2GS_016_001_ReqChapterForward(_chapterId, _point, _isAKey);
        }
        
        public static GC2GS_016_002_ReqChapterFightBoss make_002_ReqChapterFightBoss(long _chapterId)
        {
            return new GC2GS_016_002_ReqChapterFightBoss(_chapterId);
        }
        
        public static GC2GS_016_003_ReqChapterFightBossInspire make_003_ReqChapterFightBossInspire(Common.ChapterEnum.EChapterInspireType _type)
        {
            return new GC2GS_016_003_ReqChapterFightBossInspire(_type);
        }
    }
}
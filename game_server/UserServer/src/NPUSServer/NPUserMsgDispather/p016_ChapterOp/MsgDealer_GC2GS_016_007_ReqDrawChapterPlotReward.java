package NPUSServer.NPUserMsgDispather.p016_ChapterOp;

import GC2GS.p016_ChapterOp.GC2GS_016_007_ReqDrawChapterPlotReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;

public class MsgDealer_GC2GS_016_007_ReqDrawChapterPlotReward extends NPUserMsgDealer<GC2GS_016_007_ReqDrawChapterPlotReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_016_007_ReqDrawChapterPlotReward _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_CHAPTER_PLOT_REWARD);

        // 调用批量领取方法
        Result result = userData.getChapterComponent().drawPlotRewardList(_msg.getPlotIdList(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 发送奖励显示
        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        // 返回结果
        _committer.commitSucRes(US2GCWriter_016_ChapterOp.make_007_RetDrawChapterPlotReward());
    }
}

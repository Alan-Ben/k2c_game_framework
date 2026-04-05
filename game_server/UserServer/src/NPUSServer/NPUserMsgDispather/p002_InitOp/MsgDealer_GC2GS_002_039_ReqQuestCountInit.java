package NPUSServer.NPUserMsgDispather.p002_InitOp;

import Common.QuestObj.Quest_Count;
import GC2GS.p002_InitOp.GC2GS_002_039_ReqQuestCountInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

import java.util.ArrayList;


public class MsgDealer_GC2GS_002_039_ReqQuestCountInit extends NPUserMsgDealer<GC2GS_002_039_ReqQuestCountInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_039_ReqQuestCountInit _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;


        ArrayList<Quest_Count> list = new ArrayList<>();
        userData.getQuestComponent().makeCountProto(list);

        //分页推送 每条协议500个
        final int PAGE_SIZE = 500;
        int totalCount = list.size();
        int pageCount = (totalCount + PAGE_SIZE - 1) / PAGE_SIZE;
        for (int pageIndex = 0; pageIndex < pageCount; ++pageIndex)
        {
            int startIndex = pageIndex * PAGE_SIZE;
            int endIndex = Math.min(startIndex + PAGE_SIZE, totalCount);
            ArrayList<Quest_Count> subList = new ArrayList<>(list.subList(startIndex, endIndex));
            userData.sendMsgToGC(US2GCWriter_002_InitOp.make_255_OnQuestCountInit(subList));
        }

        //返回数据
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_039_RetQuestCountInit());
    }
}

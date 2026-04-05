package NPHttpServer.Http.Core.Task;

import ALBasicServer.ALTask._IALSynTask;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core.QuestNode._ANPOutBoundHttpQuestNode;

/**
 * @description: 通知Core请求任务完成
 * @author: ricci
 * @date: 2023-03-23 14:39:22
 */
public class NPHttpSynTask_QuestDone implements _IALSynTask
{
    /**
     * hs服务器http处理core
     */
    private final NPHSHttpServiceCore _m_npHsHttpServiceCore;
    /**
     * 已完成的任务
     */
    private final _ANPOutBoundHttpQuestNode _m_npOutBoundHttpQuestNode;

    public NPHttpSynTask_QuestDone(NPHSHttpServiceCore _npHsHttpServiceCore, _ANPOutBoundHttpQuestNode _npOutBoundHttpQuestNode)
    {

        _m_npHsHttpServiceCore = _npHsHttpServiceCore;
        _m_npOutBoundHttpQuestNode = _npOutBoundHttpQuestNode;
    }

    @Override
    public void run()
    {
        _m_npHsHttpServiceCore.questDone(_m_npOutBoundHttpQuestNode);
    }
}

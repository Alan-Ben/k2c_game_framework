package NPCrossGameServer.NPCrossGameCore;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;


@SuppressWarnings("rawtypes")
public class SynTask_CrossGameCategoryMsgTick implements _IALSynTask
{
    //跨服游戏实例对象
    private _ACrossGameInstanceCategory _m_ciCrossGameCategory;

    public SynTask_CrossGameCategoryMsgTick(_ACrossGameInstanceCategory _category)
    {
        _m_ciCrossGameCategory = _category;
    }

    @Override
    public void run()
    {
        //执行tick操作，如果返回失败则直接退出
        if (!_m_ciCrossGameCategory.tick())
            return;

        //注册下一个任务，都是50毫秒间隔，保证消息可以及时响应
        ALSynTaskManager.getInstance().regTask(this, 50);
    }
}

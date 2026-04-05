package NPGameRes.Refs.Quest;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.QuestEnum.EQuestType;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;

/************************
 * 针对主线任务配置数据的管理
 * @author mj
 *
 */
public class MainQuestMgr
{
    private static MainQuestMgr _g_instance = new MainQuestMgr();

    public static MainQuestMgr getInstance()
    {
        return _g_instance;
    }

    private ArrayList<RefQuest> _m_alMainQuestList;
    private MutexAtom _m_mutex;

    public MainQuestMgr()
    {
        _m_alMainQuestList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /****************
     * 初始化主线任务列表
     */
    public void init()
    {
        //先通过初始化开启的任务中获取到对应的首发主线任务
        RefQuest firstMainQuestRef = null;
        ArrayList<Long> initQuestList = RefGeneral.Ref().quest_init_open_list;
        for (int i = 0; i < initQuestList.size(); i++)
        {
            Long questId = initQuestList.get(i);
            RefQuest questRef = RefQuest.getMgr().get(questId);
            if (null == questRef)
            {
                continue;
            }

            if (EQuestType.MAIN == questRef.quest_type)
            {
                firstMainQuestRef = questRef;
                break;
            }
        }

        if (null == firstMainQuestRef)
        {
            return;
        }

        //通过首发主线任务，获取主线任务链条
        _lock();

        try
        {
            //清空主线任务列表
            _m_alMainQuestList.clear();

            //设置上限
            int maxLoop = 10000;
            //记录首发主线任务
            _m_alMainQuestList.add(firstMainQuestRef);
            //获取主线任务链条
            RefQuest nextMainQuestRef = RefQuest.getMgr().get(firstMainQuestRef.next_quest_id);
            while (null != nextMainQuestRef)
            {
                _m_alMainQuestList.add(nextMainQuestRef);
                //防止死循环，设置了保护上限
                if (_m_alMainQuestList.size() > maxLoop)
                {
                    CommLog.error("Main Quest Ref Count > {}, please check!", maxLoop);
                    break;
                }

                //获取下一条主线任务
                long nextMainQuestId = nextMainQuestRef.next_quest_id;
                nextMainQuestRef = RefQuest.getMgr().get(nextMainQuestId);
            }
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 获取全部主线任务
     * @return
     */
    public ArrayList<RefQuest> getAllList()
    {
        _lock();

        try
        {
            return new ArrayList<>(_m_alMainQuestList);
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 获取指定任务的之前的所有任务列表
     * @param _questId
     * @return
     */
    public ArrayList<RefQuest> getBeforeQuestList(long _questId)
    {
        _lock();

        try
        {
            ArrayList<RefQuest> list = new ArrayList<>();
            for (RefQuest questRef : _m_alMainQuestList)
            {
                if (questRef.quest_id >= _questId)
                    break;

                list.add(questRef);
            }

            return list;
        } finally
        {
            _unlock();
        }
    }
}

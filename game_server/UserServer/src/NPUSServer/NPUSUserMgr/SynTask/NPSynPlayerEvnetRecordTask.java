package NPUSServer.NPUSUserMgr.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPEnum.ENCounterDealType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerEventRecordComp.NPPlayerEventRecordComp;

/****************
 * 玩家数据在线状态变更处理任务
 * @author Administrator
 *
 */
public class NPSynPlayerEvnetRecordTask implements _IALSynTask
{

    private NPUSUserData _m_udUserData;

    private ENCounterDealType _m_type;

    private long _m_count;

    private final int _m_recordTypeId;

    private final long _m_recordSubId;

    public NPSynPlayerEvnetRecordTask(NPUSUserData _userData, ENCounterDealType _type, int _recordTypeId, long _recordSubId, long _count)
    {
        _m_udUserData = _userData;
        _m_type = _type;
        _m_count = _count;
        _m_recordTypeId = _recordTypeId;
        _m_recordSubId = _recordSubId;
    }

    @Override
    public void run()
    {
        if (null == _m_udUserData)
            return;
        if (_m_recordTypeId == 0)
        {
            return;
        }
        NPPlayerEventRecordComp recordComp = _m_udUserData.getEventRecordComp();
        switch (_m_type)
        {
            case ADD:
                recordComp.addRecord(_m_recordTypeId, _m_recordSubId, _m_count);
                break;
            case SET:
                recordComp.setRecord(_m_recordTypeId, _m_recordSubId, _m_count);
                break;
            case REDUCE:
                recordComp.reduceRecord(_m_recordTypeId, _m_recordSubId, _m_count);
                break;
            case SET_GT:
                recordComp.setRecordGT(_m_recordTypeId, _m_recordSubId, _m_count);
                break;
            default:
                break;
        }
    }

    /**
     * 创建线程任务执行博物馆计数统计
     * @param _userData     玩家数据
     * @param _dealType     处理类型
     * @param _recordTypeId 记录类型
     * @param _recordSubId  记录详细id
     * @param _count        数量
     */
    public static void asyncRecord(NPUSUserData _userData, ENCounterDealType _dealType, int _recordTypeId, long _recordSubId, long _count)
    {
        ALSynTaskManager.getInstance().regTask(new NPSynPlayerEvnetRecordTask(_userData, _dealType, _recordTypeId, _recordSubId, _count));
    }

    /**
     * 同步创建执行博物馆计数统计
     * @param _userData     玩家数据
     * @param _dealType     处理类型
     * @param _recordTypeId 记录类型
     * @param _recordSubId  记录详细id
     * @param _count        数量
     */
    public static void syncRecord(NPUSUserData _userData, ENCounterDealType _dealType, int _recordTypeId, long _recordSubId, long _count)
    {
        if (null == _userData)
            return;
        if (_recordTypeId == 0)
        {
            return;
        }
        NPPlayerEventRecordComp recordComp = _userData.getEventRecordComp();
        switch (_dealType)
        {
            case ADD:
                recordComp.addRecord(_recordTypeId, _recordSubId, _count);
                break;
            case SET:
                recordComp.setRecord(_recordTypeId, _recordSubId, _count);
                break;
            case REDUCE:
                recordComp.reduceRecord(_recordTypeId, _recordSubId, _count);
                break;
            case SET_GT:
                recordComp.setRecordGT(_recordTypeId, _recordSubId, _count);
                break;
            default:
                break;
        }
    }
}

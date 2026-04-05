package NPUSServer.QuestionnaireMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_QuestionnaireInfo;
import Common.Common_QuestionnaireRewardInfo;
import GS2GC.p007_CommOp.GS2GC_007_063_OnQuestionnaireStart;
import GS2GC.p007_CommOp.GS2GC_007_064_OnQuestionnaireClose;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Parse.NPItemListParse;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.QuestionnaireInfoBO;
import USDB.Bo.QuestionnairePlayerRecordBO;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

public class QuestionnaireMgr
{
    private NPUserServer _m_userServer;
    private List<QuestionnaireInfo> _m_questionnaireList;
    private MutexObject _m_mutex;

    public QuestionnaireMgr(NPUserServer _userServer)
    {
        _m_userServer = _userServer;
        _m_questionnaireList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    public NPUserServer getUserServer()
    {
        return _m_userServer;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化入口
     * @return
     */
    public boolean s_init()
    {
        //初始化问卷信息
        if (!__initQuestionnaireInfo())
            return false;

        //初始化玩家记录
        if (!__initPlayerRecord())
            return false;

        return true;
    }

    /**
     * 初始化问卷信息
     * @return
     */
    private boolean __initQuestionnaireInfo()
    {
        List<QuestionnaireInfoBO> boList = _m_userServer.getBM().getBM(QuestionnaireInfoBO.class).s_findAll();
        if (boList == null)
        {
            USLog.error(_m_userServer, "QuestionnaireMgr::_initQuestionnaireInfo failed, boList is null");
            return false;
        }

        for (QuestionnaireInfoBO bo : boList)
        {
            QuestionnaireInfo questionnaireInfo = new QuestionnaireInfo(this, bo);
            _m_questionnaireList.add(questionnaireInfo);
        }

        return true;
    }

    /**
     * 初始化玩家记录
     * @return
     */
    private boolean __initPlayerRecord()
    {
        List<QuestionnairePlayerRecordBO> boList = _m_userServer.getBM().getBM(QuestionnairePlayerRecordBO.class).s_findAll();
        if (boList == null)
        {
            USLog.error(_m_userServer, "QuestionnaireMgr::_initPlayerRecord failed, boList is null");
            return false;
        }

        for (QuestionnairePlayerRecordBO bo : boList)
        {
            QuestionnaireInfo questionnaireInfo = lookupByQuestionnaireId(bo.getQuestionnaireId());
            if (questionnaireInfo == null)
            {
                USLog.error(_m_userServer, "QuestionnaireMgr::_initPlayerRecord, questionnaireInfo is null, questionnaireId:{}", bo.getQuestionnaireId());
                continue;
            }

            questionnaireInfo._initPlayerRecord(bo);
        }

        return true;
    }

    public List<QuestionnaireInfo> getAllQuestionnaire()
    {
        return new ArrayList<>(_m_questionnaireList);
    }

    /**
     * 通过问卷实例ID查找问卷信息
     * @param _questionnaireId 问卷实例ID
     * @return
     */
    public QuestionnaireInfo lookupByQuestionnaireId(long _questionnaireId)
    {
        _lock();
        try
        {
            for (QuestionnaireInfo _questionnaireInfo : _m_questionnaireList)
            {
                if (_questionnaireInfo.getQuestionnaireId() == _questionnaireId)
                    return _questionnaireInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }


    /**
     * 通过问卷编号查找问卷信息
     * @param _questionnaireCode 问卷编号
     * @return
     */
    public QuestionnaireInfo lookupByQuestionnaireCode(String _questionnaireCode)
    {
        _lock();
        try
        {
            for (QuestionnaireInfo _questionnaireInfo : _m_questionnaireList)
            {
                if (Objects.equals(_questionnaireInfo.getQuestionnaireCode(), _questionnaireCode))
                    return _questionnaireInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 开启问卷
     * @param _urlLink           问卷链接
     * @param _questionnaireCode 问卷编码
     * @param _rewardList        奖励列表
     * @param _startTimeMs       开始时间
     * @param _closeTimeMs       结束时间
     * @return
     */
    public Result startQuestionnaire(String _urlLink, String _questionnaireCode, String _rewardList, long _startTimeMs, long _closeTimeMs)
    {
        //检查是否已经开启过
        QuestionnaireInfo questionnaireInfo;
        _lock();
        try
        {
            //解析奖励列表
            NPItemListParse itemListParse = new NPItemListParse();
            if (!itemListParse.parseFromString(_rewardList))
            {
                USLog.error(getUserServer(), "QuestionnaireMgr::startQuestionnaire, parse rewardList failed, questionnaireCode:{} rewardList:{}",
                        _questionnaireCode, _rewardList);

                return HttpErr.QUESTIONNAIRE_REWARD_LIST_PARSE_FAILED;
            }

            //检查是否已经开启过，如果没有则直接开启新的问卷活动
            questionnaireInfo = lookupByQuestionnaireCode(_questionnaireCode);
            if (questionnaireInfo == null)
            {
                QuestionnaireInfoBO bo = new QuestionnaireInfoBO();
                bo.setUrlLink(getUserServer().getBM(), _urlLink);
                bo.setQuestionnaireCode(getUserServer().getBM(), _questionnaireCode);
                bo.setStartTimeMs(getUserServer().getBM(), _startTimeMs);
                bo.setCloseTimeMs(getUserServer().getBM(), _closeTimeMs);
                NPCommon_ItemList rewardList = new NPCommon_ItemList();
                rewardList.getItemList().addAll(CommonFunc.costItemListToProto(itemListParse.getItemTypeObjList()));
                bo.setRewardList(getUserServer().getBM(), rewardList.makePackage().array());
                bo.insert(getUserServer().getBM());

                questionnaireInfo = new QuestionnaireInfo(this, bo);
                _m_questionnaireList.add(questionnaireInfo);
            } else
            {
                //如果还没关闭，则不允许再次开启
                if (!questionnaireInfo.isClose())
                {
                    USLog.error(getUserServer(), "QuestionnaireMgr::startQuestionnaire, repeated call questionnaireCode:{}", _questionnaireCode);
                    return HttpErr.QUESTIONNAIRE_ACTIVITY_REPEATED;
                }

                questionnaireInfo.chgQuestionnaireInfo(_urlLink, _questionnaireCode, itemListParse, _startTimeMs, _closeTimeMs);
            }
        } finally
        {
            _unlock();
        }

        getUserServer().getUsUserMgr().broadCastMessage(new GS2GC_007_063_OnQuestionnaireStart(questionnaireInfo.makeBaseProto()));

        return Result.SUCC;
    }

    /**
     * 修改问卷信息
     * @param _questionnaireId   问卷id
     * @param _urlLink           问卷链接
     * @param _questionnaireCode 问卷编码
     * @param _rewardList        奖励列表
     * @param _startTimeMs       开始时间
     * @param _closeTimeMs       结束时间
     * @return
     */
    public Result chgQuestionnaire(long _questionnaireId, String _urlLink, String _questionnaireCode, String _rewardList, long _startTimeMs, long _closeTimeMs)
    {
        //解析奖励列表
        NPItemListParse itemListParse = new NPItemListParse();
        if (!itemListParse.parseFromString(_rewardList))
        {
            USLog.error(getUserServer(), "QuestionnaireMgr::startQuestionnaire, parse rewardList failed, questionnaireCode:{} rewardList:{}",
                    _questionnaireCode, _rewardList);

            return HttpErr.QUESTIONNAIRE_REWARD_LIST_PARSE_FAILED;
        }

        //检查是否已经开启过，且不是关闭状态
        QuestionnaireInfo questionnaireInfo = lookupByQuestionnaireId(_questionnaireId);
        if (questionnaireInfo == null)
            return HttpErr.QUESTIONNAIRE_ACTIVITY_NOT_FOUND;

        //如果已经关闭，不允许修改
        if (questionnaireInfo.isClose())
            return HttpErr.QUESTIONNAIRE_ACTIVITY_CLOSED;

        questionnaireInfo.chgQuestionnaireInfo(_urlLink, _questionnaireCode, itemListParse, _startTimeMs, _closeTimeMs);

        getUserServer().getUsUserMgr().broadCastMessage(new GS2GC_007_063_OnQuestionnaireStart(questionnaireInfo.makeBaseProto()));

        return Result.SUCC;
    }

    /**
     * 关闭问卷
     * @param _questionnaireId   问卷id
     * @return
     */
    public Result closeQuestionnaire(long _questionnaireId)
    {
        //检查是否已经开启过，且不是关闭状态
        QuestionnaireInfo questionnaireInfo = lookupByQuestionnaireId(_questionnaireId);
        if (questionnaireInfo == null)
            return HttpErr.QUESTIONNAIRE_ACTIVITY_NOT_FOUND;

        //如果已经关闭，不允许修改
        if (questionnaireInfo.isClose())
            return HttpErr.QUESTIONNAIRE_ACTIVITY_CLOSED;

        questionnaireInfo.chgQuestionnaireTimeMs(questionnaireInfo.getStartTimeMs(),CommonFunc.getNowTimeMS());

        return Result.SUCC;
    }

    /**
     * 开始tick任务
     */
    public void startTick()
    {
        //启动问卷活动tick
        ALSynTaskManager.getInstance().regTask(new SynTask_QuestionnaireMgrTick(this));
    }

    /**
     * tick检查活动关闭
     */
    public void tick(long _nowTimeMS)
    {
        List<QuestionnaireInfo> needCloseList = new ArrayList<>();

        _lock();
        try
        {
            for (QuestionnaireInfo questionnaireInfo : _m_questionnaireList)
            {
                if (questionnaireInfo.isClose())
                {
                    continue;
                }

                if (questionnaireInfo.needClose(_nowTimeMS))
                {
                    needCloseList.add(questionnaireInfo);
                }
            }
        } finally
        {
            _unlock();
        }

        for (QuestionnaireInfo questionnaireInfo : needCloseList)
        {
            questionnaireInfo.onClose();
            getUserServer().getUsUserMgr().broadCastMessage(new GS2GC_007_064_OnQuestionnaireClose(questionnaireInfo.getQuestionnaireId()));
        }
    }

    /**
     * 添加记录
     * @param _cid
     * @param _activityCode
     */
    public Result addRecord(long _cid, String _activityCode)
    {
        QuestionnaireInfo questionnaireInfo = lookupByQuestionnaireCode(_activityCode);
        if (questionnaireInfo == null)
        {
            USLog.error(getUserServer(), "QuestionnaireMgr::addRecord, questionnaireInfo is null, _activityCode:{} cid:{}", _activityCode, _cid);
            return HttpErr.QUESTIONNAIRE_ACTIVITY_NOT_FOUND;
        }

        return questionnaireInfo.addPlayerRecord(_cid);
    }

    /**
     * 领取奖励
     * @param _userdata        cid
     * @param _questionnaireId 问卷id
     * @param _context
     */
    public Result drawReward(NPUSUserData _userdata, long _questionnaireId, NPPlayerContext _context)
    {
        QuestionnaireInfo questionnaireInfo = lookupByQuestionnaireId(_questionnaireId);
        if (questionnaireInfo == null)
        {
            USLog.error(getUserServer(), "QuestionnaireMgr::addRecord, questionnaireInfo is null, _questionnaireId:{} cid:{}", _questionnaireId, _userdata.getCid());
            return HttpErr.QUESTIONNAIRE_ACTIVITY_NOT_FOUND;
        }

        return questionnaireInfo.drawReward(_userdata, _context);
    }

    /**
     * 构造协议
     * @param _cid
     * @param _questionnaireList
     * @param _recordList
     */
    public void fillProto(long _cid, List<Common_QuestionnaireInfo> _questionnaireList, List<Common_QuestionnaireRewardInfo> _recordList)
    {
        for (QuestionnaireInfo questionnaireInfo : getAllQuestionnaire())
        {
            questionnaireInfo.fillProto(_cid, _questionnaireList, _recordList);
        }
    }

    @Override
    public String toString()
    {
        _lock();
        try{
            StringBuilder sb = new StringBuilder();
            sb.append("QuestionnaireMgr:").append("\n");
            for (QuestionnaireInfo questionnaireInfo : _m_questionnaireList)
            {
                sb.append(questionnaireInfo.toString()).append(",").append("\n");
            }
            return sb.toString();
        }finally
        {
            _unlock();
        }
    }
}

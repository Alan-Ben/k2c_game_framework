package NPUSServer.QuestionnaireMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_QuestionnaireInfo;
import Common.Common_QuestionnaireRewardInfo;
import GS2GC.p007_CommOp.GS2GC_007_065_OnQuestionnaireRewardAdd;
import GS2GC.p007_CommOp.GS2GC_007_067_OnQuestionnaireRewardChg;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Parse.NPItemListParse;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.QuestionnaireInfoBO;
import USDB.Bo.QuestionnairePlayerRecordBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class QuestionnaireInfo
{
    private QuestionnaireMgr _m_mgr;

    private long _m_dbId;
    private String _m_urlLink;
    private String _m_questionnaireCode;
    private long _m_startTimeMs;
    private long _m_closeTimeMs;
    private List<NPCommon_ItemInfo> _m_rewardList;
    private boolean _m_isClose;

    private Map<Long, QuestionnairePlayerRecordInfo> _m_playerRecordMap;
    private MutexAtom _m_mutex;

    public QuestionnaireInfo(QuestionnaireMgr _mgr, QuestionnaireInfoBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_urlLink = _bo.getUrlLink();
        _m_questionnaireCode = _bo.getQuestionnaireCode();
        _m_startTimeMs = _bo.getStartTimeMs();
        _m_closeTimeMs = _bo.getCloseTimeMs();
        _m_rewardList = new ArrayList<>();
        if (_bo.getRewardList()!=null)
        {
            NPCommon_ItemList rewardList = new NPCommon_ItemList();
            rewardList.readPackage(ByteBuffer.wrap(_bo.getRewardList()));
            _m_rewardList = rewardList.getItemList();
        }
        _m_isClose = _bo.getIsClose();

        _m_playerRecordMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public long getStartTimeMs()
    {
        return _m_startTimeMs;
    }

    public long getCloseTimeMs()
    {
        return _m_closeTimeMs;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public QuestionnaireMgr getMgr()
    {
        return _m_mgr;
    }

    /**
     * 是否关闭
     * @return
     */
    public boolean needClose(long _nowTimeMs)
    {
        return _nowTimeMs > _m_closeTimeMs;
    }

    /**
     * 是否关闭
     * @return
     */
    public boolean isClose()
    {
        return _m_isClose;
    }

    /**
     * 获取问卷实例ID
     * @return
     */
    public long getQuestionnaireId()
    {
        return _m_dbId;
    }

    /**
     * 获取问卷实例ID
     * @return
     */
    public String getQuestionnaireCode()
    {
        return _m_questionnaireCode;
    }

    /**
     * 初始化玩家记录
     * @param _bo bo
     */
    protected void _initPlayerRecord(QuestionnairePlayerRecordBO _bo)
    {
        QuestionnairePlayerRecordInfo recordInfo = new QuestionnairePlayerRecordInfo(this, _bo);
        _m_playerRecordMap.put(_bo.getCid(), recordInfo);
    }

    /**
     * 查找玩家记录
     * @param _cid cid
     * @return
     */
    public QuestionnairePlayerRecordInfo lookup(long _cid)
    {
        _lock();
        try{
            return _m_playerRecordMap.get(_cid);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 添加玩家记录
     * @param _cid cid
     */
    public Result addPlayerRecord(long _cid)
    {
        _lock();
        try{
            QuestionnairePlayerRecordInfo recordInfo = lookup(_cid);
            if (recordInfo != null)
            {
                CommLog.error("QuestionnaireInfo::addPlayerRecord, cid:{} already exist", _cid);
                return HttpErr.QUESTIONNAIRE_REWARD_ADD_REPEAT;
            }

            QuestionnairePlayerRecordBO bo = new QuestionnairePlayerRecordBO();
            bo.setCid(_m_mgr.getUserServer().getBM(), _cid);
            bo.setQuestionnaireId(_m_mgr.getUserServer().getBM(), _m_dbId);
            bo.insert(_m_mgr.getUserServer().getBM());

            recordInfo = new QuestionnairePlayerRecordInfo(this, bo);
            _m_playerRecordMap.put(_cid, recordInfo);

            //如果活动已经关闭，检查补发邮件
            if (isClose())
            {
                recordInfo.checkSendMail(_m_rewardList, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE));
                return Result.SUCC;
            }

        }finally
        {
            _unlock();
        }

        NPUSUserData userdata = _m_mgr.getUserServer().getUsUserMgr().lookupCacheUserData(_cid);
        if (userdata != null)
        {
            userdata.sendMsgToGC(new GS2GC_007_065_OnQuestionnaireRewardAdd(_m_dbId));
        }

        return Result.SUCC;
    }

    /**
     * 添加玩家记录
     * @param _userdata
     * @param _context
     */
    public Result drawReward(NPUSUserData _userdata, NPPlayerContext _context)
    {
        QuestionnairePlayerRecordInfo recordInfo = lookup(_userdata.getCid());
        if (recordInfo == null)
            return HttpErr.QUESTIONNAIRE_REWARD_NOT_FOUND;

        _lock();
        try{
            if (recordInfo.hasDraw())
                return HttpErr.QUESTIONNAIRE_REWARD_HAD_DRAW;

            recordInfo.markDraw(_context);
        }finally
        {
            _unlock();
        }

        _userdata.gainItemListP(_m_rewardList, _context);

        _userdata.sendMsgToGC(new GS2GC_007_067_OnQuestionnaireRewardChg(recordInfo.makeProto()));

        return Result.SUCC;
    }

    /**
     * 删除对应问卷下的数据
     */
    public void onClose()
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE);

        _lock();
        try{
            //检查补发邮件
            __checkSendMail(context);

            _m_isClose = true;

            //保存数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("is_close", _m_isClose ? 1 : 0);
            getMgr().getUserServer().getBM().getBM(QuestionnaireInfoBO.class).update("id", _m_dbId, updateValue);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查补发邮件
     * @param _context
     */
    private void __checkSendMail(NPPlayerContext _context)
    {
        _lock();
        try{
            for (QuestionnairePlayerRecordInfo recordInfo : _m_playerRecordMap.values())
            {
                recordInfo.checkSendMail(_m_rewardList, _context);
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 重开处理
     * @param _urlLink
     * @param _questionnaireCode
     * @param _itemListParse
     * @param _startTimeMs
     * @param _closeTimeMs
     */
    public void chgQuestionnaireInfo(String _urlLink, String _questionnaireCode, NPItemListParse _itemListParse, long _startTimeMs, long _closeTimeMs)
    {
        _lock();
        try{
            _m_urlLink = _urlLink;
            _m_questionnaireCode = _questionnaireCode;
            _m_startTimeMs = _startTimeMs;
            _m_closeTimeMs = _closeTimeMs;
            NPCommon_ItemList rewardList = new NPCommon_ItemList();
            rewardList.getItemList().addAll(CommonFunc.costItemListToProto(_itemListParse.getItemTypeObjList()));
            _m_rewardList = rewardList.getItemList();
            _m_isClose = false;

            //保存数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("url_link", _m_urlLink);
            updateValue.addValueObj("questionnaire_code", _m_questionnaireCode);
            updateValue.addValueObj("start_time_ms", _m_startTimeMs);
            updateValue.addValueObj("close_time_ms", _m_closeTimeMs);
            updateValue.addValueObj("reward_list", rewardList.makePackage().array());
            updateValue.addValueObj("is_close", _m_isClose ? 1 : 0);
            getMgr().getUserServer().getBM().getBM(QuestionnaireInfoBO.class).update("id", _m_dbId, updateValue);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 修改时间
     */
    public void chgQuestionnaireTimeMs(long _startTimeMs, long _closeTimeMs)
    {
        _lock();
        try
        {
            _m_startTimeMs = _startTimeMs;
            _m_closeTimeMs = _closeTimeMs;

            //保存数据库
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("start_time_ms", _m_startTimeMs);
            updateValue.addValueObj("close_time_ms", _m_closeTimeMs);
            getMgr().getUserServer().getBM().getBM(QuestionnaireInfoBO.class).update("id", _m_dbId, updateValue);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造基础协议
     * @return
     */
    public Common_QuestionnaireInfo makeBaseProto()
    {
        Common_QuestionnaireInfo info = new Common_QuestionnaireInfo();
        info.setQuestionnaireId(_m_dbId);
        info.setUrlLink(_m_urlLink);
        info.setQuestionnaireCode(_m_questionnaireCode);
        info.setStartTimeMs(_m_startTimeMs);
        info.setEndTimeMs(_m_closeTimeMs);
        info.getRewardList().addAll(_m_rewardList);
        return info;
    }

    /**
     * 填充协议
     * @param _cid
     * @param _questionnaireList
     * @param _recordList
     */
    public void fillProto(long _cid, List<Common_QuestionnaireInfo> _questionnaireList, List<Common_QuestionnaireRewardInfo> _recordList)
    {
        Common_QuestionnaireInfo info = makeBaseProto();
        _questionnaireList.add(info);

        QuestionnairePlayerRecordInfo recordInfo = lookup(_cid);
        if (recordInfo != null)
            _recordList.add(recordInfo.makeProto());
    }

    @Override
    public String toString()
    {
        return "QuestionnaireInfo{" +
                "_m_dbId=" + _m_dbId +
                ", _m_urlLink='" + _m_urlLink + '\'' +
                ", _m_questionnaireCode='" + _m_questionnaireCode + '\'' +
                ", _m_startTimeMs=" + _m_startTimeMs +
                ", _m_closeTimeMs=" + _m_closeTimeMs +
                ", _m_rewardList=" + CommonFunc.protoItemList2String(_m_rewardList) +
                ", _m_isClose=" + _m_isClose +
                '}';
    }
}

package NPUSServer.Arena;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ArenaObj.Arena_CelebrityRankInfo;
import NPCommon.DB.BM.BM;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ArenaCelebrityRankBO;

import java.util.ArrayList;
import java.util.List;

public class ArenaCelebrityRankMgr
{
    private NPUserServer _m_server;
    private List<ArenaCelebrityRankItem> _m_rankList;
    private MutexAtom _m_mutex;

    public ArenaCelebrityRankMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_rankList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public boolean s_init()
    {
        List<ArenaCelebrityRankBO> boList = _m_server.getBM().getBM(ArenaCelebrityRankBO.class).s_findAll();
        if (boList == null)
            return false;

        for (ArenaCelebrityRankBO bo : boList)
        {
            ArenaCelebrityRankItem info = new ArenaCelebrityRankItem(this, bo);
            _m_rankList.add(info);
        }

        _m_rankList.sort((o1, o2) -> Long.compare(o2.getDbId(), o1.getDbId()));
        return true;
    }

    /**
     * 添加名人榜
     * @param _attacker
     * @param _defenderCid
     * @param _isBot
     * @param _botName
     * @param _defeatHeroNum
     * @param _isSelectAttack
     * @param _timeMs
     */
    public void addRankItem(NPUSUserData _attacker, long _defenderCid, boolean _isBot, String _botName, int _defeatHeroNum, boolean _isSelectAttack, long _timeMs)
    {
        if (_isBot)
        {
            BM bmObj = _m_server.getBM();

            ArenaCelebrityRankBO bo = new ArenaCelebrityRankBO();
            bo.setAttackerCid(bmObj,_attacker.getCid());
            bo.setAttackerName(bmObj, _attacker.getPlayerComponent().getName());
            bo.setDefenderName(bmObj, _botName);
            bo.setDefeatHeroNum(bmObj, _defeatHeroNum);
            bo.setIsSelectAttack(bmObj, _isSelectAttack);
            bo.setTimestamp(bmObj, _timeMs);
            bo.insert(bmObj);

            _addRankItem(bo);
        }else
        {
            _m_server.getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _defenderCid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
            {
                @Override
                public void handle(Boolean _isSuc, PlayerInfo_IconShow _data)
                {
                    if (!_isSuc)
                    {
                        USLog.error(_m_server, "addRankItem get defender info fail, defenderCid:{}", _defenderCid);
                        return;
                    }

                    BM bmObj = _m_server.getBM();

                    ArenaCelebrityRankBO bo = new ArenaCelebrityRankBO();
                    bo.setAttackerCid(bmObj,_attacker.getCid());
                    bo.setAttackerName(bmObj, _attacker.getPlayerComponent().getName());
                    bo.setDefenderName(bmObj, _data.getPlayerName());
                    bo.setDefeatHeroNum(bmObj, _defeatHeroNum);
                    bo.setIsSelectAttack(bmObj, _isSelectAttack);
                    bo.setTimestamp(bmObj, _timeMs);
                    bo.insert(bmObj);

                    _addRankItem(bo);
                }
            });
        }
    }

    /**
     * 添加名人榜
     * @param _bo
     */
    private void _addRankItem(ArenaCelebrityRankBO _bo)
    {
        _lock();
        try{
            ArenaCelebrityRankItem item = new ArenaCelebrityRankItem(this, _bo);
            _m_rankList.add(item);

            //检查移除过期数据
            _checkRemoveExpiredItem();
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查超过数量的数据
     */
    private void _checkRemoveExpiredItem()
    {
        _lock();
        try{
            while (_m_rankList.size() > RefGeneral.Ref().arena_celebrity_rank_limit_num)
            {
                ArenaCelebrityRankItem item = _m_rankList.remove(0);
                
                if (item != null)
                    item.discard();
            }
        }finally
        {
            _unlock();
        }
    }


    /**
     * 获取名人榜
     * @param _startDbId
     * @return
     */
    public List<Arena_CelebrityRankInfo> getRankList(long _startDbId)
    {
        _lock();
        try{
            List<Arena_CelebrityRankInfo> list = new ArrayList<>();
            for (ArenaCelebrityRankItem item : _m_rankList)
            {
                if (item.getDbId() <= _startDbId)
                    break;

                list.add(item.makeProto());
            }
            return list;
        }finally
        {
            _unlock();
        }
    }



}

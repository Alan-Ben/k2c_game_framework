package NPUSServer.NPUSUserMgr.UserComp.ArenaComp;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_ChgPlayerRecord;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.ENCounterDealType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerArenaFightReportBO;
import USDB.Bo.PlayerOfflineRewardBO;

public class ArenaFunc
{
    /**
     * 添加反击消息
     * @param _cid
     * @param _opponent
     * @param _defeatNum
     * @param _deductInfluence
     * @param _nowTimeMS
     */
    public static void addFightBackMsg(long _cid, NPUSUserData _opponent, int _defeatNum, int _deductInfluence, long _nowTimeMS)
    {
        _addFightReportMsg(_cid, _opponent, _defeatNum, _deductInfluence, _nowTimeMS, true);
    }

    /**
     * 添加战报消息
     * @param _cid
     * @param _opponent
     * @param _defeatNum
     * @param _deductInfluence
     * @param _nowTimeMS
     */
    public static void addFightReportMsg(long _cid, NPUSUserData _opponent, int _defeatNum, int _deductInfluence, long _nowTimeMS)
    {
        _addFightReportMsg(_cid, _opponent, _defeatNum, _deductInfluence, _nowTimeMS, false);
    }

    /**
     * 添加战报消息
     * @param _cid
     * @param _opponent
     * @param _defeatNum
     * @param _deductInfluence
     * @param _nowTimeMS
     * @param _fromCelebrityRank 是否来自名人榜
     */
    public static void _addFightReportMsg(long _cid, NPUSUserData _opponent, int _defeatNum, int _deductInfluence, long _nowTimeMS, boolean _fromCelebrityRank)
    {
        int serverId = CommonFunc.parseServerTypeIdFromCid(_cid);
        if (serverId == _opponent.getUSServer().getServerTypeId())
        {
            BM bmObj = _opponent.getUSServer().getBM();

            PlayerArenaFightReportBO bo = new PlayerArenaFightReportBO();
            bo.setCid(bmObj, _cid);
            bo.setOpponentCid(bmObj, _opponent.getCid());
            bo.setDefeatHeroNum(bmObj, _defeatNum);
            bo.setDeductInfluence(bmObj, _deductInfluence);
            bo.setTimestamp(bmObj, _nowTimeMS);
            bo.setFromCelebrityRand(bmObj, _fromCelebrityRank);
            bo.insert(bmObj);

            NPUSUserData userData = _opponent.getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
            if (userData != null)
            {
                userData.safeCall(() -> userData.getArenaComponent().getFightReportMgr().addFightReport(bo));
            }
        } else
        {
            //TODO 跨服战报
        }
    }

    /**
     * 添加反击消息
     * @param _cid
     * @param _defeatNum
     */
    public static void recordBeenDefeatHeroNum(long _cid, int _defeatNum, NPUserServer _server)
    {
        int serverId = CommonFunc.parseServerTypeIdFromCid(_cid);
        if (serverId == _server.getServerTypeId())
        {
            NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
            if (userData != null)
            {
                userData.safeCall(() ->
                {
                    userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ARENA_BEEN_DEFEAT_HERO_COUNT, _defeatNum,
                            NPPlayerContext.createNew(ENPGameEvent.ARENA_ROUND_ATTACK));
                });
            } else
            {
                BM bmObj = _server.getBM();

                Offline_ChgPlayerRecord offlineData = new Offline_ChgPlayerRecord();
                offlineData.setType(ENPPlayerRecordParam.ARENA_BEEN_DEFEAT_HERO_COUNT);
                offlineData.setDealType(ENCounterDealType.ADD);
                offlineData.setNum(_defeatNum);

                PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
                bo.setCid(bmObj, _cid);
                bo.setRewardType(bmObj, EOfflineRewardEnum.CHG_PLAYER_RECORD.ordinal());
                bo.setOfflineData(bmObj, CommonFunc.ByteBfferToBytes(offlineData.makePackage()));
                bo.insert(bmObj);
            }
        } else
        {
            //TODO ：跨服发送消息让对应被击败服务器进行相关处理
        }
    }
}

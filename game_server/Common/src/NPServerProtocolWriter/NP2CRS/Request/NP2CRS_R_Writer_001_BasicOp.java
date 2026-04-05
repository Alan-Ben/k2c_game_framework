package NPServerProtocolWriter.NP2CRS.Request;


import Common.ServerObj.ServerObj_RankObjInfo;
import NP2CRS_R.p001_CrossRankOp.*;

import java.util.List;

public class NP2CRS_R_Writer_001_BasicOp
{
    public static NP2CRS_R_001_001_RegCrossInstance make_001_RegCrossInstance(long _crossInstanceId, long _joinerId)
    {
        NP2CRS_R_001_001_RegCrossInstance protocol = new NP2CRS_R_001_001_RegCrossInstance();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setJoinerId(_joinerId);

        return protocol;
    }

    public static NP2CRS_R_001_002_UnregCrossInstance make_002_UnregCrossInstance(long _crossInstanceId, long _joinerId)
    {
        NP2CRS_R_001_002_UnregCrossInstance protocol = new NP2CRS_R_001_002_UnregCrossInstance();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setJoinerId(_joinerId);

        return protocol;
    }

    public static NP2CRS_R_001_003_RegCrossRank make_003_RegCrossRank(long _crossInstanceId, long _rankId, long _joinerId)
    {
        NP2CRS_R_001_003_RegCrossRank protocol = new NP2CRS_R_001_003_RegCrossRank();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setRankId(_rankId);
        protocol.setJoinerId(_joinerId);

        return protocol;
    }

    public static NP2CRS_R_001_004_UnregCrossRank make_004_UnregCrossRank(long _crossInstanceId, long _rankId, long _joinerId)
    {
        NP2CRS_R_001_004_UnregCrossRank protocol = new NP2CRS_R_001_004_UnregCrossRank();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setRankId(_rankId);
        protocol.setJoinerId(_joinerId);

        return protocol;
    }

    public static NP2CRS_R_001_005_RequestCrossInstance make_005_RequestCrossInstance()
    {
        NP2CRS_R_001_005_RequestCrossInstance protocol = new NP2CRS_R_001_005_RequestCrossInstance();

        return protocol;
    }

    public static NP2CRS_R_001_006_DiscardCrossInstance make_006_DiscardCrossInstance(long _crossInstanceId)
    {
        NP2CRS_R_001_006_DiscardCrossInstance protocol = new NP2CRS_R_001_006_DiscardCrossInstance();
        protocol.setCrossInstanceId(_crossInstanceId);

        return protocol;
    }

    public static NP2CRS_R_001_007_ReqCrossRankServerInfo make_007_ReqCrossRankServerInfo()
    {
        return new NP2CRS_R_001_007_ReqCrossRankServerInfo();
    }

    public static NP2CRS_R_001_008_RegUploadRankData make_008_RegUploadRankData(long _crossInstanceId, long _rankId, List<ServerObj_RankObjInfo> _rankList)
    {
        NP2CRS_R_001_008_RegUploadRankData proto = new NP2CRS_R_001_008_RegUploadRankData();
        proto.setCrossInstanceId(_crossInstanceId);
        proto.setRankId(_rankId);
        proto.getRankList().addAll(_rankList);
        return proto;
    }

    public static NP2CRS_R_001_010_SetCrossRankScore make_010_SetCrossRankScore(long _crossInstanceId, long _rankId, long _objId, long _scoreSourceId, long _score, long _updatedMs)
    {
        NP2CRS_R_001_010_SetCrossRankScore protocol = new NP2CRS_R_001_010_SetCrossRankScore();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setRankId(_rankId);
        protocol.setObjId(_objId);
        protocol.setScoreSourceId(_scoreSourceId);
        protocol.setScore(_score);
        protocol.setUpdateTimeMs(_updatedMs);
        return protocol;
    }

    public static NP2CRS_R_001_011_SetCrossRankSubScore make_011_SetCrossRankSubScore(long _crossInstanceId, long _rankId, long _objId, long _objSubId, long _scoreSourceId, long _score, long _updatedMs)
    {
        NP2CRS_R_001_011_SetCrossRankSubScore protocol = new NP2CRS_R_001_011_SetCrossRankSubScore();
        protocol.setCrossInstanceId(_crossInstanceId);
        protocol.setRankId(_rankId);
        protocol.setObjId(_objId);
        protocol.setSubObjId(_objSubId);
        protocol.setScoreSourceId(_scoreSourceId);
        protocol.setScore(_score);
        protocol.setUpdateTimeMs(_updatedMs);
        return protocol;
    }

    public static NP2CRS_R_001_020_GetCrossRankListSize make_020_GetCrossRankListSize(long _crossInstanceId, long _rankId)
    {
        return new NP2CRS_R_001_020_GetCrossRankListSize(_crossInstanceId, _rankId);
    }

    public static NP2CRS_R_001_021_GetCrossRankBaseList make_021_GetCrossRankBaseList(long _crossInstanceId, long _rankId, int _limit)
    {
        return new NP2CRS_R_001_021_GetCrossRankBaseList(_crossInstanceId, _rankId, _limit);
    }

    public static NP2CRS_R_001_022_GetCrossRankBaseByRank make_022_GetCrossRankBaseByRank(long _crossInstanceId, long _rankId, int _rank)
    {
        return new NP2CRS_R_001_022_GetCrossRankBaseByRank(_crossInstanceId, _rankId, _rank);
    }

    public static NP2CRS_R_001_023_GetCrossRankBaseByKey make_023_GetCrossRankBaseByKey(long _crossInstanceId, long _rankId, long _key)
    {
        return new NP2CRS_R_001_023_GetCrossRankBaseByKey(_crossInstanceId, _rankId, _key);
    }

    public static NP2CRS_R_001_024_DumpCrossRankListByUs make_024_DumpCrossRankListByUs(long _crossInstanceId, long _rankId, int _usId, int _limitRank)
    {
        return new NP2CRS_R_001_024_DumpCrossRankListByUs(_crossInstanceId, _rankId, _limitRank, _usId);
    }

    public static NP2CRS_R_001_025_GetCrossRankBaseByKey2 make_025_GetCrossRankBaseByKey2(long _crossInstanceId, long _rankId, long _key, long _subKey)
    {
        return new NP2CRS_R_001_025_GetCrossRankBaseByKey2(_crossInstanceId, _rankId, _key, _subKey);
    }

    public static NP2CRS_R_001_026_GetCrossRankBaseSubListByKey make_026_GetCrossRankBaseSubListByKey(long _crossInstanceId, long _rankId, long _key)
    {
        return new NP2CRS_R_001_026_GetCrossRankBaseSubListByKey(_crossInstanceId, _rankId, _key);
    }
}

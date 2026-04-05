package NPCrossRankServer.NPGeneralListener.RequestDispather.Write;

import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import NP2CRS_RB.p001_CrossRankOp.*;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;

import java.util.List;

public class NPWriter_001_CrossRankOp
{
    public static NP2CRS_RB_001_001_RegCrossInstance make_001_RegCrossInstance()
    {
        NP2CRS_RB_001_001_RegCrossInstance protocol = new NP2CRS_RB_001_001_RegCrossInstance();

        return protocol;
    }

    public static NP2CRS_RB_001_002_UnregCrossInstance make_002_UnregCrossInstance()
    {
        NP2CRS_RB_001_002_UnregCrossInstance protocol = new NP2CRS_RB_001_002_UnregCrossInstance();

        return protocol;
    }
    
    public static NP2CRS_RB_001_003_RegCrossRank make_003_RegCrossRank(NPCrossRankRankList _rankList)
    {
        NP2CRS_RB_001_003_RegCrossRank protocol = new NP2CRS_RB_001_003_RegCrossRank();
        protocol.setCrossRankInstanceId(_rankList.getInstanceId());

        return protocol;
    }

    public static NP2CRS_RB_001_004_UnregCrossRank make_004_UnregCrossRank()
    {
        NP2CRS_RB_001_004_UnregCrossRank protocol = new NP2CRS_RB_001_004_UnregCrossRank();

        return protocol;
    }
    
    public static NP2CRS_RB_001_005_RequestCrossInstance make_005_RequestCrossInstance(long _crossInstanceId)
    {
        NP2CRS_RB_001_005_RequestCrossInstance protocol = new NP2CRS_RB_001_005_RequestCrossInstance();
        protocol.setCrossInstanceId(_crossInstanceId);

        return protocol;
    }

    public static NP2CRS_RB_001_006_DiscardCrossInstance make_006_DiscardCrossInstance()
    {
        NP2CRS_RB_001_006_DiscardCrossInstance protocol = new NP2CRS_RB_001_006_DiscardCrossInstance();

        return protocol;
    }

    public static NP2CRS_RB_001_007_RetCrossRankServerInfo make_007_RetCrossRankServerInfo(int _instanceCount)
    {
        NP2CRS_RB_001_007_RetCrossRankServerInfo protocol = new NP2CRS_RB_001_007_RetCrossRankServerInfo();
        protocol.setHadHandleInstanceNum(_instanceCount);
        //暂时还没有上限定义，所以先设置为最大值
        protocol.setHandleInstanceLimit(Integer.MAX_VALUE);
        return protocol;
    }
    
    public static NP2CRS_RB_001_008_RegUploadRankData make_008_RegUploadRankData()
    {
        return new NP2CRS_RB_001_008_RegUploadRankData();
    }

    public static NP2CRS_RB_001_010_SetCrossRankScore make_010_SetCrossRankScore()
    {
        NP2CRS_RB_001_010_SetCrossRankScore protocol = new NP2CRS_RB_001_010_SetCrossRankScore();

        return protocol;
    }

    public static NP2CRS_RB_001_011_SetCrossRankSubScore make_011_SetCrossRankSubScore()
    {
        NP2CRS_RB_001_011_SetCrossRankSubScore protocol = new NP2CRS_RB_001_011_SetCrossRankSubScore();

        return protocol;
    }

    public static NP2CRS_RB_001_020_GetCrossRankListSize make_020_GetCrossRankListSize(int _rankSize)
    {
        NP2CRS_RB_001_020_GetCrossRankListSize protocol = new NP2CRS_RB_001_020_GetCrossRankListSize();
        protocol.setRankSize(_rankSize);
        return protocol;
    }

    public static NP2CRS_RB_001_021_GetCrossRankBaseList make_021_GetCrossRankBaseList(List<Rank_BaseItem> rankBaseList)
    {
        NP2CRS_RB_001_021_GetCrossRankBaseList protocol = new NP2CRS_RB_001_021_GetCrossRankBaseList();
        protocol.getRankItemList().addAll(rankBaseList);
        return protocol;
    }

    public static NP2CRS_RB_001_022_GetCrossRankBaseByRank make_022_GetCrossRankBaseByRank(Rank_BaseItem _rankItem)
    {
        NP2CRS_RB_001_022_GetCrossRankBaseByRank protocol = new NP2CRS_RB_001_022_GetCrossRankBaseByRank();
        if (_rankItem != null)
            protocol.setRankItem(_rankItem);
        return protocol;
    }

    public static NP2CRS_RB_001_023_GetCrossRankBaseByKey make_023_GetCrossRankBaseByKey(Rank_BaseItem _rankItem)
    {
        NP2CRS_RB_001_023_GetCrossRankBaseByKey protocol = new NP2CRS_RB_001_023_GetCrossRankBaseByKey();
        if (_rankItem != null)
            protocol.setRankItem(_rankItem);
        return protocol;
    }

    public static NP2CRS_RB_001_024_DumpCrossRankListByUs make_024_GetCrossRankBaseListByUs(List<Rank_ItemDump> rankBaseList)
    {
        NP2CRS_RB_001_024_DumpCrossRankListByUs protocol = new NP2CRS_RB_001_024_DumpCrossRankListByUs();
        protocol.getRankItemList().addAll(rankBaseList);
        return protocol;
    }

    public static NP2CRS_RB_001_025_GetCrossRankBaseByKey2 make_025_GetCrossRankBaseByKey2(Rank_BaseItem _rankItem)
    {
    	NP2CRS_RB_001_025_GetCrossRankBaseByKey2 protocol = new NP2CRS_RB_001_025_GetCrossRankBaseByKey2();
        if (_rankItem != null)
            protocol.setRankItem(_rankItem);
        return protocol;
    }

    public static NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey make_026_GetCrossRankBaseSubListByKey(List<Rank_BaseSubItem> _list)
    {
    	NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey protocol = new NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey();
    	protocol.getSubList().addAll(_list);
        return protocol;
    }
}

package NPCrossRankServer.GeneralV;

import NPCrossRankServer.NPCrossRankServer;

public class CrsID
{
    /*****
     * 用自增id生成全球US唯一id
     * @param _autoId
     * @return
     */
    private static long __makeCrsId(long _autoId)
    {
        //autoId+2位服务器id
        return _autoId * 100 + NPCrossRankServer.getInstance().getServerTypeId();
    }
    
    /****************
     * 跨服排行榜实例ID
     * @return
     */
    public static long makeCrossInstanceId()
    {
        return __makeCrsId(GeneralVMgr.getInstance().makeNewId(EGeneralVType.CROSS_INSTANCE));
    }
}

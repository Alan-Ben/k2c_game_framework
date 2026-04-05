package NPUSServer.GeneralV;

import NPCommon.Enum.EUsParam;
import NPUSServer.NPUserServer;

public class UsID
{
    /*****
     * 用自增id生成全球US唯一id
     * @param _autoId
     * @return
     */
    public static long makeUsId(NPUserServer _server, long _autoId)
    {
        //autoId+2位区域id+5位服务器id 0100001
        return (_autoId * 100 + _server.getServerAreaId()) * 100000 + _server.getServerTypeId();
    }
    public static long makeUsId(int _areaId, int _usId, long _autoId)
    {
        //autoId+2位区域id+5位服务器id 0100001
        return (_autoId * 100 + _areaId) * 100000 + _usId;
    }

    //可以直接使用玩家自身bo自增实例ID即可
//    /*****
//     * 生成邮件id
//     * @return
//     */
//    public static long makeMailId(NPUserServer _server)
//    {
//        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.MAIL_ID));
//    }

    /*****
     * 生成Cid
     * @return
     */
    public static long makeCid(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.CID));
    }

    /*****
     * 生成工会id
     * @return
     */
    public static long makeGuildId(NPUserServer _server)
    {
    	//兼容已存在规则，不使用general_v数据
    	long guidIncId = _server.getUSParams().incParam(EUsParam.GUILD_ID);
    	
        return makeUsId(_server, guidIncId);
    }

    /*****
     * 生成子嗣实例id
     * @return
     */
    public static long makeChildId(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.CHILD_ID));
    }
    /*****
     * 生成成年子嗣实例id
     * @return
     */
    public static long makeAdultId(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.ADULT_ID));
    }

    /******
     * 通用宝箱实例ID
     * @return
     */
    public static long makeCommBoxInstanceId(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.COMM_BOX_ID));
    }

    /*****
     * 生成宴会id
     * @return
     */
    public static long makeDinnerId(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.DINNER_ID));
    }

    /*****
     * 生成火星矿产id
     * @return
     */
    public static long makeMarsMineId(NPUserServer _server)
    {
        return makeUsId(_server, _server.getGeneralVMgr().makeNewId(EGeneralVType.MARS_MINE_ID));
    }
}

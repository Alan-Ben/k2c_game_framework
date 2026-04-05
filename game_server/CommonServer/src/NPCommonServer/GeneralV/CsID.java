package NPCommonServer.GeneralV;

import NPEnum.ENPCommonGeneralEnum;

public class CsID
{
    /*****
     * 用自增id生成全球US唯一id
     * @param _autoId
     * @return
     */
    private static long __makeCsId(long _autoId, int _serverType, long _serverTypeId)
    {
        //autoId+2位发起服务器Type+5位发起服务器TypeId
        return (_autoId * 100 + _serverType) * 100000 + _serverTypeId;
    }

    public static long makeCsId(ENPCommonGeneralEnum _enum, int _serverType, long _serverTypeId)
    {
        return __makeCsId(GeneralVMgr.getInstance().makeNewId(_enum), _serverType, _serverTypeId);
    }
}

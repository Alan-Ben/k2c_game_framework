package NPHttpServer.Http.Entity;

import ALBasicCommon.ALSerializeMaker;
import Common.ServerObj.ServerObj_PHPParamList;

/**
 * 后台平台参数列表
 * 
 */
public class NPEntityPHPParamList
{
    private ServerObj_PHPParamList _m_objPHPParamListObj;

    public NPEntityPHPParamList()
    {
    	_m_objPHPParamListObj = new ServerObj_PHPParamList();
    }

    public ServerObj_PHPParamList getPHPParamListObj() {return _m_objPHPParamListObj;}
    
    //用于标记最新的的数据序列号
    private static long _g_serial;
    static public synchronized long buildDataSerial()
    {
    	_g_serial = ALSerializeMaker.makeNewSerialize();
    	return _g_serial;
    }
    static public long getDataSerial()
    {
    	return _g_serial;
    }
}

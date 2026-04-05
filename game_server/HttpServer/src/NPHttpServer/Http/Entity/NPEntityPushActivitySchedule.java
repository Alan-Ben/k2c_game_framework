package NPHttpServer.Http.Entity;

/**
 * 后台推送活动排期
 * @author mj
 *
 */
public class NPEntityPushActivitySchedule
{
	//后台操作序列号，用于处理完成后返回后台，用于后台数据变更
	private long _m_lPHPOpSerial;
	//活动排期文件名称
    private String _m_sFileName;
   
    public NPEntityPushActivitySchedule()
    {
    	this._m_lPHPOpSerial = 0;
        this._m_sFileName = "";
    }
    
    public long getPHPOpSerial()
    {
    	return _m_lPHPOpSerial;
    }
    public void setPHPOpSerial(long _phpOpSerial)
    {
    	_m_lPHPOpSerial = _phpOpSerial;
    }

    public String getFileName()
    {
        return _m_sFileName;
    }
    public void setFileName(String _fileName)
    {
    	_m_sFileName = _fileName;
    }
}

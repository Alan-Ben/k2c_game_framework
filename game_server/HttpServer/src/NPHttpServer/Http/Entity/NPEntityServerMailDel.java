package NPHttpServer.Http.Entity;

import java.util.ArrayList;

/**
 * @description: 移除全服邮件
 */
public class NPEntityServerMailDel
{
    /**
     * 服务器TypeID列表
     */
    private ArrayList<Integer> _m_alUsTypeIdList;
    /**
     * 后台邮件id
     */
    private long _m_lPhpMailId;

    public NPEntityServerMailDel()
    {
        this._m_alUsTypeIdList = new ArrayList<>();
    }

    public ArrayList<Integer> getUsTypeIdList()
    {
        return _m_alUsTypeIdList;
    }
    
    public long getPHPMailId()
    {
    	return _m_lPhpMailId;
    }

    /**
     * 增加服务器id到列表
     * @param _usTypeId US服务器id
     */
    public void addUsTypeId(int _usTypeId)
    {
    	_m_alUsTypeIdList.add(_usTypeId);
    }
    
    /**
     * 需要召回的全服邮件ID（平台赋予的邮件ID）
     * @param _phpMailId
     */
    public void setPHPMailId(long _phpMailId)
    {
    	_m_lPhpMailId = _phpMailId;
    }
}

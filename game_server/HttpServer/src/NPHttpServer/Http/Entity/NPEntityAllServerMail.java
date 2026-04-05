package NPHttpServer.Http.Entity;

import java.util.ArrayList;

/**
 * @description: 后台邮件
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityAllServerMail
{
    /**
     * 服务器TypeID列表
     */
    private ArrayList<Integer> usTypeIdList;
    /**
     * 邮件
     */
    private NPEntityServerMail platFromMail;

    public NPEntityAllServerMail()
    {
        this.usTypeIdList = new ArrayList<>();
        this.platFromMail = new NPEntityServerMail();
    }

    public ArrayList<Integer> getUsTypeIdList()
    {
        return usTypeIdList;
    }

    public NPEntityServerMail getPlatFromMail()
    {
        return platFromMail;
    }

    /**
     * 增加服务器id到列表
     * @param _usTypeId US服务器id
     */
    public void addUsTypeId(int _usTypeId)
    {
        usTypeIdList.add(_usTypeId);
    }
}

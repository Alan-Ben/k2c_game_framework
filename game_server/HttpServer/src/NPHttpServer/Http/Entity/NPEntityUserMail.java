package NPHttpServer.Http.Entity;

import java.util.ArrayList;

/**
 * @description: 后台邮件
 * @author: mark
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityUserMail
{
    /*********************************
     * 说明：NP项目只支持针对一个玩家发送邮件，但是运营该接口需要提供批量玩家数据（支持已上线项目）
     * 所以特殊处理，增加 _m_lCid 字段，用于支持该特殊场景
     *
     */
    private long _m_lCid;
    /**
     * 服务器TypeID列表
     */
    private ArrayList<Long> cidList;
    /**
     * 邮件
     */
    private NPEntityServerMail platFromMail;

    public NPEntityUserMail()
    {
        this.platFromMail = new NPEntityServerMail();
        cidList = new ArrayList<>();
    }

    public ArrayList<Long> getCidList()
    {
        return cidList;
    }

    public NPEntityServerMail getPlatFromMail()
    {
        return platFromMail;
    }

    public void addCid(long _cid)
    {
        _m_lCid = _cid;

        cidList.add(_cid);
    }

    public long getCid()
    {
        return _m_lCid;
    }
}

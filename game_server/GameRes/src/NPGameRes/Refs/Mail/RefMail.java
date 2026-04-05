package NPGameRes.Refs.Mail;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;

@RefTable(tableName = "mail")
public class RefMail extends RefBase
{
    private static RefTableContainer<RefMail> _g_mgr = new RefTableContainer<RefMail>();

    public static RefTableContainer<RefMail> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMail> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMail>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMail newRef = (RefMail) _newRef;
        id = newRef.id;
        senderId = newRef.senderId;
        typeId = newRef.typeId;
        title = newRef.title;
        content = newRef.content;
        attach_items = newRef.attach_items;
        s_read_effect = newRef.s_read_effect;
        s_del_effect = newRef.s_del_effect;
        expired_secs_attach_items = newRef.expired_secs_attach_items;
        expired_secs = newRef.expired_secs;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public int senderId;//发送者id
    public long typeId;//类型id RefMailType
    public String title;//邮件标题
    public String content;//邮件文本内容

    public ArrayList<NPCommonCostItem> attach_items;//默认跟随的附件内容

    public NPPlayerEffectListParse s_read_effect;//读取邮件时执行的效果
    public NPPlayerEffectListParse s_del_effect;//删除邮件时执行的效果

    public int expired_secs_attach_items;//未领取奖励时的有效期（秒）过期时间
    public int expired_secs;//领取奖励后的有效期（秒）过期时间
}

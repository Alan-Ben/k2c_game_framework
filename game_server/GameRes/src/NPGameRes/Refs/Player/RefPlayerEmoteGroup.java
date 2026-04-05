package NPGameRes.Refs.Player;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPTimeAddType;
import NPEnum.EQuality;

/*********************
 * 段位信息表
 *
 * @author jeb
 *
 */
@RefTable(tableName = "chat_emote_group")
public class RefPlayerEmoteGroup extends _ARefExpiredItem
{
    private static RefTableContainer<RefPlayerEmoteGroup> _g_mgr = new RefTableContainer<RefPlayerEmoteGroup>();

    public static RefTableContainer<RefPlayerEmoteGroup> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerEmoteGroup> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerEmoteGroup>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerEmoteGroup newRef = (RefPlayerEmoteGroup) _newRef;
        id = newRef.id;
        quality = newRef.quality;
        add_type = newRef.add_type;
        global_res_id = newRef.global_res_id;
        global_auto_enable = newRef.global_auto_enable;
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
    ////////////////////////

    public long id;
    //品质
    public EQuality quality;
    //重复获得的时候，时间的叠加类型
    public ENPTimeAddType add_type;
    //使用本头像需要拥有的全局资源id
    public long global_res_id;
    //全局资源匹配的时候是否自动拥有
    public boolean global_auto_enable;

    @Override
    public ENPTimeAddType getAddType()
    {
        return add_type;
    }
}

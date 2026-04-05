package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import WCGCommon.Enum.NPEnum.EWCGBulletType;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "bullet")
public class RefBullet extends RefBase
{
    private static RefTableContainer<RefBullet> _g_mgr = new RefTableContainer<RefBullet>();

    public static RefTableContainer<RefBullet> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBullet> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBullet>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBullet newRef = (RefBullet) _newRef;
        id = newRef.id;
        sfx_id = newRef.sfx_id;
        hit_sfx_id = newRef.hit_sfx_id;
        speed = newRef.speed;
        width = newRef.width;
        bullet_type = newRef.bullet_type;
        bullet_length = newRef.bullet_length;
        range_effect_list = newRef.range_effect_list;
        end_condition = newRef.end_condition;
        max_target_quantity = newRef.max_target_quantity;
        active_effect_delay_time = newRef.active_effect_delay_time;
        active_effect_list = newRef.active_effect_list;
        max_hit_end = newRef.max_hit_end;
        max_hit_trigger = newRef.max_hit_trigger;
        end_circle = newRef.end_circle;
        active_pos_effect_delay_time = newRef.active_pos_effect_delay_time;
        active_pos_effect_list = newRef.active_pos_effect_list;
        check_tar_enable_condition = newRef.check_tar_enable_condition;
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public ArrayList<Long> sfx_id;//特效名称
    public long hit_sfx_id;

    public int speed;//子弹速度  厘米/秒
    public int width;//子弹宽度

    public EWCGBulletType bullet_type;
    public int bullet_length;

    public ArrayList<Long> range_effect_list;//经过区域产生效果
    public String end_condition;
    public int max_target_quantity;
    public int active_effect_delay_time;
    public ArrayList<Long> active_effect_list;
    public boolean max_hit_end;
    public boolean max_hit_trigger;
    public boolean end_circle;
    public int active_pos_effect_delay_time;
    public ArrayList<Long> active_pos_effect_list;
    public String check_tar_enable_condition;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

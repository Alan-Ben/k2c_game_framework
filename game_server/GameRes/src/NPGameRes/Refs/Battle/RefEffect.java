package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import WCGCommon.Enum.NPEnum.ENPRelationType;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeSelectPos;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeType;
import WCGCommon.Enum.NPEnum.EWCGSortCondition;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "effect")
public class RefEffect extends RefBase
{
    private static RefTableContainer<RefEffect> _g_mgr = new RefTableContainer<RefEffect>();

    public static RefTableContainer<RefEffect> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefEffect> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefEffect>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEffect newRef = (RefEffect) _newRef;
        id = newRef.id;
        sfx_id = newRef.sfx_id;
        relation_type = newRef.relation_type;
        range_type = newRef.range_type;
        is_target_rel = newRef.is_target_rel;
        range_pos = newRef.range_pos;
        width = newRef.width;
        height = newRef.height;
        sort_condition = newRef.sort_condition;
        sort_condition_param = newRef.sort_condition_param;
        effect_target_count = newRef.effect_target_count;
        condition_info_list = newRef.condition_info_list;
        effect_info_list = newRef.effect_info_list;
        select_all = newRef.select_all;
        without_target = newRef.without_target;
        use_dodge = newRef.use_dodge;
        use_crit = newRef.use_crit;
        effect_info_list_chance = newRef.effect_info_list_chance;
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
    public long id;//效果id
    public ArrayList<Long> sfx_id;//目标特效
    public ArrayList<ENPRelationType> relation_type;//作用目标类型
    public EWCGEffectRangeType range_type;//范围类型
    public boolean is_target_rel;//是否使用目标进行关系判断
    public EWCGEffectRangeSelectPos range_pos;//范围选择位置
    public int width;//范围宽度  (扇形表示半角的正弦值)  矩形表示宽度
    public int height;//范围长度   厘米
    public EWCGSortCondition sort_condition;//选择目标按照这个条件进行排序（目标排序条件）
    public String sort_condition_param;//条件参数
    public int effect_target_count;//效果作用目标上限

    public String condition_info_list;//筛选条件列表
    public String effect_info_list;//效果信息列表

    public boolean select_all;
    public boolean without_target;
    public boolean use_dodge;
    public boolean use_crit;
    public int effect_info_list_chance;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

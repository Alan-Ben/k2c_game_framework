package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;
import NPGameRes.Refs.Battle.RefSkillActiveEffect;

import java.util.ArrayList;
import java.util.List;

public class WCGSkillActiveEffect implements _IWCGRndItem
{
    //    public List<WCGSkillActiveSelfSfxTimeLine> self_sfx_id_List;///时间轴-本体技能特效集合
    public List<Long> target_sfx_Id;//目标技能特效名
    public long bullet_id;
    public int active_effect_delay_time;//触发延迟时间  毫秒
    public ArrayList<Long> active_effect_list;//触发效果id列表
    public List<Long> active_pos_effect_list;//,对应WCGPosEffectRefObj表id
    public int active_pos_effect_delay_time;//触发延迟时间  毫秒
    public int chance;//触发概率

    /**
     * 随机概率
     */
    public int rndChance()
    {
        return chance;
    }

    public void adapt(RefSkillActiveEffect refAciveEffect)
    {
//       this.self_sfx_Id= refAciveEffect.self_sfx_Id;
        this.target_sfx_Id = refAciveEffect.target_sfx_Id;
        this.bullet_id = refAciveEffect.bullet_id;
        this.active_effect_delay_time = refAciveEffect.active_effect_delay_time;
        this.active_effect_list = refAciveEffect.active_effect_list;
        this.chance = refAciveEffect.chance;
        this.active_pos_effect_list = refAciveEffect.active_pos_effect_list;
        this.active_pos_effect_delay_time = refAciveEffect.active_pos_effect_delay_time;

    }

    @Override
    public String toString()
    {
        return String.format("子弹id:%d,延迟时间:%d,效果列表:(%d)"
                , bullet_id
                , active_effect_delay_time
                , StringFunc.joinString(",", CommonFunc.toStringList(active_effect_list)));
    }
}
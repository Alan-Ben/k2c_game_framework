package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefPosEffect;

import java.util.List;

public class WCGPosEffectRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;//效果id
    public long sfx_id;//目标位置播放的特效

    public WCGSingleConditionGroupObj single_cond_info;//自身筛选条件列表

    public List<WCGPosEffectSerializeInfo> effect_info_list;//效果信息列表

    public void adapt(RefPosEffect ref)
    {
        this.id = ref.id;
        this.sfx_id = ref.sfx_id;
        this.single_cond_info = WCGSingleConditionGroupObj.readConditionGroupList(ref.single_cond_info, "");
        this.effect_info_list = WCGPosEffectSerializeInfo.readEffectList(ref.effect_info_list);

    }

}
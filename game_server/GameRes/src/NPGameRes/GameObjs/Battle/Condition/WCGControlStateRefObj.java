package NPGameRes.GameObjs.Battle.Condition;

import NPGameRes.GameObjs.Battle._IALBasicRefObj;
import NPGameRes.Refs.Battle.RefControlState;
import WCGCommon.Enum.NPEnum.EWCGControlState;

import java.util.ArrayList;

public class WCGControlStateRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return (long) control_state.ordinal();
    }

    public EWCGControlState control_state;//控制状态
    public ArrayList<Long> add_buf_list;//当增加这个控制状态的时候，将附带的buf列表，只在控制对象增加的时候处理。可以替换SubControlState的_initSfx函数功能
    public ArrayList<Long> state_sfx_list; //当增加这个控制状态时需要附带的特效Id队列
    public ArrayList<Long> end_sfx_list;//当对象离开这个状态的时候需要给对象附加的特效Id队列

    public void adapt(RefControlState ref)
    {
        this.control_state = ref.control_state;
        this.add_buf_list = ref.add_buf_list;
        this.state_sfx_list = ref.state_sfx_list;
        this.end_sfx_list = ref.end_sfx_list;

    }
}
